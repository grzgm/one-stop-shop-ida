import { Link } from "react-router-dom";
import idaLogo from "../../assets/ida-logo.svg";
import "../../css/navbar.css";
import MenuIcon from '@mui/icons-material/Menu';

function Navbar() {
  return (
    <nav>
      <img id="nav__logo" src={idaLogo} alt="iDA" />
      <div id="nav__options">
        <div id="nav__options__left" className="heading--small">
          <Link className="nav__option heading--small" to="/">Home</Link>
          <Link className="nav__option heading--small" to="/employee-portal">EmployeePortal</Link>
          <Link className="nav__option heading--small" to="/office-details">Office Details</Link>
          <Link className="nav__option heading--small" to="/company101">Company 101</Link>
          <Link className="nav__option heading--small" to="/personal-skills">Personal Skills</Link>
          <Link className="nav__option heading--small" to="/expenses">Expenses</Link>
        </div>
        <div className="nav__burger-menu"><MenuIcon fontSize="inherit"/></div>
        <div id="nav__separator" />
        <div id="nav__options__right">
          <Link className="nav__option heading--small" to="/offices">Change Offices</Link>
          <Link className="nav__option heading--small" to="/settings">Settings</Link>
        </div>
      </div>
    </nav>
  );
}

export default Navbar;