$(document).ready(function () {
    previewRealisasiKomisiUmumPerKantor();
});

function previewRealisasiKomisiUmumPerKantor(){
    $('#btn-preview').click(function () {
        var periode = $("#periode").val();
        window.open(`ReportRealisasiKomisiUmumPerKantor?&date=${periode}`);
    });
}