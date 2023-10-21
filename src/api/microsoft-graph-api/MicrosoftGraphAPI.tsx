export interface IActionResult {
  success: boolean;
  status: string;
}

function InspectResponse(res: Response) {
  if (res.ok) {
    // Handle successful response (status code 200-299)
    return { success: true, status: "Request has been sent correctly." };
  }
  // Handle non-successful response (status code outside 200-299)
  console.error("HTTP error! status: ", res.status);
  return { success: false, status: "Request could not be send." };
}

async function IsAuth() {
  try {
    const res = await fetch(
      `http://localhost:3002/microsoft/auth/check-token`,
      {
        method: "GET",
        credentials: "include", // Include credentials (cookies) in the request
      }
    );
    return res.json();
  } catch (error) {
    console.error("Error:", error);
    return false;
  }
}

async function SendEmail(message: string, address: string): Promise<IActionResult> {
  try {
    const res = await fetch(
      `http://localhost:3002/microsoft/resources/send-email?message=${encodeURI(message)}&address=${encodeURI(address)}`,
      {
        method: "POST",
        credentials: "include", // Include credentials (cookies) in the request
      }
    );
    return InspectResponse(res);
  } catch (error) {
    console.error("Error:", error);
    return { success: false, status: "Request could not be send." };
  }
}

export { IsAuth, SendEmail, InspectResponse };
