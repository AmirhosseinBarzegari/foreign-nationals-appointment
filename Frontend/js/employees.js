checkAuth();

async function loadOfficesDropdown() {
    const response = await apiRequest('/api/office');
    if (!response) return;

    const offices = await response.json();
    const select = document.getElementById('employeeOffice');
    select.innerHTML = '<option value="">انتخاب دفتر</option>';

    offices.forEach(office => {
        const option = document.createElement('option');
        option.value = office.id;
        option.textContent = office.name;
        select.appendChild(option);
    });
}

async function loadEmployees() {
    const response = await apiRequest('/api/employee');
    if (!response) return;

    const employees = await response.json();
    const tbody = document.getElementById('employeesBody');
    tbody.innerHTML = '';

    employees.forEach(employee => {
        const row = document.createElement('tr');
        row.innerHTML = `
            <td>${employee.id}</td>
            <td>${employee.firstName} ${employee.lastName}</td>
            <td>${employee.userName}</td>
            <td>${employee.office ? employee.office.name : '-'}</td>
            <td>${employee.isActive ? 'فعال' : 'غیرفعال'}</td>
            <td>
                <button onclick="deleteEmployee(${employee.id})">حذف</button>
            </td>
        `;
        tbody.appendChild(row);
    });
}

async function addEmployee() {
    const firstName = document.getElementById('employeeFirstName').value;
    const lastName = document.getElementById('employeeLastName').value;
    const userName = document.getElementById('employeeUsername').value;
    const password = document.getElementById('employeePassword').value;
    const officeId = document.getElementById('employeeOffice').value;

    if (!firstName || !lastName || !userName || !password || !officeId) {
        return alert('همه فیلدها را پر کنید');
    }

    const response = await apiRequest('/api/employee', 'POST', {
        firstName: firstName,
        lastName: lastName,
        userName: userName,
        password: password,
        isActive: true,
        officeId: parseInt(officeId)
    });

    if (response && response.ok) {
        document.getElementById('employeeFirstName').value = '';
        document.getElementById('employeeLastName').value = '';
        document.getElementById('employeeUsername').value = '';
        document.getElementById('employeePassword').value = '';
        loadEmployees();
    } else {
        const errorText = await response.text();
        alert(errorText || 'خطا در افزودن کارمند');
    }
}

async function deleteEmployee(id) {
    if (!confirm('آیا مطمئن هستید؟')) return;

    const response = await apiRequest(`/api/employee/${id}`, 'DELETE');
    
    if (response && response.ok) {
        loadEmployees();
    } else {
        alert('خطا در حذف کارمند');
    }
}

loadOfficesDropdown();
loadEmployees();