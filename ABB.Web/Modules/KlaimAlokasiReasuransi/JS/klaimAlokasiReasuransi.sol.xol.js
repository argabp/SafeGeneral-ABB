$(document).ready(function () {
    btnSaveKlaimAlokasiReasuransiXL_Click();
});

function btnSaveKlaimAlokasiReasuransiXL_Click() {
    $('#btn-save-klaimAlokasiReasuransiXL').click(function () {
        showProgress('#KlaimAlokasiReasuransiWindow');
        setTimeout(function () {
            saveKlaimAlokasiReasuransiXL('/KlaimAlokasiReasuransi/SaveSOLXOL')
        }, 500);
    });
}

function saveKlaimAlokasiReasuransiXL(url) {
    var form = getFormData($('#KlaimAlokasiReasuransiXLForm'));
    
    var data = JSON.stringify(form);
    
    ajaxPost(url, data,
        function (response) {
            
            var parentId =
                form.kd_cb.trim() + "-" +
                form.kd_cob.trim() + "-" +
                form.kd_scob.trim() + "-" +
                form.kd_thn.trim() + "-" +
                form.no_kl.trim() + "-" +
                form.no_mts.trim();

            refreshGrid('#grid_sol_xol_' + parentId);
            if (response.Result == "OK") {
                showMessage('Success', response.Message);
                closeWindow('#KlaimAlokasiReasuransiWindow');
            }
            else if (response.Result == "ERROR")
                showMessage("Error", response.Message);
            else
                showMessage("Error", response);

            closeProgress('#KlaimAlokasiReasuransiWindow');
        }
    );
}

function dataKontrakDropDown(){
    return {
        kd_cb: $("#kd_cb").val().trim(),
        kd_cob: $("#kd_cob").val().trim()
    }
}