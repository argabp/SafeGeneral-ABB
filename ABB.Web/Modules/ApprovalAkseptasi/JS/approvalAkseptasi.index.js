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
    openPengajuanAkseptasiWindow(`/ApprovalAkseptasi/Edit?kd_cb=${dataItem.kd_cb}&kd_cob=${dataItem.kd_cob}&kd_scob=${dataItem.kd_scob}&kd_thn=${dataItem.kd_thn}&no_aks=${dataItem.no_aks}`, 'Edit');
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

function OnClickBandingApprovalAkseptasi(e) {
    dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    openWindow('#ApprovalWindow',`/ApprovalAkseptasi/BandingView`, 'Banding');
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
                window.open("/Reports/" + response.Data + "/KeteranganApprovalAkseptasi.pdf",  '_blank');
            } else {
                showMessage('Error', response.Message);
            }
            closeProgressOnGrid('#ApprovalAkseptasiGrid');
        },
    );
}

function setButtonActions(e){
    var grid = this;

    grid.tbody.find("tr").each(function(e, element) {
        var dataItem = grid.dataItem(this);
        var uid = $(this).data("uid");

        // Find button container - try locked column first, then regular
        var buttonContainer = grid.element.find(".k-grid-content-locked tr[data-uid='" + uid + "'] .k-command-cell");
        if (!buttonContainer.length) {
            buttonContainer = $(this).find(".k-command-cell");
        }

        if (buttonContainer.length) {
            // Apply your business logic to hide buttons
            if (dataItem.flag_approved != "1") {
                // Hide the custom button in this row
                buttonContainer.find(".k-grid-Approved").hide(); // "custom" is the command name
                buttonContainer.find(".k-grid-Rejected").hide(); // "custom" is the command name
                buttonContainer.find(".k-grid-Escalated").hide(); // "custom" is the command name
            } else {
                buttonContainer.find(".k-grid-Checked").hide(); // "custom" is the command name
            }

            if(dataItem.flag_banding != "1" || dataItem.flag_closing != "N"){
                buttonContainer.find(".k-grid-Banding").hide(); // "custom" is the command name
            }
        }
    });
    gridAutoFit(grid);
}