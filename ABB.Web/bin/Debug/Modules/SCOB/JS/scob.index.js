
$(document).ready(function () {
    searchKeyword_OnKeyUp();
    btnAddSCOB_Click();
});

function searchKeyword_OnKeyUp() {
    $('#SearchKeyword').keyup(function () {
        refreshGrid("#SCOBGrid");
    });
}
function btnAddSCOB_Click() {
    $('#btnAddNewSCOB').click(function () {
        openSCOBWindow('/SCOB/Add', 'Add New SCOB');
    });
}
function btnEditSCOB_OnClick(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    console.log('dataItem', dataItem);
    openSCOBWindow(`/SCOB/Edit?kd_cob=${dataItem.kd_cob}&kd_scob=${dataItem.kd_scob}`, 'Edit SCOB');
}
function btnDeleteSCOB_OnClick(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    showConfirmation('Confirmation', `Are you sure you want to delete Sub COB ${dataItem.nm_scob}?`,
        function () {
            showProgressOnGrid('#SCOBGrid');
            setTimeout(function () { deleteSCOB(dataItem.kd_cob, dataItem.kd_scob); }, 500);
        }
    );
}
function searchFilter() {
    return {
        searchkeyword: $("#SearchKeyword").val()
    }
}