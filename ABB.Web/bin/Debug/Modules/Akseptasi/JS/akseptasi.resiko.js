$(document).ready(function () {
    btnSaveAkseptasiResiko_Click();
    
    if(resiko.kd_endt === "D"){
        $("#btn-save-akseptasiResiko").hide();
    }
});

function btnSaveAkseptasiResiko_Click() {
    $('#btn-save-akseptasiResiko').click(function () {
        showProgress('#AkseptasiResikoWindow');
        setTimeout(function () {
            saveAkseptasiResiko('/Akseptasi/SaveAkseptasiResiko')
        }, 500);
    });
}


function saveAkseptasiResiko(url) {
    var form = getFormData($('#ResikoForm'));
    form.jk_wkt_ptg = $("#resiko_jk_wkt_ptg").val();
    form.pst_share_bgu = $("#resiko_pst_share_bgu").val();
    form.faktor_prd = $("#resiko_faktor_prd").val();
    form.kd_cb = $("#kd_cb").val();
    form.kd_cob = $("#kd_cob").val();
    form.kd_scob = $("#kd_scob").val();
    form.kd_thn = $("#kd_thn").val();
    form.no_aks = $("#no_aks").val();
    form.no_updt = $("#resiko_no_updt").val();
    form.tgl_mul_ptg = $("#resiko_tgl_mul_ptg").val();
    form.tgl_akh_ptg = $("#resiko_tgl_akh_ptg").val();
    form.nilai_prm = $("#resiko_nilai_prm").val();
    form.nilai_ttl_ptg = $("#resiko_nilai_ttl_ptg").val();
    form.pst_dis = $("#resiko_pst_dis").val();
    form.nilai_dis = $("#resiko_nilai_dis").val();
    form.pst_kms = $("#resiko_pst_kms").val();
    form.nilai_kms = $("#resiko_nilai_kms").val();
    
    var data = JSON.stringify(form);
    
    ajaxPost(url, data,
        function (response) {
            refreshGrid("#AkseptasiResikoGrid");
            if (response.Result == "OK") {
                showMessage('Success', response.Message);
                closeWindow('#AkseptasiResikoWindow');
            }
            else if (response.Result == "ERROR")
                showMessage("Error", response.Message);
            else
                showMessage("Error", response);

            closeProgress('#AkseptasiResikoWindow');
        }
    );
}