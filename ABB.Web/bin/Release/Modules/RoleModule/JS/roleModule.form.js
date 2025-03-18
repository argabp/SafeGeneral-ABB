var _modules;
$(document).ready(function () {
    btnAddModule_OnClick();
    initSampleInlineGrid();
});

function LoadModule(roleId) {
    $.getJSON("/RoleModule/GetModuleDropdown?roleId=" + roleId, function (data) {
        console.log("Module", data);
        _modules = data;
    });
}
function initSampleInlineGrid() {
    $("#ModuleInlineGrid").kendoGrid({
        pageable: false,
        editable: {
            "createAt": "bottom"
        },
        columns: [
            {
                field: "Modules", title: "Module",
                editor: function (container, options) {
                    comboBoxEditor({
                        container: container,
                        options: options,
                        data: _modules,
                        valueField: "Value",
                        textField: "Text",
                    });
                },
                template: function (dataItem) {
                    return dropDownTemplate(dataItem?.Modules?.Text);
                },
                width: 150
            },
            {
                command: [
                    {
                        text: " ",
                        click: btnDeleteModule_OnClick,
                        iconClass: "fa fa-trash"
                    },
                   
                ], title: "&nbsp;", width: "40px"
            }
        ]
    });
}
function btnDeleteModule_OnClick(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    showConfirmationById('#confirmModule', 'Confirmation', `Are you sure, you want to delete this data?`,
        function () {
            var grid = $("#ModuleInlineGrid").data("kendoGrid");
            var datas = grid.dataSource.data();
            for (var data of datas) {

                if (dataItem.Modules !== undefined) {
                    if (data.Modules.Id == dataItem.Modules.Id) {
                        datas.remove(data);
                        break;
                    }
                } else {
                    if (data.Modules === undefined) {
                        datas.remove(data);
                        break;
                    }
                }
            }
        }
    );
}
function btnAddModule_OnClick() {
    $('#btn-add-module').click(function () {
        var grid = $("#ModuleInlineGrid").data("kendoGrid");
        grid.addRow();
        
    });
}
function btnSubmit_Click(el, url) {
    $(el).click(function () {
        showConfirmation('Confirmation', `Do you want to save the changes?`,
            function () {
                showProgress('#RoleModuleWindow');
                setTimeout(function () {
                    saveRoleModule(url)
                }, 500);
            });
    });
}

function saveRoleModule(url) {
    var grid = $("#ModuleInlineGrid").data("kendoGrid");
    grid.saveChanges();
    var datas = grid.dataSource.data();
    var moduleids = [];

    for (var data of datas) {
        if (data.Modules !== undefined) {
            moduleids.push(parseInt(data.Modules.Value));
        }
    }
    var RoleModuleForm = getFormData($('#RoleModuleForm'));
    var dataJson = JSON.stringify($.extend(RoleModuleForm, { Modules: moduleids }));
    $.ajax({
        url: url,
        type: "POST",
        data: dataJson,
        contentType: 'application/json; charset=utf-8',

        success: function (dataReturn) {
            if (dataReturn.Result == "OK") {
                showMessage('Success', dataReturn.Message);
                refreshGrid("#RoleModuleGrid");
                closeWindow('#RoleModuleWindow')
            }
            else {
                var errors = Object.keys(dataReturn.Message).map(k => dataReturn.Message[k]);
                errors.forEach((error)=> toastr.error(error))
            }

            closeProgress('#RoleModuleWindow');
        },
        error: function (dataReturn) {
            console.log('Error Saving e:', dataReturn);
        }
    });
}
