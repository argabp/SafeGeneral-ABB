$(document).ready(function () {
    searchKeyword_OnKeyUp();
    var dataItem;
});

function searchFilterTracking() {
    return {
        searchkeyword: $("#SearchKeyword").val(),
        kd_status: "6"
    };
}

function searchKeyword_OnKeyUp() {
    $('#SearchKeyword').keyup(function () {
        refreshGrid("#ConfirmationApprovalGrid");
    });
}

function OnClickEditApproval(e) {
    e.preventDefault();
    dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    console.log('dataItem', dataItem);
    openWindow('#PesertaWindow', `/Peserta/Edit?kd_cb=${dataItem.kd_cb}&kd_product=${dataItem.kd_product}&kd_thn=${dataItem.kd_thn}&kd_rk=${dataItem.kd_rk}&no_sppa=${dataItem.no_sppa}&no_updt=${dataItem.no_updt}`, 'Edit');
}

function OnClickInfoApproval(e) {
    e.preventDefault();
    dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    console.log('dataItem', dataItem);
    openWindow('#InfoApprovalWindow',`/PengajuanAkseptasi/Info`, 'Info');
}

function OnClickProcessApproval(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));

    showConfirmation('Process', `Are you sure you want to process data ${dataItem.nomor_sppa}?`,
        function () {
            showProgressOnGrid('#ConfirmationApprovalGrid');
            processApproval(dataItem);
        }
    );
}

function OnClickCancelApproval(e) {
    e.preventDefault();
    dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    console.log('dataItem', dataItem);
    openWindow('#CancelApprovalWindow', `/PengajuanAkseptasi/CancelPopUp`, 'Cancel');
}

function processApproval(dataItem){
    var url = "/Peserta/Process";
    var data = {}

    data.kd_cb = dataItem.kd_cb;
    data.kd_product = dataItem.kd_product;
    data.no_sppa = dataItem.no_sppa;
    data.kd_rk = dataItem.kd_rk;
    data.kd_thn = dataItem.kd_thn;
    data.no_updt = dataItem.no_updt;
    data.kd_user_status = dataItem.kd_user_status;
    data.kd_approval = dataItem.kd_approval;
    data.kd_action = 1;
    data.keterangan = "Pengajuan Akseptasi sudah kami revisi, Mohon Persetujuan Pihak Asuransi"

    var json = JSON.stringify(data);

    ajaxPost(url, json,
        function (response) {
            $('#pesertaLampiranDS').val(JSON.stringify(response));
            if (response.Result == "OK") {
                showMessage('Success', response.Message);
                refreshGrid("#ConfirmationApprovalGrid");
            }
            else
                showMessage('Error', response.Message);

            closeProgressOnGrid('#ConfirmationApprovalGrid');
        }
    );
}

function exportToExcel(e){
    e.workbook.fileName = "Pengajuan Akseptasi Confirmation.xlsx";
}