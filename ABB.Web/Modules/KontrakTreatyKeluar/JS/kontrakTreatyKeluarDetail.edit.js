$(document).ready(async function () {
    await setKontrakTreatyKeluarEditedValue();
});

async function setKontrakTreatyKeluarEditedValue(){
    showProgress('#KontrakTreatyKeluarWindow');
    await restoreDropdownValue("#kd_rk_pas", "#temp_kd_rk_pas");
    await restoreDropdownValue("#kd_rk_sb_bis", "#temp_kd_rk_sb_bis");

    closeProgress('#KontrakTreatyKeluarWindow');
}
