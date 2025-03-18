$(document).ready(function () {
    btnChangePassword_Click();
});

function btnChangePassword_Click() {
    $('#btn-change-password').click(function () {
        showProgress('#UserWindow');
        setTimeout(changePassword, 500);
    });
}

function changePassword() {
    var data = $("#ChangePasswordForm").serialize();
    ajaxPost("/User/ChangePassword", data, function (response) {
        if (response.Result == "OK") {
            showMessage('Success', response.Message);
            closeWindow('#UserWindow');
        }
        else if (response.Result == "ERROR")
            $("#UserWindow").html(response.Message);
        else
            $("#UserWindow").html(response);
        closeProgress('#UserWindow');
    }, AjaxContentType.URLENCODED);   
}
