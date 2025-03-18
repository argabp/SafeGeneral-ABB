
$(document).ready(function () {
    var tabstrip = $('#pesertaTab').data("kendoTabStrip");
    tabstrip.disable(tabstrip.items()[1]);
    tabstrip.disable(tabstrip.items()[2]);
    tabstrip.disable(tabstrip.items()[3]);

    $("#btn-next-peserta").prop("disabled", true);
    $("#btn-next-pesertaSirama").prop("disabled", true);
    $("#btn-next-pesertaInfoKesehatan").prop("disabled", true);
});