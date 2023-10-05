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
        <Panel />
        <Panel />
        <Panel />
        <Panel />
        <Panel />
        <Panel />
        <Panel />
      </div>
    </div>
  );
}

export default EmployeePortal;
