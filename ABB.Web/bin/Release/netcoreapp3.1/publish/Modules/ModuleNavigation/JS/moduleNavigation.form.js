var _navigations;
$(document).ready(function () {
    btnAddNavigation_OnClick();
    initNavigationInlineGrid();
});

function LoadNavigation(moduleId) {
    $.getJSON("/ModuleNavigation/GetNavigationDropdown?moduleId=" + moduleId, function (data) {
        console.log("Navigation", data);
        _navigations = data;
    });
}
function initNavigationInlineGrid() {
    $("#NavigationInlineGrid").kendoGrid({
        pageable: false,
        editable: {
            "createAt": "bottom"
        },
        columns: [
            {
                field: "Navigations", title: "Menu",
                editor: function (container, options) {
                    comboBoxEditor({
                        container: container,
                        options: options,
                        data: _navigations,
                        valueField: "Value",
                        textField: "Text",
                    });
                },
                template: function (dataItem) {
                    return dropDownTemplate(dataItem?.Navigations?.Text);
                },
                width: 150
            },
            {
                command: [
                    {
                        text: " ",
                        click: btnDeleteNavigation_OnClick,
                        iconClass: "fa fa-trash"
                    },
                   
                ], title: "&nbsp;", width: "40px"
            }
        ]
    });
}
function btnDeleteNavigation_OnClick(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    showConfirmationById('#confirmNavigation', 'Confirmation', `Are you sure, you want to delete this data?`,
        function () {
            var grid = $("#NavigationInlineGrid").data("kendoGrid");
            var datas = grid.dataSource.data();
            for (var data of datas) {

                if (dataItem.Navigations !== undefined) {
                    if (data.Navigations.Id === dataItem.Navigations.Id) {
                        datas.remove(data);
                        break;
                    }
                } else {
                    if (data.Navigations === undefined) {
                        datas.remove(data);
                        break;
                    }
                }
            }
        }
    );
}
function btnAddNavigation_OnClick() {
    $('#btn-add-navigation').click(function () {
        var grid = $("#NavigationInlineGrid").data("kendoGrid");
        grid.addRow();
        
    });
}
function btnSubmit_Click(el, url) {
    $(el).click(function () {
        showConfirmation('Confirmation', `Do you want to save the changes?`,
            function () {
                showProgress('#ModuleNavigationWindow');
                setTimeout(function () {
                    saveModuleNavigation(url)
                }, 500);
            });
    });
}

function saveModuleNavigation(url) {
    var grid = $("#NavigationInlineGrid").data("kendoGrid");
    grid.saveChanges();
    var datas = grid.dataSource.data();
    var navigationids = [];

    for (var data of datas) {
        if (data.Navigations !== undefined) {
            navigationids.push(parseInt(data.Navigations.Value));
        }
    }
    var ModuleNavigationForm = getFormData($('#ModuleNavigationForm'));
    var dataJson = JSON.stringify($.extend(ModuleNavigationForm, { Navigations: navigationids }));
    $.ajax({
        url: url,
        type: "POST",
        data: dataJson,
        contentType: 'application/json; charset=utf-8',

        success: function (dataReturn) {
            if (dataReturn.Result == "OK") {
                showMessage('Success', dataReturn.Message);
                refreshGrid("#ModuleNavigationGrid");
                closeWindow('#ModuleNavigationWindow')
            }
            else {
                var errors = Object.keys(dataReturn.Message).map(k => dataReturn.Message[k]);
                debugger;
                errors.forEach((error)=> toastr.error(error))
            }
                

            closeProgress('#ModuleNavigationWindow');
        },
        error: function (dataReturn) {
            console.log('Error Saving e:', dataReturn);
        }
    });
}
