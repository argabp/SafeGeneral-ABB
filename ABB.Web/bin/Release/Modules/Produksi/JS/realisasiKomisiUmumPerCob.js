$(document).ready(function () {
    previewRealisasiKomisiUmumPerCob();
});

function previewRealisasiKomisiUmumPerCob(){
    $('#btn-preview').click(function () {
        var periode = $("#periode").val();
        window.open(`ReportRealisasiKomisiUmumPerCob?&date=${periode}`);
    });
}