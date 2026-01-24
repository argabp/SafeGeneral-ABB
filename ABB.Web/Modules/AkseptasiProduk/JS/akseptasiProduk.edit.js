$(document).ready(async function () {
    showProgress("#AkseptasiProdukWindow");
    await setAkseptasiProdukEditedValue();
});

async function setAkseptasiProdukEditedValue(){
    showProgress("#AkseptasiProdukWindow");
    await restoreDropdownValue("#kd_scob", "#temp_kd_scob");

    closeProgress('#AkseptasiProdukWindow');
}