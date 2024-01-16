import CardContainer from "../tiles/CardContainer";
import "../../css/components/pages/home.css";
import { BodyNormal, HeadingLarge } from "../text-wrapers/TextWrapers";
import { useContext } from "react";
import CurrentOfficeContext from "../../contexts/CurrentOfficeContext";
import OfficeFeaturesContext from "../../contexts/OfficeFeaturesContext";
import { capitalizeFirstLetter } from "../../misc/TextFunctions";

function Home() {
  const officeName = useContext(CurrentOfficeContext).currentOffice;
	const { officeFeatures } = useContext(OfficeFeaturesContext);
  const currentOfficeInformationData = officeFeatures[officeName]

  const officeDetails = [(currentOfficeInformationData.canReserveDesk ? {linkAddress:"/office-details/reserve-desk", title:"Reserve a Desk", description:"Reserve a Desk"} : {}),
  (currentOfficeInformationData.canRegisterLunch ? {linkAddress:"/office-details/lunch", title:"Lunch", description:"Sign up for Lunch"} : {}),
  {linkAddress:"/office-details/office-information", title:"Office Information", description:"Office Information Access Parking Wi-Fi How many ppl in the office"},
  (currentOfficeInformationData.canRegisterPresence ? {linkAddress:"/office-details/presence", title:"Presence", description:"Let your colleague know when you are present"} : {})].filter(item => Object.keys(item).length !== 0);
  
  const employeePortal = [
  {linkAddress: "https://werknemer.loket.nl/#/login?returnUrl=start", title: "External", description: "Werknemerloket Web Page", isOpenInNewTab: true},
  {linkAddress: "/employee-portal/sick-leave", title: "Sick Leave", description: "Register your absence"},
  {linkAddress: "/employee-portal/vacation", title: "Vacation", description: "Plan your off days and see their balance"},
  {linkAddress: "/employee-portal/scheduling", title: "Scheduling", description: "Plan out your wor schedule"}]

  return (
    <div className="content">
      <div className="description">
        <HeadingLarge>Welcome to</HeadingLarge>
        <HeadingLarge>iDA</HeadingLarge>
        <HeadingLarge>One Stop Shop</HeadingLarge>
        <BodyNormal>Place where you</BodyNormal>
        <BodyNormal>have everything you need!</BodyNormal>
      </div>
      <CardContainer title="EmployeePortal" cardProps={employeePortal}/>
      <CardContainer title={`Office Details ${capitalizeFirstLetter(officeName)}`} cardProps={officeDetails}/>
    </div>
  );
}

export default Home;
