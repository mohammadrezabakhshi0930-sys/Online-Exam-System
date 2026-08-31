const timerElement = document.getElementById('timer');

function updateTimer() {
    const minutes = Math.floor(remainingSeconds / 60);
    const seconds = remainingSeconds % 60;

    const displayMinutes = String(minutes).padStart(2, '0');
    const displaySeconds = String(seconds).padStart(2, '0');

    timerElement.textContent = `${displayMinutes}:${displaySeconds}`;

    if (remainingSeconds <= 0) {
        clearInterval(timerInterval);
        alert('زمان آزمون به پایان رسید!');
        PostFinishExam();
    } else {
        remainingSeconds--;
    }
}

const timerInterval = setInterval(updateTimer, 1000);
updateTimer();
function highlightOption(radioInput) {
    document.querySelectorAll('.option-item').forEach(item => {
        item.classList.remove('selected');
    });
    radioInput.closest('.option-item').classList.add('selected');
}

function submitAndGo(targetAnswerId) {
    document.getElementById('nextAnswerId').value = targetAnswerId;
    document.getElementById('questionForm').submit();
}

function PostFinishExam() {
    document.getElementById('isFinalSubmit').disabled = false;
    const form = document.getElementById('questionForm');
    form.submit();

}

function finishExam() {
    document.getElementById('confirmModal').style.display = 'flex';
}

function ConfirmDelete() {
    CloseConfirmModal();
    PostFinishExam();

}

function CloseConfirmModal() {
    document.getElementById('confirmModal').style.display = 'none';
}

document.addEventListener("DOMContentLoaded", function () {
    window.addEventListener('click', function (event) {
        const modal = document.getElementById('confirmModal');
        if (event.target === modal) {
            CloseConfirmModal();
        }
    });
});