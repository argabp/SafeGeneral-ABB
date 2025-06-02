$(document).ready(function () {
    btnEdit_Click();
});

function btnEdit_Click() {
    $('#btn-edit-emailTemplate').click(function () {
        showConfirmation("Confirmation", "Do you want to save the changes?", function () {
            showProgress('#EmailTemplateWindow');
            setTimeout(function () {
                saveEmailTemplate('/EmailTemplate/EditEmailTemplate')
            }, 500);
        });
    });
}
