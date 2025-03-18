$(document).ready(function () {
    previewRealisasiKlaimUmumPerKantor();
});

function previewRealisasiKlaimUmumPerKantor(){
    $('#btn-preview').click(function () {
        var periode = $("#periode").val();
        window.open(`ReportRealisasiKlaimUmumPerKantor?&date=${periode}`);
    });
}