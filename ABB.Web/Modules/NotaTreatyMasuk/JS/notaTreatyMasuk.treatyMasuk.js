$(document).ready(function () {
    searchKeywordTreatyMasuk_OnKeyUp();
});

function searchFilterTreatyMasuk(e) {
    const gridReq = buildGridRequest(e, "SearchKeywordTreatyMasuk");

    return {
        grid: gridReq,
        kd_cb: $("#kd_cb").val(),
        kd_jns_sor: $("#kd_jns_sor").val()
    };
}

function searchKeywordTreatyMasuk_OnKeyUp() {
    $('#SearchKeywordTreatyMasuk').keyup(function () {
        refreshGrid("#TreatyMasukGrid");
    });
}

function OnTreatyMasukChange(e){
    var grid = e.sender;
    var dataTreatyMasuk = grid.dataItem(this.select());
    
    $("#kd_tty_msk").getKendoTextBox().value(dataTreatyMasuk.kd_tty_msk);

    closeWindow("#TreatyMasukWindow");
}