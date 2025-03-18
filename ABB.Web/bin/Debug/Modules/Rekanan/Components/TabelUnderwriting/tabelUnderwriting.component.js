$(document).ready(function () {
    getCategorySR();
    btnPreviousTabelUnderwriting();
    btnNextTabelUnderwriting();
    initTabelUnderwritingGrid();
    loadTabelUnderwritingDS();
});

var categorySR;

function getCategorySR() {
    ajaxGet(`/Rekanan/GetCategorySR`, function (result) {
        categorySR = result;
    });
}

function btnPreviousTabelUnderwriting(){
    $('#btn-previous-tabelUnderwriting').click(function () {
        $("#rekananTab").getKendoTabStrip().select(0);
    });
}

function btnNextTabelUnderwriting(){
    $('#btn-next-tabelUnderwriting').click(function () {
        $("#rekananTab").getKendoTabStrip().select(2);
    });
}

function initTabelUnderwritingGrid() {
    $("#TabelUnderwritingGrid").kendoGrid({
        pageable: true,
        save: saveTabelUnderwriting,
        toolbar: ["create"],
        columns: [
            {
                field: "nilai_ptg_awal", title: "UP Mulai", width: 150,
                editor: function (container, options) {
                    if(options.model.id === "")
                        numericTextBoxEditor({
                            container: container,
                            options: options,
                            format: "#,###",
                            spinners: false,
                            max: 1000000000000,
                            restrictDecimals: true,
                            decimals: false
                        });
                    else
                        $('<p>' + options.model.nilai_ptg_awal +  '</p>')
                            .appendTo(container);
                },
            },
            {
                field: "nilai_ptg_akhr", title: "UP Akhir", width: 150,
                editor: function (container, options) {
                    if(options.model.id === "")
                        numericTextBoxEditor({
                            container: container,
                            options: options,
                            format: "#,###",
                            spinners: false,
                            max: 1000000000000,
                            restrictDecimals: true,
                            decimals: false
                        });
                    else
                        $('<p>' + options.model.nilai_ptg_akhr +  '</p>')
                            .appendTo(container);
                },
            },
            {
                field: "usia_awal", title: "Usia Mulai", width: 100,
                editor: function (container, options) {
                    if(options.model.id === "")
                        numericTextBoxEditor({
                            container: container,
                            options: options,
                            spinners: false,
                            max: 999,
                            restrictDecimals: true,
                            decimals: false
                        });
                    else
                        $('<p>' + options.model.usia_awal +  '</p>')
                            .appendTo(container);
                },
            },
            {
                field: "usia_akhr", title: "Usia Akhir", width: 100,
                editor: function (container, options) {
                    if(options.model.id === "")
                        numericTextBoxEditor({
                            container: container,
                            options: options,
                            spinners: false,
                            max: 999,
                            restrictDecimals: true,
                            decimals: false
                        });
                    else
                        $('<p>' + options.model.usia_akhr +  '</p>')
                            .appendTo(container);
                },
            },
            {
                field: "kd_kategori", title: "Kategori", width: 200,
                editor: function (container, options) {
                    dropDownEditor({
                        container: container,
                        options: options,
                        data: categorySR,
                        valueField: "kd_kategori",
                        textField: "nm_kategori"
                    });
                },
                template: function (dataItem) {
                    if(dataItem.nm_kategori !== undefined )
                        return dataItem.nm_kategori;

                    if(dataItem.kd_kategori === "")
                        return dataItem.nm_kategori;
                    else
                        return categorySR.find((category) => category.kd_kategori === dataItem.kd_kategori).nm_kategori;
                },
            },
            {
                command: ["edit"], title: "&nbsp;", width: "100px"
            },
            {
                command: { text: "Delete", click: deleteTabelUnderwriting, iconClass: "k-icon k-i-delete" }, title: "&nbsp;"
            },
        ],
        editable: {
            mode: "inline",
        },
        edit: function(e) {
            if(e.model.isNew()){
                e.model.kd_cb = $("#kd_cb_hidden").val();
                e.model.kd_rk = $("#kd_rk_hidden").val();
            } else {
                e.model.nm_kategori = undefined;
            }
            
            var kd_categori = e.model.kd_kategori;
            var dropdown = $(this.table.find(".k-dropdown")).find("input").getKendoDropDownList();

            dropdown.value(kd_categori);
        }
    });
}

