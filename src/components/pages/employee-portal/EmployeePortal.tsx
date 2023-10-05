import "../../../css/components/pages/employee-portal/employee-portal.css";
import { BodyNormal, HeadingLarge } from "../../text-wrapers/TextWrapers";
import Panel from "../../tiles/Panel";

function EmployeePortal() {
  return (
    <div className="content">
      <div className="description">
        <HeadingLarge>Employee Portal</HeadingLarge>
        <BodyNormal>Manage all work</BodyNormal>
        <BodyNormal>related tasks from one place!</BodyNormal>
      </div>
      <div className="content__panels">
        <Panel linkAddress="https://werknemer.loket.nl/#/login?returnUrl=%2Fstart" title="External" description="Werknemerloket Web Page"/>
        <Panel linkAddress="/employee-portal/sick-leave" title="Sick Leave" description="Register your absence"/>
        <Panel linkAddress="/employee-portal/vacation" title="Vacation" description="Plan your off days and see their balance"/>
        <Panel linkAddress="/employee-portal/scheduling" title="Scheduling" description="Plan out your wor schedule"/>
      </div>
    </div>
  );
}

export default EmployeePortal;
