
function searchFilterCopyEndors() {
    return {
        searchkeyword: $("#SearchKeywordCopyEndors").val(),
        kd_cb: $("#kd_cb").val(),
        kd_cob: $("#kd_cob").val(),
        kd_scob: $("#kd_scob").val(),
        kd_thn: $("#kd_thn").val(),
        no_pol: $("#no_aks").val(),
        no_updt: $("#no_updt").val(),
    }
}

function btnDeleteCopyEndors_OnClick(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    showConfirmation('Confirmation', `Apakah akan melanjutkan proses Copy Endorsment Delete?`,
        function () {        
            showProgressOnGrid('#CopyEndorsGrid');
            setTimeout(function () { copyEndorsDelete(dataItem); }, 500);
        }
    );
}

function copyEndorsDelete(dataItem){
    var form = {};

    form.kd_cb = dataItem.kd_cb;
    form.kd_cob = dataItem.kd_cob;
    form.kd_scob = dataItem.kd_scob;
    form.kd_thn = dataItem.kd_thn;
    form.no_aks = $("#no_aks").val();
    form.no_updt = dataItem.no_updt;
    form.no_rsk = dataItem.no_rsk;
    form.kd_endt = dataItem.kd_endt;
    form.flag_endt = "D";
    
    var data = JSON.stringify(form);
    ajaxPost(`/Akseptasi/CopyEndorsDelete`, data,  function (response) {
        if (response.Result === "OK") {
            refreshGrid("#AkseptasiResikoGrid");
            showMessage('Success', response.Message);
            closeWindow($("#CopyEndorsWindow"))
        }
        else {
            showMessage('Error', response.Message);
        }
    });
}

function btnUpdateCopyEndors_OnClick(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    showConfirmation('Confirmation', `Apakah akan melanjutkan proses Copy Endorsment Update?`,
        function () {
            showProgressOnGrid('#CopyEndorsGrid');
            setTimeout(function () { copyEndorsUpdate(dataItem); }, 500);
        }
    );
}

function copyEndorsUpdate(dataItem){
    var form = {};

    form.kd_cb = dataItem.kd_cb;
    form.kd_cob = dataItem.kd_cob;
    form.kd_scob = dataItem.kd_scob;
    form.kd_thn = dataItem.kd_thn;
    form.no_aks = $("#no_aks").val();
    form.no_updt = dataItem.no_updt;
    form.no_rsk = dataItem.no_rsk;
    form.kd_endt = dataItem.kd_endt;
    form.flag_endt = "U";

    var data = JSON.stringify(form);
    ajaxPost(`/Akseptasi/CopyEndorsUpdate`, data,  function (response) {
        if (response.Result === "OK") {
            refreshGrid("#AkseptasiResikoGrid");
            showMessage('Success', response.Message);
            closeWindow($("#CopyEndorsWindow"))
        }
        else {
            showMessage('Error', response.Message);
        }
    });
}

function btnInsertCopyEndors_OnClick(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    showConfirmation('Confirmation', `Apakah akan melanjutkan proses Copy Endorsment Insert?`,
        function () {
            showProgressOnGrid('#CopyEndorsGrid');
            setTimeout(function () { copyEndorsInsert(dataItem); }, 500);
        }
    );
}

function copyEndorsInsert(dataItem){
    var form = {};

    form.kd_cb = dataItem.kd_cb;
    form.kd_cob = dataItem.kd_cob;
    form.kd_scob = dataItem.kd_scob;
    form.kd_thn = dataItem.kd_thn;
    form.no_aks = $("#no_aks").val();
    form.no_updt = dataItem.no_updt;
    form.no_rsk = dataItem.no_rsk;
    form.kd_endt = dataItem.kd_endt;
    form.flag_endt = "U";

    var data = JSON.stringify(form);
    ajaxPost(`/Akseptasi/CopyEndorsInsert`, data,  function (response) {
        if (response.Result === "OK") {
            refreshGrid("#AkseptasiResikoGrid");
            showMessage('Success', response.Message);
            closeWindow($("#CopyEndorsWindow"))
        }
        else {
            showMessage('Error', response.Message);
        }
    });
}