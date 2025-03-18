$(document).ready(function () {
    btnEdit_Click();
    LoadIconList();
});

function btnEdit_Click() {
    $('#btn-edit-module').click(function () {
        showConfirmation("Confirmation", "Do you want to save the changes?", function () {
            showProgress('#ModuleWindow');
            setTimeout(function () {
                saveModule('/Module/Edit')
            }, 500);
        });
    });
}
