$(document).ready(function () {
    btnEdit_Click();
});

function btnEdit_Click() {
    $('#btn-edit-role').click(function () {
        showConfirmation("Confirmation", "Do you want to save the changes?", function () {
            showProgress('#RoleWindow');
            setTimeout(function () {
                saveRole('/Role/Edit')
            }, 500);
        });
    });
}
