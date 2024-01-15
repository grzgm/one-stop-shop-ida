import { useContext, useEffect, useState } from "react";
import { BodyNormal, BodySmall, HeadingLarge, HeadingSmall } from "../../text-wrapers/TextWrapers";
import Button from "../../Buttons";
import "../../../css/components/pages/office-details/lunch.css"
import { redirect } from "react-router-dom";
import { IsAuth } from "../../../api/MicrosoftGraphAPI";
import { IsAuth as IsAuthSlack } from "../../../api/SlackAPI";
import CurrentOfficeContext from "../../../contexts/CurrentOfficeContext";
import { ILunchRegistrationsItem, IsRegistered, PutLunchRegistrationsItem } from "../../../api/LunchRegistrationsAPI";
import { GetRegisteredDays, ILunchDaysItem, PutLunchDaysItem } from "../../../api/LunchDaysAPI";
import { PostSubscribe } from "../../../api/PushAPI";
import AlertContext from "../../../contexts/AlertContext";
import { IOfficeFeatures } from "../../../api/OfficeFeaturesAPI";
import OfficeFeaturesContext from "../../../contexts/OfficeFeaturesContext";

async function LunchLoader(currentOfficeFeatures: IOfficeFeatures) {
	if (currentOfficeFeatures && currentOfficeFeatures.canRegisterLunch == true) {
		// if (!(await IsAuthSlack()).payload) {
		// 	return redirect(`/slack-auth?previousLocation=${encodeURI("/office-details/lunch")}`)
		// }
		if ((await IsAuth()).payload) {
			return null
		}
		else {
			return redirect(`/microsoft-auth?previousLocation=${encodeURI("/office-details/lunch")}`)
		}
	}
	throw redirect("/")
}

