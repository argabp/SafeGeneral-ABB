$(document).ready(function () {
    btnAddNotaDinas();
});

function btnAddNotaDinas(){
    $('#btn-add-notaDinas').click(function () {
        showProgress('#NotaDinasWindow');
        setTimeout(function () {
            saveNotaDinas("/PengajuanNotaDinas/AddNotaDinas");
        }, 500);
    });
}