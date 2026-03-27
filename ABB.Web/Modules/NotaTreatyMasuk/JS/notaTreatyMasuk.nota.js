$(document).ready(async function () {
    btnSave_Click();
    await setNotaEditedValue();
});

async function setNotaEditedValue(){
    await restoreDropdownValue("#kd_rk_pas", "#temp_kd_rk_pas");
    var flag_posting = $("#tempFlag_posting").val();
    flag_posting == "Y" ? $("#flag_posting").prop("checked", true) : $("#flag_posting").prop("checked", false);
    var flag_cancel = $("#tempFlag_cancel").val();
    flag_cancel == "Y" ? $("#flag_cancel").prop("checked", true) : $("#flag_cancel").prop("checked", false);
}

function btnSave_Click() {
    $('#btn-save-nota').click(function () {
        showProgress('#NotaTreatyMasukWindow');
        setTimeout(function () {
            saveNota('/NotaTreatyMasuk/SaveNota')
        }, 500);
    });
}

function saveNota(url){
    var form = getFormData($('#NotaForm'));
    form.flag_cancel = $("#flag_cancel")[0].checked ? "Y" : "N";
    form.flag_posting = $("#flag_posting")[0].checked ? "Y" : "N";
    
    ajaxPost(url, JSON.stringify(form),
        function (response) {
            refreshGrid("#NotaTreatyMasukGrid");
            if (response.Result == "OK") {
                showMessage('Success', response.Message);
                closeWindow("#NotaTreatyMasukWindow")
            }
            else if (response.Result == "ERROR") {
                showMessage('Error', response.Message);
            }
            else {
                $("#NotaTreatyMasukWindow").html(response);
            }

            closeProgress("#NotaTreatyMasukWindow");
        }
    );
}

function dataKodePasDropDown(){
    return {
        kd_grp_pas: $("#kd_grp_pas").val().trim(),
        kd_cb: $("#kd_cb").val().trim()
    }
}

function dataRekananSorDropDown(){
    return {
        kd_jns_sor: $("#kd_jns_sor").val().trim(),
        kd_cb: $("#kd_cb").val().trim()
    }
}