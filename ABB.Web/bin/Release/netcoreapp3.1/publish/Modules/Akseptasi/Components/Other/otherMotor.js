$(document).ready(function () {
    $('#resikoOtherTab').kendoTabStrip();

    var tabstrip = $('#resikoOtherTab').data("kendoTabStrip");
    tabstrip.select(0);

    btnPreviousAkseptasiResikoOther();
});

function btnPreviousAkseptasiResikoOther(){
    $('#btn-previous-akseptasiResikoOther').click(function () {
        $("#resikoTab").getKendoTabStrip().select(2);
    });
}