$(document).ready(function () {
    $('#resikoOtherTab').kendoTabStrip();

    var tabstrip = $('#resikoOtherTab').data("kendoTabStrip");
    tabstrip.select(0);

    btnPreviousInquiryResikoOther();
});

function btnPreviousInquiryResikoOther(){
    $('#btn-previous-inquiryResikoOther').click(function () {
        $("#resikoTab").getKendoTabStrip().select(2);
    });
}