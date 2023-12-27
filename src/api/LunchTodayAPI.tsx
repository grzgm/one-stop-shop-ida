import { IActionResult, InspectResponseAsync } from "./Response";

export interface ILunchTodayItem {
	isRegistered: boolean;
	office: string;
}

async function IsRegistered(): Promise<IActionResult<ILunchTodayItem>> {
	try {
		const res = await fetch(
			`http://localhost:3002/lunch/today/get-registration`,
			{
				method: "GET",
				credentials: "include", // Include credentials (cookies) in the request
			}
		);
		return InspectResponseAsync(res);
	} catch (error) {
		console.error("Error:", error);
		return { success: false, statusText: "Request could not be send." };
	}
}

async function RegisterLunchToday(registration: boolean, office: string): Promise<IActionResult<undefined>> {
	try {
		const res = await fetch(
			`http://localhost:3002/lunch/today/put-registration?registration=${registration}&office=${office}`,
			{
				method: "PUT",
				credentials: "include", // Include credentials (cookies) in the request
			}
		);
		return InspectResponseAsync(res);
	} catch (error) {
		console.error("Error:", error);
		return { success: false, statusText: "Request could not be send." };
	}
}

export { IsRegistered, RegisterLunchToday };
