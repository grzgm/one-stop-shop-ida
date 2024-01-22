import CheckIcon from '@mui/icons-material/Check';
import ReportIcon from '@mui/icons-material/Report';
import CloseIcon from "@mui/icons-material/Close";
import { BodySmall } from './text-wrapers/TextWrapers';
import "../css/components/alert.css"

interface AlertProps {
	alertText: string;
	alertStatus: boolean;
	alertLineLength: number;
	onClick: () => void;
}

function Alert({ alertText, alertStatus, alertLineLength: alertLineLength, onClick }: AlertProps) {

	return (
		<div className={`alert ${alertStatus ? "background-colour--success" : "background-colour--fail"}`}>
			<div className='alert__main'>
				{alertStatus ? <CheckIcon /> : <ReportIcon />}
				<BodySmall>
					{alertText}
				</BodySmall>
				<CloseIcon onClick={onClick} />
			</div>
			<div className="alert__line" style={{ width: `${alertLineLength}%` }} />
		</div>
	);
}

export { Alert };