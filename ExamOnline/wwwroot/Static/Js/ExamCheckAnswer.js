function submitSingleScore(button) {
    const parent = button.closest('.q-grading');
    const input = parent.querySelector('.score-input');
    const statusDiv = parent.querySelector('.status-indicator');

    const score = input.value;
    const answerId = input.getAttribute('data-answer-id');

    if (score === "" || score < 0) {
        alert("نمره نامعتبر است");
        return;
    }

    button.disabled = true;
    statusDiv.innerHTML = "⏳ در حال ثبت...";

    fetch('/Exam/SubmitScore', {
        method: 'POST',
        headers: { 'Content-Type': 'application/x-www-form-urlencoded' },
        body: `answerId=${answerId}&score=${score}`
    })
        .then(r => r.json())
        .then(data => {
            if (data.success) {
                statusDiv.innerHTML = '<b style="color:green">✓ ثبت شد</b>';
                setTimeout(() => { statusDiv.innerHTML = ""; }, 2000);
            } else {
                statusDiv.innerHTML = `<b style="color:red">${data.data}</b>`;
            }
        })
        .finally(() => { button.disabled = false; });
}
