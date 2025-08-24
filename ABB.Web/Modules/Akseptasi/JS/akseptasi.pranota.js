$(document).ready(function () {
    btnSaveAkseptasiPranota_Click();
    btnRecalculatePremi_Click();
});

function btnSaveAkseptasiPranota_Click() {
    $('#btn-save-akseptasiPranota').click(function () {
        showProgress('#AkseptasiPranotaWindow');
        setTimeout(function () {
            saveAkseptasiPranota('/Akseptasi/SaveAkseptasiPranota')
        }, 500);
    });
}

function saveAkseptasiPranota(url) {
    var form = getFormData($('#PranotaForm'));
    form.kd_cb = $("#kd_cb").val();
    form.kd_cob = $("#kd_cob").val();
    form.kd_scob = $("#kd_scob").val();
    form.kd_thn = $("#kd_thn").val();
    form.no_aks = $("#no_aks").val();
    form.no_updt = $("#pranota_no_updt").val();

    var data = JSON.stringify(form);

    ajaxPost(url, data,
        function (response) {
            if (response.Result == "OK") {
                showMessage('Success', response.Message);
                closeWindow('#AkseptasiPranotaWindow');
            }
            else if (response.Result == "ERROR")
                showMessage("Error", response.Message);
            else
                showMessage("Error", response);

            closeProgress('#AkseptasiPranotaWindow');
        }
    );
}