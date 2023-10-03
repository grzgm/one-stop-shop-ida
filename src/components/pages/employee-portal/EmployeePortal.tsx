import { Link } from "react-router-dom";

function EmployeePortal() {
  return (
    <>
      <h1>Employee Portal</h1>
      <Link to="/employee-portal/sick-leave">Sick Leave</Link>
      <Link to="/employee-portal/vacation">Vacation</Link>
    </>
  );
}

export default EmployeePortal;
