import { IActionResult, InspectResponseAsync } from "./Response";

export interface LunchRecurringItem {
	Monday: boolean,
	Tuesday: boolean,
	Wednesday: boolean,
	Thursday: boolean,
	Friday: boolean,
}

interface LunchRecurringItemAPIResponse {
	monday: boolean;
	tuesday: boolean;
	wednesday: boolean;
	thursday: boolean;
	friday: boolean;
}

async function GetRegisteredDays(): Promise<IActionResult<LunchRecurringItem>> {
	try {
		const res = await fetch(
			`http://localhost:3002/lunch/recurring/get-registered-days`,
			{
				method: "GET",
				credentials: "include", // Include credentials (cookies) in the request
			}
		);
		const resData = await InspectResponseAsync<LunchRecurringItemAPIResponse>(res);
		let resDataConverted: IActionResult<LunchRecurringItem> = {
			success: resData.success,
			status: resData.status,
			payload: undefined,
		};
		if (resData.payload) {
			resDataConverted = {
				success: resData.success,
				status: resData.status,
				payload: {
					Monday: resData.payload.monday,
					Tuesday: resData.payload.tuesday,
					Wednesday: resData.payload.wednesday,
					Thursday: resData.payload.thursday,
					Friday: resData.payload.friday,
				},
			};
		}
		return resDataConverted;
	} catch (error) {
		console.error("Error:", error);
		return { success: false, status: "Request could not be send." };
	}
}

async function PutLunchRecurringItem(lunchRecurringItem: LunchRecurringItem){
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
		const resData = await InspectResponseAsync(res);
		return resData;
	} catch (error) {
		console.error("Error:", error);
		return { success: false, status: "Request could not be send." };
	}
}

export { GetRegisteredDays, PutLunchRecurringItem };
