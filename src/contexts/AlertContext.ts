import { createContext } from "react";
import { IActionResult } from "../api/Response";

interface AlertContextProps {
    alertResponse: IActionResult<any> | undefined;
    setAlert: (response: IActionResult<any>) => void;
    alertTimer: NodeJS.Timeout | undefined;
}

const AlertContext = createContext<AlertContextProps>({
    alertResponse: undefined,
    alertTimer: undefined,
    setAlert: () => {},
});

export default AlertContext;
