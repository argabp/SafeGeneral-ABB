$(document).ready(async function () {
    btnSavePLAReasuransi_Click();
    await setPLAReasuransiEditedValue();
});

async function setPLAReasuransiEditedValue(){
    showProgress('#PLAReasuransiWindow');
    await restoreDropdownValue("#kd_rk_pas", "#temp_kd_rk_pas");
    var flag_posting = $("#tempFlag_posting").val();
    flag_posting == "Y" ? $("#flag_posting").prop("checked", true) : $("#flag_posting").prop("checked", false);

    closeProgress('#PLAReasuransiWindow');
}

function btnSavePLAReasuransi_Click() {
    $('#btn-save-pla-reasuransi').click(function () {
        showProgress('#PLAReasuransiWindow');
        setTimeout(function () {
            savePLAReasuransi('/PLAReasuransi/SavePLAReasuransi')
        }, 500);
    });
}

function savePLAReasuransi(url){
    var form = getFormData($('#PLAReasuransiForm'));
    
    ajaxPost(url, JSON.stringify(form),
        function (response) {
            refreshGrid("#PLAReasuransiGrid");
            if (response.Result == "OK") {
                showMessage('Success', response.Message);
            } else
                showMessage('Error', response.Message);

            closeProgress("#PLAReasuransiWindow");
            closeWindow("#PLAReasuransiWindow")
        }
    );
}

function OnKodePasChange(e){
    var kd_rk_pas = $("#kd_rk_pas").data("kendoDropDownList");
    kd_rk_pas.dataSource.read({
        kd_grp_pas : e.sender._cascadedValue
    });
}

function dataKodeSCOBDropDown(){
    return {
        kd_cob: $("#temp_kd_cob").val().trim()
    }
}

function OnKodeCabangChange(e){
    var value = e.sender._cascadedValue;
    $("#temp_kd_cb").val(value);
}

function OnKodeCOBChange(e){
    var value = e.sender._cascadedValue;
    $("#temp_kd_cob").val(value);
    var kd_scob = $("#kd_scob").data("kendoDropDownList");
    kd_scob.dataSource.read({kd_cob : e.sender._cascadedValue});
}

function dataKodePasDropDown(){
    return {
        kd_grp_pas: $("#kd_grp_pas").val().trim()
    }
}