import { App, ExpressReceiver, LogLevel } from "@slack/bolt";
import { FileInstallationStore } from "@slack/oauth"
import JsonInstallationStore from "./JsonInstallationStore"
import 'dotenv/config'

const SLACK_SIGNING_SECRET = process.env.SLACK_SIGNING_SECRET;
const SLACK_CLIENT_ID = process.env.SLACK_CLIENT_ID;
const SLACK_CLIENT_SECRET = process.env.SLACK_CLIENT_SECRET;


// Initializes your app with your bot token and signing secret
const receiver = new ExpressReceiver({
  signingSecret: SLACK_SIGNING_SECRET,
  clientId: SLACK_CLIENT_ID,
  clientSecret: SLACK_CLIENT_SECRET,
  stateSecret: 'my-state-secret',
  scopes: ["chat:write"],
  // redirectUri: "https://better-terms-crash.loca.lt/slack/auth/callback", // here
  installerOptions: {
    redirectUriPath: "/slack/auth/callback", // and here!
    stateVerification: false,
    userScopes: [
      "channels:history",
      "channels:read",
      "chat:write",
      "groups:read",
      "im:history",
      "im:read",
      "im:write",
      "mpim:history",
      "mpim:read",
      "users.profile:read",
      "users.profile:write",
    ],
  },
  installationStore: new JsonInstallationStore(),
});

receiver.router.get('/slack/auth/callback', async (req, res) => {
  const { code } = req.query;
  try {
    const result = await app.client.oauth.v2.access({
      client_id: SLACK_CLIENT_ID,
      client_secret: SLACK_CLIENT_SECRET,
      code: code as string,
    });
    const userToken = result.access_token;

    // Now you have the user's access token
    console.log('User Access Token:', userToken);

    // You can save it for later use, typically in a database
    // or associate it with the user's account

    res.send('OAuth process completed successfully!');
  } catch (error) {
    console.error('OAuth error:', error);
    res.status(500).send('OAuth process failed.');
  }
});

receiver.router.get('/slack/auth/test', async (req, res) => {
  const { code } = req.query;
  try {
    // Now you have the user's access token
    console.log('TEST:', code);

    // You can save it for later use, typically in a database
    // or associate it with the user's account

    res.send('Test successfull');
  } catch (error) {
    console.error('Test error:', error);
    res.status(500).send('Test process failed.');
  }
});

// Create the Bolt App, using the receiver
const app = new App({
  receiver,
  logLevel: LogLevel.DEBUG, // set loglevel at the App level
});

(async () => {
  // Start your app
  await app.start(3001);

  console.log("⚡️ Bolt app is running!");
})();