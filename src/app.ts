import express from "express";
import axios from "axios";
import cors from "cors";
import FormData from "form-data";
import fs from "fs";
import { codeVerifier, codeChallenge } from "./helper";

const MICROSOFT_CLIENT_ID = "";
const TENANT = "organizations";
const SCOPES = "offline_access user.read mail.read mail.send";

const SRCDIR = "src/";

let access_token = ""

const app = express();
const port = 3002;

// Middleware for Cross-Origin Resource Sharing
app.use(cors());

// Middleware to parse JSON body
app.use(express.json());

// OAuth Callback URL has to be the same as in the Microsoft Application Panel under OAuth page
// const redirectUri = "http://localhost:3002/microsoft/auth/callback";
const redirectUri = encodeURI("http://localhost:3002/microsoft/auth/callback");

// OAuth Step 1: Redirect users to microsoft's authorization URL
app.get("/microsoft/auth", (req, res) => {
    const authUrl =
        `https://login.microsoftonline.com/${TENANT}/oauth2/v2.0/authorize?` +
        `client_id=${MICROSOFT_CLIENT_ID}` +
        `&response_type=code` +
        `&redirect_uri=${redirectUri}` +
        `&response_mode=query` +
        `&scope=${encodeURI(SCOPES)}` +
        `&code_challenge=${codeChallenge}` +
        `&code_challenge_method=S256`;
    res.redirect(authUrl);
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

        access_token = response.data.access_token;

        // Now you have the user's access token
        // console.log("User Access Token:", access_token);

        // You can save it for later use, typically in a database
        // or associate it with the user's account
        save_authed_user(access_token);

        res.send("OAuth process completed successfully!");
    } catch (error) {
        console.error("OAuth error:", error);
        res.status(500).send("OAuth process failed.");
    }
});

// Message endpoint
app.get("/microsoft/auth/send-message", async (req, res) => {
    let { mess, address } = req.query;
    if(address == ""){
        address = ""
    }
    console.log(mess, address);

    try {
        const data = {
              message: {
                subject: 'Local application test',
                body: {contentType: 'Text', content: mess},
                toRecipients: [{emailAddress: {address: ""}}, {emailAddress: {address: address}}]
              },
              saveToSentItems: 'false'
        };
        const config = {
            headers: {
                'Content-Type': 'application/json',
                Authorization: `Bearer ${access_token}`,
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
app.listen(port, () => {
    console.log(`⚡️ Express server is running on port ${port}`);
    console.log(`⚡️ URL Address http://localhost:${port}/microsoft/auth`);
});

function save_authed_user(obj: Object): void {
    const json = JSON.stringify(obj);
    fs.writeFile(SRCDIR + "authed_user.json", json, "utf8", (err) => {
        if (err) throw err;
    });
}
