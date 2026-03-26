$(document).ready(function () {
    btnNextDetailKontrakTreatyKeluarSCOB();
    btnPreviousDetailKontrakTreatyKeluarSCOB();
});

function searchFilterDetailKontrakTreatyKeluarSCOB(e) {
    const gridReq = buildGridRequest(e);

    return {
        grid: gridReq,
        kd_cb: $("#kd_cb").val(),
        kd_jns_sor: $("#kd_jns_sor").val(),
        kd_tty_pps: $("#kd_tty_pps").val()
    };
}

function btnNextDetailKontrakTreatyKeluarSCOB(){
    $('#btn-next-detailKontrakTreatyKeluarSCOB').click(function () {
        $("#kontrakTreatyKeluarTab").getKendoTabStrip().select(2);
    });
}

function btnPreviousDetailKontrakTreatyKeluarSCOB(){
    $('#btn-previous-detailKontrakTreatyKeluarSCOB').click(function () {
        $("#kontrakTreatyKeluarTab").getKendoTabStrip().select(0);
    });
}


function btnAddDetailKontrakTreatyKeluarSCOB_OnClick() {
    openWindow('#DetailKontrakTreatyKeluarWindow',`/KontrakTreatyKeluar/AddDetailKontrakTreatyKeluarSCOB?kd_cb=${$("#kd_cb").val()}&kd_jns_sor=${$("#kd_jns_sor").val()}&kd_tty_pps=${$("#kd_tty_pps").val()}`, 'Add SCOB');
}

function onDeleteDetailKontrakTreatyKeluarSCOB(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    showConfirmation('Confirmation', `Are you sure you want to delete SCOB?`,
        function () {
            showProgressOnGrid('#DetailKontrakTreatyKeluarSCOBGrid');
            setTimeout(function () { deleteDetailKontrakTreatyKeluarSCOB(dataItem); }, 500);
        }
    );
}

function deleteDetailKontrakTreatyKeluarSCOB(dataItem) {
    ajaxGet(`/KontrakTreatyKeluar/DeleteDetailKontrakTreatyKeluarSCOB?kd_cb=${dataItem.kd_cb}&kd_jns_sor=${dataItem.kd_jns_sor}&kd_tty_pps=${dataItem.kd_tty_pps}&kd_cob=${dataItem.kd_cob}&kd_scob=${dataItem.kd_scob}`, function (response) {
        if (response.Result) {
            showMessage('Success', 'Data has been deleted');
            refreshGrid("#DetailKontrakTreatyKeluarSCOBGrid");
        }
        else if (!response.Result) {
            showMessage('Error', 'Delete data is failed, this data is already used');
            refreshGrid("#DetailKontrakTreatyKeluarSCOBGrid");
        }
        else
            $("#DetailKontrakTreatyKeluarWindow").html(response);

        closeProgressOnGrid('#DetailKontrakTreatyKeluarSCOBGrid');
    }, AjaxContentType.URLENCODED);
}