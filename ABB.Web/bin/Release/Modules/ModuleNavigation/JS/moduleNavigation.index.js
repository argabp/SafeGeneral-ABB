
var selectedId;
$(document).ready(function () {
    searchKeyword_OnKeyUp();
    $('#btnAddModuleNavigation').click(function () {
        openModuleNavigationWindow('/ModuleNavigation/Add', 'Add Module Menu');
    });
});
function searchKeyword_OnKeyUp() {
    $('#SearchKeyword').keyup(function () {
        refreshGrid("#ModuleNavigationGrid");
    });
}

function btnEditModuleNavigation_OnClick(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    console.log('dataItem', dataItem);
    openModuleNavigationWindow(`/ModuleNavigation/Edit?moduleId=${dataItem.ModuleId}`, 'Edit Module Menu');
}
function btnDeleteModuleNavigation_OnClick(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    selectedId = dataItem.ModuleId;
        showConfirmation('Confirmation', `Are you sure? you want to delete Role Module ${dataItem.ModuleName}`, deleteModuleNavigation);
    
}

function deleteModuleNavigation() {
    showProgressOnGrid('#ModuleNavigationGrid');
    $.ajax({
        url: "/ModuleNavigation/Delete",
        type: "POST",
        data: { moduleId: selectedId },
        success: function (data) {
            if (data.Result == "OK") {
                showMessage('Success', 'Data has been deleted');
                refreshGrid("#ModuleNavigationGrid");
                closeProgressOnGrid('#ModuleNavigationGrid');
            }
            else if (data.Result == "ERROR")
                $("#ModuleNavigationWindow").html(data.Message);
            else
                $("#ModuleNavigationWindow").html(data);
        },
        error: function (data) {
            console.log('Error e:', data);
        }
    });
}
function openModuleNavigationWindow(url, title) {
    console.log(url);
    openWindow('#ModuleNavigationWindow', url, title);
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
