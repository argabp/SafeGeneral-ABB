$(document).ready(async function () {
    btnEdit_Click();

    var flag_konv = $("#tempFlag_konv").val();
    flag_konv == "N" ? $("#flag_konv").prop("checked", false) : $("#flag_konv").prop("checked", true);
    
    await restoreDropdownValue("#kd_rk_ttg", "#temp_kd_rk_ttg");
    await restoreDropdownValue("#kd_rk_sb_bis", "#temp_kd_rk_sb_bis");
    await restoreDropdownValue("#kd_rk_brk", "#temp_kd_rk_brk");
    await restoreDropdownValue("#kd_rk_pas", "#kd_rk_pas");
    await restoreDropdownValue("#kd_rk_bank", "#kd_rk_bank");
    await restoreDropdownValue("#kd_rk_mkt", "#temp_kd_rk_mkt");
    await restoreDropdownValue("#kd_scob", "#temp_kd_scob");
});

function btnEdit_Click() {
    $('#btn-edit-polisInduk').click(function () {
        showConfirmation("Confirmation", "Do you want to save the changes?", function () {
            showProgress('#PolisIndukWindow');
            setTimeout(function () {
                savePolisInduk('/PolisInduk/Edit')
            }, 500);
        });
    });
}
