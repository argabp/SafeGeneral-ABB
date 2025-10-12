$(document).ready(function () {
    setTimeout(setPranotaKoasEditedValue, 2000);
    
    $("#premi_100").getKendoNumericTextBox().value(Number(pranota.nilai_prm));
});

function setPranotaKoasEditedValue(){
    $("#pranota_koas_kd_rk_pas").data("kendoDropDownList").value($("#temp_pranota_koas_kd_rk_pas").val().trim());
}
