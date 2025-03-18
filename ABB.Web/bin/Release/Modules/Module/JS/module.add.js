$(document).ready(function () {
    btnAdd_Click();
    LoadIconList();
});

function btnAdd_Click() {
    $('#btn-add-module').click(function () {
        showConfirmation("Confirmation", "Do you want to save the changes?", function () {
            showProgress('#ModuleWindow');
            setTimeout(function () {
                saveModule('/Module/Add')
            }, 500);
        });
    });
}
