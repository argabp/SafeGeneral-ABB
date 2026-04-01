$(document).ready(function () {
    btnClosing();
});

function btnClosing(){
    $('#btn-closing-mutasiKlaim').click(function () {
        showProgress('#ClosingMutasiKlaimiWindow');
        setTimeout(function () {
            closingMutasiKlaim();
        }, 500);
    });
}

function closingMutasiKlaim(){
    var form = {};

    form.kd_cb = $("#kd_cb_mutasi_klaim").val();
    form.kd_cob = $("#kd_cob_mutasi_klaim").val();
    form.kd_scob = $("#kd_scob_mutasi_klaim").val();
    form.kd_thn = $("#kd_thn_mutasi_klaim").val();
    form.no_kl = $("#no_kl_mutasi_klaim").val();
    form.no_mts = $("#no_mts_mutasi_klaim").val();
    form.tgl_closing = kendo.toString($("#tgl_closing_mutasi_klaim").val(), "MM/dd/yyyy");

    var data = JSON.stringify(form);
    ajaxPost(`/KlaimAlokasiReasuransi/ClosingKlaimAlokasiReasuransi`, data,  function (response) {
        if (response.Result === "OK") {
            showMessage('Closing Successfully', response.Message);
        }
        else {
            showMessage('Error', 'Clossing is failed, ' + response.Message);
        };

        refreshGrid("#MutasiKlaimGrid");
        closeProgressOnGrid('#MutasiKlaimGrid');
        closeWindow("#ClosingMutasiKlaimiWindow");
    });
}