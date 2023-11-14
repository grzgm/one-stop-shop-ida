import Button from "../Buttons";
import { BodyNormal, BodySmall, HeadingLarge } from "../text-wrapers/TextWrapers";
import { useEffect, useState } from 'react';

// Convert a base64 string to Uint8Array.
// Must do this so the server can understand the VAPID_PUBLIC_KEY.
function urlB64ToUint8Array(base64String: string) {
	const padding = '='.repeat((4 - base64String.length % 4) % 4);
	const base64 = (base64String + padding)
		.replace(/\-/g, '+')
		.replace(/_/g, '/');
	const rawData = window.atob(base64);
	const outputArray = new Uint8Array(rawData.length);
	for (let i = 0; i < rawData.length; ++i) {
		outputArray[i] = rawData.charCodeAt(i);
	}
	return outputArray;
}

function Push() {
	const [subscribeButton, setSubscribeButton] = useState<boolean>(false)
	const [unsubscribeButton, setUnsubscribeButton] = useState<boolean>(false)
	const [notifyMeButton, setnotifyMeButton] = useState<boolean>(false)
	const [notifyAllButton, setnotifyAllButton] = useState<boolean>(false)

	const VAPID_PUBLIC_KEY = import.meta.env.VITE_VAPID_PUBLIC_KEY ? import.meta.env.VITE_VAPID_PUBLIC_KEY : "";

	useEffect(() => {
		// TODO add startup logic here
		if ('serviceWorker' in navigator && 'PushManager' in window) {
			navigator.serviceWorker.register('./service-worker.js').then(serviceWorkerRegistration => {
				console.info('Service worker was registered.');
				console.info({ serviceWorkerRegistration });
			}).catch(error => {
				console.error('An error occurred while registering the service worker.');
				console.error(error);
			});
			setSubscribeButton(false)
		} else {
			console.error('Browser does not support service workers or push messages.');
		}
	}, []);

	async function subscribeButtonHandler() {
		setSubscribeButton(true);
		const result = await Notification.requestPermission();
		if (result === 'denied') {
			console.error('The user explicitly denied the permission request.');
			return;
		}
		if (result === 'granted') {
			console.info('The user accepted the permission request.');
		}
		const registration = await navigator.serviceWorker.getRegistration();
		const subscribed = await registration!.pushManager.getSubscription();
		if (subscribed) {
			console.info('User is already subscribed.');
			setnotifyMeButton(false)
			setUnsubscribeButton(false)
			return;
		}
		const subscription = await registration!.pushManager.subscribe({
			userVisibleOnly: true,
			applicationServerKey: urlB64ToUint8Array(VAPID_PUBLIC_KEY)
		});
		setnotifyMeButton(false)
		fetch('http://localhost:3002/push/subscribe', {
			method: 'POST',
			headers: {
				'Content-Type': 'application/json'
			},
			body: JSON.stringify({
				subscription: subscription
			})
		});

	}

	async function unsubscribeButtonHandler() {
		const registration = await navigator.serviceWorker.getRegistration();
		const subscription = await registration!.pushManager.getSubscription();
		fetch('http://localhost:3002/push/unsubscribe', {
			method: 'POST',
			headers: {
				'Content-Type': 'application/json'
			},
			body: JSON.stringify({
				subscription: subscription
			})
		});
		const unsubscribed = await subscription!.unsubscribe();
		if (unsubscribed) {
			console.info('Successfully unsubscribed from push notifications.');
			setUnsubscribeButton(true);
			setSubscribeButton(false);
			setnotifyMeButton(true);
		}

	}

	const NotifyMeButtonHandler = async () => {
		const registration = await navigator.serviceWorker.getRegistration();
		const subscription = await registration!.pushManager.getSubscription();
		fetch('http://localhost:3002/push/send/5e430c04-3186-4560-bdb2-6ecf691047a3', {
			method: 'POST',
			headers: {
				'Content-Type': 'application/json'
			},
			body: JSON.stringify({
				title: "title",
				body: "message",
			})
		});
	}

	const NotifyAllButtonHandler = async () => {
		const response = await fetch('/notify-all', {
			method: 'POST'
		});
	}

	return (
		<div className="content">
			<div className="description">
				<HeadingLarge>Login with your</HeadingLarge>
				<HeadingLarge>Slack Account</HeadingLarge>
				<BodyNormal>Get access to all the benefits of app!</BodyNormal>
			</div>
			<main className="slack-auth-main">
			</main>
			<Button child="subscribe" onClick={subscribeButtonHandler} disabled={subscribeButton} />
			<Button child="unsubscribe" onClick={unsubscribeButtonHandler} disabled={unsubscribeButton} />
			<Button child="notify me" onClick={NotifyMeButtonHandler} disabled={notifyMeButton} />
			<Button child="notify all" onClick={NotifyAllButtonHandler} disabled={notifyAllButton} />
		</div>
	);
}

export default Push;
