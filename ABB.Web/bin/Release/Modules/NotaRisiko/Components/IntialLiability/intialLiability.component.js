$(document).ready(function () {
    btnNextIntialLiability();
    initIntialLiabilityGrid();
    loadIntialLiabilityDS();
});

function btnNextIntialLiability(){
    $('#btn-next-intialLiability').click(function () {
        $("#notaRisikoTab").getKendoTabStrip().select(1);
    });
}

function initIntialLiabilityGrid() {
    $("#IntialLiabilityGrid").kendoGrid({
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

function loadIntialLiabilityDS() {
    var intialLiabilityDS = JSON.parse($('#intialLiabilityDS').val());
    loadInlineGridDS({
        gridId: '#IntialLiabilityGrid',
        arrayObj: intialLiabilityDS,
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