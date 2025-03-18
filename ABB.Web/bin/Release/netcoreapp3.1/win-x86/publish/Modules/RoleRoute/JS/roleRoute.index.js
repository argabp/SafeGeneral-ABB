$(document).ready(function () {
    searchKeyword_OnKeyUp();
});
function searchKeyword_OnKeyUp() {
    $('#SearchKeyword').keyup(function () {
        refreshGrid("#RoleRouteGrid");
    });
}

function btnEditRoleRoute_OnClick(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    openRoleRouteWindow(`/RoleRoute/Edit?id=${dataItem.RoleId}&name=${dataItem.RoleName}`, 'Edit Role for Route');
}
function openRoleRouteWindow(url, title) {
    openWindow('#RoleRouteWindow', url, title);
}
function searchFilter() {
    return {
        searchkeyword: $("#SearchKeyword").val()
    }
}

function CheckAll(e) {
    var isChecked = e.checked;
    var actions = $("input[name=cb-" + e.name + "]");
    actions.each(function (key, element) {
        element.checked = isChecked;
        editRouteIsActive($(element));
    });
}