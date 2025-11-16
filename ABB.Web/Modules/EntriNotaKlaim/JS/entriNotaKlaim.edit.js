$(document).ready(function () {
    btnSaveEntriNotaKlaim_Click();
    setTimeout(setEntriNotaKlaimEditedValue, 1000);
});

function setEntriNotaKlaimEditedValue(){
    var flag_posting = $("#tempFlag_posting").val();
    flag_posting == "Y" ? $("#flag_posting").prop("checked", true) : $("#flag_posting").prop("checked", false);
    var flag_cancel = $("#tempFlag_cancel").val();
    flag_cancel == "Y" ? $("#flag_cancel").prop("checked", true) : $("#flag_cancel").prop("checked", false);

    $("#kd_rk_ttj").data("kendoDropDownList").value($("#temp_kd_rk_ttj").val().trim());

    closeProgress('#EntriNotaKlaimWindow');
}

function btnSaveEntriNotaKlaim_Click() {
    $('#btn-save-entriNotaKlaim').click(function () {
        showProgress('#EntriNotaKlaimWindow');
        setTimeout(function () {
            saveEntriNotaKlaim('/EntriNotaKlaim/SaveEntriNotaKlaim')
        }, 500);
    });
}


function saveEntriNotaKlaim(url){
    var form = getFormData($('#EntriNotaKlaimForm'));
    form.flag_posting = $("#flag_posting")[0].checked ? "Y" : "N";
    form.flag_cancel = $("#flag_cancel")[0].checked ? "Y" : "N";
    
    ajaxPost(url, JSON.stringify(form),
        function (response) {
            refreshGrid("#EntriNotaKlaimGrid");
            if (response.Result == "OK") {
                showMessage('Success', response.Message);
            } else
                showMessage('Error', response.Message);

            closeProgress("#EntriNotaKlaimWindow");
            closeWindow("#EntriNotaKlaimWindow")
        }
    );
}

function OnKodeTertujuChange(e){
    var kd_rk_ttj = $("#kd_rk_ttj").data("kendoDropDownList");
    kd_rk_ttj.dataSource.read({
        kd_grp_ttj : e.sender._cascadedValue
    });
}
