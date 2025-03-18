$(document).ready(function () {
    btnAdd_Click();
});

function btnAdd_Click() {
    $('#btn-add-polisInduk').click(function () {
        showConfirmation("Confirmation", "Do you want to save the changes?", function () {
            showProgress('#PolisIndukWindow');
            setTimeout(function () {
                savePolisInduk('/PolisInduk/Add')
            }, 500);
        });
    });
}
