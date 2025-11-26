$(document).ready(function () {
    $('#pranotaTab').kendoTabStrip();

    var tabstrip = $('#pranotaTab').data("kendoTabStrip");
    tabstrip.select(0);

    btnPreviousPranota();
});

function btnPreviousPranota(){
    $('#btn-previous-inquiryPranotaKoas').click(function () {
        $("#pranotaTab").getKendoTabStrip().select(0);
    });
}