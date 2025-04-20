$(document).ready(function () {
    btnPreview_Click();
});

function btnPreview_Click() {
    $('#btn-preview').click(function () {
        showProgressByElement($('#RenewalReminderForm'));
        setTimeout(function () {
            previewReport('/RenewalReminder/GenerateReport')
        }, 500);
    });
}

function previewReport(url) {
    var form = getFormData($('#RenewalReminderForm'));

    var data = JSON.stringify(form);

    ajaxPost(url, data,
        function (response) {
            if(response.Status === "OK"){
                window.open("/Reports/" + response.Data + "/RenewalReminder.pdf",  '_blank');
            } else {
                showMessage('Error', response.Message);
            }

            closeProgressByElement($('#RenewalReminderForm'));
        }
    );
}