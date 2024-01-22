import { createContext } from "react";

interface CurrentOfficeContextProps {
  currentOffice: string;
  setCurrentOffice: (newCurrentOffice: string) => void;
}

const CurrentOfficeContext = createContext<CurrentOfficeContextProps>({
  currentOffice: "utrecht",
  setCurrentOffice: () => {},
});

export default CurrentOfficeContext;
