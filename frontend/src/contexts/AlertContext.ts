import { createContext } from "react";

interface AlertContextProps {
    alertText: string;
    alertStatus: boolean;
    setAlert: (alertText: string, alertStatus: boolean) => void;
    closeAlert: () => void;
}

const AlertContext = createContext<AlertContextProps>({
    alertText: "",
    alertStatus: true,
    setAlert: () => {},
    closeAlert: () => {},
});

export default AlertContext;
