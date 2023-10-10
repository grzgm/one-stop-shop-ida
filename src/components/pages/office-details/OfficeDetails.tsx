import { useContext } from "react";
import { BodyNormal, HeadingLarge } from "../../text-wrapers/TextWrapers";
import Panel from "../../tiles/Panel";
import CurrentOfficeContext from "../../../contexts/CurrentOfficeContext";
import { officeInformationData } from "../../../assets/OfficeInformationData";

function OfficeDetails() {
  const officeName = useContext(CurrentOfficeContext).currentOffice;
  const currentOfficeInformationData = officeInformationData[officeName]

  return (
    <div className="content">
      <div className="description">
        <HeadingLarge>{officeName}</HeadingLarge>
        <HeadingLarge>Office Details</HeadingLarge>
        <BodyNormal>Manage all office</BodyNormal>
        <BodyNormal>related information!</BodyNormal>
      </div>
      {currentOfficeInformationData && <div className="content__panels">
        {currentOfficeInformationData.canReserveDesk && <Panel linkAddress="/office-details/reserve-desk" title="Reserve a Desk" description="Reserve a Desk"/>}
        {currentOfficeInformationData.canRegisterLunch && <Panel linkAddress="/office-details/lunch" title="Lunch" description="Sign up for Lunch"/>}
        <Panel linkAddress="/office-details/office-information" title="Office Information" description="Office Information Access Parking Wi-Fi How many ppl in the office"/>
        {currentOfficeInformationData.canRegisterPresence && <Panel linkAddress="/office-details/presence" title="Presence" description="Let your colleague know when you are present"/>}
      </div>}
    </div>
  );
}

export default OfficeDetails;
