$(document).ready(function () {
    btnAdd_Click();
    filePhoto_OnChange();
});

function btnAdd_Click() {
    $('#btn-add-user').click(function () {
        showProgress('#UserWindow');
        setTimeout(function () {
            saveUser('/User/Add')
        }, 500);
    });
}
