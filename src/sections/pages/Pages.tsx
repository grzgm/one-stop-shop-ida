import { Routes, Route } from "react-router-dom";
import Welcome from "./Welcome";
import Home from "./Home";
import NotFound from "./NotFound";
import Utrecht from "./offices/Utrecht";
import Eindhoven from "./offices/eindhoven";
import Amsterdam from "./offices/Amsterdam";
import OfficesLayout from "./offices/OfficesLayout";
import Offices from "./offices/Offices";

function Pages() {
  return (
    <main>
      <Routes>
        <Route path="/" element={<Home />} />
        <Route path="/welcome" element={<Welcome />} />
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

export default Pages;
