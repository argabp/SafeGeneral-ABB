$(document).ready(function () {
    searchKeyword_OnKeyUp();
    var dataItem;
});

function searchFilterTracking() {
    return {
        searchkeyword: $("#SearchKeyword").val(),
        kd_status: "17"
    };
}

function searchKeyword_OnKeyUp() {
    $('#SearchKeyword').keyup(function () {
        refreshGrid("#CancelApprovalGrid");
    });
}

function OnClickViewApproval(e) {
    e.preventDefault();
    dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    console.log('dataItem', dataItem);
    openWindow('#ViewApprovalWindow', `/Approval/View?kd_cb=${dataItem.kd_cb}&kd_product=${dataItem.kd_product}&kd_thn=${dataItem.kd_thn}&kd_rk=${dataItem.kd_rk}&no_sppa=${dataItem.no_sppa}&no_updt=${dataItem.no_updt}`, 'View');
}

function OnClickInfoApproval(e) {
    e.preventDefault();
    dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    console.log('dataItem', dataItem);
    openWindow('#InfoApprovalWindow',`/PengajuanAkseptasi/Info`, 'Info');
}