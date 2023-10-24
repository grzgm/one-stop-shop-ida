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
		return { success: false, status: "Request could not be send." };
	}
}

async function SendEmail(message: string, address: string): Promise<IActionResult<null>> {
	try {
		const res = await fetch(
			`http://localhost:3002/slack/resources/send-email?message=${encodeURI(message)}&address=${encodeURI(address)}`,
			{
				method: "POST",
				credentials: "include", // Include credentials (cookies) in the request
			}
		);
		return InspectResponseAsync(res);
	} catch (error) {
		console.error("Error:", error);
		return { success: false, status: "Request could not be send." };
	}
}

export { IsAuth, SendEmail };
