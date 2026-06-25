checkAuth();

async function loadOfficesDropdown() {
    const response = await apiRequest('/api/office');
    if (!response) return;

    const offices = await response.json();
    const select = document.getElementById('exceptionOffice');
    select.innerHTML = '<option value="">انتخاب دفتر</option>';

    offices.forEach(office => {
        const option = document.createElement('option');
        option.value = office.id;
        option.textContent = office.name;
        select.appendChild(option);
    });
}

async function loadHolidaysDropdown() {
    const response = await apiRequest('/api/holiday');
    if (!response) return;

    const holidays = await response.json();
    const select = document.getElementById('exceptionHoliday');
    select.innerHTML = '<option value="">انتخاب تعطیلی</option>';

    holidays.forEach(holiday => {
        const option = document.createElement('option');
        option.value = holiday.id;
        option.textContent = `${holiday.reason} (${holiday.date})`;
        select.appendChild(option);
    });
}

async function loadExceptions() {
    const response = await apiRequest('/api/officeholidayexception');
    if (!response) return;

    const exceptions = await response.json();
    const tbody = document.getElementById('exceptionsBody');
    tbody.innerHTML = '';

    exceptions.forEach(exception => {
        const row = document.createElement('tr');
        row.innerHTML = `
            <td>${exception.id}</td>
            <td>${exception.office ? exception.office.name : '-'}</td>
            <td>${exception.holiday ? exception.holiday.reason : '-'}</td>
            <td>${exception.holiday ? exception.holiday.date : '-'}</td>
            <td>
                <button onclick="deleteException(${exception.id})">حذف</button>
            </td>
        `;
        tbody.appendChild(row);
    });
}

async function addException() {
    const officeId = document.getElementById('exceptionOffice').value;
    const holidayId = document.getElementById('exceptionHoliday').value;

    if (!officeId || !holidayId) {
        return alert('همه فیلدها را پر کنید');
    }

    const response = await apiRequest('/api/officeholidayexception', 'POST', {
        officeId: parseInt(officeId),
        holidayId: parseInt(holidayId)
    });

    if (response && response.ok) {
        loadExceptions();
    } else {
        const errorText = await response.text();
        alert(errorText || 'خطا در افزودن استثنا');
    }
}

async function deleteException(id) {
    if (!confirm('آیا مطمئن هستید؟')) return;

    const response = await apiRequest(`/api/officeholidayexception/${id}`, 'DELETE');
    
    if (response && response.ok) {
        loadExceptions();
    } else {
        alert('خطا در حذف استثنا');
    }
}

loadOfficesDropdown();
loadHolidaysDropdown();
loadExceptions();