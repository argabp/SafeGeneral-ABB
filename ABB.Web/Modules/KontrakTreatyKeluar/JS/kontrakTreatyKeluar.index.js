$(document).ready(function () {
    searchKeyword_OnKeyUp();
});

function searchFilterKontrakTreatyKeluar(e) {
    const gridReq = buildGridRequest(e, "SearchKeyword");

    return {
        grid: gridReq
    };
}

function searchFilterDetailKontrakTreatyKeluar(e) {
    const gridReq = buildGridRequest(e);

    return {
        grid: gridReq,
        kd_cb: $("#kd_cb").val(),
        kd_jns_sor: $("#kd_jns_sor").val(),
        kd_tty_pps: $("#kd_tty_pps").val()
    };
}

function searchKeyword_OnKeyUp() {
    $('#SearchKeyword').keyup(function () {
        refreshGrid("#KontrakTreatyKeluarGrid");
    });
}

function openKontrakTreatyKeluarWindow(url, title) {
    openWindow('#KontrakTreatyKeluarWindow', url, title);
}

function onEditKontrakTreatyKeluar(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    console.log('dataItem', dataItem);
    openKontrakTreatyKeluarWindow(`/KontrakTreatyKeluar/Edit?kd_cb=${dataItem.kd_cb}&kd_jns_sor=${dataItem.kd_jns_sor}&kd_tty_pps=${dataItem.kd_tty_pps}`, 'Edit Kontrak Treaty Keluar ');
}

function onDeleteKontrakTreatyKeluar(e){
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    showConfirmation('Confirmation', `Are you sure you want to delete?`,
        function () {
            showProgressOnGrid('#KontrakTreatyKeluarGrid');

            ajaxGet(`/KontrakTreatyKeluar/DeleteKontrakTreatyKeluar?kd_cb=${dataItem.kd_cb}&kd_jns_sor=${dataItem.kd_jns_sor}&kd_tty_pps=${dataItem.kd_tty_pps}`,
                function (response) {
                    if (response.Result == "OK") {
                        showMessage('Success', response.Message);
                    } else {
                        showMessage('Error', response.Message);
                    }

                    refreshGrid("#KontrakTreatyKeluarGrid");
                    closeProgressOnGrid('#KontrakTreatyKeluarGrid');
                }
            );
        }
    );
}

function btnAddKontrakTreatyKeluar_OnClick() {
    openWindow('#KontrakTreatyKeluarWindow', `/KontrakTreatyKeluar/Add`, 'Add');
}

function btnAddDetailKontrakTreatyKeluar_OnClick() {
    openWindow('#DetailKontrakTreatyKeluarWindow', `/KontrakTreatyKeluar/AddDetail`, 'Add Detail');
}