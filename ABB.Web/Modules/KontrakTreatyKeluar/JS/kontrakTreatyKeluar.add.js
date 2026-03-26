$(document).ready(function () {
    var tabstrip = $('#kontrakTreatyKeluarTab').data("kendoTabStrip");
    tabstrip.disable(tabstrip.items()[1]);
    tabstrip.disable(tabstrip.items()[2]);
    tabstrip.disable(tabstrip.items()[3]);
    tabstrip.disable(tabstrip.items()[4]);
    tabstrip.disable(tabstrip.items()[5]);

    $("#btn-next-kontrakTreatyKeluar").prop("disabled", true);
});
