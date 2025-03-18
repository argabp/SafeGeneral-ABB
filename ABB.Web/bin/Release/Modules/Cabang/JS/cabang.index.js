
$(document).ready(function () {
    searchKeyword_OnKeyUp();
    btnAddCabang_Click();
});

function searchKeyword_OnKeyUp() {
    $('#SearchKeyword').keyup(function () {
        refreshGrid("#CabangGrid");
    });
}
function btnAddCabang_Click() {
    $('#btnAddNewCabang').click(function () {
        openCabangWindow('/Cabang/Add', 'Add New Cabang');
    });
}
function btnEditCabang_OnClick(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    console.log('dataItem', dataItem);
    openCabangWindow(`/Cabang/Edit?kd_cb=${dataItem.kd_cb}`, 'Edit Cabang');
}
function btnDeleteCabang_OnClick(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    showConfirmation('Confirmation', `Are you sure you want to delete cabang ${dataItem.nm_cb}?`,
        function () {
            showProgressOnGrid('#CabangGrid');
            setTimeout(function () { deleteCabang(dataItem.kd_cb); }, 500);
        }
    );
}
function searchFilter() {
    return {
        searchkeyword: $("#SearchKeyword").val()
    }
}