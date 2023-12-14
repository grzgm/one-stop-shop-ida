import { IActionResult, InspectResponseAsync } from "./Response";
export interface IDeskReservationItem {
	isUser: boolean;
	date: Date;
	clusterId: string;
	deskId: string;
	timeSlot: number;
}

export interface IDeskReservationsDay {
	occupied: IDeskReservationItem[];
	userReservations: IDeskReservationItem[];
}

interface IDeskCluster {
	clusterId: string;
	desks: IDesk[];
}

export interface IDesk {
	clusterId: string;
	deskId: string;
	occupied: boolean[];
	userReservations: boolean[];
}

export interface IDeskReservation {
	date: Date;
	clusterId: string;
	deskId: string;
	timeSlot: number;
}

// async function GetDeskReservationForOfficeDate(office: string, startDate?: Date, endDate?: Date): Promise<IActionResult<{ [key: string]: IDeskCluster[] }>> {
// 	try {
// 		const res = await fetch(
// 			`http://localhost:3002/desk/reservation/${office}?startDate=${startDate ? startDate.toISOString().split('T')[0] : ""}&endDate=${endDate ? endDate.toISOString().split('T')[0] : ""}`,
// 			{
// 				method: "GET",
// 				credentials: "include", // Include credentials (cookies) in the request
// 			}
// 		);
// 		return InspectResponseAsync<{ [key: string]: IDeskCluster[] }>(res);
// 	} catch (error) {
// 		console.error("Error:", error);
// 		return { success: false, statusText: "Request could not be send." };
// 	}
// }

async function GetDeskReservationOfficeLayout(office: string): Promise<IActionResult<{ [key: string]: IDeskCluster }>> {
	try {
		const res = await fetch(
			`http://localhost:3002/desk/reservation/${office}/layout`,
			{
				method: "GET",
				credentials: "include", // Include credentials (cookies) in the request
			}
		);
		return InspectResponseAsync<{ [key: string]: IDeskCluster }>(res);
	} catch (error) {
		console.error("Error:", error);
		return { success: false, statusText: "Request could not be send." };
	}
}

async function GetDeskReservationsForOfficeDate(office: string, startDate?: Date, endDate?: Date): Promise<IActionResult<{ [key: string]: IDeskReservationsDay }>> {
	try {
		const res = await fetch(
			`http://localhost:3002/desk/reservation/${office}/all?startDate=${startDate ? startDate.toISOString().split('T')[0] : ""}&endDate=${endDate ? endDate.toISOString().split('T')[0] : ""}`,
			{
				method: "GET",
				credentials: "include", // Include credentials (cookies) in the request
			}
		);
		return InspectResponseAsync<{ [key: string]: IDeskReservationsDay }>(res);
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
export { GetDeskReservationOfficeLayout, GetDeskReservationsForOfficeDate, GetDeskReservationsOfUser, PostDeskReservation, DeleteDeskReservation }
