$(document).ready(function () {
    previewRealisasiProduksiUmumPerKantor();
});

function previewRealisasiProduksiUmumPerKantor(){
    $('#btn-preview').click(function () {
        var periode = $("#periode").val();
        window.open(`ReportRealisasiProduksiUmumPerKantor?&date=${periode}`);
    });
}