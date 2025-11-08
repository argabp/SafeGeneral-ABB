$(document).ready(function () {
    btnSaveAnalisaDanEvaluasi_Click();
    btnPreviousAnalisaDanEvaluasi();
});

function btnPreviousAnalisaDanEvaluasi(){
    $('#btn-previous-analisaDanEvaluasi').click(function () {
        $("#RegisterKlaimTab").getKendoTabStrip().select(1);
    });
}

function btnSaveAnalisaDanEvaluasi_Click() {
    $('#btn-save-analisaDanEvaluasi').click(function () {
        showProgress('#RegisterKlaimWindow');
        setTimeout(function () {
            saveAnalisaDanEvaluasi('/RegisterKlaim/SaveAnalisaDanEvaluasi')
        }, 500);
    });
}

function saveAnalisaDanEvaluasi(url) {
    var form = getFormData($('#AnalisaDanEvaluasiForm'));
    form.kd_cb = $("#kd_cb").val();
    form.kd_cob = $("#kd_cob").val();
    form.kd_scob = $("#kd_scob").val();
    form.kd_thn = $("#kd_thn").val();
    form.no_kl = $("#no_kl").val();
    form.ket_anev = $("#ket_anev").getKendoEditor().value();
    var data = JSON.stringify(form);
    ajaxPost(url,  data,
        function (response) {
            if (response.Result == "OK") {
                showMessage('Success', response.Message);
            }
            else if (response.Result == "ERROR")
                showMessage("Error", response.Message);
            else
                showMessage("Error", response);

            closeProgress('#RegisterKlaimWindow');
        }
    );
}