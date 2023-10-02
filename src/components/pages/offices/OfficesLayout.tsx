import { Link, Outlet } from "react-router-dom";

function OfficesLayout() {
  return (
    <>
      <ol>
        <li>
          <Link to="/offices/utrecht">Utrecht</Link>
        </li>
        <li>
          <Link to="/offices/eindhoven">Eindhoven</Link>
        </li>
        <li>
          <Link to="/offices/amsterdam">Amsterdam</Link>
        </li>
      </ol>
      <Outlet />
    </>
  );
}

export default OfficesLayout;
