$(document).ready(function () {
    btnSaveAkseptasiOtherMotorDetail_Click();
});

function btnSaveAkseptasiOtherMotorDetail_Click() {
    $('#btn-save-akseptasiOtherMotorDetail').click(function () {
        showProgress('#AkseptasiOtherMotorDetailWindow');
        setTimeout(function () {
            saveAkseptasiOtherMotorDetail('/Akseptasi/SaveAkseptasiOtherMotorDetail')
        }, 500);
    });
}

function saveAkseptasiOtherMotorDetail(url) {
    var form = getFormData($('#OtherMotorDetailForm'));
    form.kd_jns_ptg_thn = $("#akseptasi_other_motor_detail_kd_jns_ptg_thn").val();
    form.nilai_casco = $("#akseptasi_other_motor_detail_nilai_casco").val();
    form.nilai_prm_casco = $("#akseptasi_other_motor_detail_nilai_prm_casco").val();
    form.nilai_rsk_sendiri = $("#akseptasi_other_motor_detail_nilai_rsk_sendiri").val();
    form.nilai_prm_aog = $("#akseptasi_other_motor_detail_nilai_prm_aog").val();
    form.nilai_prm_hh = $("#akseptasi_other_motor_detail_nilai_prm_hh").val();
    form.nilai_tjh = $("#akseptasi_other_motor_detail_nilai_tjh").val();
    form.nilai_prm_tjh = $("#akseptasi_other_motor_detail_nilai_prm_tjh").val();
    form.nilai_tjp = $("#akseptasi_other_motor_detail_nilai_tjp").val();
    form.nilai_prm_tjp = $("#akseptasi_other_motor_detail_nilai_prm_tjp").val();
    form.nilai_pap = $("#akseptasi_other_motor_detail_nilai_pap").val();
    form.nilai_prm_pap = $("#akseptasi_other_motor_detail_nilai_prm_pap").val();
    form.nilai_pad = $("#akseptasi_other_motor_detail_nilai_pad").val();
    form.nilai_prm_pad = $("#akseptasi_other_motor_detail_nilai_prm_pad").val();
    form.nilai_pap_med = $("#akseptasi_other_motor_detail_nilai_pap_med").val();
    form.nilai_prm_pap_med = $("#akseptasi_other_motor_detail_nilai_prm_pap_med").val();
    form.nilai_pad_med = $("#akseptasi_other_motor_detail_nilai_pad_med").val();
    form.nilai_prm_pad_med = $("#akseptasi_other_motor_detail_nilai_prm_pad_med").val();
    form.nilai_prm_trs = $("#akseptasi_other_motor_detail_nilai_prm_trs").val();
    form.nilai_prm_banjir = $("#akseptasi_other_motor_detail_nilai_prm_banjir").val();
    form.kd_cb = $("#kd_cb").val();
    form.kd_cob = $("#kd_cob").val();
    form.kd_scob = $("#kd_scob").val();
    form.kd_thn = $("#kd_thn").val();
    form.no_aks = $("#no_aks").val();
    form.no_updt = $("#other_motor_detail_no_updt").val();
    form.no_rsk = resiko.no_rsk;
    form.kd_endt = $("#resiko_other_motor_detail_kd_endt").val();
    
    var data = JSON.stringify(form);
    
    ajaxPost(url, data,
        function (response) {
            refreshGrid("#AkseptasiOtherMotorDetailGrid");
            if (response.Result == "OK") {
                showMessage('Success', response.Message);
                closeWindow('#AkseptasiOtherMotorDetailWindow');
            }
            else if (response.Result == "ERROR")
                showMessage("Error", response.Message);
            else
                showMessage("Error", response);

            closeProgress('#AkseptasiOtherMotorDetailWindow');
        }
    );
}