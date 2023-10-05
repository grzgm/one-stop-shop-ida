import personHappy from "../../assets/avatar-person-happy.svg";
import KeyboardArrowRightIcon from "@mui/icons-material/KeyboardArrowRight";
import "../../css/components/tiles/card.css";
import { BodySmall, HeadingSmall } from "../text-wrapers/TextWrapers";

function Card() {
  return (
    <div className="card">
      <div className="card__text">
        <div className="card__text__title">
          <img className="card__text__icon" src={personHappy} alt="iDA" />
          <HeadingSmall>Title</HeadingSmall>
        </div>
        <BodySmall>Description</BodySmall>
      </div>
      <div className="card__arrow">
        <KeyboardArrowRightIcon fontSize="inherit" />
      </div>
    </div>
  );
}

export default Card;
