import { Link } from "react-router-dom";
import "../../css/sidebar.css";
import KeyboardArrowRightIcon from "@mui/icons-material/KeyboardArrowRight";
import CloseIcon from "@mui/icons-material/Close";
import { Dispatch, SetStateAction } from "react";

interface SidebarProps {
  onPressCloseSidebar: Dispatch<SetStateAction<boolean>>;
}

function Sidebar({ onPressCloseSidebar }: SidebarProps) {
  return (
    <>
      <nav id="sidebar">
        {/* <div className="sidebar__burger-menu heading--large">
        <MenuIcon fontSize="inherit" />
      </div> */}
        <div
          className="sidebar__burger-menu heading--large"
          onClick={() => onPressCloseSidebar(false)}
        >
          <CloseIcon fontSize="inherit" />
        </div>
        <div id="sidebar__options">
          <div id="sidebar__options__top" className="heading--small">
            <Link className="sidebar__option heading--small" to="/" onClick={() => onPressCloseSidebar(false)}>
              Home
              <div className="sidebar__arrow heading--large">
                <KeyboardArrowRightIcon fontSize="inherit" />
              </div>
            </Link>
            <Link className="sidebar__option heading--small" to="/employee-portal" onClick={() => onPressCloseSidebar(false)}>
              EmployeePortal
              <div className="sidebar__arrow heading--large">
                <KeyboardArrowRightIcon fontSize="inherit" />
              </div>
            </Link>
            <Link className="sidebar__option heading--small" to="/office-details" onClick={() => onPressCloseSidebar(false)}>
              Office Details
              <div className="sidebar__arrow heading--large">
                <KeyboardArrowRightIcon fontSize="inherit" />
              </div>
            </Link>
            <Link className="sidebar__option heading--small" to="/company101" onClick={() => onPressCloseSidebar(false)}>
              Company 101
              <div className="sidebar__arrow heading--large">
                <KeyboardArrowRightIcon fontSize="inherit" />
              </div>
            </Link>
            <Link className="sidebar__option heading--small" to="/personal-skills" onClick={() => onPressCloseSidebar(false)}>
              Personal Skills
              <div className="sidebar__arrow heading--large">
                <KeyboardArrowRightIcon fontSize="inherit" />
              </div>
            </Link>
            <Link className="sidebar__option heading--small" to="https://accounts.rydoo.com/login?signin=4bfbb925942188e56808788796a0fe72" target="_blank" onClick={() => onPressCloseSidebar(false)}>
              Expenses
              <div className="sidebar__arrow heading--large">
                <KeyboardArrowRightIcon fontSize="inherit" />
              </div>
            </Link>
          </div>
          <div id="sidebar__options__bottom">
            <Link className="sidebar__option heading--small" to="/offices" onClick={() => onPressCloseSidebar(false)}>
              Change Offices
              <div className="sidebar__arrow heading--large">
                <KeyboardArrowRightIcon fontSize="inherit" />
              </div>
            </Link>
            <Link className="sidebar__option heading--small" to="/settings" onClick={() => onPressCloseSidebar(false)}>
              Settings
              <div className="sidebar__arrow heading--large">
                <KeyboardArrowRightIcon fontSize="inherit" />
              </div>
            </Link>
          </div>
        </div>
      </nav>
      <div id="cover" onClick={() => onPressCloseSidebar(false)} />
    </>
  );
}

export default Sidebar;
