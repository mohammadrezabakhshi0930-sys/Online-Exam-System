
function CreateCategory() {
    var Name = $("#NameCategory").val().trim();

    if (Name.length < 1) {
        showAlert('باید نام کتگوری را وارد کنید');
        return;
    }
    $.ajax({
        url: '/Category/CreateCategory',
        type: 'POST',
        data: { CategoryName: Name },
        success: function (Result) {
            if (Result.success) {
                location.reload();
            } else {
                showAlert(Result.data);
            }
        },
        error: function () {
            showAlert("خطا در برقراری ارتباط با سرور");
        }
    });
}
function EditCategory(ID) {
    var currentName = $("#NameCategory-" + ID).attr("title");

    $("#NameCategory").val(currentName).focus();

    $("#EditCategoryId").val(ID);

    $("#btnSubmitCategory").text("ویرایش دسته");
    $("#btnSubmitCategory").attr("onclick", "UpdateCategory()");

    $("#btnCancelEdit").show();
}

function CancelEdit() {
    $("#NameCategory").val("");
    $("#EditCategoryId").val("0");
    $("#btnSubmitCategory").text("ایجاد دسته");
    $("#btnSubmitCategory").attr("onclick", "CreateCategory()");
    $("#btnCancelEdit").hide();
}
function UpdateCategory() {
    var categoryId = $("#EditCategoryId").val();
    var newName = $("#NameCategory").val().trim();

    if (newName.length < 1) {
        showAlert('نام جدید را وارد کنید');
        return;
    }

    $.ajax({
        url: '/Category/EditCategory',
        type: 'POST',
        data: {
            Id: categoryId,
            Name: newName
        },
        success: function (Result) {
            if (Result.success) {
                location.reload();
            } else {
                showAlert(Result.data);
            }
        },
        error: function () {
            showAlert("خطا در برقراری ارتباط با سرور");
        }
    });
}
var categoryIdToDelete = null;

function DeleteCategory(ID) {
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
        url: '/Category/DeleteCategory',
        type: 'GET',
        data: { Id: ID },
        success: function (Result) {
            if (Result.success) {
                showAlert(Result.data);
                setTimeout(function () {
                    location.reload();
                }, 3000);
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