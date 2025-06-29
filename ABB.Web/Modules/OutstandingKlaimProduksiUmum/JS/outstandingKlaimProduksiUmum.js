$(document).ready(function () {
    refreshOutstandingKlaimProduksiUmumGrid();
});

function dataOutstandingKlaimProduksiUmum(){
    return {
        TanggalAkhir: $("#TanggalAkhir").val(),
        KodeCabang: $("#KodeCabang").val()
    }
}

function refreshOutstandingKlaimProduksiUmumGrid(){
    $('#btn-refresh-grid').click(function () {
        $("#custom-loader").show();
        ajaxPost("/OutstandingKlaimProduksiUmum/GetOutstandingKlaimProduksiUmum", JSON.stringify(dataOutstandingKlaimProduksiUmum()), (result) => {
            if(result.Error === undefined){
                $("#grid").remove();
                $("#OutstandingKlaimProduksiUmumForm").append('<div id="grid"></div>')
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
            fileName: "Outstanding Klaim Produksi Umum.xlsx",
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