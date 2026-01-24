$(document).ready(async function () {
    showProgress("#ApprovalKlaimWindow");
    await setApprovalKlaimEditedValue();
});

async function setApprovalKlaimEditedValue(){
    showProgress("#ApprovalKlaimWindow");
    await restoreDropdownValue("#kd_scob", "#temp_kd_scob");

    closeProgress('#ApprovalKlaimWindow');
}