$(document).ready(function () {
    searchKeywordLookup_OnKeyUp();
});

function searchKeywordLookup_OnKeyUp() {
    $('#SearchKeywordLookup').keyup(function () {
        refreshGrid("#LookUpGrid");
    });
}

function lookUpData(){
    return {
        searchKeyword: $("#SearchKeywordLookup").val()
    }
}

function onLookUpChange(e){
    var lookUpData = $("#LookUpGrid").data("kendoGrid").dataItem(e.sender.select());
    closeWindow("#LookUpWindow");
    $("#kd_cb").val(lookUpData.kd_cb);
    $("#kd_rk").val(lookUpData.kd_rk);
    $("#nm_rk").val(lookUpData.nm_rk);
}

