$(document).ready(function () {
    previewRealisasiKlaimUmum();
});

function previewRealisasiKlaimUmum(){
    $('#btn-preview').click(function () {
        var kd_cb = $("#kd_cb").val();
        var periode = $("#periode").val();
        window.open(`ReportRealisasiKlaimUmum?kd_cb=${kd_cb.trim()}&date=${periode}`);
    });
}