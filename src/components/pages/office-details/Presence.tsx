import { redirect } from "react-router-dom";
import { IOfficeFeatures } from "../../../api/OfficeFeaturesAPI";

function PresenceLoader(currentOfficeFeatures: IOfficeFeatures) {
      if (currentOfficeFeatures && currentOfficeFeatures.canRegisterPresence == true) {
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
