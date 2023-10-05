import "../../../css/components/pages/employee-portal/employee-portal.css";
import Panel from "../../tiles/Panel";

function EmployeePortal() {
  return (
    <div className="content">
      <div className="description">
        <h1>Employee Portal</h1>
        <p>Manage all work</p>
        <p>related tasks from one place!</p>
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
