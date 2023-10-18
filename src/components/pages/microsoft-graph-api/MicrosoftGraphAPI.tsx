function isAuth() {
    return fetch(`http://localhost:3002/check-token`, {
        method: 'GET',
        credentials: 'include' // Include credentials (cookies) in the request
    }).then(response => response.json())
        .catch(error => console.error('Error:', error));
}

export { isAuth };