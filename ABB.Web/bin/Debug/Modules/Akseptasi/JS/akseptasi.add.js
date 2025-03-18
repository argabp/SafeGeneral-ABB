$(document).ready(function () {
    var tabstrip = $('#akseptasiTab').data("kendoTabStrip");
    tabstrip.disable(tabstrip.items()[1]);
    tabstrip.disable(tabstrip.items()[2]);

    $("#btn-next-akseptasi").prop("disabled", true);
    $("#btn-next-akseptasiResiko").prop("disabled", true);
});
