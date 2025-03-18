$(document).ready(function () {
    searchKeyword_OnKeyUp();
    var dataItem;
});

function searchFilterTracking() {
    return {
        searchkeyword: $("#SearchKeyword").val(),
        kd_status: "15"
    };
}

function searchKeyword_OnKeyUp() {
    $('#SearchKeyword').keyup(function () {
        refreshGrid("#ApprovedApprovalGrid");
    });
}

function OnClickViewApproval(e) {
    e.preventDefault();
    dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    console.log('dataItem', dataItem);
    openWindow('#PesertaWindow', `/PengajuanAkseptasi/View?kd_cb=${dataItem.kd_cb}&kd_product=${dataItem.kd_product}&kd_thn=${dataItem.kd_thn}&kd_rk=${dataItem.kd_rk}&no_sppa=${dataItem.no_sppa}&no_updt=${dataItem.no_updt}`, 'View');
}

function OnClickInfoApproval(e) {
    e.preventDefault();
    dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    console.log('dataItem', dataItem);
    openWindow('#InfoApprovalWindow',`/PengajuanAkseptasi/Info`, 'Info');
}

function OnClickReceivedApproval(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));

    showConfirmation('Received', `Are you sure you want to received data ${dataItem.nomor_sppa}?`,
        function () {
            showProgressOnGrid('#ApprovedApprovalGrid');
            acceptApproval(dataItem);
        }
    );
}


function OnClickCancelApproval(e) {
    e.preventDefault();
    dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    console.log('dataItem', dataItem);
    openWindow('#CancelApprovalWindow', `/PengajuanAkseptasi/CancelPopUp`, 'Cancel');
}

function acceptApproval(dataItem){
    var url = "/PengajuanAkseptasi/AcceptApproval";
    var data = {}

    data.kd_cb = dataItem.kd_cb;
    data.kd_product = dataItem.kd_product;
    data.no_sppa = dataItem.no_sppa;
    data.kd_rk = dataItem.kd_rk;
    data.kd_thn = dataItem.kd_thn;
    data.no_updt = dataItem.no_updt;
    data.kd_user_status = dataItem.kd_user_status;
    data.kd_action = 5;
    data.kd_approval = dataItem.kd_approval;
    data.keterangan = "Persetujuan Akseptasi Pihak Asuransi sudah kami terima";

    var json = JSON.stringify(data);

    ajaxPost(url, json,
        function (response) {
            if (response.Result == "OK") {
                showMessage('Success', response.Message);
                refreshGrid("#ApprovedApprovalGrid");
            }
            else
                showMessage('Error', response.Message);

            closeProgressOnGrid('#ApprovedApprovalGrid');
        }
    );
}

function exportToExcel(e){
    e.workbook.fileName = "Pengajuan Akseptasi Approved.xlsx";
}