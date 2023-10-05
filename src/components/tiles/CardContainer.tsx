import Card from "./Card";
import "../../css/components/tiles/card-container.css";
import { HeadingLarge } from "../text-wrapers/TextWrapers";

function CardContainer({ amountOfChildren = 4 }) {
  const cards = [];
  for (let i = 0; i < amountOfChildren; i++) {
    cards.push(<Card key={i} />);
  }

  return (
    <div className="card-container">
      <div className="card-container__title">
        <HeadingLarge>Category</HeadingLarge>
      </div>
      <div className="card-container__cards">{cards}</div>
    </div>
  );
}

export default CardContainer;
