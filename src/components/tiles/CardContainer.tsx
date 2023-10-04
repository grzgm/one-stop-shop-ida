import Card from "./Card";
import "../../css/components/tiles/card-container.css"

function CardContainer() {
    return (
        <div className="card-container">
            <div className="card-container__title">
                <h1>Category</h1>
            </div>
            <div className="card-container__cards">
                <Card />
                <Card />
                <Card />
                <Card />
            </div>
        </div>)
}

export default CardContainer;