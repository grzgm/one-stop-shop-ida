# Slack API PoC

## Technology

- Slack API,
- Node.js,
- TypeScript,
- Express server,
- Localtunnel for creating HTTPS endpoint

## Features

- Slack V2 OAuth 2.0 flow authentication,
- Storing user access tokens
- Sending custom message to any conversation

## Slack Permissions

- Conversation Manipulation
- Setting custom Status

## Endpoints

### Slack's authorization URL

> Redirects to Slack's authorization page

`https://localhost:{PORT}/slack/auth` - API Route

> Example

`https://pretty-tips-rush.loca.lt/slack/auth`

### OAuth callback

> Handles the OAuth callback and acquires Access Token

`https://localhost:{PORT}/slack/auth/callback` - API Route

> Example

`https://pretty-tips-rush.loca.lt/slack/auth/callback`

### Sending personalised message

> Handles sending a message

`https://localhost:{PORT}/slack/auth/send-message` - API Route

| Query Parameter | Type   | Description        |
| --------------- | ------ | ------------------ |
| *mess*          | string | Message to be send |
| *conversations* | string | Conversation id    |

> Example

`https://pretty-tips-rush.loca.lt/slack/auth/send-message?mess=testing_sending&conversations=D05AAAAAAAA`
