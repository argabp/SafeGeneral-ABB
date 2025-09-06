$(document).ready(function () {
    searchKeyword_OnKeyUp();
});

function searchKeyword_OnKeyUp() {
    $('#SearchKeyword').keyup(function () {
        refreshGrid("#EndorsementPolisGrid");
    });
}

function searchFilter() {
    return {
        searchkeyword: $("#SearchKeyword").val()
    }
}

function btnEndorsementPolisNormal_OnClick(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    showConfirmation('Confirmation', `Apakah anda akan melanjutkan Proses Endorsement Normal?`,
        function () {
            showProgressOnGrid('#EndorsementPolisGrid');
            setTimeout(function () { endorsementPolis(dataItem, "/EndorsementPolis/EndorsementPolisNormal"); }, 500);
        }
    );
}

function btnEndorsementPolisCancel_OnClick(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    showConfirmation('Confirmation', `Apakah anda akan melanjutkan Proses Endorsement Cancel?`,
        function () {
            showProgressOnGrid('#EndorsementPolisGrid');
            setTimeout(function () { endorsementPolis(dataItem, "/EndorsementPolis/EndorsementPolisCancel"); }, 500);
        }
    );
}

function btnEndorsementPolisNoPremium_OnClick(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    showConfirmation('Confirmation', `Apakah anda akan melanjutkan Proses Endorsement No Premium?`,
        function () {
            showProgressOnGrid('#EndorsementPolisGrid');
            setTimeout(function () { endorsementPolis(dataItem, "/EndorsementPolis/EndorsementPolisNoPremium"); }, 500);
        }
    );
}

function endorsementPolis(dataItem, url){

    var form = {};

    form.kd_cb = dataItem.kd_cb;
    form.kd_cob = dataItem.kd_cob;
    form.kd_scob = dataItem.kd_scob;
    form.kd_thn = dataItem.kd_thn;
    form.no_pol = dataItem.no_pol;
    form.no_updt = dataItem.no_updt;

    var data = JSON.stringify(form);
    ajaxPost(url, data,  function (response) {
        if (response.Status === "OK") {
            showMessage('Success', 'Proses Endorsment Normal Berhasil, Silahkan dicek pada menu Akseptasi Polis');
            refreshGrid("#EndorsementPolisGrid");
        }
        else {
            showMessage('Error', 'Pembatalan gagal, ' + response.Message);
            refreshGrid("#EndorsementPolisGrid");
        };

        closeProgressOnGrid('#EndorsementPolisGrid');
    });
}