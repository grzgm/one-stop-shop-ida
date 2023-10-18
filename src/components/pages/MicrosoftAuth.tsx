import Button from "../Buttons";
import { BodyNormal, HeadingLarge } from "../text-wrapers/TextWrapers";

function MicrosoftAuth() {
	return (
		<div className="content">
			<div className="description">
				<HeadingLarge>Login with your</HeadingLarge>
				<HeadingLarge>Microsoft Account</HeadingLarge>
				<BodyNormal>Get access to all the benefits of app!</BodyNormal>
			</div>
			<main className="microsoft-auth-main">
				<Button child="Log in" onClick={() => (console.log())} />
			</main>
		</div>
	);
}

export default MicrosoftAuth;
