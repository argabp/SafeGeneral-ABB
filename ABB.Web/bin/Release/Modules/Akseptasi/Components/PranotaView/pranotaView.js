$(document).ready(function () {
    $('#pranotaTab').kendoTabStrip();

    var tabstrip = $('#pranotaTab').data("kendoTabStrip");
    tabstrip.select(0);

    btnPreviousPranota();
});


function btnPreviousPranota(){
    $('#btn-previous-akseptasiPranotaKoas').click(function () {
        $("#pranotaTab").getKendoTabStrip().select(0);
    });
}