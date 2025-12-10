$(document).ready(function () {
    searchKeyword_OnKeyUp();
    var dataItem;
});

function searchKeyword_OnKeyUp() {
    $('#SearchKeyword').keyup(function () {
        refreshGrid("#ApprovalMutasiKlaimGrid");
    });
}

function openPengajuanMutasiKlaimWindow(url, title) {
    openWindow('#RegisterKlaimWindow', url, title);
}

function btnClickEditApprovalMutasiKlaim(e) {
    e.preventDefault();
    dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    console.log('dataItem', dataItem);
    openPengajuanMutasiKlaimWindow(`/ApprovalMutasiKlaim/Edit?kd_cb=${dataItem.kd_cb}&kd_cob=${dataItem.kd_cob}&kd_scob=${dataItem.kd_scob}&kd_thn=${dataItem.kd_thn}&no_kl=${dataItem.no_kl}`, 'Edit');
}

function OnClickInfoApprovalMutasiKlaim(e) {
    e.preventDefault();
    dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    console.log('dataItem', dataItem);
    openWindow('#InfoApprovalMutasiKlaimWindow',`/ApprovalMutasiKlaim/Info`, 'Info');
}

function OnClickProcessApprovalMutasiKlaim(e) {
    dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    openWindow('#ApprovalWindow',`/ApprovalMutasiKlaim/ProcessView`, 'Process');
}

function OnClickEscalatedApprovalMutasiKlaim(e) {
    dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    openWindow('#ApprovalWindow',`/ApprovalMutasiKlaim/EscalatedView`, 'Escalated');
}

function OnClickSettledApprovalMutasiKlaim(e) {
    dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    openWindow('#ApprovalWindow',`/ApprovalMutasiKlaim/SettledView`, 'Settled');
}

function OnClickPrintApprovalMutasiKlaim(e) {
    showProgressOnGrid('#ApprovalMutasiKlaimGrid');
    dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    
    var data = {
        kd_cb: dataItem.kd_cb,
        kd_cob: dataItem.kd_cob,
        kd_scob: dataItem.kd_scob,
        kd_thn: dataItem.kd_thn,
        no_kl: dataItem.no_kl,
    }
    
    ajaxPost("/ApprovalMutasiKlaim/GenerateReport", JSON.stringify(data),
        function (response) {
            if(response.Status === "OK"){
                window.open("/Reports/" + response.Data + "/ApprovalMutasiKlaim.pdf",  '_blank');
                window.open("/Reports/" + response.Data + "/KeteranganApprovalMutasiKlaim.pdf",  '_blank');
            } else {
                showMessage('Error', response.Message);
            }
            closeProgressOnGrid('#ApprovalMutasiKlaimGrid');
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
            if (dataItem.flag_approved != "0") {
                // Hide the custom button in this row
                buttonContainer.find(".k-grid-Process").hide(); // "custom" is the command name
            } else {
                buttonContainer.find(".k-grid-Escalated").hide(); // "custom" is the command name
                buttonContainer.find(".k-grid-Settled").hide(); // "custom" is the command name
            }
        }
    });
    gridAutoFit(grid);
}