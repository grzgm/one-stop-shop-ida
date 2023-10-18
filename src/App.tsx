import './App.css'
import { RouterProvider, createBrowserRouter, createRoutesFromElements } from "react-router-dom";
import Router from './Router';

function App() {
  const customBrowserRouter = createBrowserRouter(createRoutesFromElements(
    Router()
  ))

  return (
    <div>
      <RouterProvider router={customBrowserRouter} />
    </div>
  )
}

export default App
