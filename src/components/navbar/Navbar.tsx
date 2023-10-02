import { Link } from "react-router-dom";

function Navbar() {
  return (
    <nav>
      <ul>
        <li>
          <Link to="/">Home</Link>
        </li>
        <li>
          <Link to="/employeeportal">EmployeePortal</Link>
        </li>
        <li>
          <Link to="/offices">Offices</Link>
        </li>
      </ul>
    </nav>
  );
}

export default Navbar;
