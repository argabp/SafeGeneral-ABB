$(document).ready(function () {
    var tabstrip = $('#inquiryTab').data("kendoTabStrip");
    tabstrip.disable(tabstrip.items()[1]);
    tabstrip.disable(tabstrip.items()[2]);

    $("#btn-next-inquiry").prop("disabled", true);
    $("#btn-next-inquiryResiko").prop("disabled", true);
});
