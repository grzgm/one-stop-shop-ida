import "./css/App.css";
import Router from "./routes/Router";
import { useState } from "react";
import CurrentOfficeContext from "../src/contexts/CurrentOfficeContext.ts"
import { RouterProvider, createBrowserRouter, createRoutesFromElements } from "react-router-dom";
import Cookies from "universal-cookie";

function App() {
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

  const customBrowserRouter = createBrowserRouter(createRoutesFromElements(
    Router(currentOffice)
  ))

  return (
    <>
      <CurrentOfficeContext.Provider value={{ currentOffice: currentOffice, setCurrentOffice: setCurrentOfficeAndCookie }}>
        <RouterProvider router={customBrowserRouter} />
      </CurrentOfficeContext.Provider>
    </>
  );
}

export default App;
