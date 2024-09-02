import { useContext } from "react";
import { BodyNormal, HeadingLarge } from "../../text-wrapers/TextWrapers";
import Panel from "../../tiles/Panel";
import CurrentOfficeContext from "../../../contexts/CurrentOfficeContext";
import OfficeFeaturesContext from "../../../contexts/OfficeFeaturesContext";
import { capitalizeFirstLetter } from "../../../misc/TextFunctions";

function OfficeDetails() {
  const officeName = useContext(CurrentOfficeContext).currentOffice;
  const { officeFeatures } = useContext(OfficeFeaturesContext);
  const currentOfficeInformationData = officeFeatures[officeName]

  return (
    <div className="content">
      <div className="description">
        <HeadingLarge>{capitalizeFirstLetter(officeName)}</HeadingLarge>
        <HeadingLarge>Office Details</HeadingLarge>
        <BodyNormal>Manage all office</BodyNormal>
        <BodyNormal>related information!</BodyNormal>
      </div>
      {currentOfficeInformationData && <div className="content__panels">
        {currentOfficeInformationData.canReserveDesk && <Panel linkAddress="reserve-desk" title="Reserve a Desk" description="Reserve a Desk" />}
        {currentOfficeInformationData.canRegisterLunch && <Panel linkAddress={`${import.meta.env.VITE_BASE_URL_PATH ? import.meta.env.VITE_BASE_URL_PATH : ""}/slack-auth?previousLocation=${import.meta.env.VITE_BASE_URL_PATH ? import.meta.env.VITE_BASE_URL_PATH : ""}/office-details/lunch`} title="Lunch" description="Sign up for Lunch" />}
        <Panel linkAddress="office-information" title="Office Information" description="Office Information Access Parking Wi-Fi How many ppl in the office" />
        {currentOfficeInformationData.canRegisterPresence && <Panel linkAddress="presence" title="Presence" description="Let your colleague know when you are present" />}
      </div>}
    </div>
  );
}

export default OfficeDetails;
