import Card from "./Card";
import "../../css/components/tiles/card-container.css";

function CardContainer({ amountOfChildren = 4 }) {
  const cards = [];
  for (let i = 0; i < amountOfChildren; i++) {
    cards.push(<Card key={i} />);
  }

  return (
    <div className="card-container">
      <div className="card-container__title">
        <h1>Category</h1>
      </div>
      <div className="card-container__cards">{cards}</div>
    </div>
  );
}

export default CardContainer;
