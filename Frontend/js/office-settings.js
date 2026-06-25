checkAuth();

let allSettings = [];
let currentSettingsId = null;

async function loadOfficesDropdown() {
    const response = await apiRequest('/api/office');
    if (!response) return;

    const offices = await response.json();
    const select = document.getElementById('settingsOffice');
    select.innerHTML = '<option value="">انتخاب دفتر</option>';

    offices.forEach(office => {
        const option = document.createElement('option');
        option.value = office.id;
        option.textContent = office.name;
        select.appendChild(option);
    });
}

async function loadAllSettings() {
    const response = await apiRequest('/api/officesettings');
    if (!response) return;
    allSettings = await response.json();
}

function loadSelectedSettings() {
    const officeId = parseInt(document.getElementById('settingsOffice').value);
    const settingsForm = document.getElementById('settingsForm');

    if (!officeId) {
        settingsForm.style.display = 'none';
        return;
    }

    const settings = allSettings.find(s => s.officeId === officeId);

    if (!settings) {
        alert('این دفتر هنوز تنظیماتی ندارد');
        settingsForm.style.display = 'none';
        return;
    }

    currentSettingsId = settings.id;
    document.getElementById('dailyCapacity').value = settings.dailyCapacity;
    document.getElementById('appointmentDuration').value = settings.appointmentDuration;
    document.getElementById('startTime').value = settings.startTime;
    document.getElementById('endTime').value = settings.endTime;

    const workingDaysArray = settings.workingDays.split(',');
    document.querySelectorAll('.working-days input[type="checkbox"]').forEach(cb => {
        cb.checked = workingDaysArray.includes(cb.value);
    });

    settingsForm.style.display = 'block';
}

async function saveSettings() {
    
    const officeId = parseInt(document.getElementById('settingsOffice').value);
    const dailyCapacity = document.getElementById('dailyCapacity').value;
    const appointmentDuration = document.getElementById('appointmentDuration').value;
    const startTime = document.getElementById('startTime').value.substring(0, 5);
    const endTime = document.getElementById('endTime').value.substring(0, 5);

    const checkboxes = document.querySelectorAll('.working-days input[type="checkbox"]:checked');
    const workingDays = Array.from(checkboxes).map(cb => cb.value).join(',');

    if (!dailyCapacity || !appointmentDuration || !startTime || !endTime || !workingDays) {
        return alert('همه فیلدها را پر کنید');
    }

    const response = await apiRequest(`/api/officesettings/${currentSettingsId}`, 'PUT', {
        officeId: officeId,
        dailyCapacity: parseInt(dailyCapacity),
        appointmentDuration: parseInt(appointmentDuration),
        startTime: startTime + ':00',
        endTime: endTime + ':00',
        workingDays: workingDays
    });

    if (response && response.ok) {
        alert('تنظیمات با موفقیت ذخیره شد');
        loadAllSettings();
    } else {
        const errorText = await response.text();
        alert(errorText || 'خطا در ذخیره تنظیمات');
    }
}

loadOfficesDropdown();
loadAllSettings();