import { IActionResult, InspectResponseAsync } from "./Response";

async function IsRegistered(): Promise<IActionResult<boolean>> {
	try {
		const res = await fetch(
			`http://localhost:3002/lunch/today/is-registered`,
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

async function RegisterLunchToday(officeName: string, registration: boolean): Promise<IActionResult<undefined>> {
	try {
		const res = await fetch(
			`http://localhost:3002/lunch/today/lunch-today-registration?officeName=${officeName}&registration=${registration}`,
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
