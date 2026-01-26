$(document).ready(function () {
    searchKeywordAkseptasiPolis_OnKeyUp();
});

function searchFilterAkseptasiPolis() {
    return {
        page: $("#AkseptasiPolisGrid").data("kendoGrid").dataSource.page(),
        pageSize: $("#AkseptasiPolisGrid").data("kendoGrid").dataSource.pageSize(),

        sortField: $("#AkseptasiPolisGrid").data("kendoGrid").dataSource.sort()?.[0]?.field,
        sortDir: $("#AkseptasiPolisGrid").data("kendoGrid").dataSource.sort()?.[0]?.dir,

        filterField: $("#AkseptasiPolisGrid").data("kendoGrid").dataSource.filter()?.filters?.[0]?.field,
        filterValue: $("#AkseptasiPolisGrid").data("kendoGrid").dataSource.filter()?.filters?.[0]?.value,
        searchKeyword: $("#SearchKeywordAkseptasiPolis").val(),
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