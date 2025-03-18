$(document).ready(function () {
    searchKeyword_OnKeyUp();
});

function searchKeyword_OnKeyUp() {
    $('#SearchKeyword').keyup(function () {
        refreshGrid("#RenewalPolisGrid");
    });
}

function searchFilter() {
    return {
        searchkeyword: $("#SearchKeyword").val()
    }
}

function openRenewalPolisWindow(url, title) {
    openWindow('#RenewalPolisWindow', url, title);
}

function dataSCOB(){
    return {
        kd_cob: $("#kd_cob").val()
    }
}

function btnRenewalPolis_OnClick(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    showConfirmation('Confirmation', `Apakah anda akan melanjutkan Proses Renewal Polis?`,
        function () {
            showProgressOnGrid('#RenewalPolisGrid');
            setTimeout(function () { renewalPolis(dataItem); }, 500);
        }
    );
}

function renewalPolis(dataItem){
    var form = {};

    form.kd_cb = dataItem.kd_cb;
    form.kd_cob = dataItem.kd_cob;
    form.kd_scob = dataItem.kd_scob;
    form.kd_thn = dataItem.kd_thn;
    form.no_pol = dataItem.no_pol;
    form.no_updt = dataItem.no_updt;
    form.kd_scob_new = dataItem.kd_scob;
    
    var data = JSON.stringify(form);
    
    ajaxPost(`/RenewalPolis/RenewalPolis`, data,  function (response) {
        if (response.Status === "OK") {
            showMessage('Success', 'Data behasil di renwal');
        }
        else {
            showMessage('Error', 'Pembatalan gagal, ' + response.Message);
        }

        refreshGrid("#RenewalPolisGrid");
        closeProgressOnGrid('#RenewalPolisGrid');
    });
}