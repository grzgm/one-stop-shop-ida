import { createContext } from "react";
import { IOfficeFeatures } from "../api/OfficeFeaturesAPI";
import { officeInformationUtrechtDefaultData } from "../assets/OfficeInformationData";

interface OfficeFeaturesContextProps {
  officeFeatures: { [key: string]: IOfficeFeatures };
  setUpAllOfficeFeatures: () => void
}

const OfficeFeaturesContext = createContext<OfficeFeaturesContextProps>({
  officeFeatures: officeInformationUtrechtDefaultData,
  setUpAllOfficeFeatures: () => {},
});

export default OfficeFeaturesContext;