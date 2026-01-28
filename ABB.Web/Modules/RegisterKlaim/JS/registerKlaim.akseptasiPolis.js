$(document).ready(function () {
    searchKeywordAkseptasiPolis_OnKeyUp();
});

function searchFilterAkseptasiPolis(e) {
    const gridReq = buildGridRequest(e, "SearchKeywordAkseptasiPolis");

    return {
        grid: gridReq,
        kd_cb: $("#kd_cb").val(),
        kd_cob: $("#kd_cob").val(),
        kd_scob: $("#kd_scob").val()
    };
}

function searchKeywordAkseptasiPolis_OnKeyUp() {
    $('#SearchKeywordAkseptasiPolis').keyup(function () {
        refreshGrid("#AkseptasiPolisGrid");
    });
}

function OnAkseptasiChange(e){
    var grid = e.sender;
    var dataAkseptasi = grid.dataItem(this.select());
    
    $("#no_rsk").getKendoTextBox().value(dataAkseptasi.no_rsk);
    $("#no_pol").val(dataAkseptasi.no_pol);
    $("#kd_thn_pol").val(dataAkseptasi.kd_thn);
    $("#no_updt").val(dataAkseptasi.no_updt);
    $("#no_pol_lama").getKendoMaskedTextBox().value(dataAkseptasi.no_pol_ttg);
    $("#no_pol_lama").getKendoMaskedTextBox().trigger("change");

    closeWindow("#ApprovalWindow");
}