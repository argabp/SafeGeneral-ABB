$(document).ready(function () {
    btnPreviousObyek();
    btnNextObyek();
    searchKeywordObyek_OnKeyUp();
    btnAddAkseptasiObyek_Click();
});


function btnPreviousObyek(){
    $('#btn-previous-akseptasiResikoObyek').click(function () {
        $("#resikoTab").getKendoTabStrip().select(1);
    });
}
function btnNextObyek(){
    $('#btn-next-akseptasiResikoObyek').click(function () {
        $("#resikoTab").getKendoTabStrip().select(3);
    });
}

function searchKeywordObyek_OnKeyUp() {
    $('#SearchKeywordObyek').keyup(function () {
        refreshGrid("#AkseptasiObyekGrid");
    });
}

function openAkseptasiObyekWindow(url, title) {
    openWindow('#AkseptasiObyekWindow', url, title);
}

function btnAddAkseptasiObyek_Click() {
    $('#btnAddNewAkseptasiObyek').click(function () {
        openAkseptasiObyekWindow(`/Akseptasi/AddObyek?kd_cb=${$("#kd_cb").val()}&kd_cob=${$("#kd_cob").val()}&kd_scob=${$("#kd_scob").val()}&kd_thn=${$("#kd_thn").val()}&no_aks=${$("#no_aks").val()}&no_updt=${$("#no_updt").val()}&no_rsk=${resiko.no_rsk}&pst_share=${resiko.pst_share_bgu}`, 'Add New Obyek');
    });
}

function btnEditAkseptasiObyek_OnClick(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    console.log('dataItem', dataItem);
    openAkseptasiObyekWindow(`/Akseptasi/EditObyek?kd_cb=${dataItem.kd_cb}&kd_cob=${dataItem.kd_cob}&kd_scob=${dataItem.kd_scob}&kd_thn=${dataItem.kd_thn}&no_aks=${dataItem.no_aks}&no_updt=${dataItem.no_updt}&no_rsk=${dataItem.no_rsk}&kd_endt=${dataItem.kd_endt}&no_oby=${dataItem.no_oby}&pst_share=${resiko.pst_share_bgu}`, 'Edit Obyek');
}

function btnDeleteAkseptasiObyek_OnClick(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    showConfirmation('Confirmation', `Are you sure you want to delete Akseptasi Obyek?`,
        function () {
            showProgressOnGrid('#AkseptasiObyekGrid');
            setTimeout(function () { deleteAkseptasiObyek(dataItem); }, 500);
        }
    );
}
function searchFilterObyek() {
    return {
        searchkeyword: $("#SearchKeywordObyek").val(),
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

function deleteAkseptasiObyek(dataItem) {
    ajaxGet(`/Akseptasi/DeleteObyek?kd_cb=${dataItem.kd_cb}&kd_cob=${dataItem.kd_cob}&kd_scob=${dataItem.kd_scob}&kd_thn=${dataItem.kd_thn}&no_aks=${dataItem.no_aks}&no_updt=${dataItem.no_updt}&no_rsk=${dataItem.no_rsk}&kd_endt=${dataItem.kd_endt}&no_oby=${dataItem.no_oby}`, function (response) {
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
