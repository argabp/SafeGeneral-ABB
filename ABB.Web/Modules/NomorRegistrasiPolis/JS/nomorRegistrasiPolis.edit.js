$(document).ready(async function () {
    btnSaveNomorRegistrasiPolis_Click();
    await setNomorRegistrasiPolisEditedValue();
});

async function setNomorRegistrasiPolisEditedValue(){
    showProgress('#NomorRegistrasiPolisWindow');
    await restoreDropdownValue("#kd_scob", "#temp_kd_scob");

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