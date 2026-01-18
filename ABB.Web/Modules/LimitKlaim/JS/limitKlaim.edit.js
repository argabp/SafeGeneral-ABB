$(document).ready(function () {
    showProgress("#LimitKlaimWindow")
    setTimeout(setLimitKlaimEditedValue, 2000);
});

function setLimitKlaimEditedValue(){
    $("#kd_scob").data("kendoDropDownList").value($("#temp_kd_scob").val().trim());

    closeProgress('#LimitKlaimWindow');
}