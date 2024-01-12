import { useContext } from "react";
import CurrentOfficeContext from "../../../contexts/CurrentOfficeContext";
import { BodyNormal, HeadingLarge, HeadingSmall } from "../../text-wrapers/TextWrapers";
import "../../../css/components/pages/office-details/office-information.css"
import OfficeFeaturesContext from "../../../contexts/OfficeFeaturesContext";
import { officeInformationUtrechtDefaultData } from "../../../assets/OfficeInformationData";

function OfficeInformation() {
	const officeName = useContext(CurrentOfficeContext).currentOffice ?? 'Utrecht';
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
						<section>
							<HeadingSmall>{capitalizeFirstLetter(addSpaceBeforeCapitalLetters(information.toString()))}</HeadingSmall>
							<BodyNormal>{value.toString()}</BodyNormal>
						</section>)
				})}

			</main>
		</ div>
	);
}
function capitalizeFirstLetter(str: string) {
	return str.charAt(0).toUpperCase() + str.slice(1);
}

function addSpaceBeforeCapitalLetters(str: string) {
	// Use a regular expression to match capital letters
	// and insert a space before them using replace
	return str.replace(/([A-Z])/g, ' $1');
}

export default OfficeInformation;
