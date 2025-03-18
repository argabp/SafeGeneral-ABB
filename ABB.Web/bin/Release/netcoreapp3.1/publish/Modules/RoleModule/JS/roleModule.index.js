
var selectedId;
$(document).ready(function () {
    searchKeyword_OnKeyUp();
    $('#btnAddRoleModule').click(function () {
        openRoleModuleWindow('/RoleModule/Add', 'Add Role Module');
    });
});
function searchKeyword_OnKeyUp() {
    $('#SearchKeyword').keyup(function () {
        refreshGrid("#RoleModuleGrid");
    });
}

function btnEditRoleModule_OnClick(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    console.log('dataItem', dataItem);
        openRoleModuleWindow(`/RoleModule/Edit?roleId=${dataItem.RoleId}`, 'Edit Role Module');
}
function btnDeleteRoleModule_OnClick(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    selectedId = dataItem.RoleId;
        showConfirmation('Confirmation', `Are you sure? you want to delete Role Module ${dataItem.RoleName}`, deleteRoleModule);
    
}

function deleteRoleModule() {
    showProgressOnGrid('#RoleModuleGrid');
    $.ajax({
        url: "/RoleModule/Delete",
        type: "POST",
        data: { roleId: selectedId },
        success: function (data) {
            if (data.Result == "OK") {
                showMessage('Success', 'Data has been deleted');
                refreshGrid("#RoleModuleGrid");
                closeProgressOnGrid('#RoleModuleGrid');
            }
            else if (data.Result == "ERROR")
                $("#RoleModuleWindow").html(data.Message);
            else
                $("#RoleModuleWindow").html(data);
        },
        error: function (data) {
            console.log('Error e:', data);
        }
    });
}
function openRoleModuleWindow(url, title) {
    console.log(url);
    openWindow('#RoleModuleWindow', url, title);
}
function searchFilter() {
    return {
        searchkeyword: $("#SearchKeyword").val()
    }
}
function replaceString(value) {
    if (value != null)
        return value.replace(/,/g, "<br />");
    else
        return "";
}
