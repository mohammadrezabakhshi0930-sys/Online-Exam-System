let categoryRuleIndex = 0;

function toggleQuestionIndex(checkbox) {
    const parentRow = checkbox.closest('.q-item-row');
    const examIdHidden = parentRow.querySelector('.q-exam-id');

    if (checkbox.checked) {
        examIdHidden.removeAttribute('disabled');
    } else {
        examIdHidden.setAttribute('disabled', 'disabled');
    }
}

function addCategoryRuleRow() {
    const catSelect = document.getElementById('catSelect');
    const catCount = document.getElementById('catCount');
    const examId = document.querySelector('input[name="IdExam"]').value;

    const categoryId = catSelect.value;
    const categoryName = catSelect.options[catSelect.selectedIndex].text;
    const count = catCount.value;

    if (!categoryId || !count || count <= 0) {
        alert("لطفاً هم دسته‌بندی و هم تعداد سوالات را به درستی وارد کنید.");
        return;
    }

    const existingCategories = document.querySelectorAll('.selected-cat-id');
    for (let input of existingCategories) {
        if (input.value === categoryId) {
            alert("این دسته‌بندی قبلاً به قوانین اضافه شده است.");
            return;
        }
    }

    const container = document.getElementById('dynamic-rules-list');

    const ruleRowHtml = `
        <div class="selection-item" id="dyn-rule-${categoryRuleIndex}">
            <span><strong>دسته‌بندی:</strong> ${categoryName} | <strong>تعداد سوال:</strong> ${count}</span>

            <!-- مقدار دهی به آرایه CategorySelect در DTO -->
            <input type="hidden" name="CategorySelect.Index" value="${categoryRuleIndex}" />
            <input type="hidden" name="CategorySelect[${categoryRuleIndex}].IdExam" value="${examId}" />
            <input type="hidden" name="CategorySelect[${categoryRuleIndex}].IdCategory" value="${categoryId}" class="selected-cat-id" />
            <input type="hidden" name="CategorySelect[${categoryRuleIndex}].Count" value="${count}" />

            <button type="button" class="btn-delete-small" onclick="removeDynamicRule(${categoryRuleIndex})">حذف</button>
        </div>
    `;

    container.insertAdjacentHTML('beforeend', ruleRowHtml);
    categoryRuleIndex++;

    catSelect.value = "";
    catCount.value = "";
}

function removeDynamicRule(idx) {
    document.getElementById(`dyn-rule-${idx}`).remove();
}

function filterList() {
    let input = document.getElementById('qSearch').value.toLowerCase();
    let rows = document.querySelectorAll('.q-item-row');
    rows.forEach(row => {
        let text = row.querySelector('.q-text').innerText.toLowerCase();
        row.style.display = text.includes(input) ? '' : 'none';
    });
}
