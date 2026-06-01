$(document).ready(function () {
    btnNextResikoOtherCargo();
});

function btnNextResikoOtherCargo(){
    $('#btn-next-akseptasiResikoOtherCargo').click(function () {
        $("#resikoOtherTab").getKendoTabStrip().select(1);
    });
}
