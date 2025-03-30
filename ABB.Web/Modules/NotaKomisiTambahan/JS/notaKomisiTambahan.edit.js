$(document).ready(function () {
    setTimeout(setNotaKomisiTambahanEditedValue, 1000);
});

function setNotaKomisiTambahanEditedValue(){
    var flag_posting = $("#tempFlag_posting").val();
    flag_posting == "Y" ? $("#flag_posting").prop("checked", true) : $("#flag_posting").prop("checked", false);

    $("#kd_rk_ttj").data("kendoDropDownList").value($("#temp_kd_rk_ttj").val().trim());

    closeProgress('#NotaKomisiTambahanWindow');
}
