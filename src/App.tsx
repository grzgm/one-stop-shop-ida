import "./css/App.css";
import Router from "./routes/Router";
import { useState } from "react";
import CurrentOfficeContext from "../src/contexts/CurrentOfficeContext.ts"
import { RouterProvider, createBrowserRouter, createRoutesFromElements } from "react-router-dom";

function App() {
  const [currentOffice, setCurrentOffice] = useState("Utrecht")


  const customBrowserRouter = createBrowserRouter(createRoutesFromElements(
    Router()
  ))

  return (
    <>
      <CurrentOfficeContext.Provider value={{ currentOffice: currentOffice, setCurrentOffice: setCurrentOffice }}>
        <RouterProvider router={customBrowserRouter} />
      </CurrentOfficeContext.Provider>
    </>
  );
}

export default App;
