$(document).ready(function () {
    btnSaveMutasiKlaimBeban_Click();
});

function btnSaveMutasiKlaimBeban_Click() {
    $('#btn-save-mutasiKlaimBeban').click(function () {
        showProgress('#MutasiKlaimWindow');
        setTimeout(function () {
            saveMutasiKlaimBeban('/MutasiKlaim/SaveMutasiKlaimBeban')
        }, 500);
    });
}

function saveMutasiKlaimBeban(url) {
    var form = getFormData($('#MutasiKlaimBebanForm'));
    
    var parentId =
        form.kd_cb.trim() +
        form.kd_cob.trim() +
        form.kd_scob.trim() +
        form.kd_thn.trim() +
        form.no_kl.trim() 

    var mutasiGridName = "grid_obyek_" + parentId;
    var mutasiGridElement = $("#" + mutasiGridName);
    
    var data = JSON.stringify(form);
    ajaxPost(url,  data,
        function (response) {
            refreshGrid(mutasiGridElement);
            if (response.Result == "OK") {
                showMessage('Success', response.Message);
            }
            else if (response.Result == "ERROR")
                showMessage("Error", response.Message);
            else
                showMessage("Error", response);

            closeProgress('#MutasiKlaimWindow');
            closeWindow('#MutasiKlaimWindow')
        }
    );
}

function dataKodeSCOBDropDown(){
    return {
        kd_cob: $("#temp_kd_cob").val().trim()
    }
}

function OnKodeCOBChange(e){
    var value = e.sender._cascadedValue;
    $("#temp_kd_cob").val(value);
    var kd_scob = $("#kd_scob").data("kendoDropDownList");
    kd_scob.dataSource.read({kd_cob : e.sender._cascadedValue});
}

function OnNilaiBebanChange(e){
    ajaxGet(`/MutasiKlaim/GenerateNilaiRecovery?nilai_jns_org=${e.sender.value()}&kd_mtu=${$("#kd_mtu").val()}`, (returnValue) => {
        $("#nilai_jns").getKendoNumericTextBox().value(returnValue.split(",")[1]);
    });
}