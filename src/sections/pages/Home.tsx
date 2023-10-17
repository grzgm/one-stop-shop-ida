import { useEffect, useState } from "react";
import { Link } from "react-router-dom";

function Home() {
  const [isAuth, setIsAuth] = useState(false)
  useEffect(() => {
    fetch(`http://localhost:3002/check-token`, {
      method: 'GET',
      credentials: 'include' // Include credentials (cookies) in the request
  }).then(response => response.json())
      .then(data => setIsAuth(data))
      .catch(error => console.error('Error:', error));
  }, []);

  return (
    <>
      <h1>Home</h1>
      {!isAuth && <Link to={`http://localhost:3002/microsoft/auth`}>Log in with Microsoft</Link>}
    </>
  );
}

export default Home;
