import { Link } from "react-router-dom";

function Home() {

  return (
    <>
      <h1>Home</h1>
      <Link to={`http://localhost:3002/microsoft/auth`}>Log in with Microsoft</Link>
    </>
  );
}

export default Home;
