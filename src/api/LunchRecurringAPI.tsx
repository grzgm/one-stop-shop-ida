import { IActionResult } from "./Response";
import ExecuteApiCall from "./Request";

export interface ILunchRecurringItem {
	monday: boolean;
	tuesday: boolean;
	wednesday: boolean;
	thursday: boolean;
	friday: boolean;
}

async function GetRegisteredDays(): Promise<IActionResult<ILunchRecurringItem>> {
	try {
		return ExecuteApiCall<ILunchRecurringItem>(`/lunch/recurring/get-registered-days`, "GET");
	} catch (error) {
		console.error("Error:", error);
		return { success: false, statusText: "Request could not be send." };
	}
}

async function PutLunchRecurringItem(lunchRecurringItem: ILunchRecurringItem): Promise<IActionResult<undefined>>{
	try {
		return ExecuteApiCall<undefined>(`/lunch/recurring/get-registered-days`, "GET", JSON.stringify(lunchRecurringItem));
	} catch (error) {
		console.error("Error:", error);
		return { success: false, statusText: "Request could not be send." };
	}
}

export { GetRegisteredDays, PutLunchRecurringItem };
