var QuestionIdToDelete = null;

function DeleteQuestionCategory(ID) {
    QuestionIdToDelete = ID;
    $('#confirmModal').css('display', 'flex');
}

function ConfirmDelete() {
    if (QuestionIdToDelete != null) {
        executeDeleteq(QuestionIdToDelete);
    }
    CloseConfirmModal();
}

function CloseConfirmModal() {
    QuestionIdToDelete = null;
    $('#confirmModal').css('display', 'none');
}

function executeDeleteq(ID) {
    $.ajax({
        url: '/Question/EditCategoryQuestion',
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
            QuestionIdToDelete = null;
            $('#confirmModal').css('display', 'none');
        }
    });
});