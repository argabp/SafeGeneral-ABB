
var isactiveTmp = kendo.template($('#isactive-column-template').html());
var selectedId;
$(document).ready(function () {
    searchKeyword_OnKeyUp();
    $('#btnAddNavigation').click(function () {
        openNavWindow('/Navigation/Add', 'Add New Menu');
    });
});
function searchKeyword_OnKeyUp() {
    $('#SearchKeyword').keyup(function () {
        refreshGrid("#NavGrid");
    });
}

function btnEditNav_OnClick(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    console.log('dataItem', dataItem);
        openNavWindow(`/Navigation/Edit?id=${dataItem.NavigationId}`, 'Edit Menu');
}
function btnDeleteNav_OnClick(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    selectedId = dataItem.NavigationId;
        showConfirmation('Confirmation', `Are you sure? you want to delete Menu ${dataItem.Text}`, deleteNav);
    
}

function deleteNav() {
    showProgressOnGrid('#NavGrid');
    $.ajax({
        url: "/Navigation/Delete",
        type: "POST",
        data: { NavigationId: selectedId },
        success: function (data) {
            if (data.Result == "OK") {
                showMessage('Success', 'Data has been deleted');
                refreshGrid("#NavGrid");
                closeProgressOnGrid('#NavGrid');
            }
            else if (data.Result == "ERROR")
                $("#NavWindow").html(data.Message);
            else
                $("#NavWindow").html(data);
        },
        error: function (data) {
            console.log('Error e:', data);
        }
    });
}
function openNavWindow(url, title) {
    console.log(url);
    openWindow('#NavWindow', url, title);
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
