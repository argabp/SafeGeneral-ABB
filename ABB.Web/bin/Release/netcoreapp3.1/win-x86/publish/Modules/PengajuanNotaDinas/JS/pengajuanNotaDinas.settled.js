$(document).ready(function () {
    searchKeyword_OnKeyUp();
    var dataItem;
});

function searchFilterTracking() {
    return {
        searchkeyword: $("#SearchKeyword").val(),
        kd_status: "25"
    };
}

function searchKeyword_OnKeyUp() {
    $('#SearchKeyword').keyup(function () {
        refreshGrid("#SettledApprovalGrid");
    });
}

function OnClickInfoApproval(e) {
    e.preventDefault();
    dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    console.log('dataItem', dataItem);
    openWindow('#InfoApprovalWindow',`/PengajuanNotaDinas/Info`, 'Info');
}

function btnPrintNotaDinas_OnClick(e) {
    e.preventDefault();
    dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    console.log('dataItem', dataItem);
    window.open(`ReportNotaDinas?input_str=${dataItem.id_nds}`);
    window.open(`ReportNotaDinasDetail?input_str=${dataItem.id_nds}`);
}

function OnClickAttachmentNasLife(e) {
    e.preventDefault();
    dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    console.log('dataItem', dataItem);
    openWindow('#AttachmentNasLifeWindow',`/PengajuanNotaDinas/AttachmentNasLife`, 'Attachment');
}