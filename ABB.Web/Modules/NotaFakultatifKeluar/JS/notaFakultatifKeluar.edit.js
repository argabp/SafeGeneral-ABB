$(document).ready(async function () {
    await setNotaFakultatifKeluarEditedValue();
});

async function setNotaFakultatifKeluarEditedValue(){
    showProgress('#NotaFakultatifKeluarWindow');
    await restoreDropdownValue("#kd_rk_pas", "#temp_kd_rk_pas");
    await restoreDropdownValue("#kd_rk_sb_bis", "#temp_kd_rk_sb_bis");

    var flag_cancel = $("#tempFlag_cancel").val();
    flag_cancel == "Y" ? $("#flag_cancel").prop("checked", true) : $("#flag_cancel").prop("checked", false);

    var flag_posting = $("#tempFlag_posting").val();
    flag_posting == "Y" ? $("#flag_posting").prop("checked", true) : $("#flag_posting").prop("checked", false);

    closeProgress('#NotaFakultatifKeluarWindow');
}
