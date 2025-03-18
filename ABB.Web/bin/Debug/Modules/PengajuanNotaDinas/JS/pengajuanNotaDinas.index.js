$(document).ready(function () {
    searchKeyword_OnKeyUp();
    btnAddNotaDinas_Click();
});

function openNotaDinasWindow(url, title) {
    openWindow('#NotaDinasWindow', url, title);
}

function searchKeyword_OnKeyUp() {
    $('#SearchKeyword').keyup(function () {
        refreshGrid("#NotaDinasaGrid");
    });
}

function btnAddNotaDinas_Click() {
    $('#btnAddPengajuanNotaDinas').click(function () {
        openNotaDinasWindow('/PengajuanNotaDinas/Add', 'Add New');
    });
}

function btnEditNotaDinas_OnClick(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    console.log('dataItem', dataItem);
    openNotaDinasWindow(`/PengajuanNotaDinas/Edit?id_nds=${dataItem.id_nds}`, 'Edit');
}

function btnDeleteNotaDinas_OnClick(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));

    showConfirmation('Confirmation', `Are you sure you want to delete data ${dataItem.no_nds} ?`,
        function () {
            showProgressOnGrid('#NotaDinasGrid');
            deleteNotaDinas(dataItem);
        }
    );
}

function btnPrintNotaDinas_OnClick(e) {
    e.preventDefault();
    dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    console.log('dataItem', dataItem);
    window.open(`ReportNotaDinas?input_str=${dataItem.id_nds}`);
    window.open(`ReportNotaDinasDetail?input_str=${dataItem.id_nds}`);
}

function btnProcessNotaDinas_OnClick(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));

    showConfirmation('Confirmation', `Are you sure you want to process ${dataItem.no_nds} ?`,
        function () {
            showProgressOnGrid('#NotaDinasGrid');
            processNotaDinas(dataItem);
        }
    );
}

function deleteNotaDinas(dataItem) {
    var url = `/PengajuanNotaDinas/DeleteNotaDinas?id_nds=${dataItem.id_nds}`;
    ajaxGet(url,  function (response) {
        if (response.Result == "OK") {
            showMessage('Success', response.Message);
            refreshGrid("#NotaDinasGrid");
            closeProgressOnGrid('#NotaDinasGrid');
        } else
            showMessage('Error', response.Message);
        closeProgressOnGrid('#NotaDinasGrid');
    }, AjaxContentType.URLENCODED);
}

function processNotaDinas(dataItem){
    var url = `/PengajuanNotaDinas/ProcessNotaDinas?id_nds=${dataItem.id_nds}`;
    ajaxGet(url,  function (response) {
        if (response.Result == "OK") {
            showMessage('Success', response.Message);
            refreshGrid("#NotaDinasGrid");
            closeProgressOnGrid('#NotaDinasGrid');
        } else
            showMessage('Error', response.Message);
        closeProgressOnGrid('#NotaDinasGrid');
    }, AjaxContentType.URLENCODED);
}