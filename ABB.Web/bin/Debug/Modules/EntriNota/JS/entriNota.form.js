function btnSaveEntriNota_Click(url) {
    $('#btn-save-entriNota').click(function () {
        showConfirmation("Confirmation", "Do you want to save the changes?", function () {
            showProgress('#EntriNotaWindow');
            setTimeout(function () {
                saveEntriNota(url)
            }, 500);
        });
    });
}

function btnAddDetailEntriNota_OnClick() {
    $('#btn-add-detailEntriNota').click(function () {
        var grid = $("#DetailEntriNotaGrid").data("kendoGrid");
        grid.addRow();
    });
}

function btnDeleteDetailEntriNota_OnClick(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    showConfirmationById('#confirmDetailEntriNota', 'Confirmation', `Are you sure, you want to delete this data?`,
        function () {
            var grid = $("#DetailEntriNotaGrid").data("kendoGrid");
            var datas = grid.dataSource.data();
            for (var data of datas) {

                if (data.no_ang == dataItem.no_ang) {
                    datas.remove(data);
                    break;
                }
            }
        }
    );
}

function initDetailEntriNotaGrid() {
    $("#DetailEntriNotaGrid").kendoGrid({
        pageable: {
            pageSizes: ['all', 10, 20, 50, 100],
            refresh: true,
            buttonCount: 5
        },
        height: 280,
        columns: [
            {
                field: "tgl_ang", title: "Tanggal Angsuran",
                editor: function (container, options) {
                    comboBoxEditor({
                        container: container,
                        options: options
                    });
                }
            },
            {
                field: "tgl_jth_tempo", title: "Tanggal Jatuh Tempo",
                editor: function (container, options) {
                    comboBoxEditor({
                        container: container,
                        options: options
                    });
                }
            },
            {
                field: "pst_ang", title: "Angsuran (%)",
                editor: function (container, options) {
                    numericTextBoxEditor({
                        container: container,
                        options: options,
                        format: "#.##"
                    });
                }
            },
            {
                field: "nilai_ang", title: "Jumlah Angsuran",
                editor: function (container, options) {
                    numericTextBoxEditor({
                        container: container,
                        options: options,
                        format: "#.##"
                    });
                }
            },
            {
                command: [{
                    text: " ",
                    click: btnDeleteDetailEntriNota_OnClick,
                    iconClass: "fa fa-trash",
                }], title: "&nbsp;", width: "80px"
            }
        ],
        editable: true,
    });
}

function loadDetailEntriNotaDatasources() {
    var detailEntriNotaDatasource = JSON.parse($('#DetailEntriNotaDatasource').val());
    console.log("2" + navigationDatasource);
    loadInlineGridDS({
        gridId: '#DetailEntriNotaGrid',
        arrayObj: detailEntriNotaDatasource,
        fieldKey: "no_ang",
        model: {
            id: "no_ang",
            fields: {
                no_ang: { type: "number" },
                tgl_ang: { type: "datetime" },
                tgl_jth_tempo: { type: "datetime" },
                pst_ang: { type: "number" },
                nilai_ang: { type: "number" }
            }
        }
    });
}

// Format totalNilaiAng as currency (money format)
var currencyFormatter = new Intl.NumberFormat('en-US', {
    style: 'decimal',
    minimumFractionDigits: 3,
    maximumFractionDigits: 3
});

function OnDetailNotaDataBound(e) {
    // Get the grid instance
    var grid = e.sender;

    // Find the total rows
    var totalRow = $("#total"+e.sender.element[0].id.split("_")[2]);

    // Calculate sum of `pst_ang` and `nilai_ang`
    var totalPstAng = 0;
    var totalNilaiAng = 0;

    grid.dataSource.view().forEach(function(dataItem) {
        totalPstAng += dataItem.pst_ang || 0;  // Ensure we sum the value or add 0 if undefined
        totalNilaiAng += dataItem.nilai_ang || 0;
    });

    // Create the HTML content with two columns for percentage and money
    var formattedHTML = `<div class="row col-sm-12" id="total${e.sender.element[0].id.split("_")[2]}">` +
        '<h2 class="col-sm-9">Total Presentase Angsuran & Nilai Angsuran: ' + totalPstAng.toFixed(2) + "%" + '</h2>' +
        '<h2 class="col-sm-2">' + currencyFormatter.format(totalNilaiAng) + '</h2>' +
        '</div>';

    // Update the content of the totalRow with the formatted HTML
    totalRow.html(formattedHTML);
}
