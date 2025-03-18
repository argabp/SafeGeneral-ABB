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

function onKrediturBound(e){
    var kd_rk = $("#kd_rk").data("kendoDropDownList");
    var kreditur = $("#tempKreditur");

    if(kreditur.length > 0)
        kd_rk.value(kreditur.val())
}