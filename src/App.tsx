import { useEffect } from 'react';
import './App.css'
import Navbar from './sections/navbar/Navbar'
import Pages from './sections/pages/Pages'
import axios from 'axios';

function App() {

  useEffect(() => {
    // Set session data on the server
    axios.get('http://localhost:3002/set-session', { withCredentials: true })
      .then(response => response.data)
      .then(data => console.log(data))
      .catch(error => console.error('Error:', error));
    // Get session data from the server
    axios.get('http://localhost:3002/get-session', { withCredentials: true })
      .then(response => response.data)
      .then(data => localStorage.setItem("session-id", data.userId))
      .catch(error => console.error('Error:', error));
  }, []);


  return (
    <div>
      <Navbar/>
      <Pages/>
    </div>
  )
}

export default App
