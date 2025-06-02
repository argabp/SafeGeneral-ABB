$(document).ready(function () {
    searchKeyword_OnKeyUp();
    var dataItem;
    btnAddPengajuanAkseptasi_Click();
});

function searchKeyword_OnKeyUp() {
    $('#SearchKeyword').keyup(function () {
        refreshGrid("#PengajuanAkseptasiGrid");
    });
}

function openPengajuanAkseptasiWindow(url, title) {
    openWindow('#PengajuanAkseptasiWindow', url, title);
}

function btnAddPengajuanAkseptasi_Click() {
    $('#btnAddNewPengajuanAkseptasi').click(function () {
        openPengajuanAkseptasiWindow('/PengajuanAkseptasi/Add', 'Add New Pengajuan Akseptasi');
    });
}

function OnClickEditPengajuanAkseptasi(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    console.log('dataItem', dataItem);
    openPengajuanAkseptasiWindow(`/PengajuanAkseptasi/Edit?kd_cb=${dataItem.kd_cb}&kd_cob=${dataItem.kd_cob}&kd_scob=${dataItem.kd_scob}&kd_thn=${dataItem.kd_thn}&no_aks=${dataItem.no_aks}`, 'Edit Pengajuan Akseptasi');
}

function OnClickViewPengajuanAkseptasi(e) {
    e.preventDefault();
    dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    console.log('dataItem', dataItem);
    openPengajuanAkseptasiWindow(`/PengajuanAkseptasi/View?kd_cb=${dataItem.kd_cb}&kd_cob=${dataItem.kd_cob}&kd_scob=${dataItem.kd_scob}&kd_thn=${dataItem.kd_thn}&no_aks=${dataItem.no_aks}`, 'View Pengajuan Akseptasi');
}

function OnClickInfoPengajuanAkseptasi(e) {
    e.preventDefault();
    dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    console.log('dataItem', dataItem);
    openWindow('#InfoPengjuanAkseptasiWindow',`/PengajuanAkseptasi/Info`, 'Info Pengajuan Akseptasi');
}

function OnClickSubmitPengajuanAkseptasi(e) {
    dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    openWindow('#SubmitWindow',`/PengajuanAkseptasi/SubmitView`, 'Submit Pengajuan Akseptasi');
}

function OnClickPrintPengajuanAkseptasi(e) {
    showProgressOnGrid('#PengajuanAkseptasiGrid');
    dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    
    var data = {
        kd_cb: dataItem.kd_cb,
        kd_cob: dataItem.kd_cob,
        kd_scob: dataItem.kd_scob,
        kd_thn: dataItem.kd_thn,
        no_aks: dataItem.no_aks,
    }
    
    ajaxPost("/PengajuanAkseptasi/GenerateReport", JSON.stringify(data),
        function (response) {
            if(response.Status === "OK"){
                window.open("/Reports/" + response.Data + "/PengajuanAkseptasi.pdf",  '_blank');
            } else {
                showMessage('Error', response.Message);
            }
            closeProgressOnGrid('#PengajuanAkseptasiGrid');
        },
    );
}
