$(document).ready(function () {
    $('#chkIsDescriptive').change(function () {
        if (this.checked) {
            $('#descriptiveSection').show();
            $('#txtCorrectAnswer').prop('disabled', false).prop('required', true);

            $('#multipleChoiceSection').hide();
            $('#multipleChoiceSection').find('input, select, button').prop('disabled', true);
            $('.option-input').prop('required', false);
            $('.correct-radio').prop('required', false);
        } else {
            $('#descriptiveSection').hide();
            $('#txtCorrectAnswer').prop('disabled', true).prop('required', false);

            $('#multipleChoiceSection').show();
            $('#multipleChoiceSection').find('input, select, button').prop('disabled', false);
            $('.option-input').prop('required', true);
            $('.correct-radio').prop('required', true);
        }
    }).change();

    $('#btnAddOption').click(function () {
        var optionHtml = `
                    <div class="option-row" style="display:none;">
                        <input type="radio" name="CorrectOptionRadio" class="correct-radio" required />
                        <input type="text" name="Answer" class="input-control option-input" placeholder="متن گزینه جدید..." required />
                        <button type="button" class="btn-delete-option" onclick="removeOption(this)">حذف</button>
                    </div>`;
        var $newRow = $(optionHtml);
        $('#optionsWrapper').append($newRow);
        $newRow.slideDown(200);
    });

    $(document).on('change', '.correct-radio', function () {
        updateCorrectAnswerHiddenField();
    });

    
    $(document).on('input', '.option-input', function () {
        if ($(this).siblings('.correct-radio').is(':checked')) {
            updateCorrectAnswerHiddenField();
        }
    });

    function updateCorrectAnswerHiddenField() {
        const selectedRadio = $('.correct-radio:checked');

        if (selectedRadio.length > 0) {
            const textValue = selectedRadio.closest('.option-row').find('.option-input').val();
            $('#hfCorrectAnswer').val(textValue);
        }
    }
});

function removeOption(button) {
    var rowsCount = $('#optionsWrapper .option-row').length;
    if (rowsCount <= 2) {
        alert('یک سوال تستی باید حداقل دارای ۲ گزینه باشد.');
        return;
    }
    $(button).closest('.option-row').slideUp(200, function () {
        $(this).remove();
    });
}