function loadTabelUnderwritingDS() {
    var tabelUnderwritingDS = JSON.parse($('#tabelUnderwritingDS').val());
    loadInlineGridDS({
        gridId: '#TabelUnderwritingGrid',
        arrayObj: tabelUnderwritingDS,
        fieldKey: "Id",
        model: {
            id: "Id",
            fields: {
                kd_cb: { type: "string" },
                kd_rk: { type: "string" },
                nilai_ptg_awal: { type: "numeric" },
                nilai_ptg_akhr: { type: "numeric"},
                usia_awal: { type: "numeric" },
                usia_akhr: { type: "numeric" },
                kd_kategori: { type: "string" },
            }
        }
    });
}

function saveTabelUnderwriting(e){
    showProgressOnGrid("#TabelUnderwritingGrid");

    var data = {}
    
    data.kd_cb = e.model.kd_cb;
    data.kd_rk = e.model.kd_rk;
    data.nilai_ptg_awal = e.model.nilai_ptg_awal;
    data.nilai_ptg_akhr = e.model.nilai_ptg_akhr;
    data.usia_awal = e.model.usia_awal;
    data.usia_akhr = e.model.usia_akhr;
    data.kd_kategori = e.model.kd_kategori;

    var stringifyData = JSON.stringify(data);
    
    ajaxPost("/Rekanan/SaveTabelUnderwriting", stringifyData, function (response) {
        closeProgressOnGrid("#TabelUnderwritingGrid");
        if (response.Result == "OK") {
            showMessage('Success', response.Message);
            $("#TabelUnderwritingGrid").getKendoGrid().saveChanges();
        }
        else if (response.Result == "ERROR")
            showMessage('Error', response.Message);
        else
            $("#tabTabelUnderwriting").html(response);

        ajaxGet(`/Rekanan/GetTabelUnderwritings?kd_rk=${kd_rk}&kd_cb=${kd_cb}`,
            function (response) {
                $('#tabelUnderwritingDS').val(JSON.stringify(response.Data));
            }
        );
    })
}

function deleteTabelUnderwriting(event){
    var dataItem = $("#TabelUnderwritingGrid").getKendoGrid().dataItem($(event.target).parent().parent()[0])
    showConfirmation('Confirmation', `Are you sure you want to delete this data?`,
        function () {
            showProgressOnGrid("#TabelUnderwritingGrid")

            var kd_cb = $("#kd_cb_hidden").val();
            var kd_rk = $("#kd_rk_hidden").val();

            var form = {};

            form.kd_cb = kd_cb;
            form.kd_rk = kd_rk;
            form.nilai_ptg_awal = dataItem.nilai_ptg_awal;
            form.nilai_ptg_akhr = dataItem.nilai_ptg_akhr;
            form.usia_awal = dataItem.usia_awal;
            form.usia_akhr = dataItem.usia_akhr;

            var data = JSON.stringify(form);

            ajaxPost("/Rekanan/DeleteTabelUnderwriting", data,
                function (response) {
                    if (response.Result == "OK")
                        showMessage('Success', response.Message);
                    else
                        showMessage('Error', response.Message);

                    closeProgressOnGrid("#TabelUnderwritingGrid");

                    ajaxGet(`/Rekanan/GetTabelUnderwritings?kd_rk=${kd_rk}&kd_cb=${kd_cb}`,
                        function (response) {
                            $('#tabelUnderwritingDS').val(JSON.stringify(response.Data));
                            $("#TabelUnderwritingGrid").getKendoGrid().dataSource.remove(dataItem);
                        }
                    );
                }
            );
        }
    );
}