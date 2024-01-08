import { IActionResult } from "./Response";
import ExecuteApiCall from "./Request";

async function IsAuth(): Promise<IActionResult<boolean>> {
	try {
		return await ExecuteApiCall<boolean>(`/microsoft/auth/is-auth`, "GET");
	} catch (error) {
		console.error("Error:", error);
		return { success: false, statusText: "Request could not be send." };
	}
}

async function SendEmail(message: string, address: string): Promise<IActionResult<null>> {
	try {
		return await ExecuteApiCall<null>(`/microsoft/resources/send-email?message=${encodeURI(message)}&address=${encodeURI(address)}`, "POST");
	} catch (error) {
		console.error("Error:", error);
		return { success: false, statusText: "Request could not be send." };
	}
}

async function CreateEvent(address: string, title: string, startDate: string, endDate: string, description?: string): Promise<IActionResult<null>> {
	try {
		return await ExecuteApiCall<null>(`/microsoft/resources/create-event?address=${encodeURI(address)}&title=${title}&startDate=${startDate}&endDate=${endDate}&description=${description}`, "GET");
	} catch (error) {
		console.error("Error:", error);
		return { success: false, statusText: "Request could not be send." };
	}
}

export { IsAuth, SendEmail, CreateEvent };
