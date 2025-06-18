$(document).ready(function () {
    showProgress("#LimitAkseptasiWindow")
    setTimeout(setLimitAkseptasiEditedValue, 2000);
});

function setLimitAkseptasiEditedValue(){
    $("#kd_scob").data("kendoDropDownList").value($("#temp_kd_scob").val().trim());

    closeProgress('#LimitAkseptasiWindow');
}