$(document).ready(function () {
    btnNextResikoOtherCargo();
    btnSaveAkseptasiResikoOtherCargo_Click();
});

function btnNextResikoOtherCargo(){
    $('#btn-next-akseptasiResikoOtherCargo').click(function () {
        $("#resikoOtherTab").getKendoTabStrip().select(1);
    });
}


function btnSaveAkseptasiResikoOtherCargo_Click() {
    $('#btn-save-akseptasiResikoOtherCargo').click(function () {
        showProgress('#AkseptasiWindow');
        setTimeout(function () {
            saveAkseptasiResikoOtherCargo('/Akseptasi/SaveAkseptasiOtherCargo')
        }, 500);
    });
}

function saveAkseptasiResikoOtherCargo(url) {
    var form = new FormData($('#OtherForm')[0]);
    form.append("kd_cb", $("#kd_cb").val())
    form.append("kd_cob", $("#kd_cob").val())
    form.append("kd_scob", $("#kd_scob").val())
    form.append("kd_thn", $("#kd_thn").val())
    form.append("no_aks", $("#no_aks").val())
    form.append("no_updt", $("#resiko_other_no_updt").val())
    form.append("no_rsk", resiko.no_rsk);
    form.append("kd_endt", resiko.kd_endt);
    form.append("no_pol_ttg", $("#no_pol_ttg").val());

    $("#linkFileOtherCargo").getKendoUpload().getFiles().forEach((data, index) => {
        form.append("file", data.rawFile);
    });

    ajaxUpload(url, form,
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

function OnKodeAlatAngkutChange(e){
    ajaxGet(`/Akseptasi/GenerateNamaKapal?kd_kapal=${e.sender._cascadedValue}`, (returnValue) => {
        $("#nm_kapal").getKendoTextBox().value(returnValue.split(",")[1]);
    });
}