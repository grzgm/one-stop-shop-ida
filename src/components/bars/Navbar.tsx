import { Link } from "react-router-dom";
import idaLogo from "../../assets/ida-logo.svg";
import "../../css/navbar.css";
import MenuIcon from '@mui/icons-material/Menu';
import { Dispatch, SetStateAction } from "react";

interface NavbarProps {
  onPressOpenSidebar: Dispatch<SetStateAction<boolean>>;
  navbarOptionsRef: React.RefObject<HTMLDivElement>;
}

function Navbar({ onPressOpenSidebar, navbarOptionsRef }: NavbarProps) {
  return (
    <nav id="navbar">
      <Link to="/"><img id="navbar__logo" src={idaLogo} alt="iDA" /></Link>
      <div id="navbar__options" ref={navbarOptionsRef}>
        <div id="navbar__options__left" className="heading--small">
          <Link className="navbar__option heading--small" to="/">Home</Link>
          <Link className="navbar__option heading--small" to="/employee-portal">EmployeePortal</Link>
          <Link className="navbar__option heading--small" to="/office-details">Office Details</Link>
          <Link className="navbar__option heading--small" to="/company101">Company 101</Link>
          <Link className="navbar__option heading--small" to="/personal-skills">Personal Skills</Link>
          <Link className="navbar__option heading--small" to="/expenses">Expenses</Link>
        </div>
        <div className="navbar__burger-menu" onClick={() => onPressOpenSidebar(true)}><MenuIcon fontSize="inherit" /></div>
        <div id="navbar__separator" />
        <div id="navbar__options__right">
          <Link className="navbar__option heading--small" to="/offices">Change Offices</Link>
          <Link className="navbar__option heading--small" to="/settings">Settings</Link>
        </div>
      </div>
    </nav>
  );
}

export default Navbar;