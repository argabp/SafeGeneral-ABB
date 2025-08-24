$(document).ready(function () {
    btnNextPranota();
    btnRecalculatePremi_Click();
});

var pranota;

function btnNextPranota(){
    $('#btn-next-akseptasiPranota').click(function () {
        $("#pranotaTab").getKendoTabStrip().select(1);
    });
}

function searchKeywordPranota_OnKeyUp() {
    $('#SearchKeywordPranota').keyup(function () {
        refreshGrid("#AkseptasiPranotaGrid");
    });
}

function openAkseptasiPranotaWindow(url, title) {
    openWindow('#AkseptasiPranotaWindow', url, title);
}

function btnRecalculatePremi_Click() {
    $('#btnRecalculatePremi').click(function () {
        showProgress('#AkseptasiWindow');
        setTimeout(function () {
            ajaxGet(`/Akseptasi/RecalculatePremi?kd_cb=${$("#kd_cb").val()}&kd_cob=${$("#kd_cob").val()}&kd_scob=${$("#kd_scob").val()}&kd_thn=${$("#kd_thn").val()}&no_aks=${$("#no_aks").val()}&no_updt=${$("#no_updt").val()}`,
                function (response) {
                    refreshGrid("#AkseptasiPranotaGrid");
                    closeProgress('#AkseptasiWindow');
                }
            );
        }, 500);
    });
}

function btnEditAkseptasiPranota_OnClick(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    openAkseptasiPranotaWindow(`/Akseptasi/EditPranota?kd_cb=${dataItem.kd_cb}&kd_cob=${dataItem.kd_cob}&kd_scob=${dataItem.kd_scob}&kd_thn=${dataItem.kd_thn}&no_aks=${dataItem.no_aks}&no_updt=${dataItem.no_updt}&kd_mtu=${dataItem.kd_mtu}`, 'Edit Pranota');
}

function btnDeleteAkseptasiPranota_OnClick(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    showConfirmation('Confirmation', `Are you sure you want to delete Akseptasi Pranota?`,
        function () {
            showProgressOnGrid('#AkseptasiPranotaGrid');
            setTimeout(function () { deleteAkseptasiPranota(dataItem); }, 500);
        }
    );
}

function searchFilterPranota() {
    return {
        searchkeyword: $("#SearchKeywordPranota").val(),
        kd_cb: $("#kd_cb").val(),
        kd_cob: $("#kd_cob").val(),
        kd_scob: $("#kd_scob").val(),
        kd_thn: $("#kd_thn").val(),
        no_aks: $("#no_aks").val(),
        no_updt: $("#no_updt").val(),
    }
}

function deleteAkseptasiPranota(dataItem) {
    ajaxGet(`/Akseptasi/DeletePranota?kd_cb=${dataItem.kd_cb}&kd_cob=${dataItem.kd_cob}&kd_scob=${dataItem.kd_scob}&kd_thn=${dataItem.kd_thn}&no_aks=${dataItem.no_aks}&no_updt=${dataItem.no_updt}&kd_mtu=${dataItem.kd_mtu}`, function (response) {
        if (response.Result) {
            showMessage('Success', 'Data has been deleted');
            refreshGrid("#AkseptasiPranotaGrid");
        }
        else if (!response.Result) {
            showMessage('Error', 'Delete data is failed, this data is already used');
            refreshGrid("#AkseptasiPranotaGrid");
        }
        else
            $("#AkseptasiPranotaWindow").html(response);

        closeProgressOnGrid('#AkseptasiPranotaGrid');
    }, AjaxContentType.URLENCODED);
}

function OnPranotaChange(e){
    var grid = e.sender;
    pranota = grid.dataItem(this.select());
    refreshGrid("#AkseptasiPranotaKoasGrid");
}