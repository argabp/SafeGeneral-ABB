$(document).ready(function () {
    var dataItem;
});

function OnClickPrintSertifikat(e){
    e.preventDefault();
    dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    console.log('dataItem', dataItem);
    window.open(`ReportSertifikat?input_str=${dataItem.kd_cb.trim()},${dataItem.kd_product.trim()},${dataItem.kd_thn},${dataItem.kd_rk},${dataItem.no_sppa},${dataItem.no_updt}`);
}

function OnClickInfo(e) {
    e.preventDefault();
    dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    console.log('dataItem', dataItem);
    openWindow('#InfoWindow',`/ProsesRekonsiliasi/Info`, 'Info');
}


function dataInfo() {
    return {
        kd_cb: dataItem.kd_cb,
        kd_product: dataItem.kd_product,
        kd_thn: dataItem.kd_thn,
        kd_rk: dataItem.kd_rk,
        no_sppa: dataItem.no_sppa,
        no_updt: dataItem.no_updt,
    };
}