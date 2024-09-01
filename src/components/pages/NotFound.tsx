import { useEffect } from "react";
import { useNavigate } from "react-router-dom";
import { HeadingLarge } from "../text-wrapers/TextWrapers";

function NotFound() {
  const navigate = useNavigate();

  useEffect(() => {
    setTimeout(() => {
      navigate("/");
    }, 1000);
  }, []);
  return (
    <div className="content">
          <div className="description">
                <HeadingLarge>Not Found</HeadingLarge>
          </div>
    </div>
  );
}

export default NotFound;
