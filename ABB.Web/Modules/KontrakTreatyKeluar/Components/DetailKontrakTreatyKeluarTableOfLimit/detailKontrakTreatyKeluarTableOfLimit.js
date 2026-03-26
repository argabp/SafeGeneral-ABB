$(document).ready(function () {
    btnPreviousDetailKontrakTreatyKeluarTableOfLimit();
});

function searchFilterDetailKontrakTreatyKeluarTableOfLimit(e) {
    const gridReq = buildGridRequest(e);

    return {
        grid: gridReq,
        kd_cb: $("#kd_cb").val(),
        kd_jns_sor: $("#kd_jns_sor").val(),
        kd_tty_pps: $("#kd_tty_pps").val()
    };
}

function btnPreviousDetailKontrakTreatyKeluarTableOfLimit(){
    $('#btn-previous-detailKontrakTreatyKeluarTableOfLimit').click(function () {
        $("#kontrakTreatyKeluarTab").getKendoTabStrip().select(4);
    });
}


function btnAddDetailKontrakTreatyKeluarTableOfLimit_OnClick() {
    openWindow('#DetailKontrakTreatyKeluarWindow',`/KontrakTreatyKeluar/AddDetailKontrakTreatyKeluarTableOfLimit?kd_cb=${$("#kd_cb").val()}&kd_jns_sor=${$("#kd_jns_sor").val()}&kd_tty_pps=${$("#kd_tty_pps").val()}`, 'Add Table Of Limit');
}

function onEditDetailKontrakTreatyKeluarTableOfLimit(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    console.log('dataItem', dataItem);
    openWindow('#DetailKontrakTreatyKeluarWindow', `/KontrakTreatyKeluar/EditDetailKontrakTreatyKeluarTableOfLimit?kd_cb=${dataItem.kd_cb}&kd_jns_sor=${dataItem.kd_jns_sor}&kd_tty_pps=${dataItem.kd_tty_pps}&kd_okup=${dataItem.kd_okup}&category_rsk=${dataItem.category_rsk}&kd_kls_konstr=${dataItem.kd_kls_konstr}`, 'Edit Table Of Limit');
}

function onDeleteDetailKontrakTreatyKeluarTableOfLimit(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    showConfirmation('Confirmation', `Are you sure you want to delete Table Of Limit?`,
        function () {
            showProgressOnGrid('#DetailKontrakTreatyKeluarTableOfLimitGrid');
            setTimeout(function () { deleteDetailKontrakTreatyKeluarTableOfLimit(dataItem); }, 500);
        }
    );
}

function deleteDetailKontrakTreatyKeluarTableOfLimit(dataItem) {
    ajaxGet(`/KontrakTreatyKeluar/DeleteDetailKontrakTreatyKeluarTableOfLimit?kd_cb=${dataItem.kd_cb}&kd_jns_sor=${dataItem.kd_jns_sor}&kd_tty_pps=${dataItem.kd_tty_pps}&kd_okup=${dataItem.kd_okup}&category_rsk=${dataItem.category_rsk}&kd_kls_konstr=${dataItem.kd_kls_konstr}`, function (response) {
        if (response.Result) {
            showMessage('Success', 'Data has been deleted');
            refreshGrid("#DetailKontrakTreatyKeluarTableOfLimitGrid");
        }
        else if (!response.Result) {
            showMessage('Error', 'Delete data is failed, this data is already used');
            refreshGrid("#DetailKontrakTreatyKeluarTableOfLimitGrid");
        }
        else
            $("#DetailKontrakTreatyKeluarWindow").html(response);

        closeProgressOnGrid('#DetailKontrakTreatyKeluarTableOfLimitGrid');
    }, AjaxContentType.URLENCODED);
}