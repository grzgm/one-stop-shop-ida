if ('serviceWorker' in navigator) {
  window.addEventListener('load', () => {
    navigator.serviceWorker.register('/sw-push.js', { scope: '/' })
  })
}

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
        image: image || undefined
    });

    // Ensure the toast notification is displayed before exiting this function
    event.waitUntil(promiseChain);
}
});

self.addEventListener("notificationclick", (event) => {
  // TODO
  event.notification.close();
  event.waitUntil(self.clients.openWindow("https://web.dev"));
});
