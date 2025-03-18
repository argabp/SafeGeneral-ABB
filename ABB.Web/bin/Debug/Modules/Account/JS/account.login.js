$(document).ready(function () {
    password_KeyDown();
});

function password_KeyDown(){
    $("#Password").keypress(function (event) {

        if (event.key === "Enter") {
            $("#LoginForm").submit();
        }
    });
}

function getUserDatabase(){
    let username = $("#Username").val();
    let databaseDropdown = $("#UserDatabase")[0];
    databaseDropdown.options.length = 0;
    ajaxGet("/Account/GetUserCabang?username=" + username, function (response) {
        databaseDropdown.options[databaseDropdown.options.length] = new Option('Pilih Cabang', '');
        response.forEach((value) => {
            databaseDropdown.options[databaseDropdown.options.length] = new Option(value.Text, value.Value);
        })
    }, AjaxContentType.URLENCODED);
}