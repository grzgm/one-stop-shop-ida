import OfficeSpace from "../../OfficeSpace";
import { BodyNormal, HeadingLarge } from "../../text-wrapers/TextWrapers";
import "../../../css/components/pages/office-details/reserve-desk.css"

function ReserveDesk() {
    return (
      <div className="content">
        <div className="description">
          <HeadingLarge>Reserve a Desk</HeadingLarge>
          <BodyNormal>Choose time slot</BodyNormal>
        </div>
        <main className="reserve-desk-main">
          <OfficeSpace/>
        </main>
      </div>
    );
  }
  
  export default ReserveDesk;
  