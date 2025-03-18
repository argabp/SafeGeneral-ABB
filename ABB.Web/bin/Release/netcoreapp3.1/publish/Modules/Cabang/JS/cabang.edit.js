$(document).ready(function () {
    btnEdit_Click();
});

function btnEdit_Click() {
    $('#btn-edit-cabang').click(function () {
        showConfirmation("Confirmation", "Do you want to save the changes?", function () {
            showProgress('#CabangWindow');
            setTimeout(function () {
                saveCabang('/Cabang/Edit')
            }, 500);
        });
    });
}
