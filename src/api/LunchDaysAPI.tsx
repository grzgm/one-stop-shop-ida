import { IActionResult } from "./Response";
import ExecuteApiCall from "./Request";

export interface ILunchDaysItem {
	monday: boolean;
	tuesday: boolean;
	wednesday: boolean;
	thursday: boolean;
	friday: boolean;
}

async function GetRegisteredDays(): Promise<IActionResult<ILunchDaysItem>> {
	try {
		return {
			success: true,
			statusCode: 200,
			statusText: "OK",
			payload: {
				monday: false,
				tuesday: false,
				wednesday: false,
				thursday: false,
				friday: false,
			},
		}
		// return await ExecuteApiCall<ILunchDaysItem>(`/lunch/days/get-days`, "GET");
	} catch (error) {
		console.error("Error:", error);
		return { success: false, statusText: "Request could not be send." };
	}
}

async function PutLunchDaysItem(lunchDaysItem: ILunchDaysItem): Promise<IActionResult<undefined>> {
	try {
		return {
			success: true,
			statusCode: 200,
			statusText: "OK",
		}
		// return await ExecuteApiCall<undefined>(`/lunch/days/update-days`, "PUT", JSON.stringify(lunchDaysItem));
	} catch (error) {
		console.error("Error:", error);
		return { success: false, statusText: "Request could not be send." };
	}
}

export { GetRegisteredDays, PutLunchDaysItem };
