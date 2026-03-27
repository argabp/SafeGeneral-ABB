$(document).ready(function () {
    searchKeyword_OnKeyUp();
});

function searchFilterKontrakTreatyKeluarXOL(e) {
    const gridReq = buildGridRequest(e, "SearchKeyword");

    return {
        grid: gridReq
    };
}

function searchFilterDetailKontrakTreatyKeluarXOL(e) {
    const gridReq = buildGridRequest(e);

    return {
        grid: gridReq,
        kd_cb: $("#kd_cb").val(),
        kd_jns_sor: $("#kd_jns_sor").val(),
        kd_tty_npps: $("#kd_tty_npps").val()
    };
}

function searchKeyword_OnKeyUp() {
    $('#SearchKeyword').keyup(function () {
        refreshGrid("#KontrakTreatyKeluarXOLGrid");
    });
}

function openKontrakTreatyKeluarXOLWindow(url, title) {
    openWindow('#KontrakTreatyKeluarXOLWindow', url, title);
}

function onEditKontrakTreatyKeluarXOL(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    console.log('dataItem', dataItem);
    openKontrakTreatyKeluarXOLWindow(`/KontrakTreatyKeluarXOL/Edit?kd_cb=${dataItem.kd_cb}&kd_jns_sor=${dataItem.kd_jns_sor}&kd_tty_npps=${dataItem.kd_tty_npps}`, 'Edit');
}

function onDeleteKontrakTreatyKeluarXOL(e){
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    showConfirmation('Confirmation', `Are you sure you want to delete?`,
        function () {
            showProgressOnGrid('#KontrakTreatyKeluarXOLGrid');

            ajaxGet(`/KontrakTreatyKeluarXOL/DeleteKontrakTreatyKeluarXOL?kd_cb=${dataItem.kd_cb}&kd_jns_sor=${dataItem.kd_jns_sor}&kd_tty_npps=${dataItem.kd_tty_npps}`,
                function (response) {
                    if (response.Result == "OK") {
                        showMessage('Success', response.Message);
                    } else {
                        showMessage('Error', response.Message);
                    }

                    refreshGrid("#KontrakTreatyKeluarXOLGrid");
                    closeProgressOnGrid('#KontrakTreatyKeluarXOLGrid');
                }
            );
        }
    );
}

function btnAddKontrakTreatyKeluarXOL_OnClick() {
    openWindow('#KontrakTreatyKeluarXOLWindow', `/KontrakTreatyKeluarXOL/Add`, 'Add');
}

function btnAddDetailKontrakTreatyKeluarXOL_OnClick() {
    openWindow('#DetailKontrakTreatyKeluarXOLWindow', `/KontrakTreatyKeluarXOL/AddDetail`, 'Add Detail');
}