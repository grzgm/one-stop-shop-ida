import personHappy from "../../assets/avatar-person-happy.svg";
import KeyboardArrowRightIcon from "@mui/icons-material/KeyboardArrowRight";
import "../../css/components/tiles/panel.css";
import { Link } from "react-router-dom";

interface PanelProps {
  linkAddress: string;
  title?: string;
  description?: string;
}

function Panel({linkAddress, title = "Title", description = "Description"}: PanelProps) {
  return (
    <Link className="panel" to={linkAddress}>
      <div className="panel__text">
        <h3>{title}</h3>
        <p className="body--small">{description}</p>
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
