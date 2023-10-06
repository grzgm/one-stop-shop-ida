import "../css/components/buttons.css";
import { ButtonsLarge, ButtonsNormal } from "./text-wrapers/TextWrapers";

interface ButtonsProps {
  child: string;
}

function Button({ child }: ButtonsProps) {
  return (
    <button className="button button-animation buttons--normal">{child}</button>
  );
}

function ButtonAlt({ child }: ButtonsProps) {
    return (
      <button className="button-alt buttons--normal">{child}</button>
    );
  }

export default Button;
export { ButtonAlt };
