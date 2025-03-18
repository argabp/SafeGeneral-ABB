$(document).ready(function () {
    previewRealisasiCabangDanPemasaran();
});

function previewRealisasiCabangDanPemasaran(){
    $('#btn-preview').click(function () {
        var kd_cb = $("#kd_cb").val();
        var periode = $("#periode").val();
        window.open(`ReportRealisasiCabangDanPemasaran?kd_cb=${kd_cb.trim()}&date=${periode}`);
    });
}