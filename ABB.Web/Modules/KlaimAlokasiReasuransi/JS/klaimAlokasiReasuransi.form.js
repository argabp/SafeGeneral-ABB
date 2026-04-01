$(document).ready(function () {
    btnSaveMutasiKlaim_Click();
});

function btnSaveMutasiKlaim_Click() {
    $('#btn-save-mutasiKlaim').click(function () {
        showProgress('#KlaimAlokasiReasuransiWindow');
        setTimeout(function () {
            saveMutasiKlaim('/KlaimAlokasiReasuransi/Save')
        }, 500);
    });
}

function saveMutasiKlaim(url) {
    var form = getFormData($('#MutasiKlaimForm'));
    
    var data = JSON.stringify(form);
    
    ajaxPost(url, data,
        function (response) {

            refreshGrid('#MutasiKlaimGrid');
            if (response.Result == "OK") {
                showMessage('Success', response.Message);
                closeWindow('#KlaimAlokasiReasuransiWindow');
            }
            else if (response.Result == "ERROR")
                showMessage("Error", response.Message);
            else
                showMessage("Error", response);

            closeProgress('#KlaimAlokasiReasuransiWindow');
        }
    );
}

$(document).ready(function () {
    setTimeout(() => {
        var flag_konv = $("#tempFlag_konv").val();
        flag_konv == "Y" ? $("#flag_konv").prop("checked", true) : $("#flag_konv").prop("checked", false);
        var flag_closing = $("#tempFlag_closing").val();
        flag_closing == "Y" ? $("#flag_closing").prop("checked", true) : $("#flag_closing").prop("checked", false);
        var flag_reas = $("#tempFlag_reas").val();
        flag_reas == "Y" ? $("#flag_reas").prop("checked", true) : $("#flag_reas").prop("checked", false);
    }, 1000);
});


function OnKodeCabangChange(e){
    var value = e.sender._cascadedValue;
    $("#temp_kd_cb").val(value);
    var nomor_register = value.trim() + "." + $("#kd_cob").val().trim() + $("#kd_scob").val().trim() + "." + $("#kd_thn").val().trim() + "." + $("#no_kl").val().trim();
    $("#temp_nomor_register").val(nomor_register);
}

function OnKodeCOBChange(e){
    var value = e.sender._cascadedValue;
    $("#temp_kd_cob").val(value);
    var kd_scob = $("#kd_scob").data("kendoDropDownList");
    kd_scob.dataSource.read({kd_cob : e.sender._cascadedValue});
    var nomor_register = $("#kd_cb").val().trim() + "." + value.trim() + $("#kd_scob").val().trim() + "." + $("#kd_thn").val().trim() + "." + $("#no_kl").val().trim();
    $("#temp_nomor_register").val(nomor_register);
}

function OnKodeSCOBChange(e){
    var value = e.sender._cascadedValue;
    $("#temp_kd_scob").val(value);
    var nomor_register = $("#kd_cb").val().trim() + "." + $("#kd_cob").val().trim() + value.trim() + "." + $("#kd_thn").val().trim() + "." + $("#no_kl").val().trim();
    $("#temp_nomor_register").val(nomor_register);
}

function dataKodeSCOBDropDown(){
    return {
        kd_cb: $("#temp_kd_cb").val().trim(),
        kd_cob: $("#temp_kd_cob").val().trim()
    }
}
function dataKodeMataUangDropDown(){
    return {
        kd_cb: $("#temp_kd_cb").val().trim()
    }
}
function dataKodeCOBDropDown(){
    return {
        kd_cb: $("#temp_kd_cb").val().trim()
    }
}