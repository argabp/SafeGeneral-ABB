$(document).ready(function () {
    btnSaveAkseptasiOtherCargoDetail_Click();
});

function btnSaveAkseptasiOtherCargoDetail_Click() {
    $('#btn-save-akseptasiOtherCargoDetail').click(function () {
        showProgress('#AkseptasiOtherCargoDetailWindow');
        setTimeout(function () {
            saveAkseptasiOtherCargoDetail('/Akseptasi/SaveAkseptasiOtherCargoDetail')
        }, 500);
    });
}

function saveAkseptasiOtherCargoDetail(url) {
    var form = getFormData($('#OtherCargoDetailForm'));
    form.kd_cb = $("#kd_cb").val();
    form.kd_cob = $("#kd_cob").val();
    form.kd_scob = $("#kd_scob").val();
    form.kd_thn = $("#kd_thn").val();
    form.no_aks = $("#no_aks").val();
    form.no_updt = $("#other_motor_detail_no_updt").val();
    form.no_rsk = resiko.no_rsk;
    form.kd_endt = $("#resiko_other_cargo_detail_kd_endt").val();
    form.no_urut = $("#resiko_other_cargo_no_urut").val();
    form.no_bl = $("#resiko_other_cargo_detail_no_bl").val();
    form.no_inv = $("#resiko_other_cargo_detail_no_inv").val();
    form.no_po = $("#resiko_other_cargo_detail_no_po").val();
    form.no_pol_ttg = $("#no_pol_ttg").val();
    
    var data = JSON.stringify(form);
    
    ajaxPost(url, data,
        function (response) {
            refreshGrid("#AkseptasiOtherCargoDetailGrid");
            if (response.Result == "OK") {
                showMessage('Success', response.Message);
                closeWindow('#AkseptasiOtherCargoDetailWindow');
            }
            else if (response.Result == "ERROR")
                showMessage("Error", response.Message);
            else
                showMessage("Error", response);

            closeProgress('#AkseptasiOtherCargoDetailWindow');
        }
    );
}