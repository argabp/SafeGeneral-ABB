$(document).ready(function () {
    btnNextDetailKontrakTreatyKeluarCoverage();
    btnPreviousDetailKontrakTreatyKeluarCoverage();
});

function searchFilterDetailKontrakTreatyKeluarCoverage(e) {
    const gridReq = buildGridRequest(e);

    return {
        grid: gridReq,
        kd_cb: $("#kd_cb").val(),
        kd_jns_sor: $("#kd_jns_sor").val(),
        kd_tty_pps: $("#kd_tty_pps").val()
    };
}

function btnNextDetailKontrakTreatyKeluarCoverage(){
    $('#btn-next-detailKontrakTreatyKeluarCoverage').click(function () {
        $("#kontrakTreatyKeluarTab").getKendoTabStrip().select(3);
    });
}

function btnPreviousDetailKontrakTreatyKeluarCoverage(){
    $('#btn-previous-detailKontrakTreatyKeluarCoverage').click(function () {
        $("#kontrakTreatyKeluarTab").getKendoTabStrip().select(1);
    });
}


function btnAddDetailKontrakTreatyKeluarCoverage_OnClick() {
    openWindow('#DetailKontrakTreatyKeluarWindow',`/KontrakTreatyKeluar/AddDetailKontrakTreatyKeluarCoverage?kd_cb=${$("#kd_cb").val()}&kd_jns_sor=${$("#kd_jns_sor").val()}&kd_tty_pps=${$("#kd_tty_pps").val()}`, 'Add Coverage');
}

function onEditDetailKontrakTreatyKeluarCoverage(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    console.log('dataItem', dataItem);
    openWindow('#DetailKontrakTreatyKeluarWindow', `/KontrakTreatyKeluar/EditDetailKontrakTreatyKeluarCoverage?kd_cb=${dataItem.kd_cb}&kd_jns_sor=${dataItem.kd_jns_sor}&kd_tty_pps=${dataItem.kd_tty_pps}&kd_cvrg=${dataItem.kd_cvrg}`, 'Edit Coverage');
}

function onDeleteDetailKontrakTreatyKeluarCoverage(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    showConfirmation('Confirmation', `Are you sure you want to delete Coverage?`,
        function () {
            showProgressOnGrid('#DetailKontrakTreatyKeluarCoverageGrid');
            setTimeout(function () { deleteDetailKontrakTreatyKeluarCoverage(dataItem); }, 500);
        }
    );
}

function deleteDetailKontrakTreatyKeluarCoverage(dataItem) {
    ajaxGet(`/KontrakTreatyKeluar/DeleteDetailKontrakTreatyKeluarCoverage?kd_cb=${dataItem.kd_cb}&kd_jns_sor=${dataItem.kd_jns_sor}&kd_tty_pps=${dataItem.kd_tty_pps}&kd_cvrg=${dataItem.kd_cvrg}`, function (response) {
        if (response.Result) {
            showMessage('Success', 'Data has been deleted');
            refreshGrid("#DetailKontrakTreatyKeluarCoverageGrid");
        }
        else if (!response.Result) {
            showMessage('Error', 'Delete data is failed, this data is already used');
            refreshGrid("#DetailKontrakTreatyKeluarCoverageGrid");
        }
        else
            $("#DetailKontrakTreatyKeluarWindow").html(response);

        closeProgressOnGrid('#DetailKontrakTreatyKeluarCoverageGrid');
    }, AjaxContentType.URLENCODED);
}