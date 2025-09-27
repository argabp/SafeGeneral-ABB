$(document).ready(function () {
    btnSaveNomorRegistrasiPolis_Click();
    setTimeout(setNomorRegistrasiPolisEditedValue, 1000);
});

function setNomorRegistrasiPolisEditedValue(){
    $("#kd_scob").data("kendoDropDownList").value($("#temp_kd_scob").val().trim());

    closeProgress('#NomorRegistrasiPolisWindow');
}

function btnSaveNomorRegistrasiPolis_Click() {
    $('#btn-save-nomorRegistrasiPolis').click(function () {
        showProgress('#NomorRegistrasiPolisWindow');
        setTimeout(function () {
            saveNomorRegistrasiPolis('/NomorRegistrasiPolis/SaveNomorRegistrasiPolis')
        }, 500);
    });
}

function saveNomorRegistrasiPolis(url){
    var form = getFormData($('#NomorRegistrasiPolisForm'));
    form.no_pol_ttg = $("#no_pol_ttg").getKendoMaskedTextBox().raw();
    
    ajaxPost(url, JSON.stringify(form),
        function (response) {
            refreshGrid("#NomorRegistrasiPolisGrid");
            if (response.Result == "OK") {
                showMessage('Success', response.Message);
            } else
                showMessage('Error', response.Message);

            closeProgress("#NomorRegistrasiPolisWindow");
            closeWindow("#NomorRegistrasiPolisWindow")
        }
    );
}

function dataKodeSCOBDropDown(){
    return {
        kd_cob: $("#temp_kd_cob").val().trim()
    }
}