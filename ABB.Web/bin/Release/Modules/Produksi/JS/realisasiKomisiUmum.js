$(document).ready(function () {
    previewRealisasiKomisiUmum();
});

function previewRealisasiKomisiUmum(){
    $('#btn-preview').click(function () {
        var kd_cb = $("#kd_cb").val();
        var periode = $("#periode").val();
        window.open(`ReportRealisasiKomisiUmum?kd_cb=${kd_cb.trim()}&date=${periode}`);
    });
}