import { useEffect, useState } from "react";
import Navbar from "./components/bars/Navbar";
import Sidebar from "./components/bars/Sidebar";
import { Outlet } from "react-router-dom";

function AppOverlay() {
    const maxWindowWidth = 1450
    const [windowWidth, setWindowWidth] = useState(window.innerWidth);
    const [showSidebar, switchShowSidebar] = useState(false)

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
        <>
            <Navbar onPressOpenSidebar={switchShowSidebar} />
            {windowWidth <= maxWindowWidth && showSidebar && (<Sidebar onPressCloseSidebar={switchShowSidebar} />)}
            <Outlet />
        </>
    );
}

export default AppOverlay;
