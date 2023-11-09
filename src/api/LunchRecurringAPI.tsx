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
		const resData = await InspectResponseAsync<ILunchRecurringItem>(res);
		let resDataConverted: IActionResult<ILunchRecurringItem> = {
			success: resData.success,
			status: resData.status,
			payload: undefined,
		};
		if (resData.payload) {
			resDataConverted = {
				success: resData.success,
				status: resData.status,
				payload: {
					monday: resData.payload.monday,
					tuesday: resData.payload.tuesday,
					wednesday: resData.payload.wednesday,
					thursday: resData.payload.thursday,
					friday: resData.payload.friday,
				},
			};
		}
		return resDataConverted;
	} catch (error) {
		console.error("Error:", error);
		return { success: false, status: "Request could not be send." };
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
		const resData = await InspectResponseAsync<undefined>(res);
		return resData;
	} catch (error) {
		console.error("Error:", error);
		return { success: false, status: "Request could not be send." };
	}
}

export { GetRegisteredDays, PutLunchRecurringItem };
