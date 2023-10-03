import { Link } from "react-router-dom";

function Navbar() {
  return (
    <nav>
      <ul>
        <li>
          <Link to="/">Home</Link>
        </li>
        <li>
          <Link to="/employee-portal">EmployeePortal</Link>
        </li>
        <li>
          <Link to="/office-details">Office Details</Link>
        </li>
        <li>
          <Link to="/company101">Company 101</Link>
        </li>
        <li>
          <Link to="/personal-skills">Personal Skills</Link>
        </li>
        <li>
          <Link to="/expenses">Expenses</Link>
        </li>
        <li>
          <Link to="/offices">Offices</Link>
        </li>
        <li>
          <Link to="/settings">Settings</Link>
        </li>
      </ul>
    </nav>
  );
}

export default Navbar;
