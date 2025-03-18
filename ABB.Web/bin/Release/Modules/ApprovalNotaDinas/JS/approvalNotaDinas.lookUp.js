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
        kd_lookup: kodeLookUp,
        searchKeyword: $("#SearchKeywordLookup").val()
    }
}
function onLookUpChange(e){
    var keterangan = $("#LookUpGrid").data("kendoGrid").dataItem(e.sender.select()).Keterangan;
    closeWindow("#LookUpWindow");
    $(keteranganTextArea).val(keterangan);
}

