checkAuth();

async function loadPlansDropdown() {
    const response = await apiRequest('/api/plan');
    if (!response) return;

    const plans = await response.json();
    const select = document.getElementById('appointmentPlan');
    select.innerHTML = '<option value="">انتخاب طرح</option>';

    plans.forEach(plan => {
        const option = document.createElement('option');
        option.value = plan.id;
        option.textContent = plan.name;
        select.appendChild(option);
    });
}

async function loadOfficesDropdown() {
    const officeId = localStorage.getItem('officeId');
    const response = await apiRequest(`/api/office/${officeId}`);
    if (!response) return;

    const office = await response.json();
    const select = document.getElementById('appointmentOffice');
    select.innerHTML = `<option value="${office.id}">${office.name}</option>`;
}

async function loadAppointments() {
    const response = await apiRequest('/api/appointment');
    if (!response) return;

    const appointments = await response.json();
    const tbody = document.getElementById('appointmentsBody');
    tbody.innerHTML = '';

    appointments.forEach(appointment => {
        const row = document.createElement('tr');
        row.innerHTML = `
            <td>${appointment.id}</td>
            <td>${appointment.foreignerCode}</td>
            <td>${appointment.plan ? appointment.plan.name : '-'}</td>
            <td>${appointment.office ? appointment.office.name : '-'}</td>
            <td>${appointment.appointmentDate}</td>
            <td>${appointment.appointmentTime}</td>
            <td>${appointment.status}</td>
            <td>
                <button onclick="deleteAppointment(${appointment.id})">لغو نوبت</button>
            </td>
        `;
        tbody.appendChild(row);
    });
}

async function addAppointment() {
    const foreignerCode = document.getElementById('foreignerCode').value;
    const planId = document.getElementById('appointmentPlan').value;
    const officeId = document.getElementById('appointmentOffice').value;
    const appointmentDate = document.getElementById('appointmentDate').value;
    const appointmentTime = document.getElementById('appointmentTime').value;

    if (!foreignerCode || !planId || !officeId || !appointmentDate || !appointmentTime) {
        return alert('همه فیلدها را پر کنید');
    }

    const response = await apiRequest('/api/appointment', 'POST', {
        foreignerCode: foreignerCode,
        planId: parseInt(planId),
        officeId: parseInt(officeId),
        appointmentDate: appointmentDate,
        appointmentTime: appointmentTime + ':00',
        status: 'Active'
    });

    if (response && response.ok) {
        document.getElementById('foreignerCode').value = '';
        loadAppointments();
    } else {
        const errorText = await response.text();
        alert(errorText || 'خطا در ثبت نوبت');
    }
}

async function deleteAppointment(id) {
    if (!confirm('آیا مطمئن هستید؟')) return;

    const response = await apiRequest(`/api/appointment/${id}`, 'DELETE');
    
    if (response && response.ok) {
        loadAppointments();
    } else {
        const errorText = await response.text();
        alert(errorText || 'خطا در لغو نوبت');
    }
}

loadPlansDropdown();
loadOfficesDropdown();
loadAppointments();