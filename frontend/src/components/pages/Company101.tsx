import Button from "../Buttons";
import { BodyNormal, HeadingLarge } from "../text-wrapers/TextWrapers";

function Company101() {

	return (
		<div className="content">
			<div className="description">
				<HeadingLarge>Company 101</HeadingLarge>
				<BodyNormal>All you need to know when starting</BodyNormal>
			</div>
			<main>
				<Button child="Initiation Course" onClick={() => window.open(`https://xploregroup.atlassian.net/wiki/spaces/IDANL/pages/8255046389/iDA+NL+employee+orientation+course`)} />
			</main>
		</div>
	);
}

export default Company101;
