var categoryIdToDelete = null;

function DeleteType(ID) {
    categoryIdToDelete = ID;
    $('#confirmModal').css('display', 'flex');
}

function ConfirmDelete() {
    if (categoryIdToDelete != null) {
        executeDelete(categoryIdToDelete);
    }
    CloseConfirmModal();
}

function CloseConfirmModal() {
    categoryIdToDelete = null;
    $('#confirmModal').css('display', 'none');
}

function executeDelete(ID) {
    $.ajax({
        url: '/ExamQuestions/Delete',
        type: 'GET',
        data: { Id: ID },
        success: function (Result) {
            if (Result.success) {
                location.reload();
            } else {
                showAlert(Result.data);
            }
        },
        error: function () {
            showAlert("خطایی در ارتباط با سرور رخ داد.");
        }
    });
}

$(document).ready(function () {

    $(window).on('click', function (event) {
        if ($(event.target).is('#confirmModal')) {
            categoryIdToDelete = null;
            $('#confirmModal').css('display', 'none');
        }
    });
});