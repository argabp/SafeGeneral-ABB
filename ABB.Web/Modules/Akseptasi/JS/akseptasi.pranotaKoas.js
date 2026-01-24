$(document).ready(async function () {
    btnSaveAkseptasiPranotaKoas_Click();
    await setPranotaKoasEditedValue();
    
    $("#premi_100").getKendoNumericTextBox().value(Number(pranota.nilai_prm));
});

async function setPranotaKoasEditedValue(){
    await restoreDropdownValue("#pranota_koas_kd_rk_pas", "#temp_pranota_koas_kd_rk_pas");
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
    form.nilai_kms = $("#pranota_koas_nilai_kms").val();
    form.nilai_prm = $("#pranota_koas_nilai_prm").val();
    form.pst_dis = $("#pranota_koas_pst_dis").val();
    form.pst_hf = $("#pranota_koas_pst_hf").val();
    form.pst_kms = $("#pranota_koas_pst_kms").val();
    form.pst_share = $("#pranota_koas_pst_share").val();
    form.kd_cb = $("#kd_cb").val();
    form.kd_cob = $("#kd_cob").val();
    form.kd_scob = $("#kd_scob").val();
    form.kd_thn = $("#kd_thn").val();
    form.no_aks = $("#no_aks").val();
    form.no_updt = pranota.no_updt;
    
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
    $("#temp_pranota_koas_kd_grp_pas").val(e.sender._cascadedValue);
    pranota_koas_kd_rk_pas.dataSource.read({kd_grp_rk : e.sender._cascadedValue, kd_cb: $("#kd_cb").val()});
}

function OnPranotaKoasPstShareChange(e){
    ajaxGet(`/Akseptasi/GenerateNilaiPrmKoas?pst_share=${e.sender.value()}&nilai_prm_leader=${$("#premi_100").val()}&pst_hf=${$("#pranota_koas_pst_hf").val()}`, (returnValue) => {
        $("#pranota_koas_nilai_prm").getKendoNumericTextBox().value(returnValue.split(",")[1]);
    });
}

function OnPranotaKoasPstDisChange(e){
    ajaxGet(`/Akseptasi/GenerateNilaiDisKoas?pst_dis=${e.sender.value()}&nilai_prm=${$("#pranota_koas_nilai_prm").val()}`, (returnValue) => {
        $("#pranota_koas_nilai_dis").getKendoNumericTextBox().value(returnValue.split(",")[1]);
    });
}

function OnPranotaKoasPstHfChange(e){
    ajaxGet(`/Akseptasi/GenerateNilaiPrmKoas?pst_share=${$("#pranota_koas_pst_share").val()}&nilai_prm_leader=${$("#pranota_koas_nilai_prm").val()}&pst_hf=${e.sender.value()}`, (returnValue) => {
        $("#pranota_koas_nilai_hf").getKendoNumericTextBox().value(returnValue.split(",")[1]);
    });
}

function OnPranotaKoasPstKmsChange(e){
    ajaxGet(`/Akseptasi/GenerateNilaiKmsKoas?pst_kms=${e.sender.value()}&nilai_prm=${$("#pranota_koas_nilai_prm").val()}&nilai_dis=${$("#pranota_koas_nilai_dis").val()}`, (returnValue) => {
        $("#pranota_koas_nilai_kms").getKendoNumericTextBox().value(returnValue.split(",")[1]);
    });
}