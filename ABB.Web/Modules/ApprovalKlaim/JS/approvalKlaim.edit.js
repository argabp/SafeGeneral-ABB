$(document).ready(function () {
    showProgress("#ApprovalKlaimWindow")
    setTimeout(setApprovalKlaimEditedValue, 2000);
});

function setApprovalKlaimEditedValue(){
    $("#kd_scob").data("kendoDropDownList").value($("#temp_kd_scob").val().trim());

    closeProgress('#ApprovalKlaimWindow');
}