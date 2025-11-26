
$(document).ready(function () {
    $('#RegisterKlaimTab').kendoTabStrip();

    var tabstrip = $('#RegisterKlaimTab').data("kendoTabStrip");
    tabstrip.disable(tabstrip.items()[1]);
    tabstrip.disable(tabstrip.items()[2]);
    tabstrip.select(0);

    $("#btn-next-registerKlaim").prop("disabled", true);
});