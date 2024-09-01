import { IActionResult } from "./Response";
import ExecuteApiCall from "./Request";

export interface ILunchRegistrationsItem {
	registrationDate?: Date;
	office: string;
}

async function IsRegistered(): Promise<IActionResult<ILunchRegistrationsItem>> {
	try {
		return {
			success: true,
			statusCode: 200,
			statusText: "OK",
			payload: {
				office: "utrecht",
			},
		}
		// return await ExecuteApiCall<ILunchRegistrationsItem>(`/lunch/registrations/get-registration`, "GET");
	} catch (error) {
		console.error("Error:", error);
		return { success: false, statusText: "Request could not be send." };
	}
}

async function PutLunchRegistrationsItem(registration: boolean, office: string): Promise<IActionResult<ILunchRegistrationsItem>> {
	try {
		return {
			success: true,
			statusCode: 200,
			statusText: "OK",
			payload: {
				registrationDate: new Date(),
				office: office,
			},
		}
		// return await ExecuteApiCall<ILunchRegistrationsItem>(`/lunch/registrations/put-registration?registration=${registration}&office=${office}`, "PUT");
	} catch (error) {
		console.error("Error:", error);
		return { success: false, statusText: "Request could not be send." };
	}
}

export { IsRegistered, PutLunchRegistrationsItem };
