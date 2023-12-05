import { IActionResult, InspectResponseAsync } from "./Response";

export interface IDesk{
	office: string;
	date: Date;
    clusterId: number;
    deskId: number;
	timeSlot: number;

}

export async function GetDeskReservationForOfficeDate(office: string, date: Date): Promise<IActionResult<IDesk[]>> {
	try {
		const res = await fetch(
			`http://localhost:3002/desk/reservation/${office}?date=${date.toISOString().split('T')[0]}`,
			{
				method: "GET",
				credentials: "include", // Include credentials (cookies) in the request
			}
		);
		return InspectResponseAsync<IDesk[]>(res);
	} catch (error) {
		console.error("Error:", error);
		return { success: false, statusText: "Request could not be send." };
	}
}

// async function PutLunchRecurringItem(lunchRecurringItem: Desk): Promise<IActionResult<undefined>>{
// 	try {
// 		const res = await fetch(
// 			`http://localhost:3002/lunch/recurring/update-registered-days`,
// 			{
// 				method: "PUT",
// 				credentials: "include", // Include credentials (cookies) in the request
// 				headers: {
// 				'Content-Type': 'application/json'
// 				},
// 				body: JSON.stringify(lunchRecurringItem),
// 			}
// 		);
// 		return InspectResponseAsync(res);
// 	} catch (error) {
// 		console.error("Error:", error);
// 		return { success: false, statusText: "Request could not be send." };
// 	}
// }

// async function RegisterLunchRecurring(officeName: string): Promise<IActionResult<undefined>> {
// 	try {
// 		const res = await fetch(
// 			`http://localhost:3002/lunch/recurring/register-for-lunch-recurring?officeName=${officeName}`,
// 			{
// 				method: "PUT",
// 				credentials: "include", // Include credentials (cookies) in the request
// 			}
// 		);
// 		return InspectResponseAsync(res);
// 	} catch (error) {
// 		console.error("Error:", error);
// 		return { success: false, statusText: "Request could not be send." };
// 	}
// }

// export { GetDeskReservationForOfficeDate, PutLunchRecurringItem, RegisterLunchRecurring };
