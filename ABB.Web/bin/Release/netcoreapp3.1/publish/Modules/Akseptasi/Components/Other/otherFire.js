$(document).ready(function () {
    btnPreviousOther();
    btnSaveAkseptasiResikoOther_Click();
    setTimeout(setOtherFireEditedValue, 2000);
});

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