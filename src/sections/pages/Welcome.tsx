import { useEffect } from "react";

function Welcome() {
  useEffect(() => {
    // Get session data from the server
    fetch(`http://localhost:3002/get-token`, {
      method: 'GET',
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