const API_URL = 'http://localhost:5062';

document.getElementById('loginForm').addEventListener('submit', async function(e) {
    e.preventDefault();
    
    const username = document.getElementById('username').value;
    const password = document.getElementById('password').value;
    const errorMessage = document.getElementById('errorMessage');
    
    function decodeToken(token) {
    const payload = token.split('.')[1];
    const decoded = JSON.parse(atob(payload));
    return decoded;
}

    try {
        const response = await fetch(`${API_URL}/api/auth/login`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ username, password })
        });

        if (response.ok) {
        const data = await response.json();
        localStorage.setItem('token', data.token);
        
        const decoded = decodeToken(data.token);
        localStorage.setItem('officeId', decoded.OfficeId);
        
        window.location.href = 'pages/dashboard.html';
        } else {
            errorMessage.textContent = 'نام کاربری یا رمز عبور اشتباه است';
        }

    } catch (error) {
        errorMessage.textContent = 'خطا در ارتباط با سرور';
    }
});