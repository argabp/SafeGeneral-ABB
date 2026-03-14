$(document).ready(function () {
    searchKeyword_OnKeyUp();
});

function searchFilterKontrakTreatyMasuk(e) {
    const gridReq = buildGridRequest(e, "SearchKeyword");

    return {
        grid: gridReq
    };
}

function searchKeyword_OnKeyUp() {
    $('#SearchKeyword').keyup(function () {
        refreshGrid("#KontrakTreatyMasukGrid");
    });
}

function openKontrakTreatyMasukWindow(url, title) {
    openWindow('#KontrakTreatyMasukWindow', url, title);
}

function onEditKontrakTreatyMasuk(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    console.log('dataItem', dataItem);
    openKontrakTreatyMasukWindow(`/KontrakTreatyMasuk/Edit?kd_cb=${dataItem.kd_cb}&kd_jns_sor=${dataItem.kd_jns_sor}&kd_tty_msk=${dataItem.kd_tty_msk}`, 'Edit Kontrak Treaty Masuk');
}

function onDeleteKontrakTreatyMasuk(e){
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    showConfirmation('Confirmation', `Are you sure you want to delete?`,
        function () {
            showProgressOnGrid('#KontrakTreatyMasukGrid');

            ajaxGet(`/KontrakTreatyMasuk/DeleteKontrakTreatyMasuk?kd_cb=${dataItem.kd_cb}&kd_jns_sor=${dataItem.kd_jns_sor}&kd_tty_msk=${dataItem.kd_tty_msk}`,
                function (response) {
                    if (response.Result == "OK") {
                        showMessage('Success', response.Message);
                    } else {
                        showMessage('Error', response.Message);
                    }

                    refreshGrid("#KontrakTreatyMasukGrid");
                    closeProgressOnGrid('#KontrakTreatyMasukGrid');
                }
            );
        }
    );
}

function btnAddKontrakTreatyMasuk_OnClick() {
    openWindow('#KontrakTreatyMasukWindow', `/KontrakTreatyMasuk/Add`, 'Add');
}