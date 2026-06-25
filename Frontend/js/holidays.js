checkAuth();
let editingHolidayId = null;

async function loadProvincesDropdown() {
    const response = await apiRequest('/api/province');
    if (!response) return;

    const provinces = await response.json();
    const select = document.getElementById('holidayProvince');

    provinces.forEach(province => {
        const option = document.createElement('option');
        option.value = province.id;
        option.textContent = province.name;
        select.appendChild(option);
    });
}

async function loadOfficesDropdown() {
    const response = await apiRequest('/api/office');
    if (!response) return;

    const offices = await response.json();
    const select = document.getElementById('holidayOffice');

    offices.forEach(office => {
        const option = document.createElement('option');
        option.value = office.id;
        option.textContent = office.name;
        select.appendChild(option);
    });
}

async function loadHolidays() {
    const response = await apiRequest('/api/holiday');
    if (!response) return;

    const holidays = await response.json();
    const tbody = document.getElementById('holidaysBody');
    tbody.innerHTML = '';

    holidays.forEach(holiday => {
        const row = document.createElement('tr');
        row.innerHTML = `
            <td>${holiday.id}</td>
            <td>${holiday.date}</td>
            <td>${holiday.reason}</td>
            <td>${holiday.isOfficial ? 'رسمی' : 'غیررسمی'}</td>
            <td>${holiday.province ? holiday.province.name : 'کل کشور'}</td>
            <td>${holiday.office ? holiday.office.name : '-'}</td>
            <td>
                <button onclick="editHoliday(${holiday.id}, '${holiday.date}', '${holiday.reason}', ${holiday.isOfficial}, ${holiday.provinceId || null}, ${holiday.officeId || null})">ویرایش</button>
                <button onclick="deleteHoliday(${holiday.id})">حذف</button>
            </td>   
        `;
        tbody.appendChild(row);
    });
}

function editHoliday(id, date, reason, isOfficial, provinceId, officeId) {
    editingHolidayId = id;
    document.getElementById('holidayDate').value = date;
    document.getElementById('holidayReason').value = reason;
    document.getElementById('holidayIsOfficial').checked = isOfficial;
    document.getElementById('holidayProvince').value = provinceId || '';
    document.getElementById('holidayOffice').value = officeId || '';
}

async function addHoliday() {
    const date = document.getElementById('holidayDate').value;
    const reason = document.getElementById('holidayReason').value;
    const isOfficial = document.getElementById('holidayIsOfficial').checked;
    const provinceId = document.getElementById('holidayProvince').value;
    const officeId = document.getElementById('holidayOffice').value;

    if (!date || !reason) {
    return alert('فیلدهای ضروری را پر کنید');
}

    const body = {
        date: date,
        reason: reason,
        isOfficial: isOfficial,
        provinceId: provinceId ? parseInt(provinceId) : null,
        officeId: officeId ? parseInt(officeId) : null
    };

    let response;
    if (editingHolidayId) {
        response = await apiRequest(`/api/holiday/${editingHolidayId}`, 'PUT', body);
        editingHolidayId = null;
    } else {
        response = await apiRequest('/api/holiday', 'POST', body);
    }

    if (response && response.ok) {
        document.getElementById('holidayDate').value = '';
        document.getElementById('holidayReason').value = '';
        document.getElementById('holidayIsOfficial').checked = false;
        loadHolidays();
    } else {
        const errorText = await response.text();
        alert(errorText || 'خطا در ذخیره تعطیلی');
    }
}

async function deleteHoliday(id) {
    if (!confirm('آیا مطمئن هستید؟')) return;

    const response = await apiRequest(`/api/holiday/${id}`, 'DELETE');
    
    if (response && response.ok) {
        loadHolidays();
    } else {
        alert('خطا در حذف تعطیلی');
    }
}

loadProvincesDropdown();
loadOfficesDropdown();
loadHolidays();