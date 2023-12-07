import { IActionResult, InspectResponseAsync } from "./Response";

interface IDeskCluster {
    clusterId: string;
    desks: IDesk[];
}

export interface IDesk{
    clusterId: string;
    deskId: string;
	occupied: boolean[];
}

export interface IDeskReservation{
	date: Date;
    clusterId: string;
    deskId: string;
	timeSlot: number;
}

async function GetDeskReservationForOfficeDate(office: string, date: Date): Promise<IActionResult<IDeskCluster[]>> {
	try {
		const res = await fetch(
			`http://localhost:3002/desk/reservation/${office}?date=${date.toISOString().split('T')[0]}`,
			{
				method: "GET",
				credentials: "include", // Include credentials (cookies) in the request
			}
		);
		return InspectResponseAsync<IDeskCluster[]>(res);
	} catch (error) {
		console.error("Error:", error);
		return { success: false, statusText: "Request could not be send." };
	}
}

async function GetDeskReservationsOfUser(office: string, date: Date): Promise<IActionResult<IDeskReservation[]>> {
	try {
		const res = await fetch(
			`http://localhost:3002/desk/reservation/${office}/user?date=${date.toISOString().split('T')[0]}`,
			{
				method: "GET",
				credentials: "include", // Include credentials (cookies) in the request
			}
		);
		return InspectResponseAsync<IDeskReservation[]>(res);
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

async function PostDeskReservation(office: string, date: Date, clusterId: string, deskId: string, timeSlots: number[]): Promise<IActionResult<undefined>> {
	try {
		const res = await fetch(
			`http://localhost:3002/desk/reservation/${office}?date=${date.toISOString().split('T')[0]}&clusterId=${clusterId}&deskId=${deskId}&timeSlots=${timeSlots.join("&timeSlots=")}`,
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

async function DeleteDeskReservation(office: string, date: Date, clusterId: string, deskId: string, timeSlots: number[]): Promise<IActionResult<undefined>> {
	try {
		const res = await fetch(
			`http://localhost:3002/desk/reservation/${office}?date=${date.toISOString().split('T')[0]}&clusterId=${clusterId}&deskId=${deskId}&timeSlots=${timeSlots.join("&timeSlots=")}`,
			{
				method: "DELETE",
				credentials: "include", // Include credentials (cookies) in the request
			}
		);
		return InspectResponseAsync(res);
	} catch (error) {
		console.error("Error:", error);
		return { success: false, statusText: "Request could not be send." };
	}
}

// export { GetDeskReservationForOfficeDate, PutLunchRecurringItem, RegisterLunchRecurring };
export { GetDeskReservationForOfficeDate, GetDeskReservationsOfUser, PostDeskReservation, DeleteDeskReservation}
