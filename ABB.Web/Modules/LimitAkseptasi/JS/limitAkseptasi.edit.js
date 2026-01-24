$(document).ready(async function () {
    showProgress("#LimitAkseptasiWindow");
    await setLimitAkseptasiEditedValue();
});

async function setLimitAkseptasiEditedValue(){
    showProgress("#LimitAkseptasiWindow");
    await restoreDropdownValue("#kd_scob", "#temp_kd_scob");

    closeProgress('#LimitAkseptasiWindow');
}