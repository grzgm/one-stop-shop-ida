import "./css/App.css";
import Navbar from "./components/navbar/Navbar";
import Router from "./routes/Router";
import Sidebar from "./components/bars/Sidebar";
import { useContext, useEffect, useState } from "react";
import { ShowSidebarContext } from "./contexts/SidebarContext";

function App() {
  const maxWindowWidth = 1450
  const [windowWidth, setWindowWidth] = useState(window.innerWidth);
  const [showSidebar, switchShowSidebar] = useState(false)

  useEffect(() => {
    const handleResize = () => {
      setWindowWidth(window.innerWidth);
      if(window.innerWidth > maxWindowWidth){
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
      <Navbar onPressOpenSidebar={switchShowSidebar}/>
      {windowWidth <= maxWindowWidth && showSidebar && (<Sidebar onPressCloseSidebar={switchShowSidebar}/>)}
      <Router />
    </div>
  );
}

export default App;
