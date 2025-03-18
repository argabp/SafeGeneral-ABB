$(document).ready(function () {
    searchKeyword_OnKeyUp();
    var dataItem;
});

function searchKeyword_OnKeyUp() {
    $('#SearchKeyword').keyup(function () {
        refreshGrid("#NotaDinasGrid");
    });
}

function OnClickInfoApproval(e) {
    e.preventDefault();
    dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    console.log('dataItem', dataItem);
    openWindow('#InfoApprovalWindow',`/ApprovalNotaDinas/Info`, 'Info');
}

function OnClickPrintSertifikat(e) {
    e.preventDefault();
    dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    console.log('dataItem', dataItem);
    window.open(`ReportNotaDinas?input_str=${dataItem.id_nds}`);
    window.open(`ReportNotaDinasDetail?input_str=${dataItem.id_nds}`);
}

function OnClickConfirmationApproval(e) {
    e.preventDefault();
    dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    console.log('dataItem', dataItem);
    openWindow('#ConfirmationApprovalWindow', `/ApprovalNotaDinas/Confirmation`, 'Confirmation');
}

function OnClickSettledApproval(e) {
    e.preventDefault();
    dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    console.log('dataItem', dataItem);
    openWindow('#SettledApprovalWindow', `/ApprovalNotaDinas/Settled`, 'Settled');
}

function OnClickApprovedApproval(e) {
    e.preventDefault();
    dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    console.log('dataItem', dataItem);
    openWindow('#ApprovedApprovalWindow', `/ApprovalNotaDinas/Approved`, 'Approved');
}