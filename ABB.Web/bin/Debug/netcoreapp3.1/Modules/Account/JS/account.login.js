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