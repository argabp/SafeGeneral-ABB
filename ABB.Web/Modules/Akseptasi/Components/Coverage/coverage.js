$(document).ready(function () {
    btnPreviousCoverage();
    btnNextCoverage();
    searchKeywordCoverage_OnKeyUp();
    btnAddAkseptasiCoverage_Click();
});


function btnPreviousCoverage(){
    $('#btn-previous-akseptasiResikoCoverage').click(function () {
        $("#resikoTab").getKendoTabStrip().select(0);
    });
}
function btnNextCoverage(){
    $('#btn-next-akseptasiResikoCoverage').click(function () {
        $("#resikoTab").getKendoTabStrip().select(2);
    });
}

function searchKeywordCoverage_OnKeyUp() {
    $('#SearchKeywordCoverage').keyup(function () {
        refreshGrid("#AkseptasiCoverageGrid");
    });
}

function openAkseptasiCoverageWindow(url, title) {
    openWindow('#AkseptasiCoverageWindow', url, title);
}


function btnAddAkseptasiCoverage_Click() {
    $('#btnAddNewAkseptasiCoverage').click(function () {
        openAkseptasiCoverageWindow(`/Akseptasi/AddCoverage?kd_cb=${$("#kd_cb").val()}&kd_cob=${$("#kd_cob").val()}&kd_scob=${$("#kd_scob").val()}&kd_thn=${$("#kd_thn").val()}&no_aks=${$("#no_aks").val()}&no_updt=${$("#no_updt").val()}&no_rsk=${resiko.no_rsk}&kd_endt=${resiko.kd_endt}`, 'Add New Coverage');
    });
}
function btnEditAkseptasiCoverage_OnClick(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    console.log('dataItem', dataItem);
    openAkseptasiCoverageWindow(`/Akseptasi/EditCoverage?kd_cb=${dataItem.kd_cb}&kd_cob=${dataItem.kd_cob}&kd_scob=${dataItem.kd_scob}&kd_thn=${dataItem.kd_thn}&no_aks=${dataItem.no_aks}&no_updt=${dataItem.no_updt}&no_rsk=${dataItem.no_rsk}&kd_endt=${dataItem.kd_endt}&kd_cvrg=${dataItem.kd_cvrg}`, 'Edit Coverage');
}
function btnDeleteAkseptasiCoverage_OnClick(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    showConfirmation('Confirmation', `Are you sure you want to delete Akseptasi Coverage?`,
        function () {
            showProgressOnGrid('#AkseptasiCoverageGrid');
            setTimeout(function () { deleteAkseptasiCoverage(dataItem); }, 500);
        }
    );
}
function searchFilterCoverage() {
    return {
        searchkeyword: $("#SearchKeywordCoverage").val(),
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

function deleteAkseptasiCoverage(dataItem) {
    ajaxGet(`/Akseptasi/DeleteCoverage?kd_cb=${dataItem.kd_cb}&kd_cob=${dataItem.kd_cob}&kd_scob=${dataItem.kd_scob}&kd_thn=${dataItem.kd_thn}&no_aks=${dataItem.no_aks}&no_updt=${dataItem.no_updt}&no_rsk=${dataItem.no_rsk}&kd_endt=${dataItem.kd_endt}&kd_cvrg=${dataItem.kd_cvrg}`, function (response) {
        if (response.Result) {
            showMessage('Success', 'Data has been deleted');
            refreshGrid("#AkseptasiCoverageGrid");
        }
        else if (!response.Result) {
            showMessage('Error', 'Delete data is failed, this data is already used');
            refreshGrid("#AkseptasiCoverageGrid");
        }
        else
            $("#AkseptasiCoverageWindow").html(response);

        closeProgressOnGrid('#AkseptasiCoverageGrid');
    }, AjaxContentType.URLENCODED);
}
