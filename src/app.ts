import express from "express";
import axios from "axios";
import cors from "cors";
import { codeVerifier, codeChallenge, save_authed_user } from "./helper";

const MY_MAIL = ""
const MICROSOFT_CLIENT_ID = "";
const TENANT = "organizations";
const SCOPES = "offline_access user.read mail.read mail.send calendars.readwrite";
let ACCESS_TOKEN = "";
let REFRESH_TOKEN = "";


// Server setup
const PORT = 3002;
const app = express();

// Middleware 
// Cross-Origin Resource Sharing
app.use(cors());
// Parse JSON body
app.use(express.json());

// OAuth Callback URL has to be the same as in the Microsoft Application Panel under OAuth page
const redirectUri = "http://localhost:3002/microsoft/auth/callback";

// OAuth Step 1: Redirect users to microsoft's authorization URL
app.get("/microsoft/auth", (req, res) => {
    const authUrl =
        `https://login.microsoftonline.com/${TENANT}/oauth2/v2.0/authorize?` +
        `client_id=${MICROSOFT_CLIENT_ID}` +
        `&response_type=code` +
        `&redirect_uri=${redirectUri}` +
        `&response_mode=query` +
        `&scope=${SCOPES}` +
        `&code_challenge=${codeChallenge}` +
        `&code_challenge_method=S256`;
    res.redirect(encodeURI(authUrl));
});

// OAuth Step 2: Handle the OAuth callback
app.get("/microsoft/auth/callback", async (req, res) => {
    const { code } = req.query;
    try {
        const data = {
            client_id: MICROSOFT_CLIENT_ID,
            scope: SCOPES,
            code: code,
            redirect_uri: redirectUri,
            grant_type: "authorization_code",
            code_verifier: codeVerifier,
        };
        const response = await axios.post(
            "https://login.microsoftonline.com/organizations/oauth2/v2.0/token",
            data,
            {
                headers: {
                    "content-type": "application/x-www-form-urlencoded",
                    Origin: "http://localhost",
                },
            }
        );

        ACCESS_TOKEN = response.data.access_token;
        REFRESH_TOKEN = response.data.refresh_token;

        // Now you have the user's access token
        // console.log("User Access Token:", ACCESS_TOKEN);

        // Save access token to file
        // save_authed_user(response.data);

        res.send("OAuth process completed successfully!");
    } catch (error) {
        console.error("OAuth error:", error);
        res.status(500).send("OAuth process failed.");
    }
});

// Refresh OAuth Token
app.get("/microsoft/auth/refresh", async (req, res) => {
    try {
        const data = {
            client_id: MICROSOFT_CLIENT_ID,
            scope: SCOPES,
            refresh_token: REFRESH_TOKEN,
            redirect_uri: redirectUri,
            grant_type: "refresh_token",
        };
        const response = await axios.post(
            "https://login.microsoftonline.com/organizations/oauth2/v2.0/token",
            data,
            {
                headers: {
                    "content-type": "application/x-www-form-urlencoded",
                    Origin: "http://localhost",
                },
            }
        );

        ACCESS_TOKEN = response.data.access_token;
        REFRESH_TOKEN = response.data.refresh_token;

        // Now you have the user's access token
        // console.log("User Access Token:", ACCESS_TOKEN);

        // Save access token to file
        // save_authed_user(response.data);

        res.send("OAuth process completed successfully!");
    } catch (error) {
        console.error("OAuth error:", error);
        res.status(500).send("OAuth process failed.");
    }
});

// Email endpoint
app.get("/microsoft/auth/send-email", async (req, res) => {
    let { mess, address } = req.query;
    if (address == undefined) {
        address = MY_MAIL;
    }

    try {
        const data = {
            message: {
                subject: "Local application test",
                body: { contentType: "Text", content: mess },
                toRecipients: [
                    { emailAddress: { address: MY_MAIL } },
                    { emailAddress: { address: address } },
                ],
            },
            // saveToSentItems: 'false'
        };
        const config = {
            headers: {
                "Content-Type": "application/json",
                Authorization: `Bearer ${ACCESS_TOKEN}`,
            },
        };

        const response = await axios.post(
            "https://graph.microsoft.com/v1.0/me/sendMail",
            data,
            config
        );

        res.send(`Email successful. <br> ${mess} ${address} <br> ${JSON.stringify(response.data)}`);
    } catch (error) {
        console.error("Sending Email error:", error);
        res.status(500).send(`Process of Sending Email failed. <br> ${error}`);
    }
});

// Create Outlook events
app.get("/microsoft/auth/new-event", async (req, res) => {
    let { address } = req.query;
    if (address == undefined) {
        address = MY_MAIL;
    }
    console.log(address);

    try {
        const data = {
            subject: "Test event",
            body: {
                contentType: "text",
                content: "Testing if adding events with new participants works",
            },
            start: {
                dateTime: "2023-09-25T09:00:00",
                timeZone: "Europe/Warsaw",
            },
            end: {
                dateTime: "2023-09-25T10:00:00",
                timeZone: "Europe/Warsaw",
            },
            attendees: [
                {
                    emailAddress: {
                        address: MY_MAIL,
                    },
                    type: "required",
                },
                {
                    emailAddress: {
                        address: address,
                    },
                    type: "required",
                },
            ],
        };

        const config = {
            headers: {
                "Content-Type": "application/json",
                Authorization: `Bearer ${ACCESS_TOKEN}`,
            },
        };

        const response = await axios.post(
            "https://graph.microsoft.com/v1.0/me/events",
            data,
            config
        );

        res.send(`Event successful. <br> ${address} <br> ${JSON.stringify(response.data)}`);
    } catch (error) {
        console.error("Creating Event error:", error);
        res.status(500).send(`Process of Creating Event failed. <br> ${error}`);
    }
});

// Start the Express server
app.listen(PORT, () => {
    console.log(`⚡️ Express server is running on port ${PORT}`);
    console.log(`⚡️ URL Address http://localhost:${PORT}/microsoft/auth`);
});
