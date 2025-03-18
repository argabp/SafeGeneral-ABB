
function lookUpData(){
    return {
        kd_lookup: "14"
    }
}
function onLookUpChange(e){
    var keterangan = $("#LookUpGrid").data("kendoGrid").dataItem(e.sender.select()).Keterangan;
    closeWindow("#LookUpWindow");
    $("#keteranganCancelPopUp").val(keterangan);
}

