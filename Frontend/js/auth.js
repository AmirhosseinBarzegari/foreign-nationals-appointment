const API_URL = 'http://localhost:5062';

document.getElementById('loginForm').addEventListener('submit', async function(e) {
    e.preventDefault();
    
    const username = document.getElementById('username').value;
    const password = document.getElementById('password').value;
    const errorMessage = document.getElementById('errorMessage');
    
    errorMessage.textContent = '';

    try {
        const response = await fetch(`${API_URL}/api/auth/login`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ username, password })
        });

        if (response.ok) {
            const data = await response.json();
            // ذخیره token
            localStorage.setItem('token', data.token);
            // رفتن به صفحه اصلی
            window.location.href = 'dashboard.html';
        } else {
            errorMessage.textContent = 'نام کاربری یا رمز عبور اشتباه است';
        }
    } catch (error) {
        errorMessage.textContent = 'خطا در ارتباط با سرور';
    }
});