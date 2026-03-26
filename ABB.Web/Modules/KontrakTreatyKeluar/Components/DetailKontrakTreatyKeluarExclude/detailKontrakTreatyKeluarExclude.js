$(document).ready(function () {
    btnNextDetailKontrakTreatyKeluarExclude();
    btnPreviousDetailKontrakTreatyKeluarExclude();
});

function searchFilterDetailKontrakTreatyKeluarExclude(e) {
    const gridReq = buildGridRequest(e);

    return {
        grid: gridReq,
        kd_cb: $("#kd_cb").val(),
        kd_jns_sor: $("#kd_jns_sor").val(),
        kd_tty_pps: $("#kd_tty_pps").val()
    };
}

function btnNextDetailKontrakTreatyKeluarExclude(){
    $('#btn-next-detailKontrakTreatyKeluarExclude').click(function () {
        $("#kontrakTreatyKeluarTab").getKendoTabStrip().select(4);
    });
}

function btnPreviousDetailKontrakTreatyKeluarExclude(){
    $('#btn-previous-detailKontrakTreatyKeluarExclude').click(function () {
        $("#kontrakTreatyKeluarTab").getKendoTabStrip().select(2);
    });
}


function btnAddDetailKontrakTreatyKeluarExclude_OnClick() {
    openWindow('#DetailKontrakTreatyKeluarWindow',`/KontrakTreatyKeluar/AddDetailKontrakTreatyKeluarExclude?kd_cb=${$("#kd_cb").val()}&kd_jns_sor=${$("#kd_jns_sor").val()}&kd_tty_pps=${$("#kd_tty_pps").val()}`, 'Add Exclude');
}

function onDeleteDetailKontrakTreatyKeluarExclude(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    showConfirmation('Confirmation', `Are you sure you want to delete Exclude?`,
        function () {
            showProgressOnGrid('#DetailKontrakTreatyKeluarExcludeGrid');
            setTimeout(function () { deleteDetailKontrakTreatyKeluarExclude(dataItem); }, 500);
        }
    );
}

function deleteDetailKontrakTreatyKeluarExclude(dataItem) {
    ajaxGet(`/KontrakTreatyKeluar/DeleteDetailKontrakTreatyKeluarExclude?kd_cb=${dataItem.kd_cb}&kd_jns_sor=${dataItem.kd_jns_sor}&kd_tty_pps=${dataItem.kd_tty_pps}&kd_okup=${dataItem.kd_okup}`, function (response) {
        if (response.Result) {
            showMessage('Success', 'Data has been deleted');
            refreshGrid("#DetailKontrakTreatyKeluarExcludeGrid");
        }
        else if (!response.Result) {
            showMessage('Error', 'Delete data is failed, this data is already used');
            refreshGrid("#DetailKontrakTreatyKeluarExcludeGrid");
        }
        else
            $("#DetailKontrakTreatyKeluarWindow").html(response);

        closeProgressOnGrid('#DetailKontrakTreatyKeluarExcludeGrid');
    }, AjaxContentType.URLENCODED);
}