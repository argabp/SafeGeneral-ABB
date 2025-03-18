$(document).ready(function () {
    btnPreviousSubsequent();
    initSubsequentGrid();
    loadSubsequentDS();
});

function btnPreviousSubsequent(){
    $('#btn-previous-subsequent').click(function () {
        $("#notaRisikoTab").getKendoTabStrip().select(0);
    });
}

function initSubsequentGrid() {
    $("#SubsequentGrid").kendoGrid({
        pageable: true,
        columns: [
            {
                field: "PeriodeProses", title: "Periode Proses", width: 150, format: "{0: dd-MM-yyyy}"
            },
            {
                field: "BELclaimRelease", title: "BEL Claim Release", width: 150, template: function (model) {
                    return model.BELclaimRelease % 1 === 0 ? model.BELclaimRelease.toFixed(2) : model.BELclaimRelease;
                },
            },
            {
                field: "BELexpenseRelease", title: "BEL Expense Release", width: 150, template: function (model) {
                    return model.BELexpenseRelease % 1 === 0 ? model.BELexpenseRelease.toFixed(2) : model.BELexpenseRelease;
                },
            },
            {
                field: "RARelease", title: "RA Release", width: 150, template: function (model) {
                    return model.RARelease % 1 === 0 ? model.RARelease.toFixed(2) : model.RARelease;
                },
            },
            {
                field: "CSMRelease", title: "CSM Release", width: 150, template: function (model) {
                    return model.CSMRelease % 1 === 0 ? model.CSMRelease.toFixed(2) : model.CSMRelease;
                },
            },
            {
                field: "LRCRelease", title: "LRC Release", width: 150, template: function (model) {
                    return model.LRCRelease % 1 === 0 ? model.LRCRelease.toFixed(2) : model.LRCRelease;
                },
            },
            {
                field: "BELreleaseMovement", title: "BEL Release Movement", width: 150, template: function (model) {
                    return model.BELreleaseMovement % 1 === 0 ? model.BELreleaseMovement.toFixed(2) : model.BELreleaseMovement;
                },
            },
            {
                field: "RAReleaseMovement", title: "RA Release Movement", width: 150, template: function (model) {
                    return model.RAReleaseMovement % 1 === 0 ? model.RAReleaseMovement.toFixed(2) : model.RAReleaseMovement;
                },
            },
            {
                field: "CSMReleaseMovement", title: "CSM Release Movement", width: 150, template: function (model) {
                    return model.CSMReleaseMovement % 1 === 0 ? model.CSMReleaseMovement.toFixed(2) : model.CSMReleaseMovement;
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

function loadSubsequentDS() {
    var subsequentDS = JSON.parse($('#subsequentDS').val());
    loadInlineGridDS({
        gridId: '#SubsequentGrid',
        arrayObj: subsequentDS,
        fieldKey: "Id",
        model: {
            id: "Id",
            fields: {
                PeriodeProses: { type: "date" },
                BELclaimRelease: { type: "date" },
                BELexpenseRelease: { type: "numeric" },
                RARelease: { type: "numeric" },
                CSMRelease: { type: "numeric" },
                LRCRelease: { type: "numeric" },
                BELreleaseMovement: { type: "numeric" },
                RAReleaseMovement: { type: "numeric" },
                CSMReleaseMovement: { type: "numeric" },
                LRCReleaseMovement: { type: "numeric" }
            }
        },
        aggregate: [
            { field: "LRCReleaseMovement", aggregate: "sum" }
        ]
    });
}