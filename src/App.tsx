import "./css/App.css";
import Router from "./routes/Router";
import { useState } from "react";
import CurrentOfficeContext from "../src/contexts/CurrentOfficeContext.ts"
import { RouterProvider, createBrowserRouter, createRoutesFromElements } from "react-router-dom";
import Cookies from "universal-cookie";
import AlertContext from "./contexts/AlertContext.ts";
import { Alert } from "./components/Alert.tsx";

function App() {
	// Current Office
	const cookies = new Cookies();
	let currentOfficeCookies = cookies.get("currentOffice")

	if (!currentOfficeCookies) {
		currentOfficeCookies = "Utrecht"
		cookies.set("currentOffice", JSON.stringify(currentOfficeCookies), { path: "/" })
	}
	const [currentOffice, setCurrentOffice] = useState(currentOfficeCookies)

	const setCurrentOfficeAndCookie = (newCurrentOffice: string) => {
		setCurrentOffice(newCurrentOffice);
		cookies.set("currentOffice", newCurrentOffice, { path: '/' });
	}

	// Alert System
	const [alertText, setAlertText] = useState<string>("");
	const [alertStatus, setAlertStatus] = useState<boolean>(true);
	const [alertTimer, setAlertTimer] = useState<NodeJS.Timeout | undefined>(undefined);
	const [alertLineInterval, setAlertLineInterval] = useState<NodeJS.Timeout | undefined>(undefined);
	const [alertLineLength, setAlertLineLength] = useState<number>(100);
	const alertVisibilityTime = 3000;
	const alertIntervalDuration = (alertVisibilityTime - 200) / (100 / 5);

	const setAlert = (alertText: string, alertStatus: boolean) => {
		setAlertText(alertText);
		setAlertStatus(alertStatus);
		clearTimeout(alertTimer);
		clearInterval(alertLineInterval);
		// Reset Alert Line Length
		setAlertLineLength(100)

		setAlertTimer(setTimeout(() => {
			setAlertText("");
		}, alertVisibilityTime));

		// Update the line length every half second
		setAlertLineInterval(setInterval(() => {
			// Decrease the line length by a certain amount
			setAlertLineLength((prevLength) => Math.max(0, prevLength - 5));
			if (alertLineLength <= 0) clearInterval(alertLineInterval);
		}, alertIntervalDuration));
	}

	const closeAlert = () => {
		clearTimeout(alertTimer);
		clearInterval(alertLineInterval);
		setAlertText("");
	}

	const customBrowserRouter = createBrowserRouter(createRoutesFromElements(
		Router(currentOffice)
	))

	return (
		<>
			<CurrentOfficeContext.Provider value={{ currentOffice: currentOffice, setCurrentOffice: setCurrentOfficeAndCookie }}>
				<AlertContext.Provider value={{
					alertText: alertText,
					alertStatus: alertStatus,
					setAlert: setAlert,
					closeAlert: closeAlert,
				}}>
					<>
						<RouterProvider router={customBrowserRouter} />
						{alertText && <Alert alertText={alertText} alertStatus={alertStatus} alertLineLength={alertLineLength} onClick={closeAlert} />}
					</>
				</AlertContext.Provider>
			</CurrentOfficeContext.Provider>
		</>
	);
}

export default App;
