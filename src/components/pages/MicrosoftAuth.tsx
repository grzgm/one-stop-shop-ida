import { useSearchParams } from 'react-router-dom';
import Button from "../Buttons";
import { BodyNormal, BodySmall, HeadingLarge } from "../text-wrapers/TextWrapers";
import { InspectResponse } from '../../api/microsoft-graph-api/MicrosoftGraphAPI';

function MicrosoftAuth() {
	// Get the search parameters from the URL
	const [searchParams] = useSearchParams();
  
	// Access specific query parameters
	const queryPreviousLocation = searchParams.get('previousLocation');
	const previousLocation = queryPreviousLocation ? queryPreviousLocation : "";
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
				{serverResponse 
				? <BodySmall additionalClasses={[InspectResponse(serverResponse).success ? "font-colour--success" : "font-colour--fail"]}>{`${InspectResponse(serverResponse).status} Try again later.`}</BodySmall>
				: <Button child="Log in" onClick={() => window.location.href = `http://localhost:3002/microsoft/auth?route=${encodeURI(previousLocation)}`} />}
			</main>
		</div>
	);
}

export default MicrosoftAuth;
