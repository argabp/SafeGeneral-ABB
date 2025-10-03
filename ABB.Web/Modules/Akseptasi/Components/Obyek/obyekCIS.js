$(document).ready(function () {
    btnPreviousObyekCIS();
    btnNextObyekCIS();
    searchKeywordObyekCIS_OnKeyUp();
});


function btnPreviousObyekCIS(){
    $('#btn-previous-akseptasiResikoObyekCIS').click(function () {
        $("#resikoTab").getKendoTabStrip().select(1);
    });
}
function btnNextObyekCIS(){
    $('#btn-next-akseptasiResikoObyekCIS').click(function () {
        $("#resikoTab").getKendoTabStrip().select(3);
    });
}

function searchKeywordObyekCIS_OnKeyUp() {
    $('#SearchKeywordObyekCIS').keyup(function () {
        refreshGrid("#AkseptasiObyekGrid");
    });
}

function openAkseptasiObyekCISWindow(url, title) {
    openWindow('#AkseptasiObyekWindow', url, title);
}

function btnDeleteAkseptasiObyekCIS_OnClick(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    showConfirmation('Confirmation', `Are you sure you want to delete Akseptasi Obyek?`,
        function () {
            showProgressOnGrid('#AkseptasiObyekGrid');
            setTimeout(function () { deleteAkseptasiObyekCIS(dataItem); }, 500);
        }
    );
}
function searchFilterObyekCIS() {
    return {
        searchkeyword: $("#SearchKeywordObyekCIS").val(),
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

function deleteAkseptasiObyekCIS(dataItem) {
    ajaxGet(`/Akseptasi/DeleteObyekCIS?kd_cb=${dataItem.kd_cb}&kd_cob=${dataItem.kd_cob}&kd_scob=${dataItem.kd_scob}&kd_thn=${dataItem.kd_thn}&no_aks=${dataItem.no_aks}&no_updt=${dataItem.no_updt}&no_rsk=${dataItem.no_rsk}&kd_endt=${dataItem.kd_endt}&no_oby=${dataItem.no_oby}`, function (response) {
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

function saveAkseptasiObyekCIS(dataItem) {   

    var data = {
        kd_cb : $("#kd_cb").val(),
        kd_cob : $("#kd_cob").val(),
        kd_scob : $("#kd_scob").val(),
        kd_thn : $("#kd_thn").val(),
        no_aks : $("#no_aks").val(),
        no_updt : $("#resiko_obyek_no_updt").val(),
        no_rsk : resiko.no_rsk,
        kd_endt : resiko.kd_endt,
        no_pol_ttg : $("#no_pol_ttg").val(),
        tgl_oby: dataItem.model.tgl_oby.toDateString(),
        nilai_saldo: dataItem.model.nilai_saldo,
        no_oby: dataItem.model.no_oby,
    };

    ajaxPost("/Akseptasi/SaveAkseptasiObyekCIS", JSON.stringify(data),
        function (response) {
            refreshGrid("#AkseptasiObyekGrid");
            refreshGrid("#AkseptasiResikoGrid");
            if (response.Result == "OK") {
                showMessage('Success', response.Message);
                closeWindow('#AkseptasiObyekWindow');
            }
            else if (response.Result == "ERROR")
                showMessage("Error", response.Message);
            else
                showMessage("Error", response);

            closeProgress('#AkseptasiObyekWindow');
        }
    );
}