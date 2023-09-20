import express from "express";
import axios from "axios";
import FormData from "form-data";
import fs from "fs";

const SLACK_SIGNING_SECRET = "acf31a7376b19c7d55fafabfd3bed248";
const SLACK_CLIENT_ID = "12922529104.5894776759175";
const SLACK_CLIENT_SECRET = "cf00d1d6730ca5b040d79309530ecef6";

const SRCDIR = "src/";

let access_token = "";

const app = express();
const port = 3001;

// Middleware to parse JSON body
app.use(express.json());

// OAuth Callback URL has to be the same as in the Slack Application Panel under OAuth page
// const redirectUri = "http://localhost:3001/slack/auth/callback";
const redirectUri = "https://real-hands-tickle.loca.lt/slack/auth/callback";

// OAuth Step 1: Redirect users to Slack's authorization URL
app.get("/slack/auth", (req, res) => {
    const authUrl = `https://slack.com/oauth/v2/authorize?client_id=${SLACK_CLIENT_ID}&&scope=&user_scope=channels%3Ahistory%2Cchannels%3Aread%2Cchat%3Awrite%2Cgroups%3Aread%2Cim%3Ahistory%2Cim%3Aread%2Cim%3Awrite%2Cmpim%3Ahistory%2Cmpim%3Aread%2Cusers.profile%3Aread%2Cusers.profile%3Awrite&redirect_uri=${redirectUri}`;
    res.redirect(authUrl);
});

// OAuth Step 2: Handle the OAuth callback
app.get("/slack/auth/callback", async (req, res) => {
    const { code } = req.query;
    try {
        const formData = new FormData();
        formData.append("code", code as string);
        formData.append("client_id", SLACK_CLIENT_ID);
        formData.append("client_secret", SLACK_CLIENT_SECRET);

        const config = {
            headers: {
                "Content-Type": "multipart/form-data",
            },
        };
        const response = await axios.post(
            "https://slack.com/api/oauth.v2.access",
            formData,
            config
        );

        console.log(response);

        access_token = response.data.authed_user.access_token;

        // Now you have the user's access token
        console.log("User Access Token:", access_token);

        // You can save it for later use, typically in a database
        // or associate it with the user's account

        res.send("OAuth process completed successfully!");
    } catch (error) {
        console.error("OAuth error:", error);
        res.status(500).send("OAuth process failed.");
    }
});

// Message endpoint
app.get("/slack/auth/send-message", async (req, res) => {
    const { mess, conversations } = req.query;
    console.log(mess, conversations);
    console.log(access_token);

    try {
        const data = {
            text: mess,
            channel: conversations,
        };
        const config = {
            headers: {
                Authorization: `Bearer ${access_token}`,
            },
        };

        const response = await axios.post(
            "https://slack.com/api/chat.postMessage",
            data,
            config
        );

        // Now you have the code received from Slack
        // You can save it for later use, typically in a database
        // or associate it with the user's account

        res.send(`Message successful. \n ${response}`);
    } catch (error) {
        console.error("Sending Message error:", error);
        res.status(500).send(`Process of Sending Message failed. \n ${error}`);
    }
});

// Start the Express server
app.listen(port, () => {
    console.log(`⚡️ Express server is running on port ${port}`);
});

function save_authed_user(obj: Object): void {
    const json = JSON.stringify(obj);
    fs.writeFile(SRCDIR + "authed_user.json", json, "utf8", (err) => {
        if (err) throw err;
    });
}
