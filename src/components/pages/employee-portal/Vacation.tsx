import "../../../css/components/pages/employee-portal/vacation.css"
import Calendar from "./Calendar";
import { BodyNormal, HeadingLarge } from "../../text-wrapers/TextWrapers";

function Vacation() {
      return (
        <div className="content">
          <div className="description">
            <HeadingLarge>Plan your Vacation</HeadingLarge>
            <BodyNormal>Choose time slot</BodyNormal>
          </div>
          <main className="vacation-main">
            <Calendar/>
          </main>
        </div>
      );
}

export default Vacation;
