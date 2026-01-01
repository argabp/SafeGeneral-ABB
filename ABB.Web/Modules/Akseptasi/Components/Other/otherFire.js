$(document).ready(function () {
    btnPreviousOther();
    btnSaveAkseptasiResikoOther_Click();
    setTimeout(setOtherFireEditedValue, 2000);
    btnOpenLokasResiko();
    btnDeleteAkseptasiResikoOtherFire_Click();

    if($("#IsNewOther").val() === "True"){
        $("#btn-delete-akseptasiResikoOtherFire").hide();
    }
});

function btnDeleteAkseptasiResikoOtherFire_Click(){
    $('#btn-delete-akseptasiResikoOtherFire').click(function () {
        showConfirmation('Confirmation', `Are you sure you want to delete?`,
            function () {
                showProgress('#AkseptasiWindow');
                setTimeout(function () { deleteAkseptasiResikoOtherFire(); }, 500);
            }
        );
    });
}

function deleteAkseptasiResikoOtherFire() {
    var data = {
        kd_cb: $("#kd_cb").val(),
        kd_cob: $("#kd_cob").val(),
        kd_scob: $("#kd_scob").val(),
        kd_thn: $("#kd_thn").val(),
        no_aks: $("#no_aks").val(),
        no_updt: $("#resiko_other_fire_no_updt").val(),
        no_rsk: resiko.no_rsk,
        kd_endt: $("#resiko_other_fire_kd_endt").val()
    }

    ajaxPost(`/Akseptasi/DeleteOtherFire`, JSON.stringify(data), function (response) {
        if (response.Result) {
            showMessage('Success', 'Data has been deleted');
            $("#btn-delete-akseptasiResikoOtherFire").hide();
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

function setOtherFireEditedValue(){
    $("#kd_kab").data("kendoDropDownList").value($("#temp_kd_kab").val().trim());
    $("#kd_kec").data("kendoDropDownList").value($("#temp_kd_kec").val().trim());
    $("#kd_kel").data("kendoDropDownList").value($("#temp_kd_kel").val().trim());
}

function dataKodeKabupatenDropDown(){
    return {
        kd_prop: $("#temp_kd_prop").val()
    }
}

function dataKodeKecamatanDropDown(){
    return {
        kd_prop: $("#temp_kd_prop").val(),
        kd_kab: $("#temp_kd_kab").val()
    }
}

function dataKodeKelurahanDropDown(){
    return {
        kd_prop: $("#temp_kd_prop").val(),
        kd_kab: $("#temp_kd_kab").val(),
        kd_kec: $("#temp_kd_kec").val()
    }
}

function btnPreviousOther(){
    $('#btn-previous-akseptasiResikoOther').click(function () {
        $("#resikoTab").getKendoTabStrip().select(2);
    });
}

function btnSaveAkseptasiResikoOther_Click() {
    $('#btn-save-akseptasiResikoOtherFire').click(function () {
        showProgress('#AkseptasiWindow');
        setTimeout(function () {
            saveAkseptasiResikoOther('/Akseptasi/SaveAkseptasiOtherFire')
        }, 500);
    });
}

function btnOpenLokasResiko() {
    $('#lokasi-resiko').click(function () {
        openWindow('#LokasiResikoWindow', "/Akseptasi/LokasiResiko", "Lokasi Resiko");
    });
}

function saveAkseptasiResikoOther(url) {
    var form = getFormData($('#OtherFireForm'));
    form.kd_cb = $("#kd_cb").val();
    form.kd_cob = $("#kd_cob").val();
    form.kd_scob = $("#kd_scob").val();
    form.kd_thn = $("#kd_thn").val();
    form.no_aks = $("#no_aks").val();
    form.no_updt = $("#resiko_other_fire_no_updt").val();
    form.no_rsk = resiko.no_rsk;
    form.kd_endt = $("#resiko_other_fire_kd_endt").val();
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

function OnKodePropinsiChange(e){
    var value = e.sender._cascadedValue;
    var kd_kab = $("#kd_kab").data("kendoDropDownList");
    kd_kab.dataSource.read({kd_prop : value});
}

function OnKodeKabupatenChange(e){
    var value = e.sender._cascadedValue;
    var kd_kec = $("#kd_kec").data("kendoDropDownList");
    kd_kec.dataSource.read({kd_prop: $("#kd_prop").val(), kd_kab: value});
}

function OnKodeKecamatanChange(e){
    var value = e.sender._cascadedValue;
    var kd_kel = $("#kd_kel").data("kendoDropDownList");
    kd_kel.dataSource.read({kd_prop: $("#kd_prop").val(), kd_kab: $("#kd_kab").val(), kd_kec: value });
}

function onLokasiResikoChange(e){
    ajaxGet("/Akseptasi/GetLokasiResikoDetail?kd_lok_rsk=" + e.sender._value, (returnValue) => {
        $("#almt_rsk").getKendoTextArea().value(returnValue[0].split(",")[1]);
        $("#kd_pos_rsk").getKendoTextBox().value(returnValue[4].split(",")[1]);
        $("#kd_prop").getKendoDropDownList().value(returnValue[5].split(",")[1].trim());
        $("#kd_prop").getKendoDropDownList().trigger("change");
        $("#kt_rsk").getKendoTextBox().value(returnValue[6].split(",")[1]);
        
        setTimeout(() => {
            $("#kd_kab").getKendoDropDownList().value(returnValue[1].split(",")[1].trim());
            $("#kd_kab").getKendoDropDownList().trigger("change");
            setTimeout(() => {
                $("#kd_kec").getKendoDropDownList().value(returnValue[2].split(",")[1].trim());
                $("#kd_kec").getKendoDropDownList().trigger("change");
                setTimeout(() => {
                    $("#kd_kel").getKendoDropDownList().value(returnValue[3].split(",")[1].trim());
                }, 500);
            }, 500);
        }, 500);
    });
}

function onKodeOkpasiChange(e){
    ajaxGet(`/Akseptasi/GetKodeOkupasiDetail?kd_zona=${$("#kd_zona").val()}&kd_kls_konst=${$("#kd_kls_konst").val()}&kd_okup=${e.sender._cascadedValue}&kd_scob=${$("#kd_scob").val()}`, (returnValue) => {
        $("#ket_okup").getKendoTextArea().value(returnValue[0].split(",")[1]);
    });
}