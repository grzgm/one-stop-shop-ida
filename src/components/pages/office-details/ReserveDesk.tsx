import OfficeSpace from "../../OfficeSpace";
import { BodyNormal, BodySmall, HeadingLarge } from "../../text-wrapers/TextWrapers";
import "../../../css/components/pages/office-details/reserve-desk.css"
import { officeInformationData } from "../../../assets/OfficeInformationData";
import { redirect } from "react-router-dom";
import { IsAuth } from "../../../api/MicrosoftGraphAPI";

async function ReserveDeskLoader(officeName: string) {
	const currentOfficeInformationData = officeInformationData[officeName]
	if (currentOfficeInformationData && currentOfficeInformationData.canReserveDesk == true) {
		if ((await IsAuth()).payload) {
			return null
		}
		else {
			return redirect(`/microsoft-auth?previousLocation=${encodeURI("/office-details/reserve-desk")}`)
		}
	}
	throw redirect("/")
}

function ReserveDesk() {
	return (
		<div className="content">
			<div className="description">
				<HeadingLarge>Reserve a Desk</HeadingLarge>
				<BodyNormal>Choose time slot</BodyNormal>
				<BodySmall>Remember you cannot have two identical time slots</BodySmall>
			</div>
			<main className="reserve-desk-main">
				<OfficeSpace />
			</main>
		</div>
	);
}

export default ReserveDesk;
export { ReserveDeskLoader };