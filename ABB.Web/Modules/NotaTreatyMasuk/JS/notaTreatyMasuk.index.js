$(document).ready(function () {
    searchKeyword_OnKeyUp();
});

function searchFilterNotaTreatyMasuk(e) {
    const gridReq = buildGridRequest(e, "SearchKeyword");

    return {
        grid: gridReq
    };
}

function searchKeyword_OnKeyUp() {
    $('#SearchKeyword').keyup(function () {
        refreshGrid("#NotaTreatyMasukGrid");
    });
}

function openNotaTreatyMasukWindow(url, title) {
    openWindow('#NotaTreatyMasukWindow', url, title);
}

function onEditNotaTreatyMasuk(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    console.log('dataItem', dataItem);
    openNotaTreatyMasukWindow(`/NotaTreatyMasuk/Edit?kd_cb=${dataItem.kd_cb}&kd_thn=${dataItem.kd_thn}&kd_bln=${dataItem.kd_bln}&kd_jns_sor=${dataItem.kd_jns_sor}&kd_tty_msk=${dataItem.kd_tty_msk}&kd_mtu=${dataItem.kd_mtu}&no_tr=${dataItem.no_tr}`, 'Edit');
}

function onNota(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    console.log('dataItem', dataItem);
    openNotaTreatyMasukWindow(`/NotaTreatyMasuk/Nota?kd_cb=${dataItem.kd_cb}&kd_thn=${dataItem.kd_thn}&kd_bln=${dataItem.kd_bln}&kd_jns_sor=${dataItem.kd_jns_sor}&kd_tty_msk=${dataItem.kd_tty_msk}&kd_mtu=${dataItem.kd_mtu}&no_tr=${dataItem.no_tr}`, 'Nota');
}

function onDeleteNotaTreatyMasuk(e){
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    showConfirmation('Confirmation', `Are you sure you want to delete?`,
        function () {
            showProgressOnGrid('#NotaTreatyMasukGrid');

            ajaxGet(`/NotaTreatyMasuk/DeleteNotaTreatyMasuk?kd_cb=${dataItem.kd_cb}&kd_thn=${dataItem.kd_thn}&kd_bln=${dataItem.kd_bln}&kd_jns_sor=${dataItem.kd_jns_sor}&kd_tty_msk=${dataItem.kd_tty_msk}&kd_mtu=${dataItem.kd_mtu}&no_tr=${dataItem.no_tr}`,
                function (response) {
                    if (response.Result == "OK") {
                        showMessage('Success', response.Message);
                    } else {
                        showMessage('Error', response.Message);
                    }

                    refreshGrid("#NotaTreatyMasukGrid");
                    closeProgressOnGrid('#NotaTreatyMasukGrid');
                }
            );
        }
    );
}

function btnAddNotaTreatyMasuk_OnClick() {
    openWindow('#NotaTreatyMasukWindow', `/NotaTreatyMasuk/Add`, 'Add');
}