$(document).ready(function () {
    btnSaveEntriNotaCancel_Click();
    setTimeout(setEntriNotaCancelEditedValue, 1000);
});

function setEntriNotaCancelEditedValue(){
    var flag_posting = $("#tempFlag_posting").val();
    flag_posting == "Y" ? $("#flag_posting").prop("checked", true) : $("#flag_posting").prop("checked", false);
    var flag_cancel = $("#tempFlag_cancel").val();
    flag_cancel == "Y" ? $("#flag_cancel").prop("checked", true) : $("#flag_cancel").prop("checked", false);

    $("#kd_rk_ttj").data("kendoDropDownList").value($("#temp_kd_rk_ttj").val().trim());

    closeProgress('#EntriNotaCancelWindow');
}

function btnSaveEntriNotaCancel_Click() {
    $('#btn-save-entriNotaCancel').click(function () {
        showProgress('#EntriNotaCancelWindow');
        setTimeout(function () {
            saveEntriNotaCancel('/EntriNota/SaveEntriNotaCancel')
        }, 500);
    });
}

function saveEntriNotaCancel(url){
    var form = getFormData($('#EntriNotaCancelForm'));
    form.flag_posting = $("#flag_posting")[0].checked ? "Y" : "N";
    form.flag_cancel = $("#flag_cancel")[0].checked ? "Y" : "N";
    
    ajaxPost(url, JSON.stringify(form),
        function (response) {
            refreshGrid("#EntriNotaGrid");
            if (response.Result == "OK") {
                showMessage('Success', response.Message);
            } else
                showMessage('Error', response.Message);

            closeProgress("#EntriNotaCancelWindow");
            closeWindow("#EntriNotaCancelWindow")
        }
    );
}

function OnKodeTertujuChange(e){
    var kd_rk_ttj = $("#kd_rk_ttj").data("kendoDropDownList");
    kd_rk_ttj.dataSource.read({
        kd_grp_ttj : e.sender._cascadedValue,
        kd_cb: $("#kd_cb").val().trim(),
        kd_cob: $("#kd_cob").val().trim(),
        kd_scob: $("#kd_scob").val().trim(),
        kd_thn: $("#kd_thn").val().trim(),
        no_pol: $("#no_pol").val().trim(),
        no_updt: $("#no_updt").val()
    });
}
