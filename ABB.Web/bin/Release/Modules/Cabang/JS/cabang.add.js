$(document).ready(function () {
    btnAdd_Click();
});

function btnAdd_Click() {
    $('#btn-add-cabang').click(function () {
        showConfirmation("Confirmation", "Do you want to save the changes?", function () {
            showProgress('#CabangWindow');
            setTimeout(function () {
                saveCabang('/Cabang/Add')
            }, 500);
        });
    });
}
