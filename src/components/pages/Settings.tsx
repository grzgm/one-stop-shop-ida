import { HeadingLarge, BodyNormal } from "../text-wrapers/TextWrapers";

function Settings() {
      return (
            <div className="content">
                  <div className="description">
                        <HeadingLarge>Settings</HeadingLarge>
                        <BodyNormal additionalClasses={["font-colour--fail"]}>WORK IN PROGRESS</BodyNormal>
                  </div>
            </div>
      );
}

export default Settings;
