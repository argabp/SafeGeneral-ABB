function openRoleWindow(url, title) {
    openWindow('#RoleWindow', url, title);
}

function deleteRole(id) {
    var data = { id: id };
    ajaxPostSafely("/Role/Delete", data, function (response) {
        if (response.Result) {
            showMessage('Success', 'Data has been deleted');
            refreshGrid("#RoleGrid");
            if (typeof deleteWorkspaceUserStorageByRoleId == 'function')
                deleteWorkspaceUserStorageByRoleId(id);
        }
        else if (!response.Result) {
            showMessage('Error', 'Delete data is failed, this data is already used');
            refreshGrid("#RoleGrid");
        }
        else
            $("#RoleWindow").html(response);

        closeProgressOnGrid('#RoleGrid');
    }, AjaxContentType.URLENCODED);
}
function saveRole(url) {
    var form = getFormData($('#RoleForm'));
    var data = JSON.stringify($.extend(form,
        {
            WorkspaceId: $('#hdnWorkspaceId').val()
        }));
    ajaxPostSafely(url, data,
        function (response) {
            if (response.Result == "OK") {
                showMessage('Success', response.Message);
                refreshGrid("#RoleGrid");
                closeWindow('#RoleWindow')
            }
            else if (response.Result == "ERROR")
                $("#RoleWindow").html(response.Message);
            else
                $("#RoleWindow").html(response);

            closeProgress('#RoleWindow');
        }
    );
}