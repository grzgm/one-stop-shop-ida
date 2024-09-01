import { useContext } from "react";
import CurrentOfficeContext from "../../../contexts/CurrentOfficeContext";
import { BodyNormal, HeadingLarge, HeadingSmall } from "../../text-wrapers/TextWrapers";
import "../../../css/components/pages/office-details/office-information.css"
import OfficeFeaturesContext from "../../../contexts/OfficeFeaturesContext";
import { officeInformationUtrechtDefaultData } from "../../../assets/OfficeInformationData";
import { addSpaceBeforeCapitalLetters, capitalizeFirstLetter } from "../../../misc/TextFunctions";

function OfficeInformation() {
	const officeName = useContext(CurrentOfficeContext).currentOffice ?? 'utrecht';
	const { officeFeatures } = useContext(OfficeFeaturesContext) ?? { officeInformationUtrechtDefaultData };
	const currentOfficeInformationData = officeFeatures[officeName.toLocaleLowerCase()]

	return (
		<div className="content">
			<div className="description">
				<HeadingLarge>{`${capitalizeFirstLetter(officeName)} Office Information`}</HeadingLarge>
				<BodyNormal>All information you need!</BodyNormal>
			</ div>
			<main id="office-information-main">

				{(Object.entries(currentOfficeInformationData.officeInformation)).map(([information, value]) => {
					if (information == "officeName" || information == "officeCoordinates" || !value)
						return
					return (
						<section key={value.toString()}>
							<HeadingSmall>{capitalizeFirstLetter(addSpaceBeforeCapitalLetters(information.toString()))}</HeadingSmall>
							<BodyNormal>{value.toString()}</BodyNormal>
						</section>)
				})}

			</main>
		</ div>
	);
}

export default OfficeInformation;
