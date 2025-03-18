$(document).ready(function () {
    searchKeyword_OnKeyUp();
    var dataItem;
});

function searchFilterTracking() {
    return {
        searchkeyword: $("#SearchKeyword").val(),
        kd_status: "23"
    };
}

function openNotaDinasWindow(url, title) {
    openWindow('#NotaDinasWindow', url, title);
}

function searchKeyword_OnKeyUp() {
    $('#SearchKeyword').keyup(function () {
        refreshGrid("#ConfirmationApprovalGrid");
    });
}

function OnClickEditApproval(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    console.log('dataItem', dataItem);
    openNotaDinasWindow(`/PengajuanNotaDinas/Edit?id_nds=${dataItem.id_nds}`, 'Edit');
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

function OnClickProcessApproval(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));

    showConfirmation('Confirmation', `Are you sure you want to process ${dataItem.no_nds} ?`,
        function () {
            showProgressOnGrid('#ConfirmationApprovalGrid');
            processNotaDinas(dataItem);
        }
    );
}

function processNotaDinas(dataItem){
    var url = `/PengajuanNotaDinas/ProcessNotaDinas?id_nds=${dataItem.id_nds}`;
    ajaxGet(url,  function (response) {
        if (response.Result == "OK") {
            showMessage('Success', response.Message);
            refreshGrid("#ConfirmationApprovalGrid");
            closeProgressOnGrid('#ConfirmationApprovalGrid');
        } else
            showMessage('Error', response.Message);
        closeProgressOnGrid('#ConfirmationApprovalGrid');
    }, AjaxContentType.URLENCODED);
}