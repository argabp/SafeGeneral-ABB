$(document).ready(function () {
    btnAdd_Click();
});
function btnAdd_Click() {
    $('#btn-add-emailTemplate').click(function () {
        showConfirmation("Confirmation", "Do you want to save the changes?", function () {
            showProgress('#EmailTemplateWindow');
            setTimeout(function () {
                saveEmailTemplate('/EmailTemplate/AddEmailTemplate')
            }, 500);
        });
    });
}
