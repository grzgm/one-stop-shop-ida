import { IActionResult } from "./Response";
import ExecuteApiCall from "./Request";

async function IsAuth(): Promise<IActionResult<boolean>> {
	try {
		return await ExecuteApiCall<boolean>(`/slack/auth/is-auth`, "GET");
	} catch (error) {
		console.error("Error:", error);
		return { success: false, statusText: "Request could not be send." };
	}
}

async function AuthUrl(): Promise<IActionResult<string>> {
	try {
		return await ExecuteApiCall<string>(`/slack/auth/url`, "GET");
	} catch (error) {
		console.error("Error:", error);
		return { success: false, statusText: "Request could not be send." };
	}
}

export { IsAuth, AuthUrl };
