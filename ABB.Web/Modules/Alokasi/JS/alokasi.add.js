
$(document).ready(function () {
    $('#AlokasiTab').kendoTabStrip();

    var tabstrip = $('#AlokasiTab').data("kendoTabStrip");
    tabstrip.disable(tabstrip.items()[1]);
    tabstrip.select(0);

    $("#btn-next-alokasiKlaim").prop("disabled", true);
});