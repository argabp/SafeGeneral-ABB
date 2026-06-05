$(document).ready(function () {
    searchKeyword_OnKeyUp();
});

function searchFilterProsesPremiXOLKeluar(e) {
    const gridReq = buildGridRequest(e, "SearchKeyword");

    return {
        grid: gridReq
    };
}

function searchKeyword_OnKeyUp() {
    $('#SearchKeyword').keyup(function () {
        refreshGrid("#ProsesPremiXOLKeluarGrid");
    });
}

function openProsesPremiXOLKeluarWindow(url, title) {
    openWindow('#ProsesPremiXOLKeluarWindow', url, title);
}

function onEditProsesPremiXOLKeluar(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    console.log('dataItem', dataItem);
    openProsesPremiXOLKeluarWindow(`/ProsesPremiXOLKeluar/Edit?kd_cb=${dataItem.kd_cb}&kd_thn=${dataItem.kd_thn}&kd_bln=${dataItem.kd_bln}&kd_jns_sor=${dataItem.kd_jns_sor}&kd_tty_npps=${dataItem.kd_tty_npps}&kd_mtu=${dataItem.kd_mtu}&no_tr=${dataItem.no_tr}`, 'Edit');
}

function onViewProsesPremiXOLKeluar(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    console.log('dataItem', dataItem);
    openProsesPremiXOLKeluarWindow(`/ProsesPremiXOLKeluar/View?kd_cb=${dataItem.kd_cb}&kd_thn=${dataItem.kd_thn}&kd_bln=${dataItem.kd_bln}&kd_jns_sor=${dataItem.kd_jns_sor}&kd_tty_npps=${dataItem.kd_tty_npps}&kd_mtu=${dataItem.kd_mtu}&no_tr=${dataItem.no_tr}`, 'View');
}

function onProsesPremiXOLKeluar(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    showConfirmation('Confirmation', `Are you sure you want to Proses?`,
        function () {
            showProgressOnGrid('#ProsesPremiXOLKeluarGrid');
            
            var payload = { ...dataItem };

            payload.tgl_closing = kendo.toString(
                dataItem.tgl_closing,
                "yyyy-MM-dd"
            );
            
            ajaxPost("/ProsesPremiXOLKeluar/Proses", JSON.stringify(payload),
                function (response) {
                    if (response.Result === "OK") {
                        showMessage("Success", response.Message)
                    } else {
                        showMessage('Error', response.Message);
                    }
                    refreshGrid('#ProsesPremiXOLKeluarGrid');
                },
            );
        }
    );
}

function onCancelProsesPremiXOLKeluar(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    showConfirmation('Confirmation', `Are you sure you want to Cancel Proses?`,
        function () {
            showProgressOnGrid('#ProsesPremiXOLKeluarGrid');

            var payload = { ...dataItem };

            payload.tgl_closing = kendo.toString(
                dataItem.tgl_closing,
                "yyyy-MM-dd"
            );

            ajaxPost("/ProsesPremiXOLKeluar/CancelProses", JSON.stringify(payload),
                function (response) {
                    if (response.Result === "OK") {
                        showMessage("Success", response.Message)
                    } else {
                        showMessage('Error', response.Message);
                    }
                    refreshGrid('#ProsesPremiXOLKeluarGrid');
                },
            );
        }
    );
}

function onDeleteProsesPremiXOLKeluar(e){
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    showConfirmation('Confirmation', `Are you sure you want to delete?`,
        function () {
            showProgressOnGrid('#ProsesPremiXOLKeluarGrid');

            ajaxGet(`/ProsesPremiXOLKeluar/DeleteProsesPremiXOLKeluar?kd_cb=${dataItem.kd_cb}&kd_thn=${dataItem.kd_thn}&kd_bln=${dataItem.kd_bln}&kd_jns_sor=${dataItem.kd_jns_sor}&kd_tty_npps=${dataItem.kd_tty_npps}&kd_mtu=${dataItem.kd_mtu}&no_tr=${dataItem.no_tr}`,
                function (response) {
                    if (response.Result == "OK") {
                        showMessage('Success', response.Message);
                    } else {
                        showMessage('Error', response.Message);
                    }

                    refreshGrid("#ProsesPremiXOLKeluarGrid");
                    closeProgressOnGrid('#ProsesPremiXOLKeluarGrid');
                }
            );
        }
    );
}

function btnAddProsesPremiXOLKeluar_OnClick() {
    openWindow('#ProsesPremiXOLKeluarWindow', `/ProsesPremiXOLKeluar/Add`, 'Add');
}

function OnProsesPremiXOLKeluarDataBound(e){
    var grid = this;

    grid.tbody.find("tr").each(function(e, element) {
        var dataItem = grid.dataItem(this);
        var uid = $(this).data("uid");

        // Find button container - try locked column first, then regular
        var buttonContainer = grid.element.find(".k-grid-content-locked tr[data-uid='" + uid + "'] .k-command-cell");
        if (!buttonContainer.length) {
            buttonContainer = $(this).find(".k-command-cell");
        }

        if (buttonContainer.length) {
            if(dataItem.flag_closing == "Y"){
                buttonContainer.find(".k-grid-EditProsesPremiXOLKeluar").hide();
                buttonContainer.find(".k-grid-DeleteProsesPremiXOLKeluar").hide();
                buttonContainer.find(".k-grid-Proses").hide();
            } else {
                buttonContainer.find(".k-grid-ViewProsesPremiXOLKeluar").hide();
                buttonContainer.find(".k-grid-CancelProses").hide();
            }
        }
    });

    gridAutoFit(grid);
}