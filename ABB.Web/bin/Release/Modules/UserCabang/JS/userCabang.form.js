function btnSaveUserCabang_Click(url) {
    $('#btn-save-userCabang').click(function () {
        showConfirmation("Confirmation", "Do you want to save the changes?", function () {
            showProgress('#UserCabangWindow');
            setTimeout(function () {
                saveUserCabang(url)
            }, 500);
        });
    });
}

function btnAddCabang_OnClick() {
    $('#btn-add-cabang').click(function () {
        var grid = $("#CabangGrid").data("kendoGrid");
        grid.addRow();
    });
}

function btnDeleteCabang_OnClick(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    showConfirmationById('#confirmCabang', 'Confirmation', `Are you sure, you want to delete this data?`,
        function () {
            var grid = $("#CabangGrid").data("kendoGrid");
            var datas = grid.dataSource.data();
            for (var data of datas) {

                if (data.Cabang.Value == dataItem.Cabang.Value) {
                    datas.remove(data);
                    break;
                }
            }
        }
    );
}

function initCabangGrid() {
    $("#CabangGrid").kendoGrid({
        pageable: {
            pageSizes: ['all', 10, 20, 50, 100],
            refresh: true,
            buttonCount: 5
        },
        height: 280,
        columns: [
            {
                field: "Cabang", title: "Cabang",
                editor: function (container, options) {
                    comboBoxEditor({
                        container: container,
                        options: options,
                        data: cabangs,
                        valueField: "Value",
                        textField: "Text"
                    });
                },
                template: function (dataItem) {
                    return dropDownTemplate(dataItem.Cabang?.Text);
                },
            },
            {
                command: [{
                    text: " ",
                    click: btnDeleteCabang_OnClick,
                    iconClass: "fa fa-trash",
                }], title: "&nbsp;", width: "80px"
            }
        ],
        editable: true,
    });
}

function loadCabangDatasources() {
    var cabangDatasource = JSON.parse($('#CabangDatasource').val());
    loadInlineGridDS({
        gridId: '#CabangGrid',
        arrayObj: cabangDatasource,
        fieldKey: "userid",
        model: {
            id: "userid",
            fields: {
                userid: { type: "text" },
                Cabang: {
                    defaultValue: {
                        Value: 0,
                        Text: "Cabang"
                    }
                }
            }
        }
    });
}