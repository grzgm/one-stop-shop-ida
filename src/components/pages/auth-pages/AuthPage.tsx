import { useSearchParams } from 'react-router-dom';
import Button from "../../Buttons";
import { BodyNormal, BodySmall, HeadingLarge } from "../../text-wrapers/TextWrapers";
import { useNavigate } from "react-router-dom";
import { useEffect, useState } from 'react';
import "../../../css/auth-pages.css"
import { IActionResult } from '../../../api/Response';
import { Auth as AuthJWT, IsAuth as IsAuthJWT } from '../../../api/AuthenticationAPI';

export interface AuthPageProps {
    authTarget: string,
    isAuth: () =>  Promise<IActionResult<boolean>>,
    authUrl: () => Promise<IActionResult<string>>,
}

function AuthPage({ authTarget, isAuth: IsAuth, authUrl: AuthUrl }: AuthPageProps) {
    authTarget = authTarget.toLowerCase()
    const [popupLogin, setPopupLogin] = useState<Window | null>();
    const [isAuth, setIsAuth] = useState<boolean>();
    const [url, setUrl] = useState<string>("");
    const navigate = useNavigate();

    useEffect(() => {
        // Auth target config
        const IsAuthWrapper = async () => {
            if ((await IsAuth()).payload) {
                navigate("/")
            }
        };
        IsAuthWrapper();

        // Client JWT config
        const getURL = async () => {
            const res = await AuthUrl()
            if (res.success)
                setUrl(res.statusText)
        }

        const UrlWrapper = async () => {
            if ((await IsAuthJWT()).payload)
            {
                await getURL();
            }
            else{
                const res = await AuthJWT()
                if(res.success)
                    await getURL()
            }
        }

        UrlWrapper()
    }, [authTarget]);

    // Pop up config
    useEffect(() => {
        // Check if the popup is closed at regular intervals
        const checkPopupClosed = setInterval(async () => {
            if (popupLogin?.closed) {
                // if popup is closed check if the user is loged in
                if ((await IsAuth()).payload) {
                    // if loged in navigate to adequat website
                    navigate(previousLocation)
                    setIsAuth(true)
                }
                else {
                    setIsAuth(false)
                }
                clearInterval(checkPopupClosed);
            }
        }, 1000);

        // Cleanup the interval when the component is unmounted
        return () => clearInterval(checkPopupClosed);
    }, [popupLogin]);

    const openPopup = () => {
        if (url)
            setPopupLogin(window.open(url, '_blank', 'width=500,height=600'));
    };

    // Get the search parameters from the URL
    const [searchParams] = useSearchParams();

    // Access specific query parameters
    const queryPreviousLocation = searchParams.get('previousLocation');
    const previousLocation = queryPreviousLocation ? queryPreviousLocation : `/${authTarget}-auth`;

    return (
        <div className="content">
            <div className="description">
                <HeadingLarge>Login with your</HeadingLarge>
                <HeadingLarge>{authTarget.charAt(0).toUpperCase() + authTarget.slice(1)} Account</HeadingLarge>
                <BodyNormal>Get access to all the benefits of app!</BodyNormal>
            </div>
            <main className="auth-main">
                {url && <Button child="Log in" onClick={() => openPopup()} />}
                {isAuth == false &&
                    <BodySmall additionalClasses={["font-colour--fail"]}>Cannot Authenticate the User</BodySmall>}
            </main>
        </div>
    );
}

export default AuthPage;
