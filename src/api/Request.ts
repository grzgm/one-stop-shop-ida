import Cookies from "universal-cookie";
import { InspectResponseAsync } from "./Response";
import { redirect } from "react-router-dom";


async function ExecuteApiCall<T>(endpointPath: string, method: string, body?: string) {
    body = body ?? '';

    const res = await fetch(
        `${import.meta.env.VITE_BACKEND_URI}${endpointPath}`,
        {
            method: method,
            credentials: "include", // Include credentials (cookies) in the request
            headers: {
                'Authorization': `Bearer ${new Cookies().get("jwt")}`,
                'Content-Type': 'application/json',
                'Body': body,
            }
        }
    );

    const inspectedRes = await InspectResponseAsync<T>(res);

    if (inspectedRes.statusCode == 401) {
        redirect(window.location.pathname)
    }
    return inspectedRes
}

export default ExecuteApiCall;