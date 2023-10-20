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

async function SendEmail() {
  try {
    const res = await fetch(
      `http://localhost:3002/microsoft/resources/send-email?message=lunchTest&address=sander.vanbeek@weareida.digital`,
      {
        method: "POST",
        credentials: "include", // Include credentials (cookies) in the request
      }
    );
    if (res.ok) {
      // Handle successful response (status code 200-299)
      return "Request has been sent correctly.";
    } else {
      // Handle non-successful response (status code outside 200-299)
      console.error("HTTP error! status: ", res.status);
      return "Request could not be send.";
    }
  } catch (error) {
    console.error("Error:", error);
	return "Request could not be send.";
  }
}

export { IsAuth, SendEmail };
