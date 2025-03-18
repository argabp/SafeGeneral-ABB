$(document).ready(function () {
    setRouteDataSource(JSON.parse($('#Routes').val()));
    btnUpdateRoleRoute_Click();
    SearchKeywordAction_OnKeyUp();
    actionRoute_OnClick();
});

function btnUpdateRoleRoute_Click() {
    $('#btn-edit-roleRoute').click(function () {
        showConfirmation('Confirmation', `Do you want to save the changes?`, function () {
            showProgress('#RoleRouteWindow');
            setTimeout(function () {
                saveRoleRoute('/RoleRoute/Edit')
            }, 500);
        });
    });
}

function saveRoleRoute(url) {
    var routeForm = getFormData($('#RouteForm'));
    var data = JSON.stringify($.extend(routeForm, { Routes: getRouteDataSource() }));
    $.ajax({
        url: url,
        type: "POST",
        contentType: 'application/json; charset=utf-8',
        data: data,
        success: function (dataReturn) {
            if (dataReturn.Result == "OK") {
                showMessage('Success', dataReturn.Message);
                refreshGrid("#RoleRouteGrid");
                closeWindow('#RoleRouteWindow')
            }
            else if (dataReturn.Result == "ERROR")
                $("#RoleRouteWindow").html(dataReturn.Message);
            else
                $("#RoleRouteWindow").html(dataReturn);

            closeProgress('#RoleRouteWindow');
        },
        error: function (dataReturn) {
            console.log('Error Saving e:', dataReturn);
        }
    });
}
function SearchKeywordAction_OnKeyUp() {
    $('#SearchKeywordAction').keyup(function () {
        showProgressByElement($('#routeDiv'));
        var params = {
            roleId: $('#RoleId').val(),
            searchKeyword: $('#SearchKeywordAction').val()
        };
        ajaxGetWithParam(`/RoleRoute/GetRoutes`, params,
            function (response) {
                $("#routeDiv").replaceWith(response);
                closeProgressByElement($('#routeDiv'));
            }
        );
    });
}
function actionRoute_OnClick() {
    $(document).on('click', '.action-route', function () {
        editRouteIsActive($(this));
    });
}
function editRouteIsActive(element) {
    var data = JSON.parse($(element).attr('route-data'));
    data.Active = $(element).is(":checked");
    editRouteDataSource(data);
}
function setRouteDataSource(items) {
    setLocalStorage("Route", items);
}
function getRouteDataSource() {
    return getLocalStorage("Route");
}

function editRouteDataSource(editItem) {
    editLocalArray("Route", "RouteId", editItem);
}