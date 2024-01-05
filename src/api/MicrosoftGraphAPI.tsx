import Cookies from "universal-cookie";
import { IActionResult, InspectResponseAsync } from "./Response";

async function IsAuth(): Promise<IActionResult<boolean>> {
	try {
		const res = await fetch(
			`http://localhost:3002/microsoft/auth/is-auth`,
			{
				method: "GET",
				credentials: "include", // Include credentials (cookies) in the request
				headers: {
					'Authorization': `Bearer ${new Cookies().get("jwt")}`,
					'Content-Type': 'application/json',
				}
			}
		);
		return InspectResponseAsync(res);
	} catch (error) {
		console.error("Error:", error);
		return { success: false, statusText: "Request could not be send." };
	}
}

async function SendEmail(message: string, address: string): Promise<IActionResult<null>> {
	try {
		const res = await fetch(
			`http://localhost:3002/microsoft/resources/send-email?message=${encodeURI(message)}&address=${encodeURI(address)}`,
			{
				method: "POST",
				credentials: "include", // Include credentials (cookies) in the request
				headers: {
					'Authorization': `Bearer ${new Cookies().get("jwt")}`,
					'Content-Type': 'application/json',
				}
			}
		);
		return InspectResponseAsync(res);
	} catch (error) {
		console.error("Error:", error);
		return { success: false, statusText: "Request could not be send." };
	}
}

async function CreateEvent(address: string, title: string, startDate: string, endDate: string, description?: string): Promise<IActionResult<null>> {
	try {
		const res = await fetch(
			`http://localhost:3002/microsoft/resources/create-event?address=${encodeURI(address)}&title=${title}&startDate=${startDate}&endDate=${endDate}&description=${description}`,
			{
				method: "POST",
				credentials: "include", // Include credentials (cookies) in the request
				headers: {
					'Authorization': `Bearer ${new Cookies().get("jwt")}`,
					'Content-Type': 'application/json',
				}
			}
		);
		return InspectResponseAsync(res);
	} catch (error) {
		console.error("Error:", error);
		return { success: false, statusText: "Request could not be send." };
	}
}

export { IsAuth, SendEmail, CreateEvent };
