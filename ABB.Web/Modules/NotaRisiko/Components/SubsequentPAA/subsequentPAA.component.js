$(document).ready(function () {
    btnPreviousSubsequentPAA();
    initSubsequentPAAGrid();
    loadSubsequentPAADS();
});

function btnPreviousSubsequentPAA(){
    $('#btn-previous-subsequentPAA').click(function () {
        $("#notaRisikoTab").getKendoTabStrip().select(0);
    });
}

function initSubsequentPAAGrid() {
    $("#SubsequentPAAGrid").kendoGrid({
        pageable: true,
        columns: [
            {
                field: "PeriodeProses", title: "Periode Proses", width: 150, format: "{0: dd-MM-yyyy}"
            },
            {
                field: "LRCRelease", title: "LRC Release", width: 150, template: function (model) {
                    return model.LRCRelease % 1 === 0 ? model.LRCRelease.toFixed(2) : model.LRCRelease;
                },
            },
            {
                field: "LRCReleaseMovement", title: "LRC Release Movement", width: 150, footerTemplate: ({LRCReleaseMovement}) => 
                    "Total: " + kendo.toString(kendo.htmlEncode(LRCReleaseMovement ? 
                        (LRCReleaseMovement.sum % 1 === 0 ? LRCReleaseMovement.sum.toFixed(2) : LRCReleaseMovement.sum) 
                        : 0), "{0:#.######}"),
                template: function (model) {
                    return model.LRCReleaseMovement % 1 === 0 ? model.LRCReleaseMovement.toFixed(2) : model.LRCReleaseMovement;
                }
            },
        ]
    });
}

function loadSubsequentPAADS() {
    var subsequentPAADS = JSON.parse($('#subsequentPAADS').val());
    loadInlineGridDS({
        gridId: '#SubsequentPAAGrid',
        arrayObj: subsequentPAADS,
        fieldKey: "Id",
        model: {
            id: "Id",
            fields: {
                PeriodeProses: { type: "date" },
                LRCRelease: { type: "numeric" },
                LRCReleaseMovement: { type: "numeric" }
            }
        },
        aggregate: [
            { field: "LRCReleaseMovement", aggregate: "sum" }
        ]
    });
}