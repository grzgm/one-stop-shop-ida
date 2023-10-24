import { useSearchParams } from 'react-router-dom';
import Button from "../Buttons";
import { BodyNormal, BodySmall, HeadingLarge } from "../text-wrapers/TextWrapers";
import { InspectResponseSync } from '../../api/Response';

function SlackAuth() {
	// Get the search parameters from the URL
	const [searchParams] = useSearchParams();
  
	// Access specific query parameters
	const queryPreviousLocation = searchParams.get('previousLocation');
	const previousLocation = queryPreviousLocation ? queryPreviousLocation : "/slack-auth";
	// Access server response
	const queryServerResponse = searchParams.get('serverResponse');
	console.log("queryServerResponse: ", queryServerResponse)
	const serverResponse = queryServerResponse ? JSON.parse(queryServerResponse) : queryServerResponse;
	
	return (
		<div className="content">
			<div className="description">
				<HeadingLarge>Login with your</HeadingLarge>
				<HeadingLarge>Slack Account</HeadingLarge>
				<BodyNormal>Get access to all the benefits of app!</BodyNormal>
			</div>
			<main className="slack-auth-main">
				{serverResponse 
				? <BodySmall additionalClasses={[InspectResponseSync(serverResponse).success ? "font-colour--success" : "font-colour--fail"]}>{`${InspectResponseSync(serverResponse).status} Try again later.`}</BodySmall>
				: <Button child="Log in" onClick={() => window.location.href = `http://localhost:3002/slack/auth?route=${encodeURI(previousLocation)}`} />}
			</main>
		</div>
	);
}

export default SlackAuth;
