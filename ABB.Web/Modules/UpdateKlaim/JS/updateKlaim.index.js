$(document).ready(function () {
    searchKeyword_OnKeyUp();
    var dataItem;
});

function searchKeyword_OnKeyUp() {
    $('#SearchKeyword').keyup(function () {
        refreshGrid("#UpdateKlaimGrid");
    });
}

function OnClickInfoUpdateKlaim(e) {
    e.preventDefault();
    dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    console.log('dataItem', dataItem);
    openWindow('#InfoUpdateKlaimWindow',`/UpdateKlaim/Info`, 'Info');
}

function OnClickClosedUpdateKlaim(e) {
    dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    openWindow('#UpdateKlaimWindow',`/UpdateKlaim/ClosedView`, 'Closed');
}

function OnClickRejectUpdateKlaim(e) {
    dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    openWindow('#UpdateKlaimWindow',`/UpdateKlaim/RejectView`, 'Reject');
}

function OnClickFinalUpdateKlaim(e) {
    dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    openWindow('#UpdateKlaimWindow',`/UpdateKlaim/FinalView`, 'Final');
}

function OnClickPrintUpdateKlaim(e) {
    showProgressOnGrid('#UpdateKlaimGrid');
    dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    
    var data = {
        kd_cb: dataItem.kd_cb,
        kd_cob: dataItem.kd_cob,
        kd_scob: dataItem.kd_scob,
        kd_thn: dataItem.kd_thn,
        no_kl: dataItem.no_kl,
    }
    
    ajaxPost("/UpdateKlaim/GenerateReport", JSON.stringify(data),
        function (response) {
            if(response.Status === "OK"){
                window.open("/Reports/" + response.Data + "/UpdateKlaim.pdf",  '_blank');
                window.open("/Reports/" + response.Data + "/KeteranganUpdateKlaim.pdf",  '_blank');
            } else {
                showMessage('Error', response.Message);
            }
            closeProgressOnGrid('#UpdateKlaimGrid');
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
            // // Apply your business logic to hide buttons
            // if (dataItem.flag_approved != "0") {
            //     // Hide the custom button in this row
            //     buttonContainer.find(".k-grid-Process").hide(); // "custom" is the command name
            // } else {
            //     buttonContainer.find(".k-grid-Escalated").hide(); // "custom" is the command name
            //     buttonContainer.find(".k-grid-Settled").hide(); // "custom" is the command name
            // }
        }
    });
    gridAutoFit(grid);
}