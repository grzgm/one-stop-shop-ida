import { IActionResult } from "./Response";
import ExecuteApiCall from "./Request";
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

async function GetDeskReservationOfficeLayout(office: string): Promise<IActionResult<{ [key: string]: IDeskCluster }>> {
	try {
		return ExecuteApiCall<{ [key: string]: IDeskCluster }>(`/desk/reservation/${office}/layout`, "GET");
	} catch (error) {
		console.error("Error:", error);
		return { success: false, statusText: "Request could not be send." };
	}
}

async function GetDeskReservationsForOfficeDate(office: string, startDate?: Date, endDate?: Date): Promise<IActionResult<{ [key: string]: IDeskReservationsDay }>> {
	try {
		return ExecuteApiCall<{ [key: string]: IDeskReservationsDay }>(`/desk/reservation/${office}/all?startDate=${startDate ? startDate.toISOString().split('T')[0] : ""}&endDate=${endDate ? endDate.toISOString().split('T')[0] : ""}`, "GET");
	} catch (error) {
		console.error("Error:", error);
		return { success: false, statusText: "Request could not be send." };
	}
}

async function GetDeskReservationsOfUser(office: string, date: Date): Promise<IActionResult<IDeskReservation[]>> {
	try {
		return ExecuteApiCall<IDeskReservation[]>(`/desk/reservation/${office}/user?date=${date.toISOString().split('T')[0]}`, "GET");
	} catch (error) {
		console.error("Error:", error);
		return { success: false, statusText: "Request could not be send." };
	}
}

async function PostDeskReservation(office: string, date: Date, clusterId: string, deskId: string, timeSlots: number[]): Promise<IActionResult<undefined>> {
	try {
		return ExecuteApiCall<undefined>(`/desk/reservation/${office}?date=${date.toISOString().split('T')[0]}&clusterId=${clusterId}&deskId=${deskId}&timeSlots=${timeSlots.join("&timeSlots=")}`, "POST");
	} catch (error) {
		console.error("Error:", error);
		return { success: false, statusText: "Request could not be send." };
	}
}

async function DeleteDeskReservation(office: string, date: Date, clusterId: string, deskId: string, timeSlots: number[]): Promise<IActionResult<undefined>> {
	try {
		return ExecuteApiCall<undefined>(`/desk/reservation/${office}?date=${date.toISOString().split('T')[0]}&clusterId=${clusterId}&deskId=${deskId}&timeSlots=${timeSlots.join("&timeSlots=")}`, "DELETE");
	} catch (error) {
		console.error("Error:", error);
		return { success: false, statusText: "Request could not be send." };
	}
}

// export { GetDeskReservationForOfficeDate, PutLunchRecurringItem, RegisterLunchRecurring };
export { GetDeskReservationOfficeLayout, GetDeskReservationsForOfficeDate, GetDeskReservationsOfUser, PostDeskReservation, DeleteDeskReservation }
