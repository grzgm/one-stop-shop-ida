import Card, { CardProps } from "./Card";
import "../../css/components/tiles/card-container.css";
import { HeadingLarge } from "../text-wrapers/TextWrapers";

interface CardContainerProps {
  cardProps?: CardProps[];
}

function CardContainer({ cardProps = [] }: CardContainerProps) {
  const cards = [];
  console.log(cardProps)
  if (cardProps.length < 1) {
    for (let i = 0; i < 4; i++) {
      cards.push(<Card key={i} />);
    }
  }
  else {
    for (let i = 0; i < cardProps.length; i++) {
      cards.push(<Card key={i} linkAddress={cardProps[i].linkAddress} title={cardProps[i].title} description={cardProps[i].description}/>);
    }
  }

  return (
    <div className="card-container">
      <div className="card-container__title">
        <HeadingLarge>Category</HeadingLarge>
      </div>
      <div className="card-container__cards">
        {cards}
      </div>
    </div>
  );
}

export default CardContainer;
