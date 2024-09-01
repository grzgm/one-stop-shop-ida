import { redirect } from "react-router-dom";
import { IOfficeFeatures } from "../../../api/OfficeFeaturesAPI";
import { HeadingLarge, BodyNormal } from "../../text-wrapers/TextWrapers";

function PresenceLoader(currentOfficeFeatures: IOfficeFeatures) {
      if (currentOfficeFeatures && currentOfficeFeatures.canRegisterPresence == true) {
            return null
      }
      throw redirect("/")
}

function Presence() {
      return (
            <div className="content">
                  <div className="description">
                        <HeadingLarge>Presence</HeadingLarge>
                        <BodyNormal additionalClasses={["font-colour--fail"]}>WORK IN PROGRESS</BodyNormal>
                  </div>
            </div>
      );
}

export default Presence;
export { PresenceLoader };
