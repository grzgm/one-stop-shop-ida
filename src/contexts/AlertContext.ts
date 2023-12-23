import { createContext } from "react";
import { IActionResult } from "../api/Response";

interface AlertContextProps {
    alertResponse: IActionResult<any> | undefined;
    setAlert: (response: IActionResult<any>) => void;
}

const AlertContext = createContext<AlertContextProps>({
    alertResponse: undefined,
    setAlert: () => {},
});

export default AlertContext;
