$(document).ready(function () {
    searchKeyword_OnKeyUp();
});

function searchKeyword_OnKeyUp() {
    $('#SearchKeyword').keyup(function () {
        refreshGrid("#PembatalanAkseptasiGrid");
    });
}

function searchFilter() {
    return {
        searchkeyword: $("#SearchKeyword").val()
    }
}

function btnBatalAkseptasi_OnClick(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    showConfirmation('Confirmation', `Apakah anda akan melanjutkan Proses Pembatalan Akseptasi?`,
        function () {
            showProgressOnGrid('#PembatalanAkseptasiGrid');
            setTimeout(function () { batalAkseptasi(dataItem); }, 500);
        }
    );
}

function batalAkseptasi(dataItem){

    var form = {};

    form.kd_cb = dataItem.kd_cb;
    form.kd_cob = dataItem.kd_cob;
    form.kd_scob = dataItem.kd_scob;
    form.kd_thn = dataItem.kd_thn;
    form.no_aks = dataItem.no_aks;
    form.no_updt = dataItem.no_updt;

    var data = JSON.stringify(form);
    ajaxPost(`/PembatalanAkseptasi/BatalAkseptasi`, data,  function (response) {
        if (response.Status === "OK") {
            showMessage('Success', 'Data behasil dibatalkan');
            refreshGrid("#PembatalanAkseptasiGrid");
        }
        else {
            showMessage('Error', 'Pembatalan gagal, ' + response.Message);
            refreshGrid("#PembatalanAkseptasiGrid");
        };

        closeProgressOnGrid('#PembatalanAkseptasiGrid');
    });
}