$(document).ready(function () {
    showProgress('#KlaimAlokasiReasuransiWindow');
    setTimeout(setKlaimAlokasiReasuransiEditedValue, 500);
});

function setKlaimAlokasiReasuransiEditedValue(){
    var kd_rk_sor = $("#kd_rk_sor").data("kendoDropDownList");
    kd_rk_sor.dataSource.read({jns_lookup : $("#kd_grp_sor").val() + ",R", kd_cb: $("#kd_cb").val(), kd_jns_sor : $("#kd_jns_sor").val()});

    
    var flag_nota = $("#tempFlag_nota").val();
    flag_nota == "Y" ? $("#flag_nota").prop("checked", true) : $("#flag_nota").prop("checked", false);
    var flag_cash_call = $("#tempFlag_cash_call").val();
    flag_cash_call == "Y" ? $("#flag_cash_call").prop("checked", true) : $("#flag_cash_call").prop("checked", false);

    setTimeout(() => {
        kd_rk_sor.value($("#temp_kd_rk_sor").val().trim());
    } , 1000)

    closeProgress('#KlaimAlokasiReasuransiWindow');
}