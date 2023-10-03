import { Routes, Route } from "react-router-dom";
import EmployeePortal from "../components/pages/employee-portal/EmployeePortal";
import Home from "../components/pages/Home";
import NotFound from "../components/pages/NotFound";
import Utrecht from "../components/pages/offices/Utrecht";
import Eindhoven from "../components/pages/offices/Eindhoven";
import Amsterdam from "../components/pages/offices/Amsterdam";
import OfficesLayout from "../components/pages/offices/OfficesLayout";
import Offices from "../components/pages/offices/Offices";
import SickLeave from "../components/pages/employee-portal/SickLeave";
import Vacation from "../components/pages/employee-portal/Vacation";
import OfficeDetails from "../components/pages/office-details/OfficeDetails";
import ReserveDesk from "../components/pages/office-details/ReserveDesk";
import Lunch from "../components/pages/office-details/Lunch";
import OfficeInformation from "../components/pages/office-details/OfficeInformation";
import Presence from "../components/pages/office-details/Presence";
import Company101 from "../components/pages/Company101";
import PersonalSkills from "../components/pages/PersonalSkills";
import Expenses from "../components/pages/Expenses";
import Settings from "../components/pages/Settings";
import ReserveDeskOverview from "../components/pages/office-details/ReserveDeskOverview";

function Router() {
  return (
    <main>
      <Routes>
        <Route path="/" element={<Home />} />
        <Route path="/employee-portal">
          <Route index element={<EmployeePortal />} />
          <Route path="sick-leave" element={<SickLeave />} />
          <Route path="vacation" element={<Vacation />} />
        </Route>
        <Route path="/office-details">
          <Route index element={<OfficeDetails />} />
          <Route path="reserve-desk" element={<ReserveDesk />} />
          <Route path="reserve-desk-overview" element={<ReserveDeskOverview />} />
          <Route path="lunch" element={<Lunch />} />
          <Route path="office-information" element={<OfficeInformation />} />
          <Route path="presence" element={<Presence />} />
        </Route>
        <Route path="/company101" element={<Company101 />} />
        <Route path="/personal-skills" element={<PersonalSkills />} />
        <Route path="/expenses" element={<Expenses />} />
        <Route path="/offices" element={<OfficesLayout />}>
          <Route index element={<Offices />} />
          <Route path="utrecht" element={<Utrecht />} />
          <Route path="eindhoven" element={<Eindhoven />} />
          <Route path="amsterdam" element={<Amsterdam />} />
        </Route>
        <Route path="/settings" element={<Settings />} />
        <Route path="*" element={<NotFound />} />
      </Routes>
    </main>
  );
}

export default Router;
