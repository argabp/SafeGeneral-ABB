$(document).ready(function () {
    btnPreviousOther();
    btnSaveAkseptasiResikoOther_Click();
});

function btnPreviousOther(){
    $('#btn-previous-akseptasiResikoOther').click(function () {
        $("#resikoTab").getKendoTabStrip().select(2);
    });
}

function dataKodeSerutyDropDown(){
    return {
        kd_cb: $("#kd_cb").val(),
        kd_grp_surety: $("#kd_grp_surety").val()
    }
}

function dataKodePrincipalDropDown(){
    return {
        kd_cb: $("#kd_cb").val(),
        kd_grp_rk: $("#kd_grp_prc").val()
    }
}

function OnChangeKodePrincipal(e){
    $("#kd_rk_prc").getKendoDropDownList().dataSource.read();
}

function dataGroupObligeeDropDown(){
    return {
        grp_obl: $("#grp_obl").val()
    }
}

function dataKodeObligeeDropDown(){
    return {
        kd_cb: $("#kd_cb").val(),
        kd_grp_rk: $("#kd_grp_obl").val()
    }
}

function dataGroupKontrakDropDown(){
    return {
        grp_kontr: $("#grp_kontr").val()
    }
}

function dataKodePekerjaanDropDown(){
    return {
        kd_grp_rk: $("#grp_jns_pekerjaan").val()
    }
}

function dataTTDSuretyDropDown(){
    return {
        kd_cb: $("#kd_cb").val()
    }
}

function btnSaveAkseptasiResikoOther_Click() {
    $('#btn-save-akseptasiResikoOtherBonding').click(function () {
        showProgress('#AkseptasiWindow');
        setTimeout(function () {
            saveAkseptasiResikoOther('/Akseptasi/SaveAkseptasiOtherBonding')
        }, 500);
    });
}

function saveAkseptasiResikoOther(url) {
    var form = getFormData($('#OtherBondingForm'));
    form.kd_cb = $("#kd_cb").val();
    form.kd_cob = $("#kd_cob").val();
    form.kd_scob = $("#kd_scob").val();
    form.kd_thn = $("#kd_thn").val();
    form.no_aks = $("#no_aks").val();
    form.no_updt = $("#resiko_other_bonding_no_updt").val();
    form.no_rsk = resiko.no_rsk;
    form.kd_endt = $("#resiko_other_bonding_kd_endt").val();
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

function OnKodeObligeeChange(e){
    var value = e.sender.value();
    ajaxGet(`/Akseptasi/GenerateNameAandAddressObligee?kd_cb=${$("#kd_cb").val()}&kd_rk_obl=${value}`, (returnValue) => {
        $("#nm_obl").getKendoTextArea().value(returnValue.split(",")[1]);
        $("#almt_obl").getKendoTextArea().value(returnValue.split(",")[4]);
    });
}
