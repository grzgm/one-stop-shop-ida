import { useContext, useState } from "react";
import { BodyNormal, BodySmall, HeadingLarge, HeadingSmall } from "../../text-wrapers/TextWrapers";
import Button from "../../Buttons";
import "../../../css/components/pages/office-details/lunch.css"
import { officeInformationData } from "../../../assets/OfficeInformationData";
import { redirect } from "react-router-dom";
import { IsAuth, SendEmail } from "../../../api/microsoft-graph-api/MicrosoftGraphAPI";
import CurrentOfficeContext from "../../../contexts/CurrentOfficeContext";

async function LunchLoader(officeName: string) {
	const currentOfficeInformationData = officeInformationData[officeName]
	if (currentOfficeInformationData.canRegisterLunch == true) {
		if (await IsAuth() == true)
		{
			return null
		}
		else{
			return redirect("/microsoft-auth")
		}
	}
	throw redirect("/")
}

function Lunch() {
	const officeName = useContext(CurrentOfficeContext).currentOffice;
	const [response, setResponse] = useState<string|null>(null)
	const [weekRegistration, setWeekRegistration] = useState<boolean[]>([false, false, false, false, false]);
	const weekDaysNames = ['Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday', 'Sunday']

	const handleCheckboxChange = (index: number) => {
		const updatedCheckedBoxes: boolean[] = [...weekRegistration];
		updatedCheckedBoxes[index] = !updatedCheckedBoxes[index];
		setWeekRegistration(updatedCheckedBoxes);
	};
	const saveLunchDays = () => {
		console.log(weekRegistration)
	};
	const registerForToday = async () => {
		console.log("today register")
		// setResponse(await SendEmail(RegisterForTodayMail(officeName), "office@ida-mediafoundry.nl"));
		setResponse(await SendEmail(RegisterForTodayMail(officeName), "grzegorz.malisz@weareida.digital"));
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
					{response && <BodySmall>{response}</BodySmall>}
					<form>
						<Button child="Register" onClick={() => registerForToday()} />
					</form>
				</div>
			</main>
		</div>
	);
}

function RegisterForTodayMail(officeName: string){
	const message = `Hi,
I would like to register for today's lunch at ${officeName} Office.
Kind Regards`
	return message
}

export default Lunch;
export { LunchLoader };