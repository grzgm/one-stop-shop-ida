import { useContext, useEffect, useState } from "react";
import { BodyNormal, BodySmall, HeadingLarge, HeadingSmall } from "../../text-wrapers/TextWrapers";
import Button from "../../Buttons";
import "../../../css/components/pages/office-details/lunch.css"
import { officeInformationData } from "../../../assets/OfficeInformationData";
import { redirect } from "react-router-dom";
import { IsAuth } from "../../../api/MicrosoftGraphAPI";
import CurrentOfficeContext from "../../../contexts/CurrentOfficeContext";
import { ILunchTodayItem, IsRegistered, RegisterLunchToday } from "../../../api/LunchTodayAPI";
import { GetRegisteredDays, ILunchRecurringItem, PutLunchRecurringItem } from "../../../api/LunchRecurringAPI";
import { PostSubscribe } from "../../../api/PushAPI";
import AlertContext from "../../../contexts/AlertContext";

async function LunchLoader(officeName: string) {
	const currentOfficeInformationData = officeInformationData[officeName]
	if (currentOfficeInformationData.canRegisterLunch == true) {
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
	const [isPushEnabled, setIsPushEnabled] = useState(false);
	const [isButtonDisabled, setIsButtonDisabled] = useState(false);
	const { setAlert } = useContext(AlertContext);

	// Lunch Today
	const [todayRegistration, setTodayRegistration] = useState<ILunchTodayItem | undefined>(undefined)

	// Lunch Recurring
	const [registeredDays, setRegisteredDays] = useState<ILunchRecurringItem>({
		monday: false,
		tuesday: false,
		wednesday: false,
		thursday: false,
		friday: false,
	});
	const weekDaysNames = ['Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday']

	// Office Dropdown
	const [selectedOffice, setSelectedOffice] = useState<string>(officeName);
	let offices = Object.keys(officeInformationData)
		.filter(key => officeInformationData[key].canRegisterLunch)
		.map(key => officeInformationData[key].officeName);
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
				isRegisteredRes.payload.registrationDate = new Date(isRegisteredRes.payload.registrationDate)
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

	// Lunch Recurring 
	const handleDayChange = async (dayName: keyof ILunchRecurringItem) => {
		const updatedCheckedBoxes = { ...registeredDays };
		updatedCheckedBoxes[dayName] = !updatedCheckedBoxes[dayName];
		setRegisteredDays(updatedCheckedBoxes);
		const response = await PutLunchRecurringItem(updatedCheckedBoxes);
		setAlert(response.statusText, response.success);
	};

	// Lunch Today
	const registerForToday = async (registration: boolean) => {
		setIsButtonDisabled(true);
		if (!isPastNoon()) {
			const response = await RegisterLunchToday(registration, selectedOffice);
			// const response = await CreateEvent("grzegorz.malisz@weareida.digital", "lunch event", new Date().toISOString(), new Date().toISOString());
			// setResponse(await SendEmail(RegisterForTodayMail(officeName), "office@ida-mediafoundry.nl"));
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
				<div className="lunch-main__recurring">
					<HeadingSmall>Register recurring</HeadingSmall>
					<BodySmall>You will get the notification</BodySmall>
					<BodySmall>to register on the given day at 9:00</BodySmall>
					{!isPushEnabled && <BodySmall additionalClasses={["font-colour--fail"]}>For full functionality enable Push Notifications</BodySmall>}
					<form className="lunch-main__form body--normal">
						{Object.keys(registeredDays).map((dayName, index) => (
							<div className="lunch-main__form__checkboxes" key={dayName}>
								<input
									type="checkbox"
									checked={registeredDays[dayName as keyof ILunchRecurringItem]}
									onChange={() => handleDayChange(dayName as keyof ILunchRecurringItem)}
									id={dayName}
								/>
								<label key={dayName} htmlFor={dayName}>
									{weekDaysNames[index]}
								</label>
							</div>
						))}
					</form>
				</div>
				<div className="lunch-main__today">
					<HeadingSmall>Register for today</HeadingSmall>
					<BodySmall>Only for today</BodySmall>
					<BodySmall>before 12:00</BodySmall>
					<form className="lunch-main__form body--normal">
						<label>Select an Office to register at: </label>
						<select 
							value={(todayRegistration?.registrationDate && isToday(todayRegistration.registrationDate)) ? 
									capitalizeFirstLetter(todayRegistration.office) : officeName} 
							onChange={handleOfficeDropdownChange} 
							className="body--normal">
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