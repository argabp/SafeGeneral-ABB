
var selectedId;
$(document).ready(function () {
    searchKeyword_OnKeyUp();
    btnAddRole_Click();
});
function searchKeyword_OnKeyUp() {
    $('#SearchKeyword').keyup(function () {
        refreshGrid("#RoleGrid");
    });
}
function btnAddRole_Click() {
    $('#btnAddNewRole').click(function () {
        openRoleWindow('/Role/Add', 'Add New Role');
    });
}
function btnEditRole_OnClick(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    console.log('dataItem', dataItem);
    openRoleWindow(`/Role/Edit?id=${dataItem.RoleId}`, 'Edit Role Page');
}
function btnDeleteRole_OnClick(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    showConfirmation('Confirmation', `Are you sure you want to delete role ${dataItem.RoleName}?`,
        function () {
            showProgressOnGrid('#RoleGrid');
            setTimeout(function () { deleteRole(dataItem.RoleId); }, 500);
        }
    );
}
function searchFilter() {
    return {
        searchkeyword: $("#SearchKeyword").val()
    }
}