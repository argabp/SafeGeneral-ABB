
$(document).ready(function () {
    var tabstrip = $('#pesertaTab').data("kendoTabStrip");
    
    if($("#nilai_ptg").val() == 0){
        tabstrip.disable(tabstrip.items()[2]);
        tabstrip.disable(tabstrip.items()[3]);

        $("#btn-next-pesertaSirama").prop("disabled", true);
    } else if($("#berat_badan").val() == 0){
        tabstrip.disable(tabstrip.items()[3]);

        $("#btn-next-pesertaInfoKesehatan").prop("disabled", true);    
    }
});