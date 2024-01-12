import { IActionResult } from "./Response";
import ExecuteApiCall from "./Request";

export interface IOfficeFeatures {
    officeName: string;
    canReserveDesk: Boolean;
    canRegisterLunch: Boolean;
    canRegisterPresence: Boolean;
    officeInformation: {
      address: string;
      coords: {
        lat: number;
        lng: number;
      };
      openingHours: string;
      accessInformation: string;
      parkingInformation: string;
      lunchInformation: string;
    };
  }

async function GetOfficeFeaturesItems(office: string): Promise<IActionResult<IOfficeFeatures>> {
	try {
		return await ExecuteApiCall<IOfficeFeatures>(`/offices/${office}`, "GET");
	} catch (error) {
		console.error("Error:", error);
		return { success: false, statusText: "Request could not be send." };
	}
}

async function GetAllOfficeFeaturesItems(): Promise<IActionResult<IOfficeFeatures[]>> {
	try {
		return await ExecuteApiCall<IOfficeFeatures[]>(`/offices/all`, "GET");
	} catch (error) {
		console.error("Error:", error);
		return { success: false, statusText: "Request could not be send." };
	}
}

export { GetOfficeFeaturesItems, GetAllOfficeFeaturesItems };
