$(document).ready(function () {
    btnSaveAkseptasiObyekCIT_Click();
});

function btnSaveAkseptasiObyekCIT_Click() {
    $('#btn-save-akseptasiObyekCIT').click(function () {
        showProgress('#AkseptasiObyekWindow');
        setTimeout(function () {
            saveAkseptasiObyekCIT('/Akseptasi/SaveAkseptasiObyekCIT')
        }, 500);
    });
}

function saveAkseptasiObyekCIT(url) {
    var form = getFormData($('#ObyekCITForm'));
    form.pst_rate = $("#resiko_obyek_cit_pst_rate").val();
    form.kd_cb = $("#kd_cb").val();
    form.kd_cob = $("#kd_cob").val();
    form.kd_scob = $("#kd_scob").val();
    form.kd_thn = $("#kd_thn").val();
    form.no_aks = $("#no_aks").val();
    form.no_updt = $("#resiko_obyek_no_updt").val();
    form.no_rsk = resiko.no_rsk;
    form.kd_endt = resiko.kd_endt;
    form.no_pol_ttg = $("#no_pol_ttg").val();
    form.pst_share = resiko.pst_share_bgu;
    
    var data = JSON.stringify(form);
    
    ajaxPost(url, data,
        function (response) {
            refreshGrid("#AkseptasiObyekGrid");
            refreshGrid("#AkseptasiResikoGrid");
            if (response.Result == "OK") {
                showMessage('Success', response.Message);
                closeWindow('#AkseptasiObyekWindow');
            }
            else if (response.Result == "ERROR")
                showMessage("Error", response.Message);
            else
                showMessage("Error", response);

            closeProgress('#AkseptasiObyekWindow');
        }
    );
}

function OnRatePaChange(e){
    ajaxGet(`/Akseptasi/GetNilaiPremiPA?nilai_pa=${$("#nilai_pa").val()}&pst_rate_pa=${e.sender.value()}`, (returnValue) => {
        $("#nilai_prm_pa").getKendoNumericTextBox().value(returnValue.split(",")[1]);
    });
}