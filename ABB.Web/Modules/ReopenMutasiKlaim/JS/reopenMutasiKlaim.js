$(document).ready(function () {
    searchKeyword_OnKeyUp();
});

function searchKeyword_OnKeyUp() {
    $('#SearchKeyword').keyup(function () {
        refreshGrid("#ReopenMutasiKlaimGrid");
    });
}

function searchFilter() {
    return {
        searchkeyword: $("#SearchKeyword").val()
    }
}

function btnReopenMutasiKlaim_OnClick(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    showConfirmation('Confirmation', `Apakah anda akan melanjutkan Proses Reopen Mutasi Klaim?`,
        function () {
            showProgressOnGrid('#ReopenMutasiKlaimGrid');
            setTimeout(function () { reopenMutasiKlaim(dataItem); }, 500);
        }
    );
}

function reopenMutasiKlaim(dataItem){

    var form = {};

    form.kd_cb = dataItem.kd_cb;
    form.kd_cob = dataItem.kd_cob;
    form.kd_scob = dataItem.kd_scob;
    form.kd_thn = dataItem.kd_thn;
    form.no_kl = dataItem.no_kl;
    form.no_mts = dataItem.no_mts;

    var data = JSON.stringify(form);
    ajaxPost(`/ReopenMutasiKlaim/ReopenMutasiKlaim`, data,  function (response) {
        if (response.Status === "OK") {
            showMessage('Success', 'Data behasil di reopen');
            refreshGrid("#ReopenMutasiKlaimGrid");
        }
        else {
            showMessage('Error', 'Reopen gagal, ' + response.Message);
            refreshGrid("#ReopenMutasiKlaimGrid");
        };

        closeProgressOnGrid('#ReopenMutasiKlaimGrid');
    });
}