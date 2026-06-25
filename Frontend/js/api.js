const API_URL = 'http://localhost:5062';

function getToken() {
    return localStorage.getItem('token');
}

function checkAuth() {
    if (!getToken()) {
        window.location.href = '../index.html';
    }
}

async function apiRequest(endpoint, method = 'GET', body = null) {
    const options = {
        method,
        headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${getToken()}`
        }
    };

    if (body) {
        options.body = JSON.stringify(body);
    }

    const response = await fetch(`${API_URL}${endpoint}`, options);
    
    if (response.status === 401) {
        localStorage.removeItem('token');
        window.location.href = '../index.html';
        return null;
    }

    return response;
}