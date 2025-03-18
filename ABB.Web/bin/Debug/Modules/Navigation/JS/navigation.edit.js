
var _gridSubNavigationDatasource;
$(document).ready(function () {
    
    var navid = $("#NavigationId").val()
    btnSubmit_Click('#btnUpdateNavigation', '/Navigation/Edit');
    LoadSubNavigation(navid);
    getSubNavigationDatasource(navid);
  
});

function getSubNavigationDatasource(navigationId) {
    $.ajax({
        url: '/Navigation/GetSubNavigationDropdownByNavId',
        type: 'POST',
        data: {
            id: navigationId
        },
        success: function (result) {
            var datasource = $("#NavInlineGrid").data("kendoGrid").dataSource;
            $("#hasSubmenu").prop('checked', true);
            $("#submenu").show();

            if (result.length > 0) {
                $.each(result, function (index, value) {
                    datasource.insert({ SubNavigations: { Id: value.Id, Description: value.Description } });
                });
            }
            
        }
    });
}





