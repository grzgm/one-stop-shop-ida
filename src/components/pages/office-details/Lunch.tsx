import { useContext, useEffect, useState } from "react";
import { BodyNormal, BodySmall, HeadingLarge, HeadingSmall } from "../../text-wrapers/TextWrapers";
import Button from "../../Buttons";
import "../../../css/components/pages/office-details/lunch.css"
import { officeInformationData } from "../../../assets/OfficeInformationData";
import { redirect } from "react-router-dom";
import { IsAuth, RegisterLunchToday } from "../../../api/MicrosoftGraphAPI";
import CurrentOfficeContext from "../../../contexts/CurrentOfficeContext";
import { IActionResult } from "../../../api/Response";
import { IsRegistered } from "../../../api/LunchTodayAPI";

async function LunchLoader(officeName: string) {
	const currentOfficeInformationData = officeInformationData[officeName]
	if (currentOfficeInformationData.canRegisterLunch == true) {
		if ((await IsAuth()).payload == true) {
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
	const [response, setResponse] = useState<IActionResult<null> | null>(null)
	const [isRegisteredToday, setIsRegisteredToday] = useState<boolean>(true)
	const [weekRegistration, setWeekRegistration] = useState<boolean[]>([false, false, false, false, false]);
	const weekDaysNames = ['Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday', 'Sunday']

	useEffect(() => {
		const IsRegisteredWrapper = async () => {
			const isRegisteredRes = await IsRegistered();
			if (isRegisteredRes.payload == undefined)
			{
				setIsRegisteredToday(true);
			}
			else{
				setIsRegisteredToday(isRegisteredRes.payload);
			}
		}
		IsRegisteredWrapper();
	  }, []);

	const handleCheckboxChange = (index: number) => {
		const updatedCheckedBoxes: boolean[] = [...weekRegistration];
		updatedCheckedBoxes[index] = !updatedCheckedBoxes[index];
		setWeekRegistration(updatedCheckedBoxes);
	};
	const saveLunchDays = () => {
		console.log(weekRegistration)
	};
	const registerForToday = async () => {
		if(!isPastNoon())
		{
			setIsRegisteredToday(true);
			const response = await RegisterLunchToday(RegisterForTodayMail(officeName));
			// const response = await CreateEvent("grzegorz.malisz@weareida.digital", "lunch event", new Date().toISOString(), new Date().toISOString());
			// setResponse(await SendEmail(RegisterForTodayMail(officeName), "office@ida-mediafoundry.nl"));
			setResponse(response);
			if (!response.success)
			{
				setIsRegisteredToday(false);
			}
		}
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
					<BodySmall>Information will be sent</BodySmall>
					<BodySmall>before 12:00 on the mentioned day</BodySmall>
					<form className="lunch-main__form body--normal">
						{weekRegistration.map((isChecked, index) => (
							<div className="lunch-main__form__checkboxes" key={index}>
								<input
									type="checkbox"
									checked={isChecked}
									onChange={() => handleCheckboxChange(index)}
									id={weekDaysNames[index]}
								/>
								<label key={index} htmlFor={weekDaysNames[index]}>
									{weekDaysNames[index]}
								</label>
							</div>
						))}
					</form>
					<Button child="Save" onClick={() => saveLunchDays()} />
				</div>
				<div className="lunch-main__today">
					<HeadingSmall>Register for today</HeadingSmall>
					<BodySmall>Only for today</BodySmall>
					<BodySmall>before 12:00</BodySmall>
					{response && <BodySmall additionalClasses={[response.success ? "font-colour--success" : "font-colour--fail"]}>{response.status}</BodySmall>}
					<form>
						<Button child="Register" disabled={isPastNoon() || isRegisteredToday} onClick={() => registerForToday()} />
					</form>
				</div>
			</main>
		</div>
	);
}

function RegisterForTodayMail(officeName: string) {
	const message = `Hi,
I would like to register for today's lunch at ${officeName} Office.
Kind Regards`
	return message
}

function isPastNoon(): boolean {
	const currentTime = new Date();
	const currentHours = currentTime.getHours();

	// Compare the current hours with 12 (noon)
	return currentHours >= 17;
}

export default Lunch;
export { LunchLoader };