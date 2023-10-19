import { useEffect } from "react";

function Welcome() {
  useEffect(() => {
    // Get session data from the server
    fetch(`http://localhost:3002/microsoft/auth/get-token`, {
      method: 'GET',
      credentials: 'include' // Include credentials (cookies) in the request
    })
      .then(response => response.json())
      .then(data => console.log(data))
      .catch(error => console.error('Error:', error));

    // // Send Email
    // fetch(`http://localhost:3002/microsoft/resources/send-email?message=test&address=grzegorz.malisz@weareida.digital`, {
    //   method: 'POST',
    //   credentials: 'include' // Include credentials (cookies) in the request
    // })
    //   .then(response => response.json())
    //   .then(data => console.log(data))
    //   .catch(error => console.error('Error:', error));

    // Create Event
    // fetch(`http://localhost:3002/microsoft/resources/create-event?address=grzegorz.malisz@weareida.digital`, {
    //   method: 'POST',
    //   credentials: 'include' // Include credentials (cookies) in the request
    // })
    //   .then(response => response.json())
    //   .then(data => console.log(data))
    //   .catch(error => console.error('Error:', error));

    // Refresh Token
    fetch(`http://localhost:3002/microsoft/auth/refresh`, {
      method: 'PUT',
      credentials: 'include' // Include credentials (cookies) in the request
    })
      .then(response => response.json())
      .then(data => console.log(data))
      .catch(error => console.error('Error:', error));
  }, []);

  return (
    <h1>Welcome</h1>
  );
}

export default Welcome;
