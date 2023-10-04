import personHappy from "../../assets/avatar-person-happy.svg"
import KeyboardArrowRightIcon from "@mui/icons-material/KeyboardArrowRight";
import "../../css/components/tiles/card.css"

function Card() {
    return (
        <div className="card">
            <div className="card__text">
                <div className="card__text__title">
                    <img className="card__text__icon" src={personHappy} alt="iDA" />
                    <h1>Title</h1>
                </div>
                Description
            </div>
            <div className="card__arrow heading--large">
                <KeyboardArrowRightIcon fontSize="inherit" />
            </div>
        </div>)
}

export default Card;