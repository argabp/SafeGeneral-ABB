$(document).ready(function () {
    btnPreviousPranotaKoas();
    searchKeywordPranotaKoas_OnKeyUp();
    btnAddAkseptasiPranotaKoas_Click();
});


function btnPreviousPranotaKoas(){
    $('#btn-previous-akseptasiResikoPranotaKoas').click(function () {
        $("#pranotaTab").getKendoTabStrip().select(0);
    });
}

function searchKeywordPranotaKoas_OnKeyUp() {
    $('#SearchKeywordPranotaKoas').keyup(function () {
        refreshGrid("#AkseptasiPranotaKoasGrid");
    });
}

function openAkseptasiPranotaKoasWindow(url, title) {
    openWindow('#AkseptasiPranotaKoasWindow', url, title);
}


function btnAddAkseptasiPranotaKoas_Click() {
    $('#btnAddNewAkseptasiPranotaKoas').click(function () {
        openAkseptasiPranotaKoasWindow(`/Akseptasi/AddPranotaKoas?kd_cb=${$("#kd_cb").val()}&kd_cob=${$("#kd_cob").val()}&kd_scob=${$("#kd_scob").val()}&kd_thn=${$("#kd_thn").val()}&no_aks=${$("#no_aks").val()}&no_updt=${$("#no_updt").val()}&kd_mtu=${$("#kd_mtu").val()}`, 'Add New Koasuransi Member');
    });
}

function btnEditAkseptasiPranotaKoas_OnClick(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    console.log('dataItem', dataItem);
    openAkseptasiPranotaKoasWindow(`/Akseptasi/EditPranotaKoas?kd_cb=${dataItem.kd_cb}&kd_cob=${dataItem.kd_cob}&kd_scob=${dataItem.kd_scob}&kd_thn=${dataItem.kd_thn}&no_aks=${dataItem.no_aks}&no_updt=${dataItem.no_updt}&kd_mtu=${dataItem.kd_mtu}&kd_grp_pas=${dataItem.kd_grp_pas}&kd_rk_pas=${dataItem.kd_rk_pas}`, 'Edit Koasuransi Member');
}
function btnDeleteAkseptasiPranotaKoas_OnClick(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    showConfirmation('Confirmation', `Are you sure you want to delete Akseptasi Pranota Koas?`,
        function () {
            showProgressOnGrid('#AkseptasiPranotaKoasGrid');
            setTimeout(function () { deleteAkseptasiPranotaKoas(dataItem); }, 500);
        }
    );
}
function searchFilterPranotaKoas() {
    return {
        searchkeyword: $("#SearchKeywordPranotaKoas").val(),
        kd_cb: pranota?.kd_cb,
        kd_cob: pranota?.kd_cob,
        kd_scob: pranota?.kd_scob,
        kd_thn: pranota?.kd_thn,
        no_aks: pranota?.no_aks,
        no_updt: pranota?.no_updt,
        kd_mtu: pranota?.kd_mtu,
    }
}

//here
function deleteAkseptasiPranotaKoas(dataItem) {
    ajaxGet(`/Akseptasi/DeletePranotaKoas?kd_cb=${dataItem.kd_cb}&kd_cob=${dataItem.kd_cob}&kd_scob=${dataItem.kd_scob}&kd_thn=${dataItem.kd_thn}&no_aks=${dataItem.no_aks}&no_updt=${dataItem.no_updt}&kd_mtu=${dataItem.kd_mtu}&kd_grp_pas=${dataItem.kd_grp_pas}&kd_rk_pas=${dataItem.kd_rk_pas}`, function (response) {
        if (response.Result) {
            showMessage('Success', 'Data has been deleted');
            refreshGrid("#AkseptasiPranotaKoasGrid");
        }
        else if (!response.Result) {
            showMessage('Error', 'Delete data is failed, this data is already used');
            refreshGrid("#AkseptasiPranotaKoasGrid");
        }
        else
            $("#AkseptasiPranotaKoasWindow").html(response);

        closeProgressOnGrid('#AkseptasiPranotaKoasGrid');
    }, AjaxContentType.URLENCODED);
}

function onPranotaKoasBound(e){
    var totalPercentage = 0;
    var totalPremi = 0;
    e.sender.dataSource.data().forEach((value, key) => {
        totalPercentage += value.pst_share;
        totalPremi += value.nilai_prm + value.nilai_kl;
    });
    
    $("#totalPercentagePranotaKoas").text(totalPercentage);
    $("#totalPremiPranotaKoas").text(totalPremi);
}
