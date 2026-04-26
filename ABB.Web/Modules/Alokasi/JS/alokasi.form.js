$(document).ready(function () {
    btnSaveAlokasi_Click();

    if($("#IsViewOnly").val() == "True"){
        $("#div-save-alokasi").hide();
    }
});

function btnSaveAlokasi_Click() {
    $('#btn-save-alokasi').click(function () {
        showProgress('#FormAlokasiWindow');
        setTimeout(function () {
            saveAlokasi('/Alokasi/SaveAlokasi')
        }, 500);
    });
}

function saveAlokasi(url) {
    var form = getFormData($('#AlokasiForm'));
    form.kd_cb = sorData.kd_cb;
    form.kd_cob = sorData.kd_cob;
    form.kd_scob = sorData.kd_scob;
    form.kd_thn = sorData.kd_thn;
    form.no_pol = sorData.no_pol;
    form.no_updt = sorData.no_updt;
    form.flag_closing = sorData?.flag_closing;
    form.tgl_closing = sorData?.tgl_closing_reas;

    var data = JSON.stringify(form);

    ajaxPost(url, data,
        function (response) {
            refreshGrid("#AlokasiGrid");
            if (response.Result == "OK") {
                showMessage('Success', response.Message);
                closeWindow('#FormAlokasiWindow');
            }
            else if (response.Result == "ERROR")
                showMessage("Error", response.Message);
            else
                showMessage("Error", response);

            closeProgress('#FormAlokasiWindow');
        }
    );
}
