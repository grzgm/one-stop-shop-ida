export interface IActionResult<T> {
	success: boolean;
	statusCode?: number;
	statusText: string;
	payload?: T;
}

async function InspectResponseAsync<T>(
	res: Response
): Promise<IActionResult<T>> {
	// Handle successful response (status code 200-299)
	if (res.ok) {
		let payload = undefined
		const statusText = await res.text();
		if(isJsonString(statusText)) payload = JSON.parse(statusText)
		return {
			success: true,
			statusCode: res.status,
			statusText: statusText,
			payload: payload,
		};
	}
	// Handle non-successful response (status code outside 200-299)
	console.error("HTTP error! status: ", res.status);
	return {
		success: false,
		statusCode: res.status,
		statusText: res.statusText,
	};
}

function InspectResponseSync<T>(res: any): IActionResult<T> {
	let payload = undefined;
	try {
		payload = res.json();
	} catch (error) {
		console.error(
			"Error while parsing response in InspectResponseAsync function: \n",
			error
		);
	}

	if (200 <= res.StatusCode && res.StatusCode <= 299) {
		// Handle successful response (status code 200-299)
		return {
			success: true,
			statusCode: res.status,
			statusText: "Request has been sent correctly.",
			payload: payload,
		};
	}
	// Handle non-successful response (status code outside 200-299)
	console.error("HTTP error! status: ", res.status);
	return {
		success: false,
		statusCode: res.status,
		statusText: res.statusText,
	};
}

function isJsonString(str: string) {
    try {
        JSON.parse(str);
    } catch (e) {
        return false;
    }
    return true;
}

export { InspectResponseAsync, InspectResponseSync };
