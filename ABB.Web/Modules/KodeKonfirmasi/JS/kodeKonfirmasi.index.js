$(document).ready(function () {
    searchKeyword_OnKeyUp();
    btnAddKodeKonfirmasi_Click();
});

function searchKeyword_OnKeyUp() {
    $('#SearchKeyword').keyup(function () {
        refreshGrid("#KodeKonfirmasiGrid");
    });
}

function openKodeKonfirmasiWindow(url, title) {
    openWindow('#KodeKonfirmasiWindow', url, title);
}

function btnAddKodeKonfirmasi_Click() {
    $('#btnAddNewKodeKonfirmasi').click(function () {
        openKodeKonfirmasiWindow('/KodeKonfirmasi/Add', 'Add New Kode Konfirmasi');
    });
}
function btnEditKodeKonfirmasi_OnClick(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    console.log('dataItem', dataItem);
    openKodeKonfirmasiWindow(`/KodeKonfirmasi/Edit?kd_cb=${dataItem.kd_cb}&kd_cob=${dataItem.kd_cob}&kd_scob=${dataItem.kd_scob}&kd_thn=${dataItem.kd_thn}&no_aks=${dataItem.no_aks}&kd_konfirm=${dataItem.kd_konfirm}`, 'Edit Kode Konfirmasi');
}
function btnDeleteKodeKonfirmasi_OnClick(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    showConfirmation('Confirmation', `Are you sure you want to delete Kode Konfirmasi?`,
        function () {
            showProgressOnGrid('#KodeKonfirmasiGrid');
            setTimeout(function () { deleteKodeKonfirmasi(dataItem); }, 500);
        }
    );
}
function searchFilter() {
    return {
        searchkeyword: $("#SearchKeyword").val()
    }
}

function deleteKodeKonfirmasi(dataItem) {
    ajaxGet(`/KodeKonfirmasi/Delete?kd_cb=${dataItem.kd_cb}&kd_cob=${dataItem.kd_cob}&kd_scob=${dataItem.kd_scob}&kd_thn=${dataItem.kd_thn}&no_aks=${dataItem.no_aks}&kd_konfirm=${dataItem.kd_konfirm}`, function (response) {
        if (response.Result) {
            showMessage('Success', 'Data has been deleted');
            refreshGrid("#KodeKonfirmasiGrid");
        }
        else if (!response.Result) {
            showMessage('Error', 'Delete data is failed, this data is already used');
            refreshGrid("#KodeKonfirmasiGrid");
        }
        else
            $("#KodeKonfirmasiWindow").html(response);

        closeProgressOnGrid('#KodeKonfirmasiGrid');
    }, AjaxContentType.URLENCODED);
}