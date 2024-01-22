import "../css/components/buttons.css";

interface ButtonsProps {
	child: string;
	disabled?: boolean;
	onClick: () => void;
}

function Button({ child, disabled, onClick }: ButtonsProps) {
	return (
		<button className="button button-animation buttons--large" onClick={onClick} disabled={disabled} type="button">{child}</button>
	);
}

function ButtonAlt({ child, disabled, onClick }: ButtonsProps) {
	return (
		<button className="button button-alt buttons--large" onClick={onClick} disabled={disabled} type="button">{child}</button>
	);
}

export default Button;
export { ButtonAlt };
