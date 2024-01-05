import { useNavigate, useSearchParams } from 'react-router-dom';
import Button from "../../Buttons";
import { BodyNormal, BodySmall, HeadingLarge } from "../../text-wrapers/TextWrapers";
import { InspectResponseSync } from '../../../api/Response';
import { IsAuth, SendMessage, SetStatus } from '../../../api/SlackAPI';
import { useEffect } from 'react';
import "../../../css/auth-pages.css"

function SlackAuth() {
	const navigate = useNavigate();

	useEffect(() => {
		const IsAuthWrapper = async () => {
			if ((await IsAuth()).payload) {
				navigate("/")
			}
		};
		IsAuthWrapper();
	}, []);

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
			<main className="auth-main">
				{serverResponse &&
					<BodySmall additionalClasses={[InspectResponseSync(serverResponse).success ? "font-colour--success" : "font-colour--fail"]}>{InspectResponseSync(serverResponse).statusText}</BodySmall>}
				<Button child="Log in" onClick={() => window.location.href = `${import.meta.env.VITE_BACKEND_URI}/slack/auth?route=${encodeURI(previousLocation)}`} />
			</main>
			<Button child="send message" onClick={() => SendMessage("new message", "D05QWNGJMAR")} />
			<Button child="set status" onClick={() => SetStatus("ReAcT ApP", ":v:")} />
			<Button child="is auth" onClick={() => console.log(IsAuth())} />
		</div>
	);
}

export default SlackAuth;
