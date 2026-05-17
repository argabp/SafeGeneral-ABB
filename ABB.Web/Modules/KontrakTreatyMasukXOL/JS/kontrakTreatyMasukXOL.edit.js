$(document).ready(async function () {
    await setKontrakTreatyMasukXOLEditedValue();
});

async function setKontrakTreatyMasukXOLEditedValue(){
    showProgress('#KontrakTreatyMasukXOLWindow');
    await restoreDropdownValue("#kd_rk_pas", "#temp_kd_rk_pas");
    await restoreDropdownValue("#kd_rk_sb_bis", "#temp_kd_rk_sb_bis");

    closeProgress('#KontrakTreatyMasukXOLWindow');
}
