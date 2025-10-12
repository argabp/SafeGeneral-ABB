$(document).ready(function () {
    showProgress('#AlokasiWindow');
    setTimeout(setAlokasiEditedValue, 500);
});

function setAlokasiEditedValue(){
    var kd_rk_sor_dp = $("#kd_rk_sor").data("kendoDropDownList");
    kd_rk_sor_dp.dataSource.read({jns_lookup : $("#kd_grp_sor").val() + ",R"});

    setTimeout(() => {
        kd_rk_sor_dp.value($("#temp_kd_rk_sor").val().trim());
    } , 1000)

    closeProgress('#AlokasiWindow');
}
