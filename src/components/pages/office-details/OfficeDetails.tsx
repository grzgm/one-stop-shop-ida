import { Link } from "react-router-dom";

function OfficeDetails() {

    return (
      <>
          <h1>Office Details</h1>
          <Link to="/office-details/reserve-desk">Reserve a Desk</Link>
          <Link to="/office-details/reserve-desk-overview">Reserve a Desk Overview</Link>
          <Link to="/office-details/lunch">Lunch</Link>
          <Link to="/office-details/office-information">Office Information</Link>
          <Link to="/office-details/presence">Presence</Link>
      </>
    );
  }
  
  export default OfficeDetails;
  