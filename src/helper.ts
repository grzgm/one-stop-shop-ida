import crypto from "crypto"

// Generate a random code verifier (43-128 characters in length)
export const codeVerifier = crypto.randomBytes(32).toString('hex');

// Create a code challenge from the code verifier
export const codeChallenge = base64UrlEncode(crypto.createHash('sha256').update(codeVerifier).digest());

function base64UrlEncode(str: Buffer) {
    let base64 = Buffer.from(str).toString('base64');
    base64 = base64.replace(/=/g, '').replace(/\+/g, '-').replace(/\//g, '_');
    console.log(`codeVerifier ${codeVerifier}`)
    console.log(`codeChallenge ${base64}`)
    return base64;
}
