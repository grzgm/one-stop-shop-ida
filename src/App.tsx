import "./css/App.css";
import Navbar from "./components/bars/Navbar";
import Router from "./routes/Router";
import Sidebar from "./components/bars/Sidebar";
import { useEffect, useState } from "react";
import CurrentOfficeContext from "../src/contexts/CurrentOfficeContext.ts"

function App() {
  const maxWindowWidth = 1450
  const [windowWidth, setWindowWidth] = useState(window.innerWidth);
  const [showSidebar, switchShowSidebar] = useState(false)
  const [currentOffice, setCurrentOffice] = useState("Utrecht")

  useEffect(() => {
    const handleResize = () => {
      setWindowWidth(window.innerWidth);
      if (window.innerWidth > maxWindowWidth) {
        switchShowSidebar(false)
      }
    };

    window.addEventListener('resize', handleResize);

    return () => {
      window.removeEventListener('resize', handleResize);
    };
  }, []);

  return (
    <div>
      <CurrentOfficeContext.Provider value={{ currentOffice: currentOffice, setCurrentOffice: setCurrentOffice }}>
        <Navbar onPressOpenSidebar={switchShowSidebar} />
        {windowWidth <= maxWindowWidth && showSidebar && (<Sidebar onPressCloseSidebar={switchShowSidebar} />)}
        <Router />
      </CurrentOfficeContext.Provider>
    </div>
  );
}

export default App;
