$(document).ready(function () {
    refreshViewSourceDataHasilGrid();
});

function dataViewSourceDataHasil(){
    return {
        TipeTransaksi: $("#TipeTransaksi").val(),
        KodeMetode: $("#KodeMetode").val(),
        Periode: $("#Periode").val(),
        Perhitungan: $("#Perhitungan").val(),
        Pengukuran: $("#Pengukuran").val(),
    }
}

function refreshViewSourceDataHasilGrid(){
    $('#btn-refresh-grid').click(function () {
        $("#custom-loader").show();
        ajaxPost("/HasilCSM/GetViewSourceDataHasil", JSON.stringify(dataViewSourceDataHasil()), (result) => {
            $("#grid").remove();
            $("#HasilCSMForm").append('<div id="grid"></div>')
            generateGrid(result);
            $("#custom-loader").hide();
        })
    });
}

function onChangePerhitungan(e){
    console.log("change Perhitungan");
    if(e.sender._cascadedValue === "P")
        $("#divPengukuran").show();
    else{
        $("#divPengukuran").hide();
        $("#Pengukuran").getKendoDropDownList().value("");
    }
}

var dateFields = [];

function generateGrid(gridData) {

    var model = generateModel(gridData[0]);

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
            fileName: "Hasil Perhitungan CSM.xlsx",
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
    });
    
    grid = $("#grid").getKendoGrid();

    for (var i = 0; i < grid.columns.length; i++) {
        if (i !== 0) {
            grid.autoFitColumn(i);
        }
    }
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