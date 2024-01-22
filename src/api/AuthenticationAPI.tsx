import { IActionResult } from "./Response";
import ExecuteApiCall from "./Request";
import Cookies from "universal-cookie";

async function IsAuth(): Promise<IActionResult<boolean>> {
	try {
		return await ExecuteApiCall<boolean>(`/authentication/is-auth`, "GET");
	} catch (error) {
		console.error("Error:", error);
		return { success: false, statusText: "Request could not be send." };
	}
}

async function Auth(): Promise<IActionResult<undefined>> {
	try {
		const res =  await ExecuteApiCall<undefined>(`/authentication/auth`, "GET");
        new Cookies().set("jwt", res.statusText, { path: "/", expires: new Date(Date.now() + (24 * 60 * 60 * 1000)), sameSite: 'none', secure: true })
        return res;
	} catch (error) {
		console.error("Error:", error);
        return { success: false, statusText: "Request could not be send." };
	}
}

export { IsAuth, Auth };
