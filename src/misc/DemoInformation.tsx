import ArrowBackIcon from '@mui/icons-material/ArrowBack';
import { useEffect, useState } from "react";
import "../css/misc.css";

function DemoInformation() {
    const [isMouseOverInformation, setIsMouseOverInformation] = useState(true);

    useEffect(() => {
        const timer = setTimeout(() => {
            setIsMouseOverInformation(false);
        }, 2500);

        return () => clearTimeout(timer);
    }, []);

    return (
        <div
            onClick={() => { window.location.href = import.meta.env.VITE_PORTFOLIO_URL }}
            onMouseEnter={() => { setIsMouseOverInformation(true) }}
            onMouseLeave={() => { setIsMouseOverInformation(false) }}
            id="demo-information"
            style={{ "width": `${!isMouseOverInformation ? "4rem" : "10rem"}` }}>
            <ArrowBackIcon />
            {isMouseOverInformation && <div id="demo-information__text" >Go back to Portfolio.</div>}
        </div>
    );
}

export default DemoInformation;