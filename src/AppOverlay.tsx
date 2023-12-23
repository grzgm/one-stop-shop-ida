import { useContext, useEffect, useRef, useState } from "react";
import Navbar from "./components/bars/Navbar";
import Sidebar from "./components/bars/Sidebar";
import { Outlet } from "react-router-dom";
import { BodySmall } from "./components/text-wrapers/TextWrapers";
import AlertContext from "./contexts/AlertContext";
import InfoIcon from '@mui/icons-material/Info';

function AppOverlay() {
	const { alertResponse } = useContext(AlertContext);
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
			{alertResponse &&
				<div className={`alert ${alertResponse.success ? "background-colour--success" : "background-colour--fail"}`}>
					<InfoIcon />
					<BodySmall>
						{alertResponse.statusText}
					</BodySmall>
				</div>}
            <Outlet />
        </>
    );
}

export default AppOverlay;
