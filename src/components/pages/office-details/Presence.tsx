import { officeInformationData } from "../../../assets/OfficeInformationData";
import { redirect } from "react-router-dom";

function PresenceLoader(officeName: string) {
      const currentOfficeInformationData = officeInformationData[officeName]
      if (currentOfficeInformationData && currentOfficeInformationData.canRegisterPresence == true) {
            return null
      }
      throw redirect("/")
}

function Presence() {

      return (
            <h1>Presence</h1>
      );
}

export default Presence;
export { PresenceLoader };
