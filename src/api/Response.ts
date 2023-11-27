export interface IActionResult<T> {
	success: boolean;
	status: string;
	payload?: T;
}

async function InspectResponseAsync<T>(
	res: Response
): Promise<IActionResult<T>> {
	let payload = undefined;
	try {
		payload = await res.json();
	} catch (error) {
		console.error(
			"Error while parsing response in InspectResponseAsync function: \n",
			error
		);
	}

	if (res.ok) {
		// Handle successful response (status code 200-299)
		return {
			success: true,
			status: "Request has been sent correctly.",
			payload: payload,
		};
	}
	// Handle non-successful response (status code outside 200-299)
	console.error("HTTP error! status: ", res.status);
	return {
		success: false,
		status: "Request could not be send.",
		payload: payload,
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
			status: "Request has been sent correctly.",
			payload: res,
		};
	}
	// Handle non-successful response (status code outside 200-299)
	console.error("HTTP error! status: ", res.status);
	return { success: false, status: "Request could not be send.", payload: res };
}

export { InspectResponseAsync, InspectResponseSync };
