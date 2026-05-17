$(document).ready(function () {
    searchKeyword_OnKeyUp();
});

function searchFilterKontrakTreatyMasukXOL(e) {
    const gridReq = buildGridRequest(e, "SearchKeyword");

    return {
        grid: gridReq
    };
}

function searchKeyword_OnKeyUp() {
    $('#SearchKeyword').keyup(function () {
        refreshGrid("#KontrakTreatyMasukXOLGrid");
    });
}

function openKontrakTreatyMasukXOLWindow(url, title) {
    openWindow('#KontrakTreatyMasukXOLWindow', url, title);
}

function onEditKontrakTreatyMasukXOL(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    console.log('dataItem', dataItem);
    openKontrakTreatyMasukXOLWindow(`/KontrakTreatyMasukXOL/Edit?kd_cb=${dataItem.kd_cb}&kd_jns_sor=${dataItem.kd_jns_sor}&kd_tty_msk=${dataItem.kd_tty_msk}`, 'Edit Kontrak Treaty Masuk');
}

function onDeleteKontrakTreatyMasukXOL(e){
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    showConfirmation('Confirmation', `Are you sure you want to delete?`,
        function () {
            showProgressOnGrid('#KontrakTreatyMasukXOLGrid');

            ajaxGet(`/KontrakTreatyMasukXOL/DeleteKontrakTreatyMasukXOL?kd_cb=${dataItem.kd_cb}&kd_jns_sor=${dataItem.kd_jns_sor}&kd_tty_msk=${dataItem.kd_tty_msk}`,
                function (response) {
                    if (response.Result == "OK") {
                        showMessage('Success', response.Message);
                    } else {
                        showMessage('Error', response.Message);
                    }

                    refreshGrid("#KontrakTreatyMasukXOLGrid");
                    closeProgressOnGrid('#KontrakTreatyMasukXOLGrid');
                }
            );
        }
    );
}

function btnAddKontrakTreatyMasukXOL_OnClick() {
    openWindow('#KontrakTreatyMasukXOLWindow', `/KontrakTreatyMasukXOL/Add`, 'Add');
}