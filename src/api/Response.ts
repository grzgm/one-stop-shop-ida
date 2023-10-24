export interface IActionResult<T> {
  success: boolean;
  status: string;
  payload?: T;
}

async function InspectResponseAsync<T>(
  res: Response
): Promise<IActionResult<T>> {
  const payload = await res.json();
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
  if (res.ok) {
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
