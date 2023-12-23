import InfoIcon from '@mui/icons-material/Info';
import { BodySmall } from './text-wrapers/TextWrapers';
import { IActionResult } from '../api/Response';

interface AlertProps {
	alertResponse: IActionResult<any>;
	onClick: () => void;
}

function Alert({ alertResponse, onClick }: AlertProps) {
	return (
		<div className={`alert ${alertResponse.success ? "background-colour--success" : "background-colour--fail"}`}>
			<InfoIcon />
			<BodySmall>
				{alertResponse.statusText}
			</BodySmall>
		</div>
	);
}

export { Alert };