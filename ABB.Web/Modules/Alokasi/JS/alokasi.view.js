
$(document).ready(function () {
    $('#AlokasiTab').kendoTabStrip();
    
    var tabstrip = $('#AlokasiTab').data("kendoTabStrip");
    tabstrip.select(0)

    setTimeout(() => {
        $("#AlokasiGrid").data("kendoGrid").hideColumn(0);
        $("#DetailAlokasiGrid").data("kendoGrid").hideColumn(0);
    }, 3000);
});