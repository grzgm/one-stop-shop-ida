import { Link, Outlet } from "react-router-dom";

function Navbar() {
  return (
    <>
      <nav>
        <ul>
          <li>
            <Link to="/">Home</Link>
          </li>
          <li>
            <Link to="/welcome">Welcome</Link>
          </li>
          <li>
            <Link to="/offices">Offices</Link>
          </li>
        </ul>
      </nav>
      <Outlet />
    </>
  );
}

export default Navbar;
