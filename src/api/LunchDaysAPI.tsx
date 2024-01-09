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
		return await ExecuteApiCall<ILunchDaysItem>(`/lunch/days/get-registered-days`, "GET");
	} catch (error) {
		console.error("Error:", error);
		return { success: false, statusText: "Request could not be send." };
	}
}

async function PutLunchDaysItem(lunchDaysItem: ILunchDaysItem): Promise<IActionResult<undefined>>{
	try {
		return await ExecuteApiCall<undefined>(`/lunch/days/get-registered-days`, "GET", JSON.stringify(lunchDaysItem));
	} catch (error) {
		console.error("Error:", error);
		return { success: false, statusText: "Request could not be send." };
	}
}

export { GetRegisteredDays, PutLunchDaysItem };
