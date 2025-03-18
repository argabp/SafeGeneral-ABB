$(document).ready(function () {
    refreshViewSourceDataHasilGrid();
});

function openNotaRisikoWindow(url, title) {
    openWindow('#NotaRisikoWindow', url, title);
}

function btnViewNotaRisiko_OnClick(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));

    openNotaRisikoWindow(`/NotaRisiko/View?id=${dataItem.Id}&kodeMetode=${dataItem.KodeMetode}`, 'View');
}

function dataNotaRisiko(){
    return {
        TipeTransaksi: $("#TipeTransaksi").val(),
        KodeMetode: $("#KodeMetode").val(),
        PeriodeAwal: $("#PeriodeAwal").val(),
        PeriodeAkhir: $("#PeriodeAkhir").val(),
        FlagRelease: $("#FlagRelease").getKendoSwitch().value(),
    }
}


function refreshViewSourceDataHasilGrid(){
    $('#btn-refresh-grid').click(function () {
        $("#custom-loader").show();
        ajaxPost("/NotaRisiko/GetSourceDatas", JSON.stringify(dataNotaRisiko()), (result) => {
            if(result.Error === undefined){
                $("#grid").remove();
                $("#NotaRisikoForm").append('<div id="grid"></div>')
                generateGrid(result);
            } else {
                showMessage("Error", result.Error)
            }
            $("#custom-loader").hide();
        })
    });
}

var dateFields = [];

function generateGrid(gridData) {

    var model = generateModel(gridData[0]);
    var columns = genereateColumns(gridData[0]);
    
    var parseFunction;

    if (dateFields.length > 0) {
        parseFunction = function (response) {
            for (var i = 0; i < response.length; i++) {
                for (var fieldIndex = 0; fieldIndex < dateFields.length; fieldIndex++) {
                    var record = response[i];
                    record[dateFields[fieldIndex]] = kendo.parseDate(record[dateFields[fieldIndex]]);
                }
            }
            return response;
        };
    }

    $("#grid").kendoGrid({
        toolbar: ["excel"],
        excel: {
            fileName: "Nota Risiko.xlsx",
            allPages: true
        },
        dataSource: {
            data: gridData,
            schema: {
                model: model
            },
            pageSize: 20
        },
        filterable: true,
        groupable: true,
        sortable: true,
        pageable: {
            refresh: true,
            pageSizes: true
        },
        navigatable: true,
        resizable: true,
        reorderable: true,
        columns: columns
    });

    grid = $("#grid").getKendoGrid();

    for (var i = 0; i < grid.columns.length; i++) {
        if (i !== 0) {
            grid.autoFitColumn(i);
        }
    }
}

function genereateColumns(gridData){
    columns = [];
    var sequence = 0;
    
    for (var property in gridData) {
        columns[sequence] = {
            field: property, title: property
        }
        
        sequence++;
    }

    columns[sequence] = { command: { iconClass:"fa fa-eye", text: "View", click: btnViewNotaRisiko_OnClick, name: "view" }, title: " ", width: "150px" }
    
    return columns;
}

function generateModel(gridData) {
    var model = {};
    model.id = "ID";
    var fields = {};
    for (var property in gridData) {
        var propType = typeof gridData[property];

        if (propType == "number") {
            fields[property] = {
                type: "number",
                validation: {
                    required: true
                }
            };
        } else if (propType == "boolean") {
            fields[property] = {
                type: "boolean",
                validation: {
                    required: true
                }
            };
        } else {
            fields[property] = {
                validation: {
                    required: true
                }
            };
        }
    }
    
    model.fields = fields;

    return model;
}