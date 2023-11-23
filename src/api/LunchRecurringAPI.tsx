import { IActionResult, InspectResponseAsync } from "./Response";

export interface ILunchRecurringItem {
	monday: boolean;
	tuesday: boolean;
	wednesday: boolean;
	thursday: boolean;
	friday: boolean;
}

async function GetRegisteredDays(): Promise<IActionResult<ILunchRecurringItem>> {
	try {
		const res = await fetch(
			`http://localhost:3002/lunch/recurring/get-registered-days`,
			{
				method: "GET",
				credentials: "include", // Include credentials (cookies) in the request
			}
		);
		// const resData = await InspectResponseAsync<ILunchRecurringItem>(res);
		// const resDataConverted: IActionResult<ILunchRecurringItem> = {
		// 	success: resData.success,
		// 	status: resData.status,
		// 	payload: resData.payload ? resData.payload as ILunchRecurringItem : undefined,
		// };
		// return resDataConverted;
		return InspectResponseAsync<ILunchRecurringItem>(res);
	} catch (error) {
		console.error("Error:", error);
		return { success: false, statusText: "Request could not be send." };
	}
}

async function PutLunchRecurringItem(lunchRecurringItem: ILunchRecurringItem): Promise<IActionResult<undefined>>{
	try {
		const res = await fetch(
			`http://localhost:3002/lunch/recurring/update-registered-days`,
			{
				method: "PUT",
				credentials: "include", // Include credentials (cookies) in the request
				headers: {
				'Content-Type': 'application/json'
				},
				body: JSON.stringify(lunchRecurringItem),
			}
		);
		return InspectResponseAsync(res);
	} catch (error) {
		console.error("Error:", error);
		return { success: false, statusText: "Request could not be send." };
	}
}

async function RegisterLunchRecurring(officeName: string): Promise<IActionResult<undefined>> {
	try {
		const res = await fetch(
			`http://localhost:3002/lunch/recurring/register-for-lunch-recurring?officeName=${officeName}`,
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

export { GetRegisteredDays, PutLunchRecurringItem, RegisterLunchRecurring };
