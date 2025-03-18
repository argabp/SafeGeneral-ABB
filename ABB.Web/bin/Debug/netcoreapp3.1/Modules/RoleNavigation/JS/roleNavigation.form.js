function btnSaveRoleForNavigation_Click(url) {
    $('#btn-save-roleNavigation').click(function () {
        showConfirmation("Confirmation", "Do you want to save the changes?", function () {
            showProgress('#RoleNavigationWindow');
            setTimeout(function () {
                saveRoleNavigation(url)
            }, 500);
        });
    });
}

function btnAddNavigation_OnClick() {
    $('#btn-add-navigation').click(function () {
        var grid = $("#NavigationGrid").data("kendoGrid");
        grid.addRow();
    });
}

function btnDeleteNavigation_OnClick(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    showConfirmationById('#confirmNavigation', 'Confirmation', `Are you sure, you want to delete this data?`,
        function () {
            var grid = $("#NavigationGrid").data("kendoGrid");
            var datas = grid.dataSource.data();
            for (var data of datas) {

                if (data.Navigation.NavigationId == dataItem.Navigation.NavigationId) {
                    datas.remove(data);
                    break;
                }
            }
        }
    );
}

function initRoleNavigationGrid() {
    $("#NavigationGrid").kendoGrid({
        pageable: false,
        height: 280,
        columns: [
            {
                field: "Navigation", title: "Menu",
                editor: function (container, options) {
                    comboBoxEditor({
                        container: container,
                        options: options,
                        data: navigations,
                        valueField: "NavigationId",
                        textField: "Text",
                        change: function (e) {
                            var grid = $("#NavigationGrid").data("kendoGrid");
                            var rowIndex = container.parent().index();
                            var data = grid.dataSource.at(rowIndex);
                            data.set("SubNavigation", []);
                        }
                    });
                },
                template: function (dataItem) {
                    return dropDownTemplate(dataItem.Navigation.Text);
                },
            },
            {
                field: "SubNavigation", title: "Sub Menu",
                editor: function (container, options) {
                    var grid = $("#NavigationGrid").data("kendoGrid");
                    debugger;
                    console.log("ini");
                    var dataItem = grid.dataItem(container.parent());
                    var ds = GetDataSource(`/RoleNavigation/GetSubNavigations?id=${dataItem?.Navigation?.NavigationId}`);
                    multiSelectEditor({
                        container: container,
                        options: options,
                        dataSource: ds,
                        valueField: "NavigationId",
                        textField: "Text"
                    });
                },
                template: function (dataItem) {
                    return multiSelectTemplate(dataItem.SubNavigation, "Text");
                },
            },
            {
                command: [{
                    text: " ",
                    click: btnDeleteNavigation_OnClick,
                    iconClass: "fa fa-trash",
                }], title: "&nbsp;", width: "80px"
            }
        ],
        editable: true,
    });
}

function loadNavigationDatasources() {
    var navigationDatasource = JSON.parse($('#NavigationDatasource').val());
    console.log("2" + navigationDatasource);
    loadInlineGridDS({
        gridId: '#NavigationGrid',
        arrayObj: navigationDatasource,
        fieldKey: "RoleId",
        model: {
            id: "RoleId",
            fields: {
                RoleId: { type: "text" },
                Navigation: {
                    defaultValue: {
                        NavigationId: 0,
                        Text: "Choose"
                    }
                },
                SubNavigation: {}
            }
        }
    });
}