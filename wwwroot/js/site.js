// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

function handleCredentialResponse(response) {
    // Google returns a JSON Web Token (JWT) credential
    const data = jwtDecode(response.credential);
    console.log("ID: " + data.sub); // Unique Google ID
    console.log('Full Name: ' + data.name);
    console.log('Email: ' + data.email);

    // Here, you would typically send the 'response.credential' JWT to your backend server
    // for secure validation and actual user session management.
}

// A simple function to decode the JWT for demonstration purposes in the frontend
// In a real application, token validation should be done on the server.
function jwtDecode(token) {
    var base64Url = token.split('.')[1];
    var base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
    var jsonPayload = decodeURIComponent(window.atob(base64).split('').map(function (c) {
        return '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2);
    }).join(''));
    return JSON.parse(jsonPayload);
};
