checkAuth();

async function loadProvincesDropdown() {
    const response = await apiRequest('/api/province');
    if (!response) return;

    const provinces = await response.json();
    const select = document.getElementById('officeProvince');
    select.innerHTML = '<option value="">انتخاب استان</option>';

    provinces.forEach(province => {
        const option = document.createElement('option');
        option.value = province.id;
        option.textContent = province.name;
        select.appendChild(option);
    });
}

async function loadOffices() {
    const response = await apiRequest('/api/office');
    if (!response) return;

    const offices = await response.json();
    const tbody = document.getElementById('officesBody');
    tbody.innerHTML = '';

    offices.forEach(office => {
        const row = document.createElement('tr');
        row.innerHTML = `
            <td>${office.id}</td>
            <td>${office.name}</td>
            <td>${office.address}</td>
            <td>${office.province ? office.province.name : '-'}</td>
            <td>
                <button onclick="deleteOffice(${office.id})">حذف</button>
            </td>
        `;
        tbody.appendChild(row);
    });
}

async function addOffice() {
    const name = document.getElementById('officeName').value;
    const address = document.getElementById('officeAddress').value;
    const provinceId = document.getElementById('officeProvince').value;
    const dailyCapacity = document.getElementById('dailyCapacity').value;
    const appointmentDuration = document.getElementById('appointmentDuration').value;
    const startTime = document.getElementById('startTime').value;
    const endTime = document.getElementById('endTime').value;

    const checkboxes = document.querySelectorAll('.working-days input[type="checkbox"]:checked');
    const workingDays = Array.from(checkboxes).map(cb => cb.value).join(',');

    if (!name || !address || !provinceId || !dailyCapacity || !appointmentDuration || !startTime || !endTime || !workingDays) {
        return alert('همه فیلدها را پر کنید');
    }

    const response = await apiRequest('/api/office/with-settings', 'POST', {
        name: name,
        address: address,
        provinceId: parseInt(provinceId),
        dailyCapacity: parseInt(dailyCapacity),
        appointmentDuration: parseInt(appointmentDuration),
        startTime: startTime + ':00',
        endTime: endTime + ':00',
        workingDays: workingDays
    });

    if (response && response.ok) {
        document.getElementById('officeName').value = '';
        document.getElementById('officeAddress').value = '';
        loadOffices();
    } else {
        const errorText = await response.text();
        alert(errorText || 'خطا در افزودن دفتر');
    }
}

async function deleteOffice(id) {
    if (!confirm('آیا مطمئن هستید؟')) return;

    const response = await apiRequest(`/api/office/${id}`, 'DELETE');
    
    if (response && response.ok) {
        loadOffices();
    } else {
        alert('خطا در حذف دفتر');
    }
}

loadProvincesDropdown();
loadOffices();