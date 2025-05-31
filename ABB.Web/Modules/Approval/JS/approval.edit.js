$(document).ready(function () {
    showProgress("#ApprovalWindow")
    setTimeout(setApprovalEditedValue, 2000);
});

function setApprovalEditedValue(){
    $("#kd_scob").data("kendoDropDownList").value($("#temp_kd_scob").val().trim());

    closeProgress('#ApprovalWindow');
}