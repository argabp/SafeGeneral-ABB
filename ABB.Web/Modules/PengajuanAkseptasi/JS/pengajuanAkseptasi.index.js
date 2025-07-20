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
        openPengajuanAkseptasiWindow('/PengajuanAkseptasi/Add', 'Add');
    });
}

function OnClickEditPengajuanAkseptasi(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    console.log('dataItem', dataItem);
    openPengajuanAkseptasiWindow(`/PengajuanAkseptasi/Edit?kd_cb=${dataItem.kd_cb}&kd_cob=${dataItem.kd_cob}&kd_scob=${dataItem.kd_scob}&kd_thn=${dataItem.kd_thn}&no_aks=${dataItem.no_aks}`, 'Edit');
}

function OnClickViewPengajuanAkseptasi(e) {
    e.preventDefault();
    dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    console.log('dataItem', dataItem);
    openPengajuanAkseptasiWindow(`/PengajuanAkseptasi/View?kd_cb=${dataItem.kd_cb}&kd_cob=${dataItem.kd_cob}&kd_scob=${dataItem.kd_scob}&kd_thn=${dataItem.kd_thn}&no_aks=${dataItem.no_aks}`, 'View');
}

function OnClickInfoPengajuanAkseptasi(e) {
    e.preventDefault();
    dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    console.log('dataItem', dataItem);
    openWindow('#InfoPengajuanAkseptasiWindow',`/PengajuanAkseptasi/Info`, 'Info');
}

function OnClickSubmitPengajuanAkseptasi(e) {
    dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    openWindow('#ApprovalWindow',`/PengajuanAkseptasi/Submit`, 'Submit');
}

function OnClickBatalAkseptasiPengajuanAkseptasi(e) {
    dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    openWindow('#ApprovalWindow',`/PengajuanAkseptasi/BatalAkseptasi`, 'Batal Akseptasi');
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
                window.open("/Reports/" + response.Data + "/KeteranganPengajuanAkseptasi.pdf",  '_blank');
            } else {
                showMessage('Error', response.Message);
            }
            closeProgressOnGrid('#PengajuanAkseptasiGrid');
        },
    );
}

function setButtonActions(e){
    var grid = this;

    // Loop through data items and rows
    grid.tbody.find("tr").each(function() {
        var dataItem = grid.dataItem(this);
        
        if($("#UserLogin").val() != dataItem.kd_user_input){
            $(this).find(".k-grid-Edit").hide(); // "custom" is the command name
            $(this).find(".k-grid-Submit").hide(); // "custom" is the command name
            $(this).find(".k-grid-Cancel").hide(); // "custom" is the command name
            $(this).find(".k-grid-BatalAkseptasi").hide(); // "custom" is the command name
        }
        
        if($("#UserLogin").val() != dataItem.kd_user_status){
            $(this).find(".k-grid-Edit").hide(); // "custom" is the command name
            $(this).find(".k-grid-Submit").hide(); // "custom" is the command name
            $(this).find(".k-grid-BatalAkseptasi").hide(); // "custom" is the command name
        }
        
        if (dataItem.status !== "New" && dataItem.status !== "Revised") {
            // Hide the custom button in this row
            $(this).find(".k-grid-Edit").hide(); // "custom" is the command name
            $(this).find(".k-grid-Submit").hide(); // "custom" is the command name
        }
        
        if(dataItem.flag_closing !== "N" && dataItem.status !== "Approved"){
            // Hide the custom button in this row
            $(this).find(".k-grid-BatalAkseptasi").hide(); // "custom" is the command name
        }
    });
    
    gridAutoFit(grid);
}