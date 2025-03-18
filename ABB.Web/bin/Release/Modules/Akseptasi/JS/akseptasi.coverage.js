$(document).ready(function () {
    btnSaveAkseptasiCoverage_Click();
});

function btnSaveAkseptasiCoverage_Click() {
    $('#btn-save-akseptasiCoverage').click(function () {
        showProgress('#AkseptasiCoverageWindow');
        setTimeout(function () {
            saveAkseptasiCoverage('/Akseptasi/SaveAkseptasiCoverage')
        }, 500);
    });
}

function saveAkseptasiCoverage(url) {
    var form = getFormData($('#CoverageForm'));
    form.pst_dis = $("#resiko_coverage_pst_dis").val();
    form.pst_rate_prm = $("#resiko_coverage_pst_rate_prm").val();
    form.stn_rate_prm = $("#resiko_coverage_stn_rate_prm").val();
    form.pst_kms = $("#resiko_coverage_pst_kms").val();
    form.kd_cb = $("#kd_cb").val();
    form.kd_cob = $("#kd_cob").val();
    form.kd_scob = $("#kd_scob").val();
    form.kd_thn = $("#kd_thn").val();
    form.no_aks = $("#no_aks").val();
    form.no_updt = $("#resiko_coverage_no_updt").val();
    form.no_rsk = resiko.no_rsk;
    form.kd_endt = resiko.kd_endt;
    
    var data = JSON.stringify(form);
    
    ajaxPost(url, data,
        function (response) {
            refreshGrid("#AkseptasiCoverageGrid");
            if (response.Result == "OK") {
                showMessage('Success', response.Message);
                closeWindow('#AkseptasiCoverageWindow');
            }
            else if (response.Result == "ERROR")
                showMessage("Error", response.Message);
            else
                showMessage("Error", response);

            closeProgress('#AkseptasiCoverageWindow');
        }
    );
}