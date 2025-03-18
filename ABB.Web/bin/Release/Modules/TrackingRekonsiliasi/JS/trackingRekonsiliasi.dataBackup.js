
$(document).ready(function () {
    $('#dataBackupTab').kendoTabStrip();

    var tabstrip = $('#dataBackupTab').data("kendoTabStrip");
    tabstrip.select(0);
});