$(document).ready(function () {
    $('#inquiryTab').kendoTabStrip();

    var tabstrip = $('#inquiryTab').data("kendoTabStrip");
    tabstrip.select(0);
});
