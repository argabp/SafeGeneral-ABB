$(document).ready(function () {
    previewRealisasiKlaimUmumPerCob();
});

function previewRealisasiKlaimUmumPerCob(){
    $('#btn-preview').click(function () {
        var periode = $("#periode").val();
        window.open(`ReportRealisasiKlaimUmumPerCob?&date=${periode}`);
    });
}