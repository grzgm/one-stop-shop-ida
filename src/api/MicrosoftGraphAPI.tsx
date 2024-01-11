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

async function AuthUrl(): Promise<IActionResult<string>> {
	try {
		return await ExecuteApiCall<string>(`/microsoft/auth/url`, "GET");
	} catch (error) {
		console.error("Error:", error);
		return { success: false, statusText: "Request could not be send." };
	}
}

async function SendEmail(message: string, address: string): Promise<IActionResult<undefined>> {
	try {
		return await ExecuteApiCall<undefined>(`/microsoft/resources/send-email?message=${encodeURI(message)}&address=${encodeURI(address)}`, "POST");
	} catch (error) {
		console.error("Error:", error);
		return { success: false, statusText: "Request could not be send." };
	}
}

async function CreateEvent(address: string, title: string, startDate: string, endDate: string, description?: string): Promise<IActionResult<undefined>> {
	try {
		return await ExecuteApiCall<undefined>(`/microsoft/resources/create-event?address=${encodeURI(address)}&title=${title}&startDate=${startDate}&endDate=${endDate}&description=${description}`, "GET");
	} catch (error) {
		console.error("Error:", error);
		return { success: false, statusText: "Request could not be send." };
	}
}

export { IsAuth, AuthUrl, SendEmail, CreateEvent };
