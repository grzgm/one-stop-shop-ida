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
		return { success: false, status: "Request could not be send." };
	}
}

async function RegisterLunchToday(officeName: string): Promise<IActionResult<null>> {
	try {
		const res = await fetch(
			`http://localhost:3002/lunch/today/register-lunch-today?officeName=${officeName}`,
			{
				method: "PUT",
				credentials: "include", // Include credentials (cookies) in the request
			}
		);
		return InspectResponseAsync(res);
	} catch (error) {
		console.error("Error:", error);
		return { success: false, status: "Request could not be send." };
	}
}

export { IsRegistered, RegisterLunchToday };