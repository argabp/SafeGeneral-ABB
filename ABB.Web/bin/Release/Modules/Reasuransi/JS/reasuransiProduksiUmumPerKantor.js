$(document).ready(function () {
    previewReasuransiProduksiUmumPerKantor();
});

function previewReasuransiProduksiUmumPerKantor(){
    $('#btn-preview').click(function () {
        var periode = $("#periode").val();
        window.open(`ReportReasuransiProduksiUmumPerKantor?&date=${periode}`);
    });
}