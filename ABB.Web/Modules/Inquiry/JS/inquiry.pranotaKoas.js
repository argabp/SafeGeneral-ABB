$(document).ready(function () {
    setTimeout(setPranotaKoasEditedValue, 2000);
    
    $("#premi_100").getKendoNumericTextBox().value(Number(pranota.nilai_prm));
});

function setPranotaKoasEditedValue(){
    $("#pranota_koas_kd_rk_pas").data("kendoDropDownList").value($("#temp_pranota_koas_kd_rk_pas").val().trim());
}

function OnKodeRekananPranotaKoasChange(e){
    var pranota_koas_kd_rk_pas = $("#pranota_koas_kd_rk_pas").data("kendoDropDownList");
    $("#temp_pranota_koas_kd_grp_pas").val(e.sender._cascadedValue);
    pranota_koas_kd_rk_pas.dataSource.read({kd_grp_rk : e.sender._cascadedValue, kd_cb: $("#kd_cb").val()});
}

function dataKodeRekananPranotaKoasDropDown(){
    return {
        kd_grp_rk: $("#temp_pranota_koas_kd_grp_pas").val().trim(),
        kd_cb: $("#temp_kd_cb").val().trim()
    }
}
