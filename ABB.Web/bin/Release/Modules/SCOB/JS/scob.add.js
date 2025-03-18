$(document).ready(function () {
    btnAdd_Click();
});

function btnAdd_Click() {
    $('#btn-add-scob').click(function () {
        showConfirmation("Confirmation", "Do you want to save the changes?", function () {
            showProgress('#SCOBWindow');
            setTimeout(function () {
                saveSCOB('/SCOB/Add')
            }, 500);
        });
    });
}
