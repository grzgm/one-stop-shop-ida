import { useEffect, useState } from "react";
import { Link, useLoaderData } from "react-router-dom";

async function HomeLoader() {
  try {
    const res = await fetch(`http://localhost:3002/microsoft/auth/check-token`, {
      method: 'GET',
      credentials: 'include' // Include credentials (cookies) in the request
    })
    return res.json();
  }
  catch (error) {
    console.error('Error:', error);
    return false;
  }
}

function Home() {
  const [isAuth, setIsAuth] = useState(useLoaderData())

  return (
    <>
      <h1>Home</h1>
      {!isAuth && <Link to={`http://localhost:3002/microsoft/auth`}>Log in with Microsoft</Link>}
    </>
  );
}

export default Home;
export { HomeLoader };