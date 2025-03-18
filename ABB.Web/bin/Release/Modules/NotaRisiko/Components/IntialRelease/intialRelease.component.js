$(document).ready(function () {
    btnNextIntialRelease();
    btnPreviousIntialRelease();
    initIntialReleaseGrid();
    loadIntialReleaseDS();
});

function btnPreviousIntialRelease(){
    $('#btn-previous-intialRelease').click(function () {
        $("#notaRisikoTab").getKendoTabStrip().select(0);
    });
}

function btnNextIntialRelease(){
    $('#btn-next-intialRelease').click(function () {
        $("#notaRisikoTab").getKendoTabStrip().select(2);
    });
}

function initIntialReleaseGrid() {
    $("#IntialReleaseGrid").kendoGrid({
        pageable: true,
        columns: [
            {
                field: "PeriodeProses", title: "Periode Proses", width: 150, format: "{0: dd-MM-yyyy}"
            },
            {
                field: "BELclaim", title: "BEL Claim", width: 150, template: function (model) {
                    return model.BELclaim % 1 === 0 ? model.BELclaim.toFixed(2) : model.BELclaim;
                },
            },
            {
                field: "BELexpense", title: "BEL Expense", width: 150, template: function (model) {
                    return model.BELexpense % 1 === 0 ? model.BELexpense.toFixed(2) : model.BELexpense;
                },
            },
            {
                field: "RA", title: "RA", width: 150, template: function (model) {
                    return model.RA % 1 === 0 ? model.RA.toFixed(2) : model.RA;
                },
            },
            {
                field: "CSM", title: "CSM", width: 150, template: function (model) {
                    return model.CSM % 1 === 0 ? model.CSM.toFixed(2) : model.CSM;
                },
            },
            {
                field: "LRC", title: "LRC", width: 150, template: function (model) {
                    return model.LRC % 1 === 0 ? model.LRC.toFixed(2) : model.LRC;
                },
            }
        ],
    });
}

function loadIntialReleaseDS() {
    var intialReleaseDS = JSON.parse($('#intialReleaseDS').val());
    loadInlineGridDS({
        gridId: '#IntialReleaseGrid',
        arrayObj: intialReleaseDS,
        fieldKey: "Id",
        model: {
            id: "Id",
            fields: {
                PeriodeProses: { type: "date" },
                BELclaim: { type: "numeric" },
                BELexpense: { type: "numeric" },
                RA: { type: "numeric" },
                CSM: { type: "numeric" },
                LRC: { type: "numeric" }
            }
        }
    });
}