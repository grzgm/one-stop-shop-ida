import { useSearchParams } from 'react-router-dom';
import Button from "../Buttons";
import { BodyNormal, HeadingLarge } from "../text-wrapers/TextWrapers";

function MicrosoftAuth() {
	// Get the search parameters from the URL
	const [searchParams] = useSearchParams();
  
	// Access specific query parameters
	const queryPreviousLocation = searchParams.get('previousLocation');
	const previousLocation = queryPreviousLocation ? queryPreviousLocation : "";
	
	return (
		<div className="content">
			<div className="description">
				<HeadingLarge>Login with your</HeadingLarge>
				<HeadingLarge>Microsoft Account</HeadingLarge>
				<BodyNormal>Get access to all the benefits of app!</BodyNormal>
			</div>
			<main className="microsoft-auth-main">
				<Button child="Log in" onClick={() => window.location.href = `http://localhost:3002/microsoft/auth?route=${encodeURI(previousLocation)}`} />
			</main>
		</div>
	);
}

export default MicrosoftAuth;
