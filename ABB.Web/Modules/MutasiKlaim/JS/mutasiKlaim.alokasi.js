$(document).ready(function () {
    btnSaveMutasiKlaimAlokasi_Click();

    if($("#IsEdit").val() === 'True')
    {
        $("#kd_grp_pas").getKendoDropDownList().readonly(true);
        $("#kd_rk_pas").getKendoDropDownList().readonly(true);
    }
});

function btnSaveMutasiKlaimAlokasi_Click() {
    $('#btn-save-mutasiKlaimAlokasi').click(function () {
        showProgress('#MutasiKlaimWindow');
        setTimeout(function () {
            saveMutasiKlaimAlokasi('/MutasiKlaim/SaveMutasiKlaimAlokasi')
        }, 500);
    });
}

function saveMutasiKlaimAlokasi(url) {
    var form = getFormData($('#MutasiKlaimAlokasiForm'));
    
    var parentId =
        form.kd_cb.trim() +
        form.kd_cob.trim() +
        form.kd_scob.trim() +
        form.kd_thn.trim() +
        form.no_kl.trim() 

    var mutasiGridName = "grid_alokasi_" + parentId;
    var mutasiGridElement = $("#" + mutasiGridName);
    
    var data = JSON.stringify(form);
    ajaxPost(url,  data,
        function (response) {
            refreshGrid(mutasiGridElement);
            if (response.Result == "OK") {
                showMessage('Success', response.Message);
            }
            else if (response.Result == "ERROR")
                showMessage("Error", response.Message);
            else
                showMessage("Error", response);

            closeProgress('#MutasiKlaimWindow');
            closeWindow('#MutasiKlaimWindow')
        }
    );
}

function dataRekananDropDown(){
    return {
        kd_grp_pas: $("#kd_grp_pas").val().trim()
    }
}

function OnKodeRekananChange(e){
    var kd_rk_pas = $("#kd_rk_pas").data("kendoDropDownList");
    kd_rk_pas.dataSource.read({kd_grp_pas : e.sender._cascadedValue});
}