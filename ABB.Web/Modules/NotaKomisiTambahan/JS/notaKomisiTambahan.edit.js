$(document).ready(async function () {
    await setNotaKomisiTambahanEditedValue();
});

async function setNotaKomisiTambahanEditedValue(){
    showProgress('#NotaKomisiTambahanWindow');
    var flag_posting = $("#tempFlag_posting").val();
    flag_posting == "Y" ? $("#flag_posting").prop("checked", true) : $("#flag_posting").prop("checked", false);

    await restoreDropdownValue("#kd_rk_ttj", "#temp_kd_rk_ttj");

    closeProgress('#NotaKomisiTambahanWindow');
}
