$(document).ready(function () {
    previewRealisasiProduksiUmumPerCob();
});

function previewRealisasiProduksiUmumPerCob(){
    $('#btn-preview').click(function () {
        var periode = $("#periode").val();
        window.open(`ReportRealisasiProduksiUmumPerCob?&date=${periode}`);
    });
}