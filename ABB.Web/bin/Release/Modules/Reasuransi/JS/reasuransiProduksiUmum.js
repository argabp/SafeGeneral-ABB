$(document).ready(function () {
    previewReasuransiProduksiUmum();
});

function previewReasuransiProduksiUmum(){
    $('#btn-preview').click(function () {
        var kd_cb = $("#kd_cb").val();
        var periode = $("#periode").val();
        window.open(`ReportReasuransiProduksiUmum?kd_cb=${kd_cb.trim()}&date=${periode}`);
    });
}