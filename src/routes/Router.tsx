import { Route } from "react-router-dom";
import EmployeePortal from "../components/pages/employee-portal/EmployeePortal";
import Home from "../components/pages/Home";
import NotFound from "../components/pages/NotFound";
import Offices from "../components/pages/offices/Offices";
import SickLeave from "../components/pages/employee-portal/SickLeave";
import Vacation from "../components/pages/employee-portal/Vacation";
import OfficeDetails from "../components/pages/office-details/OfficeDetails";
import ReserveDesk, { ReserveDeskLoader } from "../components/pages/office-details/ReserveDesk";
import Lunch, { LunchLoader } from "../components/pages/office-details/Lunch";
import OfficeInformation from "../components/pages/office-details/OfficeInformation";
import Presence, { PresenceLoader } from "../components/pages/office-details/Presence";
import Company101 from "../components/pages/Company101";
import PersonalSkills from "../components/pages/PersonalSkills";
import Expenses from "../components/pages/Expenses";
import Settings from "../components/pages/Settings";
// import ReserveDeskOverview from "../components/pages/office-details/ReserveDeskOverview";
import Scheduling from "../components/pages/employee-portal/Scheduling";
import AppOverlay, { AppLoader } from "../AppOverlay";
import PopupLogin from "../components/pages/auth-pages/PopupLogin";
import AuthPage from "../components/pages/auth-pages/AuthPage";
import { AuthUrl as MicrosoftAuthUrl, IsAuth as MicrosoftIsAuth } from "../api/MicrosoftGraphAPI";
import { AuthUrl as SlackAuthUrl, IsAuth as SlackIsAuth } from "../api/SlackAPI";
import { IOfficeFeatures } from "../api/OfficeFeaturesAPI";

function Router(currentOfficeFeatures: IOfficeFeatures) {

  return (
    <>
      {/* <Route path="/" element={<AppOverlay />} loader={async () => await AppLoader()}> */}
      <Route path="/" element={<AppOverlay />}>
        <Route index element={<Home />} />
        <Route path="/employee-portal">
          <Route index element={<EmployeePortal />} />
          <Route path="sick-leave" element={<SickLeave />} />
          <Route path="vacation" element={<Vacation />} />
          <Route path="scheduling" element={<Scheduling />} />
        </Route>
        <Route path="/office-details">
          <Route index element={<OfficeDetails />} />
          {/* <Route path="reserve-desk" element={<ReserveDesk />} loader={async () => await ReserveDeskLoader(currentOfficeFeatures)} /> */}
          <Route path="reserve-desk" element={<ReserveDesk />} />
          {/* <Route path="reserve-desk-overview" element={<ReserveDeskOverview />} /> */}
          <Route path="lunch" element={<Lunch />} loader={async () => await LunchLoader(currentOfficeFeatures)} />
          <Route path="office-information" element={<OfficeInformation />} />
          <Route path="presence" element={<Presence />} loader={() => PresenceLoader(currentOfficeFeatures)} />
        </Route>
        <Route path="/company101" element={<Company101 />} />
        <Route path="/personal-skills" element={<PersonalSkills />} />
        <Route path="/expenses" element={<Expenses />} />
        <Route path="/offices" element={<Offices />} />
        <Route path="/settings" element={<Settings />} />
        <Route path="/slack-auth" element={<AuthPage authTarget="slack" isAuth={SlackIsAuth} authUrl={SlackAuthUrl} />} />
        <Route path="*" element={<NotFound />} />
      </Route>
      <Route path="/microsoft-auth" element={<AuthPage authTarget="microsoft" isAuth={MicrosoftIsAuth} authUrl={MicrosoftAuthUrl} />} />
      <Route path="/popup-login" element={<PopupLogin />} />
    </>
  );
}

export default Router;
