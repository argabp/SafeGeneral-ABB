$(document).ready(function () {
    btnPreviousOther();
    btnSaveAkseptasiResikoOther_Click();
    btnDeleteAkseptasiResikoOtherHull_Click();
    
    if($("#kd_cob").val().trim() == "H"){
        $("#label_no_rangka").text("Nomor IMO");
        $("#label_no_msn").text("Nomor Register");
    }else {
        $("#label_no_rangka").text("Nomor Rangka");
        $("#label_no_msn").text("Nomor Mesin");
    }

    if($("#IsNewOther").val() === "True"){
        $("#btn-delete-akseptasiResikoOtherHull").hide();
    }
});

function btnPreviousOther(){
    $('#btn-previous-akseptasiResikoOther').click(function () {
        $("#resikoTab").getKendoTabStrip().select(2);
    });
}

function btnDeleteAkseptasiResikoOtherHull_Click(){
    $('#btn-delete-akseptasiResikoOtherHull').click(function () {
        showConfirmation('Confirmation', `Are you sure you want to delete?`,
            function () {
                showProgress('#AkseptasiWindow');
                setTimeout(function () { deleteAkseptasiResikoOtherHull(); }, 500);
            }
        );
    });
}

function deleteAkseptasiResikoOtherHull() {
    var data = {
        kd_cb: $("#kd_cb").val(),
        kd_cob: $("#kd_cob").val(),
        kd_scob: $("#kd_scob").val(),
        kd_thn: $("#kd_thn").val(),
        no_aks: $("#no_aks").val(),
        no_updt: resiko.no_updt,
        no_rsk: resiko.no_rsk,
        kd_endt: resiko.kd_endt
    }

    ajaxPost(`/Akseptasi/DeleteOtherHull`, JSON.stringify(data), function (response) {
        if (response.Result) {
            showMessage('Success', 'Data has been deleted');
            $("#btn-delete-akseptasiResikoOtherHull").hide();
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
            no_updt: resiko.no_updt,
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
    form.no_updt = resiko.no_updt;
    form.no_rsk = resiko.no_rsk;
    form.kd_endt = resiko.kd_endt;

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
        for (let data of returnValue){
            if(data == null) {
                continue;
            }
            
            var property = data.split(",")[0];
            var value = data.split(",")[1];
            
            switch (property){
                case "grt":
                    $("#grt").getKendoNumericTextBox().value(value);
                    break;
                case "merk_kapal":
                    $("#merk_kapal").getKendoDropDownList().value(value);
                    break;
                case "nm_kapal":
                    $("#nm_kapal").getKendoTextBox().value(value);
                    break;
                case "thn_buat":
                    $("#thn_buat").getKendoNumericTextBox().value(value);
                    break;
            }
        }
    });
}
