var navigations;
$(document).ready(function () {
    searchKeyword_OnKeyUp();
    btnAddRoleNavigation_Click();
    getNavigations();
});
function searchKeyword_OnKeyUp() {
    $('#SearchKeyword').keyup(function () {
        refreshGrid("#RoleNavigationGrid");
    });
}
function btnAddRoleNavigation_Click() {
    $('#btnAddNewRoleNavigation').click(function () {
        openRoleNavigationWindow('/RoleNavigation/Add', 'Set New Role for Menu');
    });
}

function btnEditRoleNavigation_OnClick(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    console.log('dataItem', dataItem);
    openRoleNavigationWindow(`/RoleNavigation/Edit?id=${dataItem.RoleId}`, 'Edit Role for Menu');
}
function btnDeleteRoleNavigation_OnClick(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    showConfirmation('Confirmation', `Are you sure you want to delete role for menu ${dataItem.RoleName}?`,
        function () {
            showProgressOnGrid('#RoleNavigationGrid');
            setTimeout(function () { deleteRoleNavigation(dataItem.RoleId); }, 500);
        }
    );
}
function deleteRoleNavigation(id) {
    $.ajax({
        url: "/RoleNavigation/Delete",
        type: "POST",
        data: { id: id },
        success: function (data) {
            if (data.Result == "OK") {
                showMessage('Success', 'Data has been deleted');
                refreshGrid("#RoleNavigationGrid");
                closeProgressOnGrid('#RoleNavigationGrid');
            }
            else if (data.Result == "ERROR")
                $("#RoleNavigationWindow").html(data.Message);
            else
                $("#RoleNavigationWindow").html(data);
        },
        error: function (data) {
            console.log('Error e:', data);
        }
    });
}
function openRoleNavigationWindow(url, title) {
    openWindow('#RoleNavigationWindow', url, title);
}
function searchFilter() {
    return {
        searchkeyword: $("#SearchKeyword").val()
    }
}
function saveRoleNavigation(url) {
    var grid = $("#NavigationGrid").data("kendoGrid");
    grid.saveChanges();
    var roleForm = getFormData($('#RoleForm'));
    var data = JSON.stringify($.extend(roleForm, { Navigations: grid.dataSource.data() }));
    ajaxPostSafely(url, data, function (dataReturn) {
        if (dataReturn.Result == "OK") {
            showMessage('Success', dataReturn.Message);
            refreshGrid("#RoleNavigationGrid");
            closeWindow('#RoleNavigationWindow')
        }
        else {
            var errors = Object.keys(dataReturn.Message).map(k => dataReturn.Message[k]);
            errors.forEach((error)=> toastr.error(error))
        }

        closeProgress('#RoleNavigationWindow');
    })
}

function getNavigations() {
    $.ajax({
        url: '/RoleNavigation/GetNavigations',
        type: 'GET',
        success: function (result) {
            navigations = result;
        }
    });
}