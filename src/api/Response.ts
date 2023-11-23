export interface IActionResult<T> {
	success: boolean;
	statusText: string;
	payload?: T;
}

async function InspectResponseAsync<T>(
	res: Response
): Promise<IActionResult<T>> {
	// Handle successful response (status code 200-299)
	if (res.ok) {
		let payload = undefined;
		// Try to access payload
		try {
			payload = await res.json();
		} catch (error) {
			console.error(
				"Error while parsing response in InspectResponseAsync function: \n",
				error
			);
		}
		return {
			success: true,
			statusText: "Request has been sent correctly.",
			payload: payload,
		};
	}
	// Handle non-successful response (status code outside 200-299)
	console.error("HTTP error! status: ", res.status);
	return {
		success: false,
		statusText: res.statusText,
	};
}

function InspectResponseSync<T>(res: any): IActionResult<T> {
	// Handle successful response (status code 200-299)
	if (res.ok) {
		let payload = undefined;
		// Try to access payload
		try {
			payload = res.json();
		} catch (error) {
			console.error(
				"Error while parsing response in InspectResponseAsync function: \n",
				error
			);
		}
		return {
			success: true,
			statusText: "Request has been sent correctly.",
			payload: payload,
		};
	}
	// Handle non-successful response (status code outside 200-299)
	console.error("HTTP error! status: ", res.status);
	return {
		success: false,
		statusText: res.statusText,
	};
}

export { InspectResponseAsync, InspectResponseSync };
