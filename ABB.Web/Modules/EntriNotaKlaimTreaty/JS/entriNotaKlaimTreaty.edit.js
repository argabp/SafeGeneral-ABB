$(document).ready(async function () {
    btnSaveEntriNotaKlaimTreaty_Click();
    await setEntriNotaKlaimTreatyEditedValue();
});

async function setEntriNotaKlaimTreatyEditedValue(){
    showProgress('#EntriNotaKlaimTreatyWindow');
    var flag_posting = $("#tempFlag_posting").val();
    flag_posting == "Y" ? $("#flag_posting").prop("checked", true) : $("#flag_posting").prop("checked", false);
    var flag_cancel = $("#tempFlag_cancel").val();
    flag_cancel == "Y" ? $("#flag_cancel").prop("checked", true) : $("#flag_cancel").prop("checked", false);

    await restoreDropdownValue("#kd_rk_pas", "#temp_kd_rk_pas");

    closeProgress('#EntriNotaKlaimTreatyWindow');
}

function btnSaveEntriNotaKlaimTreaty_Click() {
    $('#btn-save-entriNotaKlaimTreaty').click(function () {
        showProgress('#EntriNotaKlaimTreatyWindow');
        setTimeout(function () {
            saveEntriNotaKlaimTreaty('/EntriNotaKlaimTreaty/SaveEntriNotaKlaimTreaty')
        }, 500);
    });
}


function saveEntriNotaKlaimTreaty(url){
    var form = getFormData($('#EntriNotaKlaimTreatyForm'));
    form.flag_posting = $("#flag_posting")[0].checked ? "Y" : "N";
    form.flag_cancel = $("#flag_cancel")[0].checked ? "Y" : "N";
    
    ajaxPost(url, JSON.stringify(form),
        function (response) {
            refreshGrid("#EntriNotaKlaimTreatyGrid");
            if (response.Result == "OK") {
                showMessage('Success', response.Message);
            } else
                showMessage('Error', response.Message);

            closeProgress("#EntriNotaKlaimTreatyWindow");
            closeWindow("#EntriNotaKlaimTreatyWindow")
        }
    );
}
