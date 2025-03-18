$(document).ready(function () {
    var roleid = $("#RoleId").val()
    btnSubmit_Click('#btnUpdateRoleModule', '/RoleModule/Save');
    LoadModule(roleid);
    getModulesDatasource(roleid);
  
});

function getModulesDatasource(roleId) {
    $.ajax({
        url: '/RoleModule/GetModuleDropdownByRoleId',
        type: 'POST',
        data: {
            roleId: roleId
        },
        success: function (result) {
            var datasource = $("#ModuleInlineGrid").data("kendoGrid").dataSource;

            if (result.length > 0) {
                $.each(result, function (index, value) {
                    datasource.insert({ Modules: { Value: value.Value, Text: value.Text } });
                });
            }
            
        }
    });
}





