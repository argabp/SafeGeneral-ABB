$(document).ready(function () {
    previewReasuransiKlaimUmum();
});

function previewReasuransiKlaimUmum(){
    $('#btn-preview').click(function () {
        var kd_cb = $("#kd_cb").val();
        var periode = $("#periode").val();
        window.open(`ReportReasuransiKlaimUmum?kd_cb=${kd_cb.trim()}&date=${periode}`);
    });
}