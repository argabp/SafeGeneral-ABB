$(document).ready(function () {
    btnSaveAkseptasiPranotaKoas_Click();
    setTimeout(setPranotaKoasEditedValue, 2000);
});

function setPranotaKoasEditedValue(){
    $("#pranota_koas_kd_rk_pas").data("kendoDropDownList").value($("#temp_pranota_koas_kd_rk_pas").val().trim());
}

function btnSaveAkseptasiPranotaKoas_Click() {
    $('#btn-save-akseptasiPranotaKoas').click(function () {
        showProgress('#AkseptasiPranotaKoasWindow');
        setTimeout(function () {
            saveAkseptasiPranotaKoas('/Akseptasi/SaveAkseptasiPranotaKoas')
        }, 500);
    });
}

function saveAkseptasiPranotaKoas(url) {
    var form = getFormData($('#PranotaKoasForm'));
    form.kd_grp_pas = $("#pranota_koas_kd_grp_pas").val();
    form.kd_mtu = $("#pranota_koas_kd_mtu").val();
    form.kd_rk_pas = $("#pranota_koas_kd_rk_pas").val();
    form.nilai_dis = $("#pranota_koas_nilai_dis").val();
    form.nilai_hf = $("#pranota_koas_nilai_hf").val();
    form.nilai_kl = $("#pranota_koas_nilai_kl").val();
    form.nilai_prm = $("#pranota_koas_nilai_prm").val();
    form.pst_dis = $("#pranota_koas_pst_dis").val();
    form.pst_hf = $("#pranota_koas_pst_hf").val();
    form.pst_share = $("#pranota_koas_pst_share").val();
    form.kd_cb = $("#kd_cb").val();
    form.kd_cob = $("#kd_cob").val();
    form.kd_scob = $("#kd_scob").val();
    form.kd_thn = $("#kd_thn").val();
    form.no_aks = $("#no_aks").val();
    form.no_updt = $("#pranota_koas_no_updt").val();
    
    var data = JSON.stringify(form);
    
    ajaxPost(url, data,
        function (response) {
            refreshGrid("#AkseptasiPranotaKoasGrid");
            if (response.Result == "OK") {
                showMessage('Success', response.Message);
                closeWindow('#AkseptasiPranotaKoasWindow');
            }
            else if (response.Result == "ERROR")
                showMessage("Error", response.Message);
            else
                showMessage("Error", response);

            closeProgress('#AkseptasiPranotaKoasWindow');
        }
    );
}

function dataKodeRekananPranotaKoasDropDown(){
    return {
        kd_grp_rk: $("#temp_pranota_koas_kd_grp_pas").val().trim(),
        kd_cb: $("#temp_kd_cb").val().trim()
    }
}

function OnKodeRekananPranotaKoasChange(e){
    var pranota_koas_kd_rk_pas = $("#pranota_koas_kd_rk_pas").data("kendoDropDownList");
    pranota_koas_kd_rk_pas.dataSource.read({kd_grp_rk : e.sender._cascadedValue, kd_cb: $("#kd_cb").val()});
}