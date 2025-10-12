$(document).ready(function () {
    btnPreviousOtherCargoDetail();
    searchKeywordOtherCargoDetail_OnKeyUp();
    btnAddAkseptasiOtherCargoDetail_Click();
});


function btnPreviousOtherCargoDetail(){
    $('#btn-previous-akseptasiOtherCargoDetail').click(function () {
        $("#resikoOtherTab").getKendoTabStrip().select(0);
    });
}

function searchKeywordOtherCargoDetail_OnKeyUp() {
    $('#SearchKeywordOtherCargoDetail').keyup(function () {
        refreshGrid("#AkseptasiOtherCargoDetailGrid");
    });
}

function openAkseptasiOtherCargoDetailWindow(url, title) {
    openWindow('#AkseptasiOtherCargoDetailWindow', url, title);
}


function btnAddAkseptasiOtherCargoDetail_Click() {
    $('#btnAddNewAkseptasiOtherCargoDetail').click(function () {
        openAkseptasiOtherCargoDetailWindow(`/Akseptasi/AddOtherCargoDetail?kd_cb=${$("#kd_cb").val()}&kd_cob=${$("#kd_cob").val()}&kd_scob=${$("#kd_scob").val()}&kd_thn=${$("#kd_thn").val()}&no_aks=${$("#no_aks").val()}&no_updt=${$("#no_updt").val()}`, 'Add New Alat Angkut');
    });
}

function btnEditAkseptasiOtherCargoDetail_OnClick(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    console.log('dataItem', dataItem);
    openAkseptasiOtherCargoDetailWindow(`/Akseptasi/EditOtherCargoDetail?kd_cb=${dataItem.kd_cb}&kd_cob=${dataItem.kd_cob}&kd_scob=${dataItem.kd_scob}&kd_thn=${dataItem.kd_thn}&no_aks=${dataItem.no_aks}&no_updt=${dataItem.no_updt}&no_rsk=${dataItem.no_rsk}&kd_endt=${dataItem.kd_endt}&no_urut=${dataItem.no_urut}`, 'Edit Other Alat Angkut');
}
function btnDeleteAkseptasiOtherCargoDetail_OnClick(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    showConfirmation('Confirmation', `Are you sure you want to delete Alat Angkut?`,
        function () {
            showProgressOnGrid('#AkseptasiOtherCargoDetailGrid');
            setTimeout(function () { deleteAkseptasiOtherCargoDetail(dataItem); }, 500);
        }
    );
}
function searchFilterOtherCargoDetail() {
    return {
        searchkeyword: $("#SearchKeywordOtherCargoDetail").val(),
        kd_cb: $("#kd_cb").val(),
        kd_cob: $("#kd_cob").val(),
        kd_scob: $("#kd_scob").val(),
        kd_thn: $("#kd_thn").val(),
        no_aks: $("#no_aks").val(),
        no_updt: $("#other_cargo_detail_no_updt").val(),
        kd_endt: $("#resiko_other_kd_endt").val(),
        no_rsk: resiko?.no_rsk,
    }
}

function deleteAkseptasiOtherCargoDetail(dataItem) {
    ajaxGet(`/Akseptasi/DeleteOtherCargoDetail?kd_cb=${dataItem.kd_cb}&kd_cob=${dataItem.kd_cob}&kd_scob=${dataItem.kd_scob}&kd_thn=${dataItem.kd_thn}&no_aks=${dataItem.no_aks}&no_updt=${dataItem.no_updt}&no_rsk=${dataItem.no_rsk}&kd_endt=${dataItem.kd_endt}&no_urut=${dataItem.no_urut}`, function (response) {
        if (response.Result) {
            showMessage('Success', 'Data has been deleted');
            refreshGrid("#AkseptasiOtherCargoDetailGrid");
        }
        else if (!response.Result) {
            showMessage('Error', 'Delete data is failed, this data is already used');
            refreshGrid("#AkseptasiOtherCargoDetailGrid");
        }
        else
            $("#AkseptasiOtherCargoDetailWindow").html(response);

        closeProgressOnGrid('#AkseptasiOtherCargoDetailGrid');
    }, AjaxContentType.URLENCODED);
}