import CardContainer from "../tiles/CardContainer";
import "../../css/components/pages/home.css";
import { BodyNormal, HeadingLarge } from "../text-wrapers/TextWrapers";
// import { CardProps } from "../tiles/Card";


const officeDetails = [{linkAddress:"/office-details/reserve-desk", title:"Reserve a Desk", description:"Reserve a Desk"},
{linkAddress:"/office-details/lunch", title:"Lunch", description:"Sign up for Lunch"},
{linkAddress:"/office-details/office-information", title:"Office Information", description:"Office Information Access Parking Wi-Fi How many ppl in the office"},
{linkAddress:"/office-details/presence", title:"Presence", description:"Let your colleague know when you are present"}]

const employeePortal = [
{linkAddress: "https://werknemer.loket.nl/#/login?returnUrl=start", title: "External", description: "Werknemerloket Web Page"},
{linkAddress: "/employee-portal/sick-leave", title: "Sick Leave", description: "Register your absence"},
{linkAddress: "/employee-portal/vacation", title: "Vacation", description: "Plan your off days and see their balance"},
{linkAddress: "/employee-portal/scheduling", title: "Scheduling", description: "Plan out your wor schedule"}]

function Home() {
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
      <CardContainer title="Office Details" cardProps={officeDetails}/>
      {/* <CardContainer cardProps={[{ linkAddress: "/office-details", title: "Test", description: "Test" }]}/> */}
    </div>
  );
}

export default Home;
