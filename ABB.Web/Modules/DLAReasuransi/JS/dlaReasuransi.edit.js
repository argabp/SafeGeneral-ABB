$(document).ready(async function () {
    btnSaveDLAReasuransi_Click();
    await setDLAReasuransiEditedValue();
});

async function setDLAReasuransiEditedValue(){
    showProgress('#DLAReasuransiWindow');
    await restoreDropdownValue("#kd_rk_pas", "#temp_kd_rk_pas");

    closeProgress('#DLAReasuransiWindow');
}

function btnSaveDLAReasuransi_Click() {
    $('#btn-save-dla-reasuransi').click(function () {
        showProgress('#DLAReasuransiWindow');
        setTimeout(function () {
            saveDLAReasuransi('/DLAReasuransi/SaveDLAReasuransi')
        }, 500);
    });
}

function saveDLAReasuransi(url){
    var form = getFormData($('#DLAReasuransiForm'));
    
    ajaxPost(url, JSON.stringify(form),
        function (response) {
            refreshGrid("#DLAReasuransiGrid");
            if (response.Result == "OK") {
                showMessage('Success', response.Message);
            } else
                showMessage('Error', response.Message);

            closeProgress("#DLAReasuransiWindow");
            closeWindow("#DLAReasuransiWindow")
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