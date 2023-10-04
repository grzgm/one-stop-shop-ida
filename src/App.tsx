import "./css/App.css";
import Navbar from "./components/navbar/Navbar";
import Router from "./routes/Router";
import Sidebar from "./components/bars/Sidebar";

function App() {
  return (
    <div>
      <Navbar/>
      {/* <Sidebar /> */}
      <Router />
    </div>
  );
}

export default App;
