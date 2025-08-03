$(document).ready(function () {
    btnSaveAkseptasiObyek_Click();
});

function btnSaveAkseptasiObyek_Click() {
    $('#btn-save-akseptasiObyek').click(function () {
        showProgress('#AkseptasiObyekWindow');
        setTimeout(function () {
            saveAkseptasiObyek('/Akseptasi/SaveAkseptasiObyek')
        }, 500);
    });
}

function saveAkseptasiObyek(url) {
    var form = getFormData($('#ObyekForm'));
    form.nilai_ttl_ptg = $("#resiko_obyek_nilai_ttl_ptg").val();
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

function OnNilaiPtg100Change(e){
    // ajaxGet(`/Akseptasi/GetNilaiPtg?pst_share=${$("#pst_share").val()}&kd_grp_sb_bis=${$("#kd_grp_sb_bis").val()}&kd_rk_sb_bis=${e.sender.value()}`, (returnValue) => {
    //     $("#tgl_akh_ptg").getKendoDatePicker().value(returnValue.split(",")[1]);
    //     $("#tgl_akh_ptg").getKendoDatePicker().trigger("change");
    // });
}