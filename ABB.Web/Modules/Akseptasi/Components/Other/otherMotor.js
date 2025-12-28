$(document).ready(function () {
    btnNextResikoOtherMotor();
    btnSaveAkseptasiResikoOther_Click();
    setTimeout(setOtherMotorEditedValue, 2000);
    btnDeleteAkseptasiResikoOtherMotor_Click();

    if($("#IsNewOther").val() === "True"){
        $("#btn-delete-akseptasiResikoOtherMotor").hide();
    }
});

function btnDeleteAkseptasiResikoOtherMotor_Click(){
    $('#btn-delete-akseptasiResikoOtherMotor').click(function () {
        showConfirmation('Confirmation', `Are you sure you want to delete?`,
            function () {
                showProgress('#AkseptasiWindow');
                setTimeout(function () { deleteAkseptasiResikoOtherMotor(); }, 500);
            }
        );
    });
}

function deleteAkseptasiResikoOtherMotor() {
    var data = {
        kd_cb: $("#kd_cb").val(),
        kd_cob: $("#kd_cob").val(),
        kd_scob: $("#kd_scob").val(),
        kd_thn: $("#kd_thn").val(),
        no_aks: $("#no_aks").val(),
        no_updt: $("#resiko_other_no_updt").val(),
        no_rsk: resiko.no_rsk,
        kd_endt: $("#resiko_other_kd_endt").val()
    }

    ajaxPost(`/Akseptasi/DeleteOtherMotor`, JSON.stringify(data), function (response) {
        if (response.Result) {
            showMessage('Success', 'Data has been deleted');
            $("#btn-delete-akseptasiResikoOtherMotor").hide();
        }
        else {
            showMessage('Error', 'Delete data is failed, this data is already used');
        }

        closeProgress('#AkseptasiWindow');
    });
}

