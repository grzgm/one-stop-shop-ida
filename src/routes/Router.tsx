import { Routes, Route } from "react-router-dom";
import EmployeePortal from "../components/pages/employee-portal/EmployeePortal";
import Home from "../components/pages/Home";
import NotFound from "../components/pages/NotFound";
import Utrecht from "../components/pages/offices/Utrecht";
import Eindhoven from "../components/pages/offices/Eindhoven";
import Amsterdam from "../components/pages/offices/Amsterdam";
import OfficesLayout from "../components/pages/offices/OfficesLayout";
import Offices from "../components/pages/offices/Offices";

function Router() {
  return (
    <main>
      <Routes>
        <Route path="/" element={<Home />} />
        <Route path="/employeeportal" element={<EmployeePortal />} />
        <Route path="/offices" element={<OfficesLayout />}>
          <Route index element={<Offices />} />
          <Route path="utrecht" element={<Utrecht />} />
          <Route path="eindhoven" element={<Eindhoven />} />
          <Route path="amsterdam" element={<Amsterdam />} />
        </Route>
        <Route path="*" element={<NotFound />} />
      </Routes>
    </main>
  );
}

export default Router;
