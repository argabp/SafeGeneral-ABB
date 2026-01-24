$(document).ready(async function () {
    showProgress("#DokumenKlaimWindow");
    await setDokumenKlaimEditedValue();
});

async function setDokumenKlaimEditedValue(){
    showProgress("#DokumenKlaimWindow");
    await restoreDropdownValue("#kd_scob", "#temp_kd_scob");

    closeProgress('#DokumenKlaimWindow');
}