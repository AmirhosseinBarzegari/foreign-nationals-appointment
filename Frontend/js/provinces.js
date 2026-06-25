checkAuth();

async function loadProvinces() {
    const response = await apiRequest('/api/province');
    if (!response) return;
    
    const provinces = await response.json();
    const tbody = document.getElementById('provincesBody');
    tbody.innerHTML = '';

    provinces.forEach(province => {
        const row = document.createElement('tr');
        row.innerHTML = `
            <td>${province.id}</td>
            <td>${province.name}</td>
            <td>${province.isActive ? 'فعال' : 'غیرفعال'}</td>
            <td>
                <button onclick="deleteProvince(${province.id})">حذف</button>
            </td>
        `;
        tbody.appendChild(row);
    });
}

async function addProvince() {
    const name = document.getElementById('provinceName').value;
    if (!name) return alert('نام استان را وارد کنید');

    const response = await apiRequest('/api/province', 'POST', {
        name: name,
        isActive: true
    });

    if (response && response.ok) {
        document.getElementById('provinceName').value = '';
        loadProvinces();
    } else {
        alert('خطا در افزودن استان');
    }
}

async function deleteProvince(id) {
    // چک کردن که آیا این استان دفتر دارد
    const officesResponse = await apiRequest('/api/office');
    const offices = await officesResponse.json();
    const hasOffices = offices.some(o => o.provinceId === id);

    if (hasOffices) {
        if (!confirm('این استان دارای دفتر است. در صورت حذف، دفاتر آن نیز حذف می‌شوند. آیا مطمئن هستید؟')) {
            return;
        }
    } else {
        if (!confirm('آیا مطمئن هستید؟')) return;
    }

    const response = await apiRequest(`/api/province/${id}`, 'DELETE');
    
    if (response && response.ok) {
        loadProvinces();
    } else {
        alert('خطا در حذف استان');
    }
}

loadProvinces();