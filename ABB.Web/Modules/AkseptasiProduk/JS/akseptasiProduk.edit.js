$(document).ready(function () {
    showProgress("#AkseptasiProdukWindow")
    setTimeout(setAkseptasiProdukEditedValue, 2000);
});

function setAkseptasiProdukEditedValue(){
    $("#kd_scob").data("kendoDropDownList").value($("#temp_kd_scob").val().trim());

    closeProgress('#AkseptasiProdukWindow');
}