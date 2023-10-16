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
  {(Object.values(officeInformationData)).map((office, index)=>{
    <Panel linkAddress="/office-details" title={officeInformationData["Utrecht"].officeName} description="Orteliuslaan 25 3528BA" onClick={() => setCurrentOffice("Utrecht")} />
  })}

  return (
    <div className="content">
      <div className="description">
        <HeadingLarge>Choose your office</HeadingLarge>
        <BodyNormal>Pick from the map</BodyNormal>
        <BodyNormal>or tap on the office!</BodyNormal>
      </div>
      <OfficeMap switchOffice={SwitchOffice} closestOfficeName={closestOfficeName}/>
      <div className="content__panels">
        {(Object.values(officeInformationData)).map((office, index)=>{
            return (<Panel linkAddress="/office-details" title={office.officeName} description={office.officeInformation.address} onClick={() => setCurrentOffice(office.officeName)} />)
        })}
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
