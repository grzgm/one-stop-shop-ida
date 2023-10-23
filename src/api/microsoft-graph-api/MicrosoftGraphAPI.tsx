export interface IActionResult<T> {
  success: boolean;
  status: string;
  payload?: T;
}

async function InspectResponseAsync<T>(res: Response): Promise<IActionResult<T>> {
  const payload = await res.json();
  if (res.ok) {
    // Handle successful response (status code 200-299)
    return { success: true, status: "Request has been sent correctly.", payload: payload };
  }
  // Handle non-successful response (status code outside 200-299)
  console.error("HTTP error! status: ", res.status);
  return { success: false, status: "Request could not be send.", payload: payload};
}

function InspectResponseSync<T>(res: any): IActionResult<T> {
  if (res.ok) {
    // Handle successful response (status code 200-299)
    return { success: true, status: "Request has been sent correctly.", payload: res };
  }
  // Handle non-successful response (status code outside 200-299)
  console.error("HTTP error! status: ", res.status);
  return { success: false, status: "Request could not be send.", payload: res};
}

async function IsAuth(): Promise<IActionResult<boolean>> {
  try {
    const res = await fetch(
      `http://localhost:3002/microsoft/auth/check-token`,
      {
        method: "GET",
        credentials: "include", // Include credentials (cookies) in the request
      }
    );
    return InspectResponseAsync(res);
  } catch (error) {
    console.error("Error:", error);
    return { success: false, status: "Request could not be send." };
  }
}

async function SendEmail(message: string, address: string): Promise<IActionResult<null>> {
  try {
    const res = await fetch(
      `http://localhost:3002/microsoft/resources/send-email?message=${encodeURI(message)}&address=${encodeURI(address)}`,
      {
        method: "POST",
        credentials: "include", // Include credentials (cookies) in the request
      }
    );
    return InspectResponseAsync(res);
  } catch (error) {
    console.error("Error:", error);
    return { success: false, status: "Request could not be send." };
  }
}

export { IsAuth, SendEmail, InspectResponseAsync, InspectResponseSync };
