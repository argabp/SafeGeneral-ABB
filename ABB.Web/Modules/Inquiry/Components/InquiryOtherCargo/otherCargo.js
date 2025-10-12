$(document).ready(function () {
    btnNextResikoOtherCargo();
});

function btnNextResikoOtherCargo(){
    $('#btn-next-inquiryResikoOtherCargo').click(function () {
        $("#resikoOtherTab").getKendoTabStrip().select(1);
    });
}
