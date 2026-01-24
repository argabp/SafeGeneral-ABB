$(document).ready(async function () {
    await setPranotaKoasEditedValue();
    
    $("#premi_100").getKendoNumericTextBox().value(Number(pranota.nilai_prm));
});

async function setPranotaKoasEditedValue(){
    await restoreDropdownValue("#pranota_koas_kd_rk_pas", "#temp_pranota_koas_kd_rk_pas");
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
