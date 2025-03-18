
$(document).ready(function () {
    var tabstrip = $('#rekananTab').data("kendoTabStrip");
    tabstrip.disable(tabstrip.items()[1]);
    tabstrip.disable(tabstrip.items()[2]);
    tabstrip.disable(tabstrip.items()[3]);

    $("#btn-next-rekanan").prop("disabled", true);
});