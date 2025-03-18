$(document).ready(function () {
    btnEditNotaDinas();
});

function btnEditNotaDinas(){
    $('#btn-edit-notaDinas').click(function () {
        showProgress('#NotaDinasWindow');
        setTimeout(function () {
            saveNotaDinas("/PengajuanNotaDinas/EditNotaDinas");
        }, 500);
    });
}