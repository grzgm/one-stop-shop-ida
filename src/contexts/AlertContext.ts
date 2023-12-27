import { createContext } from "react";

interface AlertContextProps {
    alertText: string;
    alertStatus: boolean;
    setAlert: (alertText: string, alertStatus: boolean) => void;
}

const AlertContext = createContext<AlertContextProps>({
    alertText: "",
    alertStatus: true,
    setAlert: () => {},
});

export default AlertContext;
