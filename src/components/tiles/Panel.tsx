import personHappy from "../../assets/avatar-person-happy.svg";
import KeyboardArrowRightIcon from "@mui/icons-material/KeyboardArrowRight";
import "../../css/components/tiles/panel.css";
import { Link } from "react-router-dom";
import { BodySmall, HeadingSmall } from "../text-wrapers/TextWrapers";

interface PanelProps {
  linkAddress: string;
  title?: string;
  description?: string;
  isOpenInNewTab?: boolean;
  onClick?: () => void;
}

function Panel({ linkAddress, title = "Title", description = "Description", isOpenInNewTab = false, onClick = () => { } }: PanelProps) {
  const linkProps = isOpenInNewTab ? { to: linkAddress, target: "_blank", rel: "noopener noreferrer" } : { to: linkAddress };
  return (
    <Link className="panel" {...linkProps} onClick={() => onClick()}>
      <div className="panel__text">
        <HeadingSmall>{title}</HeadingSmall>
        <BodySmall>{description}</BodySmall>
      </div>
      <div className="panel__icons">
        <img className="panel__icons__icon" src={personHappy} alt="iDA" />
        <div className="panel__icons__arrow">
          <KeyboardArrowRightIcon fontSize="inherit" />
        </div>
      </div>
    </Link>
  );
}

export default Panel;
