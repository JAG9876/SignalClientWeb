// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

function handleCredentialResponse(response) {
    // Google returns a JSON Web Token (JWT) credential
    const idToken = response.credential;

    // Decode the JWT to get user info (for demonstration purposes)
    const data = jwtDecode(idToken);
    console.log("ID: " + data.sub); // Unique Google ID
    console.log('Full Name: ' + data.name);
    console.log('Email: ' + data.email);

    // Send the 'response.credential' JWT to my backend server
    fetch('/Home/SigninGoogle', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({ idToken: idToken })
    })
        .then(response => {
            if (response.ok) {
                // Handle token storage
                response.json().then(data => {
                    //console.log(data.accessToken);
                    //console.log(data.refreshToken);

                    //localStorage.setItem('accessToken', data.accessToken);
                    localStorage.setItem('refreshToken', data.refreshToken);
                    //refreshToken = localStorage.getItem('refreshToken');

                    // alert('Successfully signed in with Google');
                });
            }
            else {
                console.error('Login failed on the server.');
            }
        })
        .catch(error => {
            console.error('Error during sign-in:', error);
        })
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
