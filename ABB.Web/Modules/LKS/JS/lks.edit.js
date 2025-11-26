$(document).ready(function () {
    btnSaveLKS_Click();
    setTimeout(setLKSEditedValue, 1000);
});

function setLKSEditedValue(){
    var flag_posting = $("#tempFlag_posting").val();
    flag_posting == "Y" ? $("#flag_posting").prop("checked", true) : $("#flag_posting").prop("checked", false);

    $("#kd_rk_pas").data("kendoDropDownList").value($("#temp_kd_rk_pas").val().trim());

    closeProgress('#LKSWindow');
}

function btnSaveLKS_Click() {
    $('#btn-save-lks').click(function () {
        showProgress('#LKSWindow');
        setTimeout(function () {
            saveLKS('/LKS/SaveLKS')
        }, 500);
    });
}

function saveLKS(url){
    var form = getFormData($('#LKSForm'));
    
    ajaxPost(url, JSON.stringify(form),
        function (response) {
            refreshGrid("#LKSGrid");
            if (response.Result == "OK") {
                showMessage('Success', response.Message);
            } else
                showMessage('Error', response.Message);

            closeProgress("#LKSWindow");
            closeWindow("#LKSWindow")
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