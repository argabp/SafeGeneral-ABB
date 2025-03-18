$(document).ready(function () {
    var moduleId = $("#ModuleId").val()
    btnSubmit_Click('#btnUpdateModuleNavigation', '/ModuleNavigation/Save');
    LoadNavigation(moduleId);
    getNavigationsDatasource(moduleId);
  
});

function getNavigationsDatasource(moduleId) {
    $.ajax({
        url: '/ModuleNavigation/GetNavigationDropdownByModuleId',
        type: 'POST',
        data: {
            moduleId: moduleId
        },
        success: function (result) {
            var datasource = $("#NavigationInlineGrid").data("kendoGrid").dataSource;

            if (result.length > 0) {
                $.each(result, function (index, value) {
                    datasource.insert({ Navigations: { Value: value.Value, Text: value.Text } });
                });
            }
            
        }
    });
}





