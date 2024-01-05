self.addEventListener("install", event => {
  console.log("Service worker installed");
});
self.addEventListener("activate", event => {
  console.log("Service worker activated");
});

self.addEventListener("push", (event) => {
  console.log("Recived Push")
  if (event.data) {
    const { title, lang = 'en', body, tag, timestamp, requireInteraction, actions, image } = event.data.json();

    const promiseChain = self.registration.showNotification(title, {
        lang,
        body,
        requireInteraction,
        tag: tag || undefined,
        timestamp: timestamp ? Date.parse(timestamp) : undefined,
        actions: actions || undefined,
        image: image || "pwa-64x64.png"
    });
    console.log(promiseChain)

    // Ensure the toast notification is displayed before exiting this function
    event.waitUntil(promiseChain);
}
});

self.addEventListener("notificationclick", (event) => {
  event.notification.close();

  if (event.action === 'register') {
    event.waitUntil(self.clients.openWindow(`${import.meta.env.VITE_FRONTEND_URI}/office-details/lunch`));
  }
  event.waitUntil(self.clients.openWindow(`${import.meta.env.VITE_FRONTEND_URI}`));
});
