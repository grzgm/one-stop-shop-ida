import CardContainer from "../tiles/CardContainer";
import "../../css/components/pages/home.css"

function Home() {
  return (
    <div className="content">
      <div className="description">
        <h1>Welcome to</h1>
        <h1>iDA</h1>
        <h1>One Stop Shop</h1>
        <p>Place where you</p>
        <p>have everything you need!</p>
      </div>
      <CardContainer />
      <CardContainer />
    </div>
  );
}

export default Home;
