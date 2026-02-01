$(document).ready(function () {
    btnPreviousOtherMotorDetail();
    searchKeywordOtherMotorDetail_OnKeyUp();
    btnAddAkseptasiOtherMotorDetail_Click();
});


function btnPreviousOtherMotorDetail(){
    $('#btn-previous-akseptasiOtherMotorDetail').click(function () {
        $("#resikoOtherTab").getKendoTabStrip().select(0);
    });
}

function searchKeywordOtherMotorDetail_OnKeyUp() {
    $('#SearchKeywordOtherMotorDetail').keyup(function () {
        refreshGrid("#AkseptasiOtherMotorDetailGrid");
    });
}

function openAkseptasiOtherMotorDetailWindow(url, title) {
    openWindow('#AkseptasiOtherMotorDetailWindow', url, title);
}


function btnAddAkseptasiOtherMotorDetail_Click() {
    $('#btnAddNewAkseptasiOtherMotorDetail').click(function () {
        openAkseptasiOtherMotorDetailWindow(`/Akseptasi/AddOtherMotorDetail?kd_cb=${$("#kd_cb").val()}&kd_cob=${$("#kd_cob").val()}&kd_scob=${$("#kd_scob").val()}&kd_thn=${$("#kd_thn").val()}&no_aks=${$("#no_aks").val()}&no_updt=${$("#no_updt").val()}`, 'Add New Other Motor Detail');
    });
}

function btnEditAkseptasiOtherMotorDetail_OnClick(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    console.log('dataItem', dataItem);
    openAkseptasiOtherMotorDetailWindow(`/Akseptasi/EditOtherMotorDetail?kd_cb=${dataItem.kd_cb}&kd_cob=${dataItem.kd_cob}&kd_scob=${dataItem.kd_scob}&kd_thn=${dataItem.kd_thn}&no_aks=${dataItem.no_aks}&no_updt=${dataItem.no_updt}&no_rsk=${dataItem.no_rsk}&kd_endt=${dataItem.kd_endt}&thn_ptg_kend=${dataItem.thn_ptg_kend}`, 'Edit Other Motor Detail');
}
function btnDeleteAkseptasiOtherMotorDetail_OnClick(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    showConfirmation('Confirmation', `Are you sure you want to delete Akseptasi Resiko Other Motor Detail?`,
        function () {
            showProgressOnGrid('#AkseptasiOtherMotorDetailGrid');
            setTimeout(function () { deleteAkseptasiOtherMotorDetail(dataItem); }, 500);
        }
    );
}
function searchFilterOtherMotorDetail() {
    return {
        searchkeyword: $("#SearchKeywordOtherMotorDetail").val(),
        kd_cb: $("#kd_cb").val(),
        kd_cob: $("#kd_cob").val(),
        kd_scob: $("#kd_scob").val(),
        kd_thn: $("#kd_thn").val(),
        no_aks: $("#no_aks").val(),
        no_updt: resiko.no_updt,
        kd_endt: $resiko.kd_endt,
        no_rsk: resiko?.no_rsk,
    }
}

function deleteAkseptasiOtherMotorDetail(dataItem) {
    ajaxGet(`/Akseptasi/DeleteOtherMotorDetail?kd_cb=${dataItem.kd_cb}&kd_cob=${dataItem.kd_cob}&kd_scob=${dataItem.kd_scob}&kd_thn=${dataItem.kd_thn}&no_aks=${dataItem.no_aks}&no_updt=${dataItem.no_updt}&no_rsk=${dataItem.no_rsk}&kd_endt=${dataItem.kd_endt}&thn_ptg_kend=${dataItem.thn_ptg_kend}`, function (response) {
        if (response.Result) {
            showMessage('Success', 'Data has been deleted');
            refreshGrid("#AkseptasiOtherMotorDetailGrid");
        }
        else if (!response.Result) {
            showMessage('Error', 'Delete data is failed, this data is already used');
            refreshGrid("#AkseptasiOtherMotorDetailGrid");
        }
        else
            $("#AkseptasiOtherMotorDetailWindow").html(response);

        closeProgressOnGrid('#AkseptasiOtherMotorDetailGrid');
    }, AjaxContentType.URLENCODED);
}