function Lunch() {
	const officeName = useContext(CurrentOfficeContext).currentOffice;
	const { officeFeatures } = useContext(OfficeFeaturesContext);
	const [isPushEnabled, setIsPushEnabled] = useState(false);
	const [isButtonDisabled, setIsButtonDisabled] = useState(false);
	const { setAlert } = useContext(AlertContext);

	// Lunch Today Registration
	const [todayRegistration, setTodayRegistration] = useState<ILunchRegistrationsItem | undefined>(undefined)

	// Lunch Days
	const [registeredDays, setRegisteredDays] = useState<ILunchDaysItem>({
		monday: false,
		tuesday: false,
		wednesday: false,
		thursday: false,
		friday: false,
	});
	const weekDaysNames = ['Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday']

	// Office Dropdown
	const [selectedOffice, setSelectedOffice] = useState<string>
		((todayRegistration?.registrationDate && isToday(todayRegistration.registrationDate)) ?
			capitalizeFirstLetter(todayRegistration.office) : officeName);

	let offices = Object.keys(officeFeatures)
		.filter(key => officeFeatures[key].canRegisterLunch)
		.map(key => officeFeatures[key].officeName);
	// offices = [officeName, ...offices.filter(value => value !== officeName)]

	const handleOfficeDropdownChange = (event: React.ChangeEvent<HTMLSelectElement>) => {
		const selectedValue = event.target.value;
		setSelectedOffice(selectedValue);
	};

	useEffect(() => {
		if ("serviceWorker" in navigator && "PushManager" in window) {
			navigator.serviceWorker
				.register("/sw-push.js", { scope: "/office-details/lunch" })
				.then((serviceWorkerRegistration) => {
					console.info("Service worker was registered.");
					console.info({ serviceWorkerRegistration });
				})
				.catch((error) => {
					console.error(
						"An error occurred while registering the service worker."
					);
					console.error(error);
				});
		} else {
			console.error(
				"Browser does not support service workers or push messages."
			);
		}

		const IsRegisteredWrapper = async () => {
			const isRegisteredRes = await IsRegistered();
			if (isRegisteredRes.payload?.registrationDate) {
				isRegisteredRes.payload.registrationDate = new Date(isRegisteredRes.payload.registrationDate);

				setSelectedOffice((isRegisteredRes?.payload.registrationDate && isToday(isRegisteredRes.payload.registrationDate)) ?
					capitalizeFirstLetter(isRegisteredRes.payload.office) : officeName)
			}
			setTodayRegistration(isRegisteredRes.payload);
		}
		const GetRegisteredDaysWrapper = async () => {
			const registeredDaysRes = await GetRegisteredDays();
			if (registeredDaysRes.payload !== undefined) {
				setRegisteredDays(registeredDaysRes.payload);
			}
		}
		const PostSubscribeWrapper = async () => {
			const subscribeRes = await PostSubscribe();
			if (subscribeRes.success !== undefined) {
				setIsPushEnabled(subscribeRes.success);
			}
		}

		IsRegisteredWrapper();
		GetRegisteredDaysWrapper();
		PostSubscribeWrapper();
	}, []);

	// Lunch Days 
	const handleDayChange = async (dayName: keyof ILunchDaysItem) => {
		const updatedCheckedBoxes = { ...registeredDays };
		updatedCheckedBoxes[dayName] = !updatedCheckedBoxes[dayName];
		setRegisteredDays(updatedCheckedBoxes);
		const response = await PutLunchDaysItem(updatedCheckedBoxes);
		setAlert(response.statusText, response.success);
	};

	// Lunch Today
	const registerForToday = async (registration: boolean) => {
		setIsButtonDisabled(true);
		if (!isPastNoon()) {
			const response = await PutLunchRegistrationsItem(registration, selectedOffice);
			if (response.payload?.registrationDate) {
				response.payload.registrationDate = new Date(response.payload.registrationDate)
			}
			setAlert(response.success ? "Registration Updated" : "Cannot Update Registration", response.success);
			setTodayRegistration(response.payload);
		}
		setIsButtonDisabled(false);
	};

	return (
		<div className="content">
			<div className="description">
				<HeadingLarge>Lunch</HeadingLarge>
				<BodyNormal>Don't forget to register!</BodyNormal>
			</div>
			<main className="lunch-main">
				<div className="lunch-main__days">
					<HeadingSmall>Lunch Days</HeadingSmall>
					<BodySmall>You will get the notification</BodySmall>
					<BodySmall>to register on the given day at 9:00</BodySmall>
					{!isPushEnabled && <BodySmall additionalClasses={["font-colour--fail"]}>For full functionality enable Push Notifications</BodySmall>}
					<form className="lunch-main__form body--normal">
						{Object.keys(registeredDays).map((dayName, index) => (
							<div className="lunch-main__form__checkboxes" key={dayName}>
								<input
									type="checkbox"
									checked={registeredDays[dayName as keyof ILunchDaysItem]}
									onChange={() => handleDayChange(dayName as keyof ILunchDaysItem)}
									id={dayName}
								/>
								<label key={dayName} htmlFor={dayName}>
									{weekDaysNames[index]}
								</label>
							</div>
						))}
					</form>
				</div>
				<div className="lunch-main__registrations">
					<HeadingSmall>Register for today</HeadingSmall>
					<BodySmall>Only for today</BodySmall>
					<BodySmall>before 12:00</BodySmall>
					<form className="lunch-main__form body--normal">
						<label>Select an Office to register at: </label>
						<select value={selectedOffice} onChange={handleOfficeDropdownChange} className="body--normal">
							{offices.map((office) => (
								<option key={office} value={office}>
									{office}
								</option>
							))}
						</select>
					</form>
					{todayRegistration && (todayRegistration?.registrationDate && todayRegistration.registrationDate.setHours(0, 0, 0, 0) == new Date().setHours(0, 0, 0, 0)) ?
						<>
							<Button child="Deregister" disabled={isPastNoon() || isButtonDisabled} onClick={() => registerForToday(false)} />
							{!isPastNoon() && <BodySmall>You are already registered at: {capitalizeFirstLetter(todayRegistration.office)}</BodySmall>}
						</> :
						<Button child="Register" disabled={isPastNoon() || isButtonDisabled} onClick={() => registerForToday(true)} />
					}
				</div>
			</main>
		</div>
	);
}

function isPastNoon(): boolean {
	const currentTime = new Date();
	const currentHours = currentTime.getHours();

	// Compare the current hours with 12 (noon)
	return currentHours >= 12;
}

function capitalizeFirstLetter(str: string) {
	return str.charAt(0).toUpperCase() + str.slice(1);
}

function isToday(date: Date) {
	return date.setHours(0, 0, 0, 0) == new Date().setHours(0, 0, 0, 0);
}

export default Lunch;
export { LunchLoader };