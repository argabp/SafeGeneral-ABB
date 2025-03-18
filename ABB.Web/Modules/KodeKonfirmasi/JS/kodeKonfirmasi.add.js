$(document).ready(function () {
    btnAdd_Click();
});

function btnAdd_Click() {
    $('#btn-add-kodeKonfirmasi').click(function () {
        showConfirmation("Confirmation", "Do you want to save the changes?", function () {
            showProgress('#KodeKonfirmasiWindow');
            setTimeout(function () {
                saveKodeKonfirmasi('/KodeKonfirmasi/Add')
            }, 500);
        });
    });
}
