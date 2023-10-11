import { useContext, useEffect, useState } from "react";
import { BodyNormal, HeadingLarge } from "../../text-wrapers/TextWrapers";
import Panel from "../../tiles/Panel";
import CurrentOfficeContext from "../../../contexts/CurrentOfficeContext";
import OfficeMap from "../../OfficeMap";
import { useNavigate } from "react-router-dom";

function Offices() {
  const { currentOffice, setCurrentOffice } = useContext(CurrentOfficeContext);
  const navigate = useNavigate();

  const SwitchOffice = (officeName: string) => {
    setCurrentOffice(officeName);
    navigate("/");
  }

  return (
    <div className="content">
      <div className="description">
        <HeadingLarge>Choose your office</HeadingLarge>
        <BodyNormal>Pick from the map</BodyNormal>
        <BodyNormal>or tap on the office!</BodyNormal>
      </div>
      <div>
        <OfficeMap switchOffice={SwitchOffice} />
      </div>
      <div className="content__panels">
        <Panel linkAddress="/office-details" title="Utrecht" description="Orteliuslaan 25 3528BA" onClick={() => setCurrentOffice("Utrecht")} />
        <Panel linkAddress="/office-details" title="Amsterdam" description="Cruquiusweg 110F 1019AK" onClick={() => setCurrentOffice("Amsterdam")} />
        <Panel linkAddress="/office-details" title="Eindhoven" description="High Tech Campus 69 5656AE" onClick={() => setCurrentOffice("Eindhoven")} />
        <Panel linkAddress="/office-details" title="Kontich" description="Prins boudewijnlaan 24e 2550" onClick={() => setCurrentOffice("Kontich")} />
        <Panel linkAddress="/office-details" title="Hasselt" description="Kempische Steenweg 311 3500" onClick={() => setCurrentOffice("Hasselt")} />
        <Panel linkAddress="/office-details" title="Merelbeke" description="Guldensporenpark 88 9820" onClick={() => setCurrentOffice("Merelbeke")} />
      </div>
    </div>
  );
}

export default Offices;
