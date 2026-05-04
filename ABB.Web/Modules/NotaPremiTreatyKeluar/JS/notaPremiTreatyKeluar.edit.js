$(document).ready(async function () {
    btnSaveNotaPremiTreatyKeluar_Click();
    await setNotaPremiTreatyKeluarEditedValue();
});

async function setNotaPremiTreatyKeluarEditedValue(){
    showProgress('#NotaPremiTreatyKeluarWindow');
    var flag_posting = $("#tempFlag_posting").val();
    flag_posting == "Y" ? $("#flag_posting").prop("checked", true) : $("#flag_posting").prop("checked", false);
    var flag_cancel = $("#tempFlag_cancel").val();
    flag_cancel == "Y" ? $("#flag_cancel").prop("checked", true) : $("#flag_cancel").prop("checked", false);

    await restoreDropdownValue("#kd_rk_pas", "#temp_kd_rk_pas");
    await restoreDropdownValue("#kd_rk_tty", "#temp_kd_rk_tty");

    closeProgress('#NotaPremiTreatyKeluarWindow');
}

function btnSaveNotaPremiTreatyKeluar_Click() {
    $('#btn-save-notaPremiTreatyKeluar').click(function () {
        showProgress('#NotaPremiTreatyKeluarWindow');
        setTimeout(function () {
            saveNotaPremiTreatyKeluar('/NotaPremiTreatyKeluar/SaveNotaPremiTreatyKeluar')
        }, 500);
    });
}


function saveNotaPremiTreatyKeluar(url){
    var form = getFormData($('#NotaPremiTreatyKeluarForm'));
    form.flag_posting = $("#flag_posting")[0].checked ? "Y" : "N";
    form.flag_cancel = $("#flag_cancel")[0].checked ? "Y" : "N";
    
    ajaxPost(url, JSON.stringify(form),
        function (response) {
            refreshGrid("#NotaPremiTreatyKeluarGrid");
            if (response.Result == "OK") {
                showMessage('Success', response.Message);
            } else
                showMessage('Error', response.Message);

            closeProgress("#NotaPremiTreatyKeluarWindow");
            closeWindow("#NotaPremiTreatyKeluarWindow")
        }
    );
}

