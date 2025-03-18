$(document).ready(function () {
    searchKeyword_OnKeyUp();
});

function searchKeyword_OnKeyUp() {
    $('#SearchKeyword').keyup(function () {
        refreshGrid("#ReopenPolisGrid");
    });
}

function searchFilter() {
    return {
        searchkeyword: $("#SearchKeyword").val()
    }
}

function btnReopenPolis_OnClick(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    showConfirmation('Confirmation', `Apakah anda akan melanjutkan Proses Reopen Polis?`,
        function () {
            showProgressOnGrid('#ReopenPolisGrid');
            setTimeout(function () { reopenPolis(dataItem); }, 500);
        }
    );
}

function reopenPolis(dataItem){

    var form = {};

    form.kd_cb = dataItem.kd_cb;
    form.kd_cob = dataItem.kd_cob;
    form.kd_scob = dataItem.kd_scob;
    form.kd_thn = dataItem.kd_thn;
    form.no_pol = dataItem.no_pol;
    form.no_updt = dataItem.no_updt;

    var data = JSON.stringify(form);
    ajaxPost(`/ReopenPolis/ReopenPolis`, data,  function (response) {
        if (response.Status === "OK") {
            showMessage('Success', 'Data behasil di reopen');
            refreshGrid("#ReopenPolisGrid");
        }
        else {
            showMessage('Error', 'Pembatalan gagal, ' + response.Message);
            refreshGrid("#ReopenPolisGrid");
        };

        closeProgressOnGrid('#ReopenPolisGrid');
    });
}