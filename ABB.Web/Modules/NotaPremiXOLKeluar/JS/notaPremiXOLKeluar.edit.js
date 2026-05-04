$(document).ready(async function () {
    btnSaveNotaPremiXOLKeluar_Click();
    await setNotaPremiXOLKeluarEditedValue();
});

async function setNotaPremiXOLKeluarEditedValue(){
    showProgress('#NotaPremiXOLKeluarWindow');
    var flag_posting = $("#tempFlag_posting").val();
    flag_posting == "Y" ? $("#flag_posting").prop("checked", true) : $("#flag_posting").prop("checked", false);
    
    await restoreDropdownValue("#kd_rk_sb_bis", "#temp_kd_rk_sb_bis");
    await restoreDropdownValue("#kd_rk_ttj", "#temp_kd_rk_ttj");

    closeProgress('#NotaPremiXOLKeluarWindow');
}

function btnSaveNotaPremiXOLKeluar_Click() {
    $('#btn-save-notaPremiXOLKeluar').click(function () {
        showProgress('#NotaPremiXOLKeluarWindow');
        setTimeout(function () {
            saveNotaPremiXOLKeluar('/NotaPremiXOLKeluar/SaveNotaPremiXOLKeluar')
        }, 500);
    });
}


function saveNotaPremiXOLKeluar(url){
    var form = getFormData($('#NotaPremiXOLKeluarForm'));
    
    ajaxPost(url, JSON.stringify(form),
        function (response) {
            refreshGrid("#NotaPremiXOLKeluarGrid");
            if (response.Result == "OK") {
                showMessage('Success', response.Message);
            } else
                showMessage('Error', response.Message);

            closeProgress("#NotaPremiXOLKeluarWindow");
            closeWindow("#NotaPremiXOLKeluarWindow")
        }
    );
}

function dataKodeTertujuDropDown(){
    return {
        kd_grp_rk: $("#kd_grp_ttj").val().trim(),
        kd_cb: $("#kd_cb").val().trim()
    }
}

function dataKodeSumberBisnisDropDown(){
    return {
        kd_grp_sb_bis: $("#kd_grp_sb_bis").val().trim(),
        kd_cb: $("#kd_cb").val().trim()
    }
}