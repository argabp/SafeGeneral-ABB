$(document).ready(function () {
    refreshViewSourceDataHasilGrid();
});

function openNotaRisikoWindow(url, title) {
    openWindow('#NotaRisikoWindow', url, title);
}

function btnViewNotaRisiko_OnClick(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));

    openNotaRisikoWindow(`/NotaRisiko/View?id=${dataItem.Id}&kodeMetode=${dataItem.KodeMetode}`, 'View');
}

function dataNotaRisiko(){
    return {
        TipeTransaksi: $("#TipeTransaksi").val(),
        KodeMetode: $("#KodeMetode").val(),
        Periode: $("#Periode").val(),
        FlagRelease: $("#FlagRelease").getKendoSwitch().value(),
    }
}

function refreshViewSourceDataHasilGrid(){
    $('#btn-refresh-grid').click(function () {
        $("#NotaRisikoGrid").getKendoGrid().dataSource.read();
    });
}