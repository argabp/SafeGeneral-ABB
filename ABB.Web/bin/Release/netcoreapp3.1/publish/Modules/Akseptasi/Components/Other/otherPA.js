$(document).ready(function () {
    btnPreviousOther();
    btnSaveAkseptasiResikoOther_Click();
});

function btnPreviousOther(){
    $('#btn-previous-akseptasiResikoOther').click(function () {
        $("#resikoTab").getKendoTabStrip().select(2);
    });
}

function dataKodeJenisKreditDropDown(){
    return {
        kd_cb: $("#kd_cb").val()
    }
}

function OnJenisKreditChange(e){
    $("#kd_grp_kr").getKendoDropDownList().dataSource.read();
}

function dataJenisCoverDropDown(){
    return {
        kd_cb: $("#kd_cb").val(),
        kd_jns_kr: $("#kd_jns_kr").val()
    }
}

function dataAsuransiJiwaDropDown(){
    return {
        kd_grp_asj: $("#kd_grp_asj").val()
    }
}

function btnSaveAkseptasiResikoOther_Click() {
    $('#btn-save-akseptasiResikoOtherPA').click(function () {
        showProgress('#AkseptasiWindow');
        setTimeout(function () {
            saveAkseptasiResikoOther('/Akseptasi/SaveAkseptasiOtherPA')
        }, 500);
    });
}

function saveAkseptasiResikoOther(url) {
    var form = getFormData($('#OtherPAForm'));
    form.kd_cb = $("#kd_cb").val();
    form.kd_cob = $("#kd_cob").val();
    form.kd_scob = $("#kd_scob").val();
    form.kd_thn = $("#kd_thn").val();
    form.no_aks = $("#no_aks").val();
    form.no_updt = $("#resiko_other_pa_no_updt").val();
    form.no_rsk = resiko.no_rsk;
    form.kd_endt = $("#resiko_other_pa_kd_endt").val();
    form.kd_updt = $("#resiko_other_pa_kd_updt").val();
    form.tgl_mul_ptg = $("#resiko_other_pa_tgl_mul_ptg").val();
    form.tgl_akh_ptg = $("#resiko_other_pa_tgl_akh_ptg").val();
    form.tgl_input = $("#resiko_other_pa_tgl_input").val();

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
