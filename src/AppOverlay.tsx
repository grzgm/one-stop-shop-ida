import { useEffect, useRef, useState } from "react";
import Navbar from "./components/bars/Navbar";
import Sidebar from "./components/bars/Sidebar";
import { Outlet, redirect } from "react-router-dom";
import { IsAuth } from "./api/MicrosoftGraphAPI";

async function AppLoader() {
    if ((await IsAuth()).payload) {
        return null
    }
    else {
        return redirect(`/microsoft-auth?previousLocation=${encodeURI("/")}`)
    }
}

function AppOverlay() {
    const navbarOptionsRef = useRef<HTMLDivElement>(null);
    const maxNavbarOptionsWidth = 1360
    const [navbarOptionsWidth, setNavbarOptionsWidth] = useState(navbarOptionsRef?.current?.offsetWidth);
    const [showSidebar, switchShowSidebar] = useState(false)

    useEffect(() => {
        setNavbarOptionsWidth(navbarOptionsRef?.current?.offsetWidth);

        const resizeObserver = new ResizeObserver((entries) => {
            for (const entry of entries) {
                const newWidth = entry.contentRect.width;
                setNavbarOptionsWidth(newWidth);
                console.log(newWidth);

                if (newWidth > maxNavbarOptionsWidth) {
                    switchShowSidebar(false);
                }
            }
        });

        const navbarOptions = navbarOptionsRef.current

        if (navbarOptions) {
            resizeObserver.observe(navbarOptions);

            return () => {
                resizeObserver.disconnect();
            };
        }
    }, [navbarOptionsRef.current?.offsetWidth]);

    return (
        <>
            <Navbar onPressOpenSidebar={switchShowSidebar} navbarOptionsRef={navbarOptionsRef} />
            {(navbarOptionsWidth && navbarOptionsWidth <= maxNavbarOptionsWidth) && showSidebar && (<Sidebar onPressCloseSidebar={switchShowSidebar} />)}
            <Outlet />
        </>
    );
}

export default AppOverlay;
export { AppLoader };
