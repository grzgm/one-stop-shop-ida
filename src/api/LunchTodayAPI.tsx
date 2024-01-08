import { IActionResult } from "./Response";
import ExecuteApiCall from "./Request";

export interface ILunchTodayItem {
	registrationDate?: Date;
	office: string;
}

async function IsRegistered(): Promise<IActionResult<ILunchTodayItem>> {
	try {
		return await ExecuteApiCall<ILunchTodayItem>(`/lunch/today/get-registration`, "GET");
	} catch (error) {
		console.error("Error:", error);
		return { success: false, statusText: "Request could not be send." };
	}
}

async function RegisterLunchToday(registration: boolean, office: string): Promise<IActionResult<ILunchTodayItem>> {
	try {
		return await ExecuteApiCall<ILunchTodayItem>(`/lunch/today/put-registration?registration=${registration}&office=${office}`, "PUT");
	} catch (error) {
		console.error("Error:", error);
		return { success: false, statusText: "Request could not be send." };
	}
}

export { IsRegistered, RegisterLunchToday };
