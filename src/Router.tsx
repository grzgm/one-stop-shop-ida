import { Route } from "react-router-dom";
import Welcome from "./sections/pages/Welcome";
import Home, { HomeLoader } from "./sections/pages/Home";
import NotFound from "./sections/pages/NotFound";
import Utrecht from "./sections/pages/offices/Utrecht";
import Eindhoven from "./sections/pages/offices/Eindhoven";
import Amsterdam from "./sections/pages/offices/Amsterdam";
import OfficesLayout from "./sections/pages/offices/OfficesLayout";
import Offices from "./sections/pages/offices/Offices";
import Navbar from "./sections/navbar/Navbar";

function Router() {
  return (
    <>
      <Route path="/" element={<Navbar />} >
        <Route index element={<Home />} loader={async () => HomeLoader()}/>
        <Route path="/welcome" element={<Welcome />} />
        <Route path="/offices" element={<OfficesLayout />}>
          <Route index element={<Offices />} />
          <Route path="utrecht" element={<Utrecht />} />
          <Route path="eindhoven" element={<Eindhoven />} />
          <Route path="amsterdam" element={<Amsterdam />} />
        </Route>
        <Route path="*" element={<NotFound />} />
      </Route>
    </>
  );
}

export default Router;
