import { useSearchParams } from 'react-router-dom';
import { BodyNormal, BodySmall, HeadingLarge } from "../../text-wrapers/TextWrapers";
import { InspectResponseSync } from '../../../api/Response';
import { useEffect, useState } from 'react';
import "../../../css/auth-pages.css"

function PopupLogin() {
    const [countDown, setCountDown] = useState(5);

    useEffect(() => {
        const interval = setInterval(() => {
            if (countDown > 0) {
                setCountDown(countDown - 1);
            }
        }, 1000);

        if (countDown <= 0) {
            clearInterval(interval)
        }

        return () => clearInterval(interval);
    }, [countDown]);

    useEffect(() => {
        const ShouldCloseWindow = async () => {
            if (InspectResponseSync(serverResponse).success) {
                setTimeout(() => window.close(), 5e3)
            }
        };
        ShouldCloseWindow();
    }, []);

    // Get the search parameters from the URL
    const [searchParams] = useSearchParams();

    // Access server response
    const queryServerResponse = searchParams.get('serverResponse');
    const serverResponse = queryServerResponse && isValidJSON(queryServerResponse) ? JSON.parse(queryServerResponse) : queryServerResponse;

    return (
        <div className="popup__content">
            <div className="description">
                <HeadingLarge>Microsoft Account</HeadingLarge>
                {serverResponse && InspectResponseSync(serverResponse).success ?
                    <>
                        <BodyNormal additionalClasses={["font-colour--success"]}>You have been loged in</BodyNormal>
                        <BodyNormal additionalClasses={["font-colour--success"]}>You can close the window or it will close in {countDown.toString()}s</BodyNormal>
                    </>
                    :
                    <BodyNormal additionalClasses={["font-colour--fail"]}>We couldn't log you in</BodyNormal>}
            </div>
            <main>
                {serverResponse &&
                    <BodySmall additionalClasses={[InspectResponseSync(serverResponse).success ? "font-colour--success" : "font-colour--fail"]}>{InspectResponseSync(serverResponse).statusText}</BodySmall>}
            </main>
        </div>
    );
}

function isValidJSON(str: string) {
    try {
        JSON.parse(str);
        return true;
    } catch (e) {
        return false;
    }
}

export default PopupLogin;
