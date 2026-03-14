$(document).ready(async function () {
    await setKontrakTreatyMasukEditedValue();
});

async function setKontrakTreatyMasukEditedValue(){
    showProgress('#KontrakTreatyMasukWindow');
    await restoreDropdownValue("#kd_rk_pas", "#temp_kd_rk_pas");
    await restoreDropdownValue("#kd_rk_sb_bis", "#temp_kd_rk_sb_bis");

    closeProgress('#KontrakTreatyMasukWindow');
}
