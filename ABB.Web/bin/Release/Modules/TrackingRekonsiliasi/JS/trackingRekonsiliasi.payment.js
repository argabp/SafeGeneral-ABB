$(document).ready(function () {
    var dataItem;
});

function OnClickPrintSertifikat(e){
    e.preventDefault();
    dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    console.log('dataItem', dataItem);
    window.open(`ReportSertifikat?input_str=${dataItem.kd_cb.trim()},${dataItem.kd_product.trim()},${dataItem.kd_thn},${dataItem.kd_rk},${dataItem.no_sppa},${dataItem.no_updt}`);
}

function OnClickDataBackup(e) {
    e.preventDefault();
    dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    console.log('dataItem', dataItem);
    openWindow('#DataBackupWindow',`/Tracking/DataBackup?kd_cb=${dataItem.kd_cb}&kd_product=${dataItem.kd_product}&kd_thn=${dataItem.kd_thn}&kd_rk=${dataItem.kd_rk}&no_sppa=${dataItem.no_sppa}&no_updt=${dataItem.no_updt}`, 'Data Backup');
}

function setButtonByRoleName(e){
    var roleName = $("#roleName").val();

    if (roleName == "Supervisor")
        $(".k-grid-DataBackup").hide();
}

function exportToExcel(e){
    e.workbook.fileName = "Tracking Rekonsiliasi Payment.xlsx";
}