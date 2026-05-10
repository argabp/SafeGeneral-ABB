$(document).ready(function () {
    searchKeyword_OnKeyUp();
});

var notaTreatyMasuk;

function searchFilterNotaTreatyMasukXOL(e) {
    const gridReq = buildGridRequest(e, "SearchKeyword");

    return {
        grid: gridReq
    };
}

function searchKeyword_OnKeyUp() {
    $('#SearchKeyword').keyup(function () {
        refreshGrid("#NotaTreatyMasukXOLGrid");
    });
}

function openNotaTreatyMasukXOLWindow(url, title) {
    openWindow('#NotaTreatyMasukXOLWindow', url, title);
}

function onEditNotaTreatyMasukXOL(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    console.log('dataItem', dataItem);
    openNotaTreatyMasukXOLWindow(`/NotaTreatyMasukXOL/Edit?kd_cb=${dataItem.kd_cb}&kd_thn=${dataItem.kd_thn}&kd_bln=${dataItem.kd_bln}&kd_jns_sor=${dataItem.kd_jns_sor}&kd_tty_msk=${dataItem.kd_tty_msk}&kd_mtu=${dataItem.kd_mtu}&no_tr=${dataItem.no_tr}`, 'Edit');
}

function onViewNotaTreatyMasukXOL(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    console.log('dataItem', dataItem);
    openNotaTreatyMasukXOLWindow(`/NotaTreatyMasukXOL/View?kd_cb=${dataItem.kd_cb}&kd_thn=${dataItem.kd_thn}&kd_bln=${dataItem.kd_bln}&kd_jns_sor=${dataItem.kd_jns_sor}&kd_tty_msk=${dataItem.kd_tty_msk}&kd_mtu=${dataItem.kd_mtu}&no_tr=${dataItem.no_tr}`, 'View');
}

function onNota(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    notaTreatyMasuk = dataItem;
    console.log('dataItem', dataItem);
    openNotaTreatyMasukXOLWindow(`/NotaTreatyMasukXOL/Nota?kd_cb=${dataItem.kd_cb}&kd_thn=${dataItem.kd_thn}&kd_bln=${dataItem.kd_bln}&kd_jns_sor=${dataItem.kd_jns_sor}&kd_tty_msk=${dataItem.kd_tty_msk}&kd_mtu=${dataItem.kd_mtu}&no_tr=${dataItem.no_tr}`, 'Nota');
}

function onDeleteNotaTreatyMasukXOL(e){
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    showConfirmation('Confirmation', `Are you sure you want to delete?`,
        function () {
            showProgressOnGrid('#NotaTreatyMasukXOLGrid');

            ajaxGet(`/NotaTreatyMasukXOL/DeleteNotaTreatyMasukXOL?kd_cb=${dataItem.kd_cb}&kd_thn=${dataItem.kd_thn}&kd_bln=${dataItem.kd_bln}&kd_jns_sor=${dataItem.kd_jns_sor}&kd_tty_msk=${dataItem.kd_tty_msk}&kd_mtu=${dataItem.kd_mtu}&no_tr=${dataItem.no_tr}`,
                function (response) {
                    if (response.Result == "OK") {
                        showMessage('Success', response.Message);
                    } else {
                        showMessage('Error', response.Message);
                    }

                    refreshGrid("#NotaTreatyMasukXOLGrid");
                    closeProgressOnGrid('#NotaTreatyMasukXOLGrid');
                }
            );
        }
    );
}

function btnAddNotaTreatyMasukXOL_OnClick() {
    openWindow('#NotaTreatyMasukXOLWindow', `/NotaTreatyMasukXOL/Add`, 'Add');
}

function OnNotaTreatyMasukXOLDataBound(e){
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
                buttonContainer.find(".k-grid-EditNotaTreatyMasukXOL").hide();
                buttonContainer.find(".k-grid-DeleteNotaTreatyMasukXOL").hide();
            } else {
                buttonContainer.find(".k-grid-ViewNotaTreatyMasukXOL").hide();
            }
        }
    });

    gridAutoFit(grid);
}