$(document).ready(function () {
    var dataItem;
});

function OnClickInfo(e) {
    e.preventDefault();
    dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    console.log('dataItem', dataItem);
    openWindow('#InfoWindow',`/TrackingRekonsiliasi/Info`, 'Info');
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

function exportToExcel(e){
    e.workbook.fileName = "Tracking Rekonsiliasi Cancel Payment.xlsx";
}