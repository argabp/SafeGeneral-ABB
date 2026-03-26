$(document).ready(function () {
    btnNextDetailKontrakTreatyKeluarKoasuransi();
    btnPreviousDetailKontrakTreatyKeluarKoasuransi();
});

function searchFilterDetailKontrakTreatyKeluarKoasuransi(e) {
    const gridReq = buildGridRequest(e);

    return {
        grid: gridReq,
        kd_cb: $("#kd_cb").val(),
        kd_jns_sor: $("#kd_jns_sor").val(),
        kd_tty_pps: $("#kd_tty_pps").val()
    };
}

function btnNextDetailKontrakTreatyKeluarKoasuransi(){
    $('#btn-next-detailKontrakTreatyKeluarKoasuransi').click(function () {
        $("#kontrakTreatyKeluarTab").getKendoTabStrip().select(5);
    });
}

function btnPreviousDetailKontrakTreatyKeluarKoasuransi(){
    $('#btn-previous-detailKontrakTreatyKeluarKoasuransi').click(function () {
        $("#kontrakTreatyKeluarTab").getKendoTabStrip().select(3);
    });
}


function btnAddDetailKontrakTreatyKeluarKoasuransi_OnClick() {
    openWindow('#DetailKontrakTreatyKeluarWindow',`/KontrakTreatyKeluar/AddDetailKontrakTreatyKeluarKoasuransi?kd_cb=${$("#kd_cb").val()}&kd_jns_sor=${$("#kd_jns_sor").val()}&kd_tty_pps=${$("#kd_tty_pps").val()}`, 'Add Koasuransi');
}

function onEditDetailKontrakTreatyKeluarKoasuransi(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    console.log('dataItem', dataItem);
    openWindow('#DetailKontrakTreatyKeluarWindow', `/KontrakTreatyKeluar/EditDetailKontrakTreatyKeluarKoasuransi?kd_cb=${dataItem.kd_cb}&kd_jns_sor=${dataItem.kd_jns_sor}&kd_tty_pps=${dataItem.kd_tty_pps}&no_urut=${dataItem.no_urut}`, 'Edit Koasuransi');
}

function onDeleteDetailKontrakTreatyKeluarKoasuransi(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    showConfirmation('Confirmation', `Are you sure you want to delete Koasuransi?`,
        function () {
            showProgressOnGrid('#DetailKontrakTreatyKeluarKoasuransiGrid');
            setTimeout(function () { deleteDetailKontrakTreatyKeluarKoasuransi(dataItem); }, 500);
        }
    );
}

function deleteDetailKontrakTreatyKeluarKoasuransi(dataItem) {
    ajaxGet(`/KontrakTreatyKeluar/DeleteDetailKontrakTreatyKeluarKoasuransi?kd_cb=${dataItem.kd_cb}&kd_jns_sor=${dataItem.kd_jns_sor}&kd_tty_pps=${dataItem.kd_tty_pps}&no_urut=${dataItem.no_urut}`, function (response) {
        if (response.Result) {
            showMessage('Success', 'Data has been deleted');
            refreshGrid("#DetailKontrakTreatyKeluarKoasuransiGrid");
        }
        else if (!response.Result) {
            showMessage('Error', 'Delete data is failed, this data is already used');
            refreshGrid("#DetailKontrakTreatyKeluarKoasuransiGrid");
        }
        else
            $("#DetailKontrakTreatyKeluarWindow").html(response);

        closeProgressOnGrid('#DetailKontrakTreatyKeluarKoasuransiGrid');
    }, AjaxContentType.URLENCODED);
}