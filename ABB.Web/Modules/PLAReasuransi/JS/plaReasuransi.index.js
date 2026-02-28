$(document).ready(function () {
    searchKeyword_OnKeyUp();
});

function searchFilterPLAReasuransi(e) {
    const gridReq = buildGridRequest(e, "SearchKeyword");

    return {
        grid: gridReq
    };
}

function searchKeyword_OnKeyUp() {
    $('#SearchKeyword').keyup(function () {
        refreshGrid("#PLAReasuransiGrid");
    });
}

function openPLAReasuransiWindow(url, title) {
    openWindow('#PLAReasuransiWindow', url, title);
}

function onEditPLAReasuransi(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    console.log('dataItem', dataItem);
    openPLAReasuransiWindow(`/PLAReasuransi/Edit?kd_cb=${dataItem.kd_cb}&kd_cob=${dataItem.kd_cob}&kd_scob=${dataItem.kd_scob}&kd_thn=${dataItem.kd_thn}&no_kl=${dataItem.no_kl}&no_mts=${dataItem.no_mts}&no_pla=${dataItem.no_pla}`, 'Edit PLA Reasuransi');
}