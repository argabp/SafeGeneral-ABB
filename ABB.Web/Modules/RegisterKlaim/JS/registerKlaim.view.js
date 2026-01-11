
$(document).ready(function () {
    $('#RegisterKlaimTab').kendoTabStrip();

    var tabstrip = $('#RegisterKlaimTab').data("kendoTabStrip");
    tabstrip.select(0);
    
    setTimeout(() => {
        $(".k-grid-add").hide();
        $("#DokumenRegisterKlaimGrid").data("kendoGrid").hideColumn(0); // hide first column
        $("#DokumenRegisterKlaimGrid").data("kendoGrid").hideColumn(1); // hide second column
        $("#btn-save-registerKlaim").hide();
        $("#btn-save-analisaDanEvaluasi").hide();
    }, 3000);
});