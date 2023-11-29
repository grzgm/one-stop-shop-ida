import { IActionResult, InspectResponseAsync } from "./Response";

async function IsAuth(): Promise<IActionResult<boolean>> {
	try {
		const res = await fetch(
			`http://localhost:3002/slack/auth/check-token`,
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

async function SendMessage(message: string, channel: string): Promise<IActionResult<null>> {
	try {
		const res = await fetch(
			`http://localhost:3002/slack/send-message?message=${message}&channel=${channel}`,
			{
				method: "POST",
				credentials: "include", // Include credentials (cookies) in the request
			}
		);
		return InspectResponseAsync(res);
	} catch (error) {
		console.error("Error:", error);
		return { success: false, statusText: "Request could not be send." };
	}
}

async function SetStatus(text?: string, emoji?: string, expiration?: string): Promise<IActionResult<null>> {
	try {
		const res = await fetch(
			`http://localhost:3002/slack/set-status?text=${text}&emoji=${emoji}&expiration=${expiration}`,
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

export { IsAuth, SendMessage, SetStatus };