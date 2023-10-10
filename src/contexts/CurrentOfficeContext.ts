import { createContext, Dispatch, SetStateAction } from "react";

interface CurrentOfficeContextProps {
  currentOffice: string;
  setCurrentOffice: Dispatch<SetStateAction<string>>;
}

const CurrentOfficeContext = createContext<CurrentOfficeContextProps>({
  currentOffice: "",
  setCurrentOffice: () => {},
});

export default CurrentOfficeContext;
