import CheckIcon from '@mui/icons-material/Check';
import ReportIcon from '@mui/icons-material/Report';
import CloseIcon from "@mui/icons-material/Close";
import { BodySmall } from './text-wrapers/TextWrapers';

interface AlertProps {
    alertText: string;
    alertStatus: boolean;
	onClick: () => void;
}

function Alert({ alertText, alertStatus, onClick }: AlertProps) {
	return (
		<div className={`alert ${alertStatus ? "background-colour--success" : "background-colour--fail"}`}>
			{alertStatus ? <CheckIcon /> : <ReportIcon />}
			<BodySmall>
				{alertText}
			</BodySmall>
			<CloseIcon onClick={onClick}/>
		</div>
	);
}

export { Alert };