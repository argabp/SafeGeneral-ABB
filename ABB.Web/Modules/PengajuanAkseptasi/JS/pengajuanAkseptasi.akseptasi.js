$(document).ready(function () {
    searchKeywordAkseptasi_OnKeyUp();
});

function searchFilterAkseptasi(e) {
    const gridReq = buildGridRequest(e, "SearchKeywordAkseptasi");

    return {
        grid: gridReq,
        jns_pengajuan : $("#jns_pengajuan").val()
    };
}

function searchKeywordAkseptasi_OnKeyUp() {
    $('#SearchKeywordAkseptasi').keyup(function () {
        refreshGrid("#AkseptasiGrid");
    });
}

function OnAkseptasiChange(e){
    var grid = e.sender;
    var dataAkseptasi = grid.dataItem(this.select());

    $("#no_ref_pol").getKendoMaskedTextBox().value(dataAkseptasi.no_pol_ttg);
    
    ajaxGet("/PengajuanAkseptasi/GetDataPolis?no_pol_ttg=" + dataAkseptasi.no_pol_ttg, (returnValue) => {
        $("#kd_cb").getKendoDropDownList().value(returnValue[0].split(",")[1].trim());
        $("#kd_cb").getKendoDropDownList().trigger("change");
        $("#kd_cob").getKendoDropDownList().value(returnValue[1].split(",")[1].trim());
        $("#kd_cob").getKendoDropDownList().trigger("change");
        $("#kd_grp_mkt").getKendoDropDownList().value(returnValue[2].split(",")[1].trim());
        $("#kd_grp_mkt").getKendoDropDownList().trigger("change");
        $("#kd_grp_sb_bis").getKendoDropDownList().value(returnValue[3].split(",")[1].trim());
        $("#kd_grp_sb_bis").getKendoDropDownList().trigger("change");
        $("#kd_grp_ttg").getKendoDropDownList().value(returnValue[4].split(",")[1].trim());
        $("#kd_grp_ttg").getKendoDropDownList().trigger("change");
        $("#tgl_akh_ptg").getKendoDatePicker().value(new Date(returnValue[9].split(",")[1].trim()));
        $("#tgl_mul_ptg").getKendoDatePicker().value(new Date(returnValue[10].split(",")[1].trim()));

        setTimeout(() => {
            $("#kd_rk_mkt").getKendoDropDownList().value(returnValue[5].split(",")[1].trim());
            $("#kd_rk_sb_bis").getKendoDropDownList().value(returnValue[6].split(",")[1].trim());
            $("#kd_rk_ttg").getKendoDropDownList().value(returnValue[7].split(",")[1].trim());
            $("#kd_scob").getKendoDropDownList().value(returnValue[8].split(",")[1].trim());
            $("#kd_scob").getKendoDropDownList().trigger("change");
        }, 500);
    });

    closeWindow("#AkseptasiWindow");
}