import { Link, useSearchParams } from "react-router-dom";

function Home() {
  // const [searchParams, setSearchParams] = useSearchParams();

  // localStorage.setItem("access-token", searchParams.get("access-token") as string)
  // localStorage.setItem("refresh-token", searchParams.get("refresh-token") as string)

  console.log(localStorage.getItem("session-id"))

  return (
    <>
      <h1>Home</h1>
      <Link to={`http://localhost:3002/microsoft/auth?sessionid=${localStorage.getItem("session-id")}`} target="_blank">Log in with Microsoft</Link>
    </>
  );
}

export default Home;
