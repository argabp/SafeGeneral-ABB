$(document).ready(async function () {
    btnSaveLKT_Click();
    await setLKTEditedValue();
});

async function setLKTEditedValue(){
    showProgress('#LKTWindow');
    await restoreDropdownValue("#kd_rk_pas", "#temp_kd_rk_pas");

    closeProgress('#LKTWindow');
}

function btnSaveLKT_Click() {
    $('#btn-save-lkt').click(function () {
        showProgress('#LKTWindow');
        setTimeout(function () {
            saveLKT('/LKT/SaveLKT')
        }, 500);
    });
}

function saveLKT(url){
    var form = getFormData($('#LKTForm'));
    
    ajaxPost(url, JSON.stringify(form),
        function (response) {
            refreshGrid("#LKTGrid");
            if (response.Result == "OK") {
                showMessage('Success', response.Message);
            } else
                showMessage('Error', response.Message);

            closeProgress("#LKTWindow");
            closeWindow("#LKTWindow")
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