$(document).ready(function () {
    btnEdit_Click();
    filePhoto_OnChange();
});

function btnEdit_Click() {
    $('#btn-edit-user').click(function () {
        showProgress('#UserWindow');
        setTimeout(function () {
            saveUser('/User/Edit')
        }, 500);
    });
}
