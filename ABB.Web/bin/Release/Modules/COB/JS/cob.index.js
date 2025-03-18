$(document).ready(function () {
    searchKeyword_OnKeyUp();
    btnAddCOB_Click();
});

function searchKeyword_OnKeyUp() {
    $('#SearchKeyword').keyup(function () {
        refreshGrid("#COBGrid");
    });
}
function btnAddCOB_Click() {
    $('#btnAddNewCOB').click(function () {
        openCOBWindow('/COB/Add', 'Add New COB');
    });
}
function btnEditCOB_OnClick(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    console.log('dataItem', dataItem);
    openCOBWindow(`/COB/Edit?kd_cob=${dataItem.kd_cob}`, 'Edit COB');
}
function btnDeleteCOB_OnClick(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    showConfirmation('Confirmation', `Are you sure you want to delete COB ${dataItem.nm_cob}?`,
        function () {
            showProgressOnGrid('#COBGrid');
            setTimeout(function () { deleteCOB(dataItem.kd_cob); }, 500);
        }
    );
}
function searchFilter() {
    return {
        searchkeyword: $("#SearchKeyword").val()
    }
}