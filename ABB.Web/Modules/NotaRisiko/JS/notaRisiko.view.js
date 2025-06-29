$(document).ready(function () {
    $('#notaRisikoTab').kendoTabStrip();

    var tabstrip = $('#notaRisikoTab').data("kendoTabStrip");
    tabstrip.select(0);
});