import { useSearchParams } from 'react-router-dom';
import Button from "../Buttons";
import { BodyNormal, BodySmall, HeadingLarge } from "../text-wrapers/TextWrapers";
import { InspectResponseSync } from '../../api/Response';
import { useNavigate } from "react-router-dom";
import { IsAuth } from '../../api/MicrosoftGraphAPI';
import { useEffect } from 'react';

function MicrosoftAuth() {
	const navigate = useNavigate();

	useEffect(() => {
		const IsAuthWrapper = async () => {
			if((await IsAuth()).payload)
			{
				navigate("/")
			}
		};
		IsAuthWrapper();
	}, []);

	// Get the search parameters from the URL
	const [searchParams] = useSearchParams();
	
	// Access specific query parameters
	const queryPreviousLocation = searchParams.get('previousLocation');
	const previousLocation = queryPreviousLocation ? queryPreviousLocation : "/microsoft-auth";
	// Access server response
	const queryServerResponse = searchParams.get('serverResponse');
	console.log("queryServerResponse: ", queryServerResponse)
	const serverResponse = queryServerResponse ? JSON.parse(queryServerResponse) : queryServerResponse;

	return (
		<div className="content">
			<div className="description">
				<HeadingLarge>Login with your</HeadingLarge>
				<HeadingLarge>Microsoft Account</HeadingLarge>
				<BodyNormal>Get access to all the benefits of app!</BodyNormal>
			</div>
			<main className="microsoft-auth-main">
				{serverResponse &&
					<BodySmall additionalClasses={[InspectResponseSync(serverResponse).success ? "font-colour--success" : "font-colour--fail"]}>{InspectResponseSync(serverResponse).status}</BodySmall>}
				<Button child="Log in" onClick={() => window.location.href = `http://localhost:3002/microsoft/auth?route=${encodeURI(previousLocation)}`} />
			</main>
		</div>
	);
}

export default MicrosoftAuth;
