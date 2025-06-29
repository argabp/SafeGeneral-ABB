$(document).ready(function () {
    btnNextInitialPAA();
    initInitialPAAGrid();
    loadInitialPAADS();
});

function btnNextInitialPAA(){
    $('#btn-next-initialPAA').click(function () {
        $("#notaRisikoTab").getKendoTabStrip().select(1);
    });
}

function initInitialPAAGrid() {
    $("#InitialPAAGrid").kendoGrid({
        pageable: true,
        columns: [
            {
                field: "PeriodeProses", title: "Periode Proses", width: 150, format: "{0: dd-MM-yyyy}"
            },
            {
                field: "LRC", title: "LRC", width: 150, template: function (model) {
                    return model.LRC % 1 === 0 ? model.LRC.toFixed(2) : model.LRC;
                },
            }
        ],
    });
}

function loadInitialPAADS() {
    var initialPAADS = JSON.parse($('#initialPAADS').val());
    loadInlineGridDS({
        gridId: '#InitialPAAGrid',
        arrayObj: initialPAADS,
        fieldKey: "Id",
        model: {
            id: "Id",
            fields: {
                PeriodeProses: { type: "date" },
                LRC: { type: "numeric" },
                LRCIDR: { type: "numeric" }
            }
        }
    });
}