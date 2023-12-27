import InfoIcon from '@mui/icons-material/Info';
import { BodySmall } from './text-wrapers/TextWrapers';

interface AlertProps {
    alertText: string;
    alertStatus: boolean;
	onClick: () => void;
}

function Alert({ alertText, alertStatus, onClick }: AlertProps) {
	return (
		<div className={`alert ${alertStatus ? "background-colour--success" : "background-colour--fail"}`}>
			<InfoIcon />
			<BodySmall>
				{alertText}
			</BodySmall>
		</div>
	);
}

export { Alert };