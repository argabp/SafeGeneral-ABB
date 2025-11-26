function OnGridLokasiResikoChange(e){
    var grid = e.sender;
    var dataLokasiResiko = grid.dataItem(this.select());

    $("#kd_lok_rsk").getKendoTextBox().value(dataLokasiResiko.kd_lok_rsk);
    $("#kd_lok_rsk").getKendoTextBox().trigger("change");
    
    closeWindow("#LokasiResikoWindow");
}