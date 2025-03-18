var _subNav;
$(document).ready(function () {

    btnAddNavigation_OnClick();
    prepareSubMenu();
    LoadIconList();
    initSampleInlineGrid();
    $("input:hidden[name=IsActive]").val($("#IsActive").prop("checked"));
});
function LoadIconList() {
    var iconList = $("#Icon").data("kendoComboBox");
    iconList.setDataSource(fawe);
}
function prepareSubMenu() {
    $("#submenu").hide();
    $("#hasSubmenu").click(function () {
        $("#submenu").toggle();
    })
}
function onSwitchChange(e) {
    $("input:hidden[name=IsActive]").val(e.checked);
}
function LoadSubNavigation(navigationId) {
    $.getJSON("/Navigation/GetSubNavigationDropdown?Id=" + navigationId, function (data) {
        console.log("SUBNAV", data);
        _subNav = data;
    });
}
function initSampleInlineGrid() {
    $("#NavInlineGrid").kendoGrid({
        pageable: {
            pageSizes: ['all', 10, 20, 50, 100],
            refresh: true,
            buttonCount: 5
        },
        editable: {
            "createAt": "bottom"
        },
        columns: [
            {
                field: "SubNavigations", title: "Sub Menu",
                editor: function (container, options) {
                    comboBoxEditor({
                        container: container,
                        options: options,
                        data: _subNav,
                        valueField: "Id",
                        textField: "Description",
                    });
                },
                template: function (dataItem) {
                    return dropDownTemplate(dataItem?.SubNavigations?.Description);
                },
                width: 150
            },
            {
                command: [
                    {
                        text: " ",
                        click: btnDeleteSubNavigation_OnClick,
                        iconClass: "fa fa-trash"
                    },
                   
                ], title: "&nbsp;", width: "40px"
            }
        ]
    });
}
function btnDeleteSubNavigation_OnClick(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    showConfirmationById('#confirmSubNavigation', 'Confirmation', `Are you sure, you want to delete this data?`,
        function () {
            var grid = $("#NavInlineGrid").data("kendoGrid");
            var datas = grid.dataSource.data();
            for (var data of datas) {

                if (dataItem.SubNavigations !== undefined) {
                    if (data.SubNavigations.Id == dataItem.SubNavigations.Id) {
                        datas.remove(data);
                        break;
                    }
                } else {
                    if (data.SubNavigations === undefined) {
                        datas.remove(data);
                        break;
                    }
                }
            }
        }
    );
}
function btnAddNavigation_OnClick() {
    $('#btn-add-subnavigation').click(function () {
        var grid = $("#NavInlineGrid").data("kendoGrid");
        grid.addRow();
        
    });
}
function btnSubmit_Click(el, url) {
    $(el).click(function () {
        showConfirmation('Confirmation', `Do you want to save the changes?`,
            function () {
                showProgress('#NavWindow');

                setTimeout(function () {
                    saveNavigation(url)
                }, 500);
            });
    });
}

function saveNavigation(url) {
   

    var grid = $("#NavInlineGrid").data("kendoGrid");
    grid.saveChanges();
    var datas = grid.dataSource.data();
    var subnavids = [];

    for (var snav of datas) {
        if (snav.SubNavigations !== undefined) {
            subnavids.push({ "Id": snav.SubNavigations.Id, "Description": snav.SubNavigations.Description });
        }
    }
    var navForm = getFormData($('#NavForm'));
    var data = JSON.stringify($.extend(navForm, { SubNavigations: subnavids }));
    console.log("formDataaa", data);
    $.ajax({
        url: url,
        type: "POST",
        data: data,
        contentType: 'application/json; charset=utf-8',

        success: function (dataReturn) {
            if (dataReturn.Result == "OK") {
                showMessage('Success', dataReturn.Message);
                refreshGrid("#NavGrid");
                closeWindow('#NavWindow')
            }
            else {
                var errors = Object.keys(dataReturn.Message).map(k => dataReturn.Message[k]);
                errors.forEach((error)=> toastr.error(error))
            }

            closeProgress('#NavWindow');
        },
        error: function (dataReturn) {
            console.log('Error Saving e:', dataReturn);
        }
    });
}
