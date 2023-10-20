import express from "express";
import axios from "axios";
import FormData from "form-data";
import fs from "fs";
import 'dotenv/config'

const SLACK_SIGNING_SECRET = process.env.SLACK_SIGNING_SECRET;
const SLACK_CLIENT_ID = process.env.SLACK_CLIENT_ID;
const SLACK_CLIENT_SECRET = process.env.SLACK_CLIENT_SECRET;

const SRCDIR = "src/";

let ACCESS_TOKEN = "";

const app = express();
const port = 3001;

// Middleware to parse JSON body
app.use(express.json());

// OAuth Callback URL has to be the same as in the Slack Application Panel under OAuth page
// const redirectUri = `http://localhost:${port}/slack/auth/callback`;
const redirectUri = "https://modern-carrots-rule.loca.lt/slack/auth/callback";

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

        ACCESS_TOKEN = response.data.authed_user.access_token;

        console.log("User Access Token:", ACCESS_TOKEN);

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

    try {
        const data = {
            text: mess,
            channel: conversations,
        };
        const config = {
            headers: {
                Authorization: `Bearer ${ACCESS_TOKEN}`,
            },
        };

        const response = await axios.post(
            "https://slack.com/api/chat.postMessage",
            data,
            config
        );

        res.send(`Message successful. \n ${response}`);
    } catch (error) {
        console.error("Sending Message error:", error);
        res.status(500).send(`Process of Sending Message failed. \n ${error}`);
    }
});

// Status endpoint
app.get("/slack/auth/set-status", async (req, res) => {
    const date = new Date();
    date.setDate(date.getDate() + 1);

    try {
        const data = {
            profile: {
                status_text: "testing",
                status_emoji: ":train:",
                status_expiration: date,
            },
        };
        const config = {
            headers: {
                Authorization: `Bearer ${ACCESS_TOKEN}`,
            },
        };

        const response = await axios.post(
            "https://slack.com/api/users.profile.set",
            data,
            config
        );

        res.send(`Message successful. \n ${response}`);
    } catch (error) {
        console.error("Sending Message error:", error);
        res.status(500).send(`Process of Sending Message failed. \n ${error}`);
    }
});

// Start the Express server
app.listen(port, () => {
    console.log(`⚡️ Express server is running on port ${port}`);
    console.log(`⚡️ Link: http://localhost:${port}/slack/auth/callback`);
});

function save_authed_user(obj: Object): void {
    const json = JSON.stringify(obj);
    fs.writeFile(SRCDIR + "authed_user.json", json, "utf8", (err) => {
        if (err) throw err;
    });
}
