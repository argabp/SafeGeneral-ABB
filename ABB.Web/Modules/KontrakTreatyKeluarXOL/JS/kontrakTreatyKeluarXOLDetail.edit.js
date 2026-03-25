$(document).ready(async function () {
    await setKontrakTreatyKeluarXOLEditedValue();
});

async function setKontrakTreatyKeluarXOLEditedValue(){
    showProgress('#KontrakTreatyKeluarXOLWindow');
    await restoreDropdownValue("#kd_rk_pas", "#temp_kd_rk_pas");
    await restoreDropdownValue("#kd_rk_sb_bis", "#temp_kd_rk_sb_bis");

    closeProgress('#KontrakTreatyKeluarXOLWindow');
}
