$(document).ready(function () {
    btnPreviousOther();
    btnSaveAkseptasiResikoOther_Click();
    
    if($("#kd_cob").val().trim() == "H"){
        $("#label_no_rangka").text("Nomor IMO");
        $("#label_no_msn").text("Nomor Register");
    }else {
        $("#label_no_rangka").text("Nomor Rangka");
        $("#label_no_msn").text("Nomor Mesin");
    }
});

function btnPreviousOther(){
    $('#btn-previous-akseptasiResikoOther').click(function () {
        $("#resikoTab").getKendoTabStrip().select(2);
    });
}


function btnSaveAkseptasiResikoOther_Click() {
    $('#btn-save-akseptasiResikoOtherHull').click(function () {
        showProgress('#AkseptasiWindow');
        setTimeout(function () {
            saveAkseptasiResikoOther('/Akseptasi/SaveAkseptasiOtherHull')
        }, 500);
    });
}

function saveAkseptasiResikoOther(url) {
    var form = getFormData($('#OtherHullForm'));
    form.kd_cb = $("#kd_cb").val();
    form.kd_cob = $("#kd_cob").val();
    form.kd_scob = $("#kd_scob").val();
    form.kd_thn = $("#kd_thn").val();
    form.no_aks = $("#no_aks").val();
    form.no_updt = $("#resiko_other_hull_no_updt").val();
    form.no_rsk = resiko.no_rsk;
    form.kd_endt = $("#resiko_other_hull_kd_endt").val();

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

function OnKodeKapalChange(e){
    ajaxGet(`/Akseptasi/GenerateDataKapal?kd_kapal=${e.sender.value()}`, (returnValue) => {
        $("#grt").getKendoNumericTextBox().value(returnValue[0].split(",")[1]);
        $("#merk_kapal").getKendoDropDownList().value(returnValue[1].split(",")[1]);
        $("#nm_kapal").getKendoTextBox().value(returnValue[2].split(",")[1]);
        $("#thn_buat").getKendoNumericTextBox().value(returnValue[3].split(",")[1]);
    });
}
