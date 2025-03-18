$(document).ready(function () {
    searchKeyword_OnKeyUp();
    var dataItem;
});

function searchKeyword_OnKeyUp() {
    $('#SearchKeyword').keyup(function () {
        refreshGrid("#PesertaGrid");
    });
}

function OnClickViewApproval(e) {
    e.preventDefault();
    dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    console.log('dataItem', dataItem);
    openWindow('#PesertaWindow', `/Approval/View?kd_cb=${dataItem.kd_cb}&kd_product=${dataItem.kd_product}&kd_thn=${dataItem.kd_thn}&kd_rk=${dataItem.kd_rk}&no_sppa=${dataItem.no_sppa}&no_updt=${dataItem.no_updt}`, 'View');
}

function OnClickInfoApproval(e) {
    e.preventDefault();
    dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    console.log('dataItem', dataItem);
    openWindow('#InfoApprovalWindow',`/Approval/Info`, 'Info');
}

function OnClickProcessApproval(e) {
    e.preventDefault();
    dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    console.log('dataItem', dataItem);
    openWindow('#ProcessApprovalWindow', `/Approval/Process`, 'Process');
}

function OnClickConfirmationApproval(e) {
    e.preventDefault();
    dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    console.log('dataItem', dataItem);
    openWindow('#ConfirmationApprovalWindow', `/Approval/Confirmation`, 'Confirmation');
}

function OnClickRejectedApproval(e) {
    e.preventDefault();
    dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    console.log('dataItem', dataItem);
    openWindow('#RejectedApprovalWindow', `/Approval/Rejected`, 'Rejected');
}

function OnClickApprovedApproval(e) {
    e.preventDefault();
    dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    console.log('dataItem', dataItem);
    openWindow('#ApprovedApprovalWindow', `/Approval/Approved`, 'Approved');
}

function setButtonByRoleName(e){
    var roleName = $("#roleName").val();
    
    if (roleName == "Backup Jiwa")
        $(".k-grid-Process").hide();
    else if (roleName == "Branch Office" || roleName == "Head Office")
        $(".k-grid-Approved").hide();
}

function OnClickDataBackup(e) {
    e.preventDefault();
    dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    console.log('dataItem', dataItem);
    openWindow('#DataBackupWindow',`/Tracking/DataBackup?kd_cb=${dataItem.kd_cb}&kd_product=${dataItem.kd_product}&kd_thn=${dataItem.kd_thn}&kd_rk=${dataItem.kd_rk}&no_sppa=${dataItem.no_sppa}&no_updt=${dataItem.no_updt}`, 'Data Backup');
}