$(document).ready(function () {
    btnPreviousOther();
});

function btnPreviousOther(){
    $('#btn-previous-inquiryResikoOther').click(function () {
        $("#resikoTab").getKendoTabStrip().select(2);
    });
}

