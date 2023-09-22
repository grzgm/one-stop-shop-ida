import express from "express";
import axios from "axios";
import cors from "cors";
import { codeVerifier, codeChallenge, save_authed_user } from "./helper";

const MICROSOFT_CLIENT_ID = "";
const TENANT = "organizations";
const SCOPES = "offline_access user.read mail.read mail.send";
let ACCESS_TOKEN = ""
const PORT = 3002;

// Server setup
const app = express();

// Middleware for Cross-Origin Resource Sharing
app.use(cors());

// Middleware to parse JSON body
app.use(express.json());

// OAuth Callback URL has to be the same as in the Microsoft Application Panel under OAuth page
const redirectUri = encodeURI("http://localhost:3002/microsoft/auth/callback");

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
                    "Origin": "http://localhost",
                }
            }
        );

        ACCESS_TOKEN = response.data.access_token;

        // Now you have the user's access token
        // console.log("User Access Token:", ACCESS_TOKEN);

        // Save access token to file
        save_authed_user(response.data);

        res.send("OAuth process completed successfully!");
    } catch (error) {
        console.error("OAuth error:", error);
        res.status(500).send("OAuth process failed.");
    }
});

// Message endpoint
app.get("/microsoft/auth/send-message", async (req, res) => {
    let { mess, address } = req.query;
    if(address == undefined){
        address = ""
    }
    console.log(mess, address);

    try {
        const data = {
              message: {
                subject: 'Local application test',
                body: {contentType: 'Text', content: mess},
                toRecipients: [{emailAddress: {address: ""}}]
              },
              saveToSentItems: 'false'
        };
        const config = {
            headers: {
                'Content-Type': 'application/json',
                Authorization: `Bearer ${ACCESS_TOKEN}`,
            },
        };

        const response = await axios.post(
            "https://graph.microsoft.com/v1.0/me/sendMail",
            data,
            config
        );

        // Now you have the code received from Microsoft
        // You can save it for later use, typically in a database
        // or associate it with the user's account

        res.send(`Message successful. \n ${response}`);
    } catch (error) {
        console.error("Sending Message error:", error);
        res.status(500).send(`Process of Sending Message failed. \n ${error}`);
    }
});

// Start the Express server
app.listen(PORT, () => {
    console.log(`⚡️ Express server is running on port ${PORT}`);
    console.log(`⚡️ URL Address http://localhost:${PORT}/microsoft/auth`);
});
