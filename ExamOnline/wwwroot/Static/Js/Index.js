function ChangePage(Page, Url) {
    $.ajax({
        url: Url,
        type: 'GET',
        data: { Page: Page },
        success: function (html) {
            $("#body_Result").html(html);
        }
    });
}
$(document).ready(function () {

window.showAlert = function (message) {
    $('#alertText').text(message);
    $('#myAlertModal').css('display', 'flex');
};

$('#btnCloseModal').on('click', function () {
    $('#myAlertModal').css('display', 'none');
});

$(window).on('click', function (event) {
    if ($(event.target).is('#myAlertModal')) {
        $('#myAlertModal').css('display', 'none');
    }
});
});
const sidebar = document.getElementById('sidebar');
const toggleBtn = document.getElementById('toggle-btn');
const overlay = document.getElementById('overlay');

toggleBtn.onclick = function () {
    sidebar.classList.toggle('active');
    overlay.classList.toggle('active');
}

overlay.onclick = function () {
    sidebar.classList.remove('active');
    overlay.classList.remove('active');
}
