$(document).ready(function () {
    btnAdd_Click();
});

function btnAdd_Click() {
    $('#btn-add-role').click(function () {
        showConfirmation("Confirmation", "Do you want to save the changes?", function () {
            showProgress('#RoleWindow');
            setTimeout(function () {
                saveRole('/Role/Add')
            }, 500);
        });
    });
}
