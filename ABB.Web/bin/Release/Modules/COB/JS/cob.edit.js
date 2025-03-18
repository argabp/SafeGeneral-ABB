$(document).ready(function () {
    btnEdit_Click();
});

function btnEdit_Click() {
    $('#btn-edit-cob').click(function () {
        showConfirmation("Confirmation", "Do you want to save the changes?", function () {
            showProgress('#COBWindow');
            setTimeout(function () {
                saveCOB('/COB/Edit')
            }, 500);
        });
    });
}
