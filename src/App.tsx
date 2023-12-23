import "./css/App.css";
import Router from "./routes/Router";
import { useState } from "react";
import CurrentOfficeContext from "../src/contexts/CurrentOfficeContext.ts"
import { RouterProvider, createBrowserRouter, createRoutesFromElements } from "react-router-dom";
import Cookies from "universal-cookie";
import AlertContext from "./contexts/AlertContext.ts";
import { IActionResult } from "./api/Response.ts";

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
	const [alertResponse, setAlertResponse] = useState<IActionResult<any> | undefined>(undefined);
	const [alertTimer, setAlertTimer] = useState<NodeJS.Timeout | undefined>(undefined);

	const setAlert = (response: IActionResult<any>) => {
		setAlertResponse(response);
		clearTimeout(alertTimer)
		setAlertTimer(setTimeout(() => {
			setAlertResponse(undefined);
		}, 3000));
	}

	const customBrowserRouter = createBrowserRouter(createRoutesFromElements(
		Router(currentOffice)
	))

	return (
		<>
			<CurrentOfficeContext.Provider value={{ currentOffice: currentOffice, setCurrentOffice: setCurrentOfficeAndCookie }}>
				<AlertContext.Provider value={{
					alertResponse: alertResponse,
					setAlert: setAlert
				}}>
					<RouterProvider router={customBrowserRouter} />
				</AlertContext.Provider>
			</CurrentOfficeContext.Provider>
		</>
	);
}

export default App;
