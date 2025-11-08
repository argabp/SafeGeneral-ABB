
$(document).ready(function () {
    $('#RegisterKlaimTab').kendoTabStrip();

    var tabstrip = $('#RegisterKlaimTab').data("kendoTabStrip");
    tabstrip.disable(tabstrip.items()[1]);
    tabstrip.disable(tabstrip.items()[2]);

    $("#btn-next-registerKlaim").prop("disabled", true);
});