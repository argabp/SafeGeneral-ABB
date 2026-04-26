
$(document).ready(function () {
    $('#AlokasiTab').kendoTabStrip();
    
    var tabstrip = $('#AlokasiTab').data("kendoTabStrip");
    tabstrip.select(0);
});