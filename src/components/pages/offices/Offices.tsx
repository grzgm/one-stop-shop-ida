import { useContext, useEffect, useState } from "react";
import { BodyNormal, HeadingLarge } from "../../text-wrapers/TextWrapers";
import Panel from "../../tiles/Panel";
import CurrentOfficeContext from "../../../contexts/CurrentOfficeContext";
import OfficeMap from "../../OfficeMap";
import { useNavigate } from "react-router-dom";
import { officeInformationData } from "../../../assets/OfficeInformationData";

function Offices() {
  const { currentOffice, setCurrentOffice } = useContext(CurrentOfficeContext);
  const [position, setPosition] = useState<GeolocationPosition | null>(null);
  const [closestOfficeName, setClosestOfficeName] = useState<string | null>(null);
  const navigate = useNavigate();

  useEffect(() => {
    if ('geolocation' in navigator) {
      navigator.geolocation.getCurrentPosition(
        (pos) => {
          setPosition(pos);
          setClosestOfficeName(CalculateClosestOffice(pos.coords.latitude, pos.coords.longitude));
        },
        (error) => {
          console.error('Error getting geolocation:', error);
        }
      );
    } else {
      console.error('Geolocation not supported');
    }
  }, []);

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
        <OfficeMap switchOffice={SwitchOffice} closestOfficeName={closestOfficeName}/>
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

function CalculateClosestOffice(userLat: number, userLng: number) {
  let firstOffice = Object.values(officeInformationData)[0]
  let shortestDistance = CalculateDistance(userLat, userLng, firstOffice.officeInformation.coords.lat, firstOffice.officeInformation.coords.lat);
  let closestOfficeName = firstOffice.officeName;

  for (let office of Object.values(officeInformationData)) {
    let newDistance = CalculateDistance(userLat, userLng, office.officeInformation.coords.lat, office.officeInformation.coords.lat);
    if (newDistance < shortestDistance) {
      shortestDistance = newDistance;
      closestOfficeName = office.officeName;
    }
  }

  return closestOfficeName;
}

function CalculateDistance(userLat: number, userLng: number, destLat: number, destLng: number) {
  const earthRadius = 6371; // Radius of the Earth in km
  const latDiff = (destLat - userLat) * (Math.PI / 180);
  const lngDiff = (destLng - userLng) * (Math.PI / 180);
  const a =
    Math.sin(latDiff / 2) * Math.sin(latDiff / 2) +
    Math.cos(userLat * (Math.PI / 180)) *
    Math.cos(destLat * (Math.PI / 180)) *
    Math.sin(lngDiff / 2) *
    Math.sin(lngDiff / 2);
  const c = 2 * Math.atan2(Math.sqrt(a), Math.sqrt(1 - a));
  const distance = earthRadius * c; // Distance in km
  return distance;
}

export default Offices;
