import Button from "../Buttons";
import { BodyNormal, HeadingLarge, HeadingSmall } from "../text-wrapers/TextWrapers";

function Company101() {

	return (
		<div className="content">
			<div className="description">
				<HeadingLarge>Company 101</HeadingLarge>
				<BodyNormal>All you need to know when starting</BodyNormal>
			</div>
			<main>
				<Button child="Initiation Course" onClick={() => window.open(`https://xploregroup.atlassian.net/wiki/spaces/IDANL/pages/8255046389/iDA+NL+employee+orientation+course`)} />
				<section>
					<HeadingSmall>Get you key card</HeadingSmall>
					<BodyNormal>How to get your key card</BodyNormal>
				</section>
			</main>
		</div>
	);
}

export default Company101;
