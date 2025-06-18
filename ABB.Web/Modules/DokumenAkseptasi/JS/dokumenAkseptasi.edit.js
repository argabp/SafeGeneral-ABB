$(document).ready(function () {
    showProgress("#DokumenAkseptasiWindow")
    setTimeout(setDokumenAkseptasiEditedValue, 2000);
});

function setDokumenAkseptasiEditedValue(){
    $("#kd_scob").data("kendoDropDownList").value($("#temp_kd_scob").val().trim());

    closeProgress('#DokumenAkseptasiWindow');
}