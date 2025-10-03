$(document).ready(function () {
    btnPreviousObyekCIT();
    btnNextObyekCIT();
    searchKeywordObyekCIT_OnKeyUp();
    btnAddAkseptasiObyekCIT_Click();
});


function btnPreviousObyekCIT(){
    $('#btn-previous-akseptasiResikoObyekCIT').click(function () {
        $("#resikoTab").getKendoTabStrip().select(1);
    });
}
function btnNextObyekCIT(){
    $('#btn-next-akseptasiResikoObyekCIT').click(function () {
        $("#resikoTab").getKendoTabStrip().select(3);
    });
}

function searchKeywordObyekCIT_OnKeyUp() {
    $('#SearchKeywordObyekCIT').keyup(function () {
        refreshGrid("#AkseptasiObyekGrid");
    });
}

function openAkseptasiObyekCITWindow(url, title) {
    openWindow('#AkseptasiObyekWindow', url, title);
}


function btnAddAkseptasiObyekCIT_Click() {
    $('#btnAddNewAkseptasiObyekCIT').click(function () {
        openAkseptasiObyekCITWindow(`/Akseptasi/AddObyekCIT?kd_cb=${$("#kd_cb").val()}&kd_cob=${$("#kd_cob").val()}&kd_scob=${$("#kd_scob").val()}&kd_thn=${$("#kd_thn").val()}&no_aks=${$("#no_aks").val()}&no_updt=${$("#no_updt").val()}&no_rsk=${resiko.no_rsk}&pst_share=${resiko.pst_share_bgu}`, 'Add New Obyek');
    });
}

function btnEditAkseptasiObyekCIT_OnClick(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    console.log('dataItem', dataItem);
    openAkseptasiObyekCITWindow(`/Akseptasi/EditObyekCIT?kd_cb=${dataItem.kd_cb}&kd_cob=${dataItem.kd_cob}&kd_scob=${dataItem.kd_scob}&kd_thn=${dataItem.kd_thn}&no_aks=${dataItem.no_aks}&no_updt=${dataItem.no_updt}&no_rsk=${dataItem.no_rsk}&kd_endt=${dataItem.kd_endt}&no_oby=${dataItem.no_oby}&pst_share=${resiko.pst_share_bgu}`, 'Edit Obyek');
}
function btnDeleteAkseptasiObyekCIT_OnClick(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    showConfirmation('Confirmation', `Are you sure you want to delete Akseptasi Obyek?`,
        function () {
            showProgressOnGrid('#AkseptasiObyekGrid');
            setTimeout(function () { deleteAkseptasiObyekCIT(dataItem); }, 500);
        }
    );
}
function searchFilterObyekCIT() {
    return {
        searchkeyword: $("#SearchKeywordObyekCIT").val(),
        kd_cb: $("#kd_cb").val(),
        kd_cob: $("#kd_cob").val(),
        kd_scob: $("#kd_scob").val(),
        kd_thn: $("#kd_thn").val(),
        no_aks: $("#no_aks").val(),
        no_updt: $("#no_updt").val(),
        no_rsk: resiko?.no_rsk,
        kd_endt: resiko?.kd_endt
    }
}

function deleteAkseptasiObyekCIT(dataItem) {
    ajaxGet(`/Akseptasi/DeleteObyekCIT?kd_cb=${dataItem.kd_cb}&kd_cob=${dataItem.kd_cob}&kd_scob=${dataItem.kd_scob}&kd_thn=${dataItem.kd_thn}&no_aks=${dataItem.no_aks}&no_updt=${dataItem.no_updt}&no_rsk=${dataItem.no_rsk}&kd_endt=${dataItem.kd_endt}&no_oby=${dataItem.no_oby}`, function (response) {
        if (response.Result) {
            showMessage('Success', 'Data has been deleted');
            refreshGrid("#AkseptasiObyekGrid");
        }
        else if (!response.Result) {
            showMessage('Error', 'Delete data is failed, this data is already used');
            refreshGrid("#AkseptasiObyekGrid");
        }
        else
            $("#AkseptasiObyekWindow").html(response);

        closeProgressOnGrid('#AkseptasiObyekGrid');
    }, AjaxContentType.URLENCODED);
}
