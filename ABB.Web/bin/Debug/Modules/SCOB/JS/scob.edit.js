$(document).ready(function () {
    btnEdit_Click();
});

function btnEdit_Click() {
    $('#btn-edit-scob').click(function () {
        showConfirmation("Confirmation", "Do you want to save the changes?", function () {
            showProgress('#SCOBWindow');
            setTimeout(function () {
                saveSCOB('/SCOB/Edit')
            }, 500);
        });
    });
}
