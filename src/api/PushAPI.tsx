import { IActionResult, InspectResponseAsync } from "./Response";

async function PostSubscribe(): Promise<IActionResult<boolean>> {
	const result = await Notification.requestPermission();
	if (result === "denied") {
		console.error("The user explicitly denied the permission request.");
		return { success: false, status: "The user explicitly denied the permission request." }
	}
	if (result === "granted") {
		console.info("The user accepted the permission request.");
	}
	const registration = await navigator.serviceWorker.getRegistration();
	const subscribed = await registration!.pushManager.getSubscription();
	if (subscribed) {
		const isSubscribed = await GetIsSubscribed();
		if ( isSubscribed.payload && isSubscribed.payload == true)
		{
			console.info("User is already subscribed.");
			return { success: true, status: "User is already subscribed." }
		}
		console.info("User was not in database.");
	}
	const subscription = await registration!.pushManager.subscribe({
		userVisibleOnly: true,
		applicationServerKey: urlB64ToUint8Array(import.meta.env.VITE_VAPID_PUBLIC_KEY
			? import.meta.env.VITE_VAPID_PUBLIC_KEY
			: ""),
	});
	try {
		fetch("http://localhost:3002/push/subscribe", {
			method: "POST",
			credentials: "include", // Include credentials (cookies) in the request
			headers: {
				"Content-Type": "application/json",
			},
			body: JSON.stringify(subscription),
		});
		console.info("User has been subscribed.");
		return { success: true, status: "User has been subscribed." }
	}
	catch (error) {
		console.error("Error:", error);
		return { success: false, status: "Failed to connect to server." }
	}
}

async function GetIsSubscribed(): Promise<IActionResult<boolean>> {
	try {
		const res = await fetch("http://localhost:3002/push/is-subscribed", {
			method: "GET",
			credentials: "include", // Include credentials (cookies) in the request
		});
		return InspectResponseAsync(res);
	}
	catch (error) {
		console.error("Error:", error);
		return { success: false, status: "Failed to connect to server." }
	}
}

// async function PostUnsubscribe() {
// 	const registration = await navigator.serviceWorker.getRegistration();
// 	const subscription = await registration!.pushManager.getSubscription();
// 	fetch("http://localhost:3002/push/unsubscribe", {
// 		method: "POST",
//		credentials: "include", // Include credentials (cookies) in the request
// 		headers: {
// 			"Content-Type": "application/json",
// 		},
// 		body: JSON.stringify(subscription),
// 	});
// 	const unsubscribed = await subscription!.unsubscribe();
// 	if (unsubscribed) {
// 		console.info("Successfully unsubscribed from push notifications.");
// 	}
// }

// Convert a base64 string to Uint8Array.
// Must do this so the server can understand the VAPID_PUBLIC_KEY.
function urlB64ToUint8Array(base64String: string) {
	const padding = "=".repeat((4 - (base64String.length % 4)) % 4);
	const base64 = (base64String + padding)
		.replace(/\-/g, "+")
		.replace(/_/g, "/");
	const rawData = window.atob(base64);
	const outputArray = new Uint8Array(rawData.length);
	for (let i = 0; i < rawData.length; ++i) {
		outputArray[i] = rawData.charCodeAt(i);
	}
	return outputArray;
}

export { PostSubscribe, GetIsSubscribed };
