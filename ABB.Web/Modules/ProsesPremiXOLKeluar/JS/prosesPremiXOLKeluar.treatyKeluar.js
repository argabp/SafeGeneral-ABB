$(document).ready(function () {
    searchKeywordTreatyKeluar_OnKeyUp();
});

function searchFilterTreatyKeluar(e) {
    const gridReq = buildGridRequest(e, "SearchKeywordTreatyKeluar");

    return {
        grid: gridReq,
        kd_cb: $("#kd_cb").val(),
        kd_jns_sor: $("#kd_jns_sor").val()
    };
}

function searchKeywordTreatyKeluar_OnKeyUp() {
    $('#SearchKeywordTreatyKeluar').keyup(function () {
        refreshGrid("#TreatyKeluarGrid");
    });
}

function OnTreatyKeluarChange(e){
    var grid = e.sender;
    var dataTreatyKeluar = grid.dataItem(this.select());
    
    $("#kd_tty_npps").getKendoTextBox().value(dataTreatyKeluar.kd_tty_npps);

    closeWindow("#TreatyKeluarWindow");
}