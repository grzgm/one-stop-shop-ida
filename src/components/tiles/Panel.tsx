import personHappy from "../../assets/avatar-person-happy.svg";
import KeyboardArrowRightIcon from "@mui/icons-material/KeyboardArrowRight";
import "../../css/components/tiles/panel.css";

// interface PanelProps {
//   linkAddress: string;
// }

// function Panel({linkAddress}: PanelProps) {
function Panel() {
  return (
    <div className="panel">
      <div className="panel__text">
        <h1>Title</h1>
        <p>Description</p>
      </div>
      <div className="panel__icons">
        <img className="panel__icons__icon" src={personHappy} alt="iDA" />
        <div className="panel__icons__arrow">
          <KeyboardArrowRightIcon fontSize="inherit" />
        </div>
      </div>
    </div>
  );
}

export default Panel;
