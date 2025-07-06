$(document).ready(function () {
    searchKeyword_OnKeyUp();
    var dataItem;
});

function searchKeyword_OnKeyUp() {
    $('#SearchKeyword').keyup(function () {
        refreshGrid("#ApprovalAkseptasiGrid");
    });
}

function openPengajuanAkseptasiWindow(url, title) {
    openWindow('#PengajuanAkseptasiWindow', url, title);
}

function btnClickEditApprovalAkseptasi(e) {
    e.preventDefault();
    dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    console.log('dataItem', dataItem);
    openPengajuanAkseptasiWindow(`/ApprovalAkseptasi/Edit?kd_cb=${dataItem.kd_cb}&kd_cob=${dataItem.kd_cob}&kd_scob=${dataItem.kd_scob}&kd_thn=${dataItem.kd_thn}&no_aks=${dataItem.no_aks}`, 'View');
}

function OnClickInfoApprovalAkseptasi(e) {
    e.preventDefault();
    dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    console.log('dataItem', dataItem);
    openWindow('#InfoApprovalAkseptasiWindow',`/ApprovalAkseptasi/Info`, 'Info');
}

function OnClickCheckedApprovalAkseptasi(e) {
    dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    openWindow('#ApprovalWindow',`/ApprovalAkseptasi/CheckedView`, 'Checked');
}

function OnClickEscalatedApprovalAkseptasi(e) {
    dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    openWindow('#ApprovalWindow',`/ApprovalAkseptasi/EscalatedView`, 'Escalated');
}

function OnClickApprovedApprovalAkseptasi(e) {
    dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    openWindow('#ApprovalWindow',`/ApprovalAkseptasi/ApprovedView`, 'Approved');
}

function OnClickRejectedApprovalAkseptasi(e) {
    dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    openWindow('#ApprovalWindow',`/ApprovalAkseptasi/RejectedView`, 'Rejected');
}

function OnClickRevisedApprovalAkseptasi(e) {
    dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    openWindow('#ApprovalWindow',`/ApprovalAkseptasi/RevisedView`, 'Revised');
}

function OnClickPrintApprovalAkseptasi(e) {
    showProgressOnGrid('#ApprovalAkseptasiGrid');
    dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    
    var data = {
        kd_cb: dataItem.kd_cb,
        kd_cob: dataItem.kd_cob,
        kd_scob: dataItem.kd_scob,
        kd_thn: dataItem.kd_thn,
        no_aks: dataItem.no_aks,
    }
    
    ajaxPost("/ApprovalAkseptasi/GenerateReport", JSON.stringify(data),
        function (response) {
            if(response.Status === "OK"){
                window.open("/Reports/" + response.Data + "/ApprovalAkseptasi.pdf",  '_blank');
            } else {
                showMessage('Error', response.Message);
            }
            closeProgressOnGrid('#ApprovalAkseptasiGrid');
        },
    );

    ajaxPost("/ApprovalAkseptasi/GenerateKeteranganReport", JSON.stringify(data),
        function (response) {
            if(response.Status === "OK"){
                window.open("/Reports/" + response.Data + "/KeteranganApprovalAkseptasi.pdf",  '_blank');
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
        
        if (dataItem.flag_approved != "1") {
            // Hide the custom button in this row
            $(this).find(".k-grid-Approved").hide(); // "custom" is the command name
            $(this).find(".k-grid-Rejected").hide(); // "custom" is the command name
            $(this).find(".k-grid-Escalated").hide(); // "custom" is the command name
        } else {
            $(this).find(".k-grid-Checked").hide(); // "custom" is the command name
        }
    });

    gridAutoFit(grid);
}