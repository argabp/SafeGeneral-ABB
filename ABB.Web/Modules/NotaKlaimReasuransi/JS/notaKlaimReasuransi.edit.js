$(document).ready(async function () {
    btnSaveNotaKlaimReasuransi_Click();
    await setNotaKlaimReasuransiEditedValue();
});

async function setNotaKlaimReasuransiEditedValue(){
    showProgress('#NotaKlaimReasuransiWindow');
    var flag_posting = $("#tempFlag_posting").val();
    flag_posting == "Y" ? $("#flag_posting").prop("checked", true) : $("#flag_posting").prop("checked", false);
    var flag_cancel = $("#tempFlag_cancel").val();
    flag_cancel == "Y" ? $("#flag_cancel").prop("checked", true) : $("#flag_cancel").prop("checked", false);

    await restoreDropdownValue("#kd_rk_ttj", "#temp_kd_rk_ttj");
    await restoreDropdownValue("#kd_rk_sor", "#temp_kd_rk_sor");

    closeProgress('#NotaKlaimReasuransiWindow');
}

function btnSaveNotaKlaimReasuransi_Click() {
    $('#btn-save-notaKlaimReasuransi').click(function () {
        showProgress('#NotaKlaimReasuransiWindow');
        setTimeout(function () {
            saveNotaKlaimReasuransi('/NotaKlaimReasuransi/SaveNotaKlaimReasuransi')
        }, 500);
    });
}


function saveNotaKlaimReasuransi(url){
    var form = getFormData($('#NotaKlaimReasuransiForm'));
    form.flag_posting = $("#flag_posting")[0].checked ? "Y" : "N";
    form.flag_cancel = $("#flag_cancel")[0].checked ? "Y" : "N";
    
    ajaxPost(url, JSON.stringify(form),
        function (response) {
            refreshGrid("#NotaKlaimReasuransiGrid");
            if (response.Result == "OK") {
                showMessage('Success', response.Message);
            } else
                showMessage('Error', response.Message);

            closeProgress("#NotaKlaimReasuransiWindow");
            closeWindow("#NotaKlaimReasuransiWindow")
        }
    );
}

function OnKodeTertujuChange(e){
    var kd_rk_ttj = $("#kd_rk_ttj").data("kendoDropDownList");
    kd_rk_ttj.dataSource.read({
        kd_grp_ttj : e.sender._cascadedValue,
        kd_cb: $("#kd_cb").val()
    });
}

function OnKodeSorChange(e){
    var kd_rk_sor = $("#kd_rk_sor").data("kendoDropDownList");
    kd_rk_sor.dataSource.read({
        kd_grp_sor : e.sender._cascadedValue
    });
}
