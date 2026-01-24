$(document).ready(async function () {
    showProgress("#LimitKlaimWindow");
    await setLimitKlaimEditedValue();
});

async function setLimitKlaimEditedValue(){
    showProgress("#LimitKlaimWindow");
    await restoreDropdownValue("#kd_scob", "#temp_kd_scob");

    closeProgress('#LimitKlaimWindow');
}