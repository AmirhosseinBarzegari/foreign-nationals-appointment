checkAuth();

const urlParams = new URLSearchParams(window.location.search);
const planId = parseInt(urlParams.get('planId'));
const planName = urlParams.get('planName');

document.getElementById('planNameTitle').textContent = planName;

async function loadPlansDropdown() {
    const response = await apiRequest('/api/plan');
    if (!response) return;

    const plans = await response.json();
    const select = document.getElementById('requiredPlanSelect');
    select.innerHTML = '<option value="">انتخاب طرح پیش‌نیاز</option>';

    plans.forEach(plan => {
        if (plan.id !== planId) {
            const option = document.createElement('option');
            option.value = plan.id;
            option.textContent = plan.name;
            select.appendChild(option);
        }
    });
}

async function loadDependencies() {
    const response = await apiRequest('/api/plandependency');
    if (!response) return;

    const allDependencies = await response.json();
    const dependencies = allDependencies.filter(d => d.planId === planId);

    const tbody = document.getElementById('dependenciesBody');
    tbody.innerHTML = '';

    for (const dep of dependencies) {
        const planResponse = await apiRequest(`/api/plan`);
        const allPlans = await planResponse.json();
        const requiredPlan = allPlans.find(p => p.id === dep.requiredPlanId);

        const row = document.createElement('tr');
        row.innerHTML = `
            <td>${dep.id}</td>
            <td>${requiredPlan ? requiredPlan.name : '-'}</td>
            <td>
                <button onclick="deleteDependency(${dep.id})">حذف</button>
            </td>
        `;
        tbody.appendChild(row);
    }
}

async function addDependency() {
    const requiredPlanId = document.getElementById('requiredPlanSelect').value;
    if (!requiredPlanId) return alert('یک طرح انتخاب کنید');

    const response = await apiRequest('/api/plandependency', 'POST', {
        planId: planId,
        requiredPlanId: parseInt(requiredPlanId)
    });

    if (response && response.ok) {
        loadDependencies();
    } else {
        const errorText = await response.text();
        alert(errorText || 'خطا در افزودن وابستگی');
    }
}

async function deleteDependency(id) {
    if (!confirm('آیا مطمئن هستید؟')) return;

    const response = await apiRequest(`/api/plandependency/${id}`, 'DELETE');
    
    if (response && response.ok) {
        loadDependencies();
    } else {
        alert('خطا در حذف وابستگی');
    }
}

loadPlansDropdown();
loadDependencies();