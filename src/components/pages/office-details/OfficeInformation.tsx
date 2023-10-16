import { useContext } from "react";
import CurrentOfficeContext from "../../../contexts/CurrentOfficeContext";
import { officeInformationData } from "../../../assets/OfficeInformationData";
import { BodyNormal, HeadingLarge, HeadingSmall } from "../../text-wrapers/TextWrapers";
import "../../../css/components/pages/office-details/office-information.css"

function OfficeInformation() {
	const officeName = useContext(CurrentOfficeContext).currentOffice;
	const currentOfficeInformationData = officeInformationData[officeName]

	return (
		<div className="content">
			<div className="description">
				<HeadingLarge>{`${officeName} Office Information`}</HeadingLarge>
				<BodyNormal>All information you need!</BodyNormal>
			</ div>
			<main id="office-information-main">
				<section>
					<HeadingSmall>Address</HeadingSmall>
					<BodyNormal>{currentOfficeInformationData.officeInformation.address}</BodyNormal>
				</section>
				<section>
					<HeadingSmall>Opening Hours</HeadingSmall>
					<BodyNormal>{currentOfficeInformationData.officeInformation.openingHours}</BodyNormal>
				</section>
				<section>
					<HeadingSmall>Access Information</HeadingSmall>
					<BodyNormal>{currentOfficeInformationData.officeInformation.accessInformation}</BodyNormal>
				</section>
				<section>
					<HeadingSmall>Parking Information</HeadingSmall>
					<BodyNormal>{currentOfficeInformationData.officeInformation.parkingInformation}</BodyNormal>
				</section>
				{currentOfficeInformationData.canRegisterLunch && <section>
					<HeadingSmall>Lunch Information</HeadingSmall>
					<BodyNormal>{currentOfficeInformationData.officeInformation.lunchInformation}</BodyNormal>
				</section>}
			</main>
		</ div>
	);
}

export default OfficeInformation;
