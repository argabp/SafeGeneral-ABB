$(document).ready(async function () {
    showProgress("#DokumenAkseptasiWindow");
    await setDokumenAkseptasiEditedValue();
});

async function setDokumenAkseptasiEditedValue(){
    showProgress("#DokumenAkseptasiWindow");
    await restoreDropdownValue("#kd_scob", "#temp_kd_scob");

    closeProgress('#DokumenAkseptasiWindow');
}