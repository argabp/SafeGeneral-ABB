$(document).ready(function () {
    $('#kontrakTreatyKeluarTab').kendoTabStrip();

    var tabstrip = $('#kontrakTreatyKeluarTab').data("kendoTabStrip");
    tabstrip.select(0);
});