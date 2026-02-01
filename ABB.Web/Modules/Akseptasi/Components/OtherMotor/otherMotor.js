$(document).ready(async function () {
    btnNextResikoOtherMotor();
    btnSaveAkseptasiResikoOther_Click();
    await setOtherMotorEditedValue();
});

async function setOtherMotorEditedValue(){
    await restoreDropdownValue("#kd_merk_kend", "#temp_kd_merk_kend");
    var flag_hh = $("#tempFlag_hh").val();
    flag_hh == "Y" ? $("#flag_hh").prop("checked", true) : $("#flag_hh").prop("checked", false);
    var flag_aog = $("#tempFlag_aog").val();
    flag_aog == "Y" ? $("#flag_aog").prop("checked", true) : $("#flag_aog").prop("checked", false);
    var flag_banjir = $("#tempFlag_banjir").val();
    flag_banjir == "Y" ? $("#flag_banjir").prop("checked", true) : $("#flag_banjir").prop("checked", false);
    var flag_trs = $("#tempFlag_trs").val();
    flag_trs == "Y" ? $("#flag_trs").prop("checked", true) : $("#flag_trs").prop("checked", false);
}

function dataTipeKendaraanDropDown(){
    return {
        kd_grp_rsk: $("#temp_grp_merk_kend").val()
    }
}

function btnNextResikoOtherMotor(){
    $('#btn-next-akseptasiResikoOtherMotor').click(function () {
        $("#resikoOtherTab").getKendoTabStrip().select(1);
    });
}


function btnSaveAkseptasiResikoOther_Click() {
    $('#btn-save-akseptasiResikoOther').click(function () {
        showProgress('#AkseptasiWindow');
        setTimeout(function () {
            saveAkseptasiResikoOther('/Akseptasi/SaveAkseptasiOtherMotor')
        }, 500);
    });
}

function saveAkseptasiResikoOther(url) {
    var form = getFormData($('#OtherForm'));
    form.kd_cb = $("#kd_cb").val();
    form.kd_cob = $("#kd_cob").val();
    form.kd_scob = $("#kd_scob").val();
    form.kd_thn = $("#kd_thn").val();
    form.no_aks = $("#no_aks").val();
    form.no_updt = resiko.no_updt;
    form.no_rsk = resiko.no_rsk;
    form.kd_endt = resiko.kd_endt;
    
    form.tgl_mul_ptg = $("#resiko_other_tgl_mul_ptg").val();
    form.tgl_akh_ptg = $("#resiko_other_tgl_akh_ptg").val();
    form.pst_rate_prm = $("#resiko_other_pst_rate_prm").val();
    form.stn_rate_prm = $("#resiko_other_stn_rate_prm").val();
    form.nm_qq = $("#resiko_other_nm_qq").val();
    
    form.flag_hh = $("#flag_hh")[0].checked ? "Y" : "N";
    form.flag_banjir = $("#flag_banjir")[0].checked ? "Y" : "N";
    form.flag_aog = $("#flag_aog")[0].checked ? "Y" : "N";
    form.flag_trs = $("#flag_trs")[0].checked ? "Y" : "N";
    form.no_pol_ttg = $("#no_pol_ttg").val();

    var data = JSON.stringify(form);

    ajaxPost(url, data,
        function (response) {
            if (response.Result == "OK") {
                showMessage('Success', response.Message);
            }
            else if (response.Result == "ERROR")
                showMessage("Error", response.Message);
            else
                showMessage("Error", response);

            closeProgress('#AkseptasiWindow');
        }
    );
}

function OnMerkKendaraanChange(e){
    var value = e.sender._cascadedValue;
    var kd_merk_kend = $("#kd_merk_kend").data("kendoDropDownList");
    kd_merk_kend.dataSource.read({kd_grp_rsk : value});
}