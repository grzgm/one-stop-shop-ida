import personHappy from "../../assets/avatar-person-happy.svg";
import KeyboardArrowRightIcon from "@mui/icons-material/KeyboardArrowRight";
import "../../css/components/tiles/card.css";
import { BodySmall, HeadingSmall } from "../text-wrapers/TextWrapers";
import { Link } from "react-router-dom";

export interface CardProps {
  linkAddress?: string;
  title?: string;
  description?: string;
  isOpenInNewTab?: boolean;
  onClick?: () => void;
}

function Card({ linkAddress = "/", title = "Title", description = "Description", isOpenInNewTab = false}: CardProps) {
  const linkProps = isOpenInNewTab ? { to: linkAddress, target: "_blank", rel: "noopener noreferrer" } : { to: linkAddress };
  return (
    <Link className="card" {...linkProps}>
      <div className="card__text">
        <div className="card__text__title">
          <img className="card__text__icon" src={personHappy} alt="iDA" />
          <HeadingSmall>{title}</HeadingSmall>
        </div>
        <BodySmall>{description}</BodySmall>
      </div>
      <div className="card__arrow">
        <KeyboardArrowRightIcon fontSize="inherit" />
      </div>
    </Link>
  );
}

export default Card;