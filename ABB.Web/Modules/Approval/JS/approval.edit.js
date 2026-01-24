$(document).ready(async function () {
    showProgress("#ApprovalWindow");
    await setApprovalEditedValue();
});

async function setApprovalEditedValue(){
    showProgress("#ApprovalWindow");
    await restoreDropdownValue("#kd_scob", "#temp_kd_scob");

    closeProgress('#ApprovalWindow');
}