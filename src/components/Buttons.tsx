import "../css/components/buttons.css";

interface ButtonsProps {
  child: string;
  onClick: () => void;
}

function Button({ child, onClick }: ButtonsProps) {
  return (
    <button className="button button-animation buttons--large" onClick={onClick} type="button">{child}</button>
  );
}

function ButtonAlt({ child, onClick }: ButtonsProps) {
    return (
      <button className="button-alt buttons--large" onClick={onClick} type="button">{child}</button>
    );
  }

export default Button;
export { ButtonAlt };
