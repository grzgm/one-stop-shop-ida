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
		const res: { [key: string]: IDeskCluster } = {
			"0": {
				clusterId: "0",
				desks: [
					{ clusterId: "0", deskId: "0", occupied: [false, false, false], userReservations: [false, false, false] },
					{ clusterId: "0", deskId: "1", occupied: [false, false, false, false], userReservations: [false, false, false, false] },
					{ clusterId: "0", deskId: "2", occupied: [false, false, false, false], userReservations: [false, false, false, false] },
					{ clusterId: "0", deskId: "3", occupied: [false, false, false], userReservations: [false, false, false] },
				],
			},
			"1": {
				clusterId: "1",
				desks: [
					{ clusterId: "1", deskId: "0", occupied: [false, false, false], userReservations: [false, false, false] },
					{ clusterId: "1", deskId: "1", occupied: [false, false, false], userReservations: [false, false, false] },
					{ clusterId: "1", deskId: "2", occupied: [false, false], userReservations: [false, false] },
					{ clusterId: "1", deskId: "3", occupied: [false, false, false], userReservations: [false, false, false] },
				],
			},
		};
		return {
			success: true,
			statusCode: 200,
			statusText: "OK",
			payload: res,
		};
		// return await ExecuteApiCall<{ [key: string]: IDeskCluster }>(`/desk/reservation/${office}/layout`, "GET");
	} catch (error) {
		console.error("Error:", error);
		return { success: false, statusText: "Request could not be send." };
	}
}

async function GetDeskReservationsForOfficeDate(office: string, startDate?: Date, endDate?: Date): Promise<IActionResult<{ [key: string]: IDeskReservationsDay }>> {
	try {
		const counterDate = new Date();
		const todayDateIndex = counterDate.toISOString().split('T')[0] + 'T00:00:00';

		const payload: { [key: string]: IDeskReservationsDay } = {};

		payload[todayDateIndex] = {
			occupied: [{
				isUser: false,
				date: counterDate,
				clusterId: "0",
				deskId: "2",
				timeSlot: 2,
			}, {
				isUser: false,
				date: counterDate,
				clusterId: "1",
				deskId: "0",
				timeSlot: 0,
			}, {
				isUser: false,
				date: counterDate,
				clusterId: "1",
				deskId: "0",
				timeSlot: 1,
			}, {
				isUser: false,
				date: counterDate,
				clusterId: "1",
				deskId: "0",
				timeSlot: 2,
			}, {
				isUser: false,
				date: counterDate,
				clusterId: "1",
				deskId: "3",
				timeSlot: 0,
			}, {
				isUser: false,
				date: counterDate,
				clusterId: "1",
				deskId: "3",
				timeSlot: 2,
			},],
			userReservations: [{
				isUser: true,
				date: counterDate,
				clusterId: "1",
				deskId: "3",
				timeSlot: 1,
			},]
		};

		for (let i = 1; i <= 14; i++) {
			counterDate.setDate(counterDate.getDate() + 1);
			let nextDayDateIndex = counterDate.toISOString().split('T')[0] + 'T00:00:00';

			payload[nextDayDateIndex] = {
				occupied: [],
				userReservations: []
			}
		}

		return {
			success: true,
			statusCode: 200,
			statusText: "OK",
			payload: payload,
		}

		// return await ExecuteApiCall<{ [key: string]: IDeskReservationsDay }>(`/desk/reservation/${office}/all?startDate=${startDate ? startDate.toISOString().split('T')[0] : ""}&endDate=${endDate ? endDate.toISOString().split('T')[0] : ""}`, "GET");
	} catch (error) {
		console.error("Error:", error);
		return { success: false, statusText: "Request could not be send." };
	}
}

// async function GetDeskReservationsOfUser(office: string, date: Date): Promise<IActionResult<IDeskReservation[]>> {
// 	try {
// 		return await ExecuteApiCall<IDeskReservation[]>(`/desk/reservation/${office}/user?date=${date.toISOString().split('T')[0]}`, "GET");
// 	} catch (error) {
// 		console.error("Error:", error);
// 		return { success: false, statusText: "Request could not be send." };
// 	}
// }

async function PostDeskReservation(office: string, date: Date, clusterId: string, deskId: string, timeSlots: number[]): Promise<IActionResult<undefined>> {
	try {
		return {
			success: true,
			statusCode: 200,
			statusText: "OK"
		}
		// return await ExecuteApiCall<undefined>(`/desk/reservation/${office}?date=${date.toISOString().split('T')[0]}&clusterId=${clusterId}&deskId=${deskId}&timeSlots=${timeSlots.join("&timeSlots=")}`, "POST");
	} catch (error) {
		console.error("Error:", error);
		return { success: false, statusText: "Request could not be send." };
	}
}

async function DeleteDeskReservation(office: string, date: Date, clusterId: string, deskId: string, timeSlots: number[]): Promise<IActionResult<undefined>> {
	try {
		return {
			success: true,
			statusCode: 200,
			statusText: "OK"
		}
		// return await ExecuteApiCall<undefined>(`/desk/reservation/${office}?date=${date.toISOString().split('T')[0]}&clusterId=${clusterId}&deskId=${deskId}&timeSlots=${timeSlots.join("&timeSlots=")}`, "DELETE");
	} catch (error) {
		console.error("Error:", error);
		return { success: false, statusText: "Request could not be send." };
	}
}

// export { GetDeskReservationForOfficeDate, PutLunchRecurringItem, RegisterLunchRecurring };
// export { GetDeskReservationOfficeLayout, GetDeskReservationsForOfficeDate, GetDeskReservationsOfUser, PostDeskReservation, DeleteDeskReservation }
export { GetDeskReservationOfficeLayout, GetDeskReservationsForOfficeDate, PostDeskReservation, DeleteDeskReservation }
