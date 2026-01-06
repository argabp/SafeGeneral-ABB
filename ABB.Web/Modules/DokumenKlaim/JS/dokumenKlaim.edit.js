$(document).ready(function () {
    showProgress("#DokumenKlaimWindow")
    setTimeout(setDokumenKlaimEditedValue, 2000);
});

function setDokumenKlaimEditedValue(){
    $("#kd_scob").data("kendoDropDownList").value($("#temp_kd_scob").val().trim());

    closeProgress('#DokumenKlaimWindow');
}