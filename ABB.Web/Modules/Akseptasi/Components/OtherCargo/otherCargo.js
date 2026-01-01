$(document).ready(function () {
    btnNextResikoOtherCargo();
    btnSaveAkseptasiResikoOtherCargo_Click();
    btnDeleteAkseptasiResikoOtherCargo_Click();

    if($("#IsNewOther").val() === "True"){
        $("#btn-delete-akseptasiResikoOtherCargo").hide();
    }
});

function btnNextResikoOtherCargo(){
    $('#btn-next-akseptasiResikoOtherCargo').click(function () {
        $("#resikoOtherTab").getKendoTabStrip().select(1);
    });
}

function btnDeleteAkseptasiResikoOtherCargo_Click(){
    $('#btn-delete-akseptasiResikoOtherCargo').click(function () {
        showConfirmation('Confirmation', `Are you sure you want to delete?`,
            function () {
                showProgress('#AkseptasiWindow');
                setTimeout(function () { deleteAkseptasiResikoOtherCargo(); }, 500);
            }
        );
    });
}

function deleteAkseptasiResikoOtherCargo() {
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

    ajaxPost(`/Akseptasi/DeleteOtherCargo`, JSON.stringify(data), function (response) {
        if (response.Result) {
            showMessage('Success', 'Data has been deleted');
            $("#btn-delete-akseptasiResikoOtherCargo").hide();
        }
        else {
            showMessage('Error', 'Delete data is failed, this data is already used');
        }
        
        var dataOther = {
            kd_cb: $("#kd_cb").val(),
            kd_cob: $("#kd_cob").val(),
            kd_scob: $("#kd_scob").val(),
            kd_thn: $("#kd_thn").val(),
            no_aks: $("#no_aks").val(),
            no_updt: $("#no_updt").val(),
            no_rsk: resiko.no_rsk,
            kd_endt: resiko.kd_endt,
            pst_share: resiko.pst_share_bgu,
        }

        ajaxPost(`/Akseptasi/CheckOther`, JSON.stringify(dataOther),
            function (response) {
                $("#tabOther").html(response);
                closeProgress('#AkseptasiWindow');
            }
        );

        closeProgress('#AkseptasiWindow');
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
    form.append("kd_endt", $("#resiko_other_kd_endt").val());
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

            refreshGrid("#AkseptasiOtherCargoDetailGrid");
            
            closeProgress('#AkseptasiWindow');
        }
    );
}

function OnKodeAlatAngkutChange(e){
    ajaxGet(`/Akseptasi/GenerateNamaKapal?kd_kapal=${e.sender._cascadedValue}`, (returnValue) => {
        $("#nm_kapal").getKendoTextBox().value(returnValue.split(",")[1]);
    });
}