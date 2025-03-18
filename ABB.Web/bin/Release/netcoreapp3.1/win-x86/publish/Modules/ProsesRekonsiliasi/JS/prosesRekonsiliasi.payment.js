$(document).ready(function () {
    var dataItem;
});

function OnClickPrintSertifikat(e){
    e.preventDefault();
    dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    console.log('dataItem', dataItem);
    window.open(`ReportSertifikat?input_str=${dataItem.kd_cb.trim()},${dataItem.kd_product.trim()},${dataItem.kd_thn},${dataItem.kd_rk},${dataItem.no_sppa},${dataItem.no_updt}`);
}