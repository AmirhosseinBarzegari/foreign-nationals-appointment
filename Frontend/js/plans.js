checkAuth();

async function loadPlans() {
    const response = await apiRequest('/api/plan');
    if (!response) return;

    const plans = await response.json();
    const tbody = document.getElementById('plansBody');
    tbody.innerHTML = '';

    plans.forEach(plan => {
        const row = document.createElement('tr');
        row.innerHTML = `
            <td>${plan.id}</td>
            <td>${plan.name}</td>
            <td>${plan.codeType}</td>
            <td>${plan.startDate}</td>
            <td>${plan.endDate}</td>
            <td>
                <td>
                <button onclick="manageDependencies(${plan.id}, '${plan.name}')">وابستگی‌ها</button>
                <button onclick="togglePlanStatus(${plan.id})">${plan.isActive ? 'غیرفعال کردن' : 'فعال کردن'}</button>
                <button onclick="deletePlan(${plan.id})">حذف</button>
                </td>
            </td>
        `;
        tbody.appendChild(row);
    });
}

function manageDependencies(planId, planName) {
    window.location.href = `plan-dependencies.html?planId=${planId}&planName=${encodeURIComponent(planName)}`;
}

async function addPlan() {
    const name = document.getElementById('planName').value;
    const codeType = document.getElementById('planCodeType').value;
    const startDate = document.getElementById('planStartDate').value;
    const endDate = document.getElementById('planEndDate').value;
    const allowDuplicate = document.getElementById('planAllowDuplicate').checked;
    const maxDuplicateCount = document.getElementById('planMaxDuplicate').value;

    if (!name || !startDate || !endDate) {
        return alert('فیلدهای ضروری را پر کنید');
    }

    const response = await apiRequest('/api/plan', 'POST', {
        name: name,
        isActive: true,
        codeType: codeType,
        startDate: startDate,
        endDate: endDate,
        allowDuplicate: allowDuplicate,
        maxDuplicateCount: parseInt(maxDuplicateCount) || 0
    });

    if (response && response.ok) {
        document.getElementById('planName').value = '';
        document.getElementById('planStartDate').value = '';
        document.getElementById('planEndDate').value = '';
        loadPlans();
    } else {
        const errorText = await response.text();
        alert(errorText || 'خطا در افزودن طرح');
    }
}

async function deletePlan(id) {
    if (!confirm('آیا مطمئن هستید؟')) return;

    const response = await apiRequest(`/api/plan/${id}`, 'DELETE');
    
    if (response && response.ok) {
        loadPlans();
    } else {
        alert('خطا در حذف طرح');
    }
}

async function togglePlanStatus(id) {
    const response = await apiRequest(`/api/plan/${id}/toggle-status`, 'PUT');

    if (response && response.ok) {
        loadPlans();
    } else {
        alert('خطا در تغییر وضعیت طرح');
    }
}

loadPlans();