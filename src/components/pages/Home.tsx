import CardContainer from "../tiles/CardContainer";
import "../../css/components/pages/home.css";
import { BodyNormal, HeadingLarge } from "../text-wrapers/TextWrapers";
import { CardProps } from "../tiles/Card";

function Home() {
  return (
    <div className="content">
      <div className="description">
        <HeadingLarge>Welcome to</HeadingLarge>
        <HeadingLarge>iDA</HeadingLarge>
        <HeadingLarge>One Stop Shop</HeadingLarge>
        <BodyNormal>Place where you</BodyNormal>
        <BodyNormal>have everything you need!</BodyNormal>
      </div>
      <CardContainer />
      <CardContainer />
      {/* <CardContainer cardProps={[{ linkAddress: "/office-details", title: "Test", description: "Test" }]}/> */}
    </div>
  );
}

export default Home;