function setOtherMotorEditedValue(){
    $("#kd_merk_kend").data("kendoDropDownList").value($("#temp_kd_merk_kend").val().trim());
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
    form.no_updt = $("#resiko_other_no_updt").val();
    form.no_rsk = resiko.no_rsk;
    form.kd_endt = $("#resiko_other_kd_endt").val();

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
            refreshGrid("#AkseptasiResikoGrid");
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

function OnJumlahTempatDudukChange(e){
    var value = e.sender.value();
    ajaxGet(`/Akseptasi/GetJumlahPAPenumpang?jml_tmpt_ddk=${value}`, (returnValue) => {
        $("#jml_pap").getKendoNumericTextBox().value(returnValue.split(",")[1]);
    });
}

function OnNilaiCascoChange(e){
    var value = e.sender.value();
    ajaxGet(`/Akseptasi/GeneratePstRatePrm?kd_jns_kend=${$("#kd_jns_kend").val()}&kd_wilayah=${$("#kd_wilayah").val()}&kd_jns_ptg=${$("#kd_jns_ptg").val()}&nilai_casco=${value}`, (returnValue) => {
        $("#resiko_other_pst_rate_prm").getKendoNumericTextBox().value(returnValue.split(",")[1]);
        $("#resiko_other_pst_rate_prm").getKendoNumericTextBox().trigger("change");
    });
}

function OnPstRatePrmChange(e){
    var value = e.sender.value();
    ajaxGet(`/Akseptasi/GenerateNilaiPrmCasco?kd_guna=${$("#kd_guna").val()}&nilai_casco=${$("#nilai_casco").val()}&pst_rate_prm=${value}&stn_rate_prm=${$("#stn_rate_prm").val()}&nilai_tjh=${$("#nilai_tjh").val()}`, (returnValue) => {
        $("#nilai_prm_casco").getKendoNumericTextBox().value(returnValue.split(",")[1]);
    });
}

function OnHuruHaraChange(e){
    ajaxGet(`/Akseptasi/GeneratePstRateHH?kd_jns_kend=${$("#kd_jns_kend").val()}&kd_wilayah=${$("#kd_wilayah").val()}&kd_jns_ptg=${$("#kd_jns_ptg").val()}&nilai_casco=${$("#nilai_casco").val()}&flag_hh=${!e.checked}`, (returnValue) => {
        $("#pst_rate_hh").getKendoNumericTextBox().value(returnValue.split(",")[1]);
        $("#pst_rate_hh").getKendoNumericTextBox().trigger("change");
    });
}

function OnPstRateHHChange(e){
    var value = e.sender.value();
    ajaxGet(`/Akseptasi/GenerateNilaiPrmHH?nilai_casco=${$("#nilai_casco").val()}&pst_rate_hh=${value}&stn_rate_hh=${$("#stn_rate_hh").val()}`, (returnValue) => {
        $("#nilai_prm_hh").getKendoNumericTextBox().value(returnValue.split(",")[1]);
    });
}

function OnAOGChange(e){
    ajaxGet(`/Akseptasi/GeneratePstRateAOG?kd_jns_kend=${$("#kd_jns_kend").val()}&kd_wilayah=${$("#kd_wilayah").val()}&kd_jns_ptg=${$("#kd_jns_ptg").val()}&nilai_casco=${$("#nilai_casco").val()}&flag_aog=${!e.checked}`, (returnValue) => {
        $("#pst_rate_aog").getKendoNumericTextBox().value(returnValue.split(",")[1]);
        $("#pst_rate_aog").getKendoNumericTextBox().trigger("change");
    });
}

function OnPstRateAOGChange(e){
    var value = e.sender.value();
    ajaxGet(`/Akseptasi/GenerateNilaiPrmAOG?nilai_casco=${$("#nilai_casco").val()}&pst_rate_aog=${value}&stn_rate_aog=${$("#stn_rate_aog").val()}`, (returnValue) => {
        $("#nilai_prm_aog").getKendoNumericTextBox().value(returnValue.split(",")[1]);
    });
}

function OnBanjirChange(e){
    ajaxGet(`/Akseptasi/GeneratePstRateBanjir?kd_jns_kend=${$("#kd_jns_kend").val()}&kd_wilayah=${$("#kd_wilayah").val()}&kd_jns_ptg=${$("#kd_jns_ptg").val()}&nilai_casco=${$("#nilai_casco").val()}&flag_banjir=${!e.checked}`, (returnValue) => {
        $("#pst_rate_banjir").getKendoNumericTextBox().value(returnValue.split(",")[1]);
        $("#pst_rate_banjir").getKendoNumericTextBox().trigger("change");
    });
}

function OnPstRateBanjirChange(e){
    var value = e.sender.value();
    ajaxGet(`/Akseptasi/GenerateNilaiPrmBanjir?nilai_casco=${$("#nilai_casco").val()}&pst_rate_banjir=${value}&stn_rate_banjir=${$("#stn_rate_banjir").val()}`, (returnValue) => {
        $("#nilai_prm_banjir").getKendoNumericTextBox().value(returnValue.split(",")[1]);
    });
}

function OnTRSChange(e){
    ajaxGet(`/Akseptasi/GeneratePstRateTRS?kd_jns_kend=${$("#kd_jns_kend").val()}&kd_wilayah=${$("#kd_wilayah").val()}&kd_jns_ptg=${$("#kd_jns_ptg").val()}&nilai_casco=${$("#nilai_casco").val()}&flag_trs=${!e.checked}`, (returnValue) => {
        $("#pst_rate_trs").getKendoNumericTextBox().value(returnValue.split(",")[1]);
        $("#pst_rate_trs").getKendoNumericTextBox().trigger("change");
    });
}

function OnPstRateTRSChange(e){
    var value = e.sender.value();
    ajaxGet(`/Akseptasi/GenerateNilaiPrmTRS?nilai_casco=${$("#nilai_casco").val()}&pst_rate_trs=${value}&stn_rate_trs=${$("#stn_rate_trs").val()}`, (returnValue) => {
        $("#nilai_prm_trs").getKendoNumericTextBox().value(returnValue.split(",")[1]);
    });
}

function OnNilaiTJHChange(e){
    ajaxGet(`/Akseptasi/GenerateNilaiPrmAndPstRateTJHKend?kd_jns_kend=${$("#kd_jns_kend").val()}&kd_wilayah=${$("#kd_wilayah").val()}&kd_jns_ptg=${$("#kd_jns_ptg").val()}&nilai_tjh=${e.sender.value()}`, (returnValue) => {
        $("#pst_rate_tjh").getKendoNumericTextBox().value(returnValue[1].split(",")[1]);
        $("#nilai_prm_tjh").getKendoNumericTextBox().value(returnValue[0].split(",")[1]);
    });
}

function OnNilaiTJPChange(e){
    ajaxGet(`/Akseptasi/GenerateNilaiPrmAndPstRateTJP?kd_jns_kend=${$("#kd_jns_kend").val()}&kd_wilayah=${$("#kd_wilayah").val()}&kd_jns_ptg=${$("#kd_jns_ptg").val()}&nilai_tjp=${e.sender.value()}`, (returnValue) => {
        $("#pst_rate_tjp").getKendoNumericTextBox().value(returnValue[1].split(",")[1]);
        $("#nilai_prm_tjp").getKendoNumericTextBox().value(returnValue[0].split(",")[1]);
    });
}

function OnPAPChange(e){
    ajaxGet(`/Akseptasi/GeneratePstRatePAP?kd_jns_kend=${$("#kd_jns_kend").val()}&kd_wilayah=${$("#kd_wilayah").val()}&kd_jns_ptg=${$("#kd_jns_ptg").val()}&nilai_pap=${$("#nilai_pap").val()}`, (returnValue) => {
        $("#pst_rate_pap").getKendoNumericTextBox().value(returnValue.split(",")[1]);
        $("#pst_rate_pap").getKendoNumericTextBox().trigger("change");
    });
}

function OnPstRatePAPChange(e){
    var value = e.sender.value();
    ajaxGet(`/Akseptasi/GenerateNilaiPrmPAP?nilai_pap=${$("#nilai_pap").val()}&pst_rate_pap=${value}&stn_rate_pap=${$("#stn_rate_pap").val()}&jml_pap=${$("#jml_pap").val()}`, (returnValue) => {
        $("#nilai_prm_pap").getKendoNumericTextBox().value(returnValue.split(",")[1]);
    });
}

function OnPADChange(e){
    ajaxGet(`/Akseptasi/GeneratePstRatePAD?kd_jns_kend=${$("#kd_jns_kend").val()}&kd_wilayah=${$("#kd_wilayah").val()}&kd_jns_ptg=${$("#kd_jns_ptg").val()}&nilai_pad=${$("#nilai_pad").val()}`, (returnValue) => {
        $("#pst_rate_pad").getKendoNumericTextBox().value(returnValue.split(",")[1]);
        $("#pst_rate_pad").getKendoNumericTextBox().trigger("change");
    });
}

function OnPstRatePADChange(e){
    var value = e.sender.value();
    ajaxGet(`/Akseptasi/GenerateNilaiPrmPAD?nilai_pad=${$("#nilai_pad").val()}&pst_rate_pad=${value}&stn_rate_pad=${$("#stn_rate_pad").val()}`, (returnValue) => {
        $("#nilai_prm_pad").getKendoNumericTextBox().value(returnValue.split(",")[1]);
    });
}