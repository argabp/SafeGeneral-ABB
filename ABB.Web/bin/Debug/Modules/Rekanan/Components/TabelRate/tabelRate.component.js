$(document).ready(function () {;
    btnPreviousTabelRate();
    btnNextTabelRate();
    initTabelRateGrid();
    loadTabelRateDS();
});

var satuans = [
    {
        Value: 1,
        Text: "%",
    },
    {
        Value: 10,
        Text: "%o",
    }
];

var programs = [
    {
        Value: 1,
        Text: "Reguler"
    },
    {
        Value: 2,
        Text: "Platinum"
    }
]

function btnPreviousTabelRate(){
    $('#btn-previous-tabelRate').click(function () {
        $("#rekananTab").getKendoTabStrip().select(1);
    });
}

function btnNextTabelRate(){
    $('#btn-next-tabelRate').click(function () {
        $("#rekananTab").getKendoTabStrip().select(3);
    });
}

function initTabelRateGrid() {
    $("#TabelRateGrid").kendoGrid({
        pageable: true,
        save: saveTabelRate,
        toolbar: ["create"],
        columns: [
            {
                field: "masa_ptg", title: "Masa Asuransi", width: 150,
                editor: function (container, options) {
                    if(options.model.id === "")
                        numericTextBoxEditor({
                            container: container,
                            options: options,
                            format: "#",
                            spinners: false,
                            max: 999,
                            restrictDecimals: true,
                            decimals: false
                        });
                    else
                        $('<p>' + options.model.masa_ptg +  '</p>')
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
                field: "pst_rate", title: "Rate", width: 100,
                editor: function (container, options) {
                    numericTextBoxEditor({
                        container: container,
                        options: options,
                        format: "#.#####",
                        spinners: false,
                        max: 999,
                        restrictDecimals: true,
                        decimals: 5
                    });
                },
            },
            {
                field: "stn_rate", title: "Satuan", width: 100,
                editor: function (container, options) {
                    dropDownEditor({
                        container: container,
                        options: options,
                        data: satuans,
                        valueField: "Value",
                        textField: "Text"
                    });
                },
                template: function (dataItem) {
                    if(dataItem.stn_rate === undefined)
                        return "";
                    
                    if(dataItem.stn_rate.Value === undefined)
                        return satuans.find((satuan) => satuan.Value === dataItem.stn_rate).Text;
                    else
                        return satuans.find((satuan) => satuan.Value === dataItem.stn_rate.Value).Text;
                        
                },
            },
            {
                field: "jns_program", title: "Program", width: 100,
                editor: function (container, options) {
                    dropDownEditor({
                        container: container,
                        options: options,
                        data: programs,
                        valueField: "Value",
                        textField: "Text"
                    });
                },
                template: function (dataItem) {
                    if(dataItem.jns_program === undefined || dataItem.jns_program === null)
                        return "";

                    if(dataItem.jns_program.Value === undefined)
                        return programs.find((program) => program.Value === dataItem.jns_program).Text;
                    else
                        return programs.find((program) => program.Value === dataItem.jns_program.Value).Text;
                },
            {
            },
                command: ["edit"], title: "&nbsp;", width: "100px"
            },
            {
                command: { text: "Delete", click: deleteTabelRate, iconClass: "k-icon k-i-delete" }, title: "&nbsp;"
            },
        ],
        editable: {
            mode: "inline",
        },
        edit: function(e) {
            if(e.model.isNew()){
                e.model.kd_cb = $("#kd_cb_hidden").val();
                e.model.kd_rk = $("#kd_rk_hidden").val();
            }
            
            var satuan = e.model.stn_rate;
            var program = e.model.jns_program;
            var dropdownSatuan = $($(this.table.find(".k-dropdown"))[0]).find("input").getKendoDropDownList();
            var dropdownProgram = $($(this.table.find(".k-dropdown"))[1]).find("input").getKendoDropDownList();
            
            if(satuan !== undefined)
                if(satuan.Value === undefined)
                    dropdownSatuan.value(satuan);
                else
                    dropdownSatuan.value(satuan.Value);
                
            if(program !== undefined)
                if(program.Value === undefined)
                    dropdownProgram.value(program);
                else
                    dropdownProgram.value(program.Value);
        }
    });
}

function loadTabelRateDS() {
    var tabelRateDS = JSON.parse($('#tabelRateDS').val());
    loadInlineGridDS({
        gridId: '#TabelRateGrid',
        arrayObj: tabelRateDS,
        fieldKey: "Id",
        model: {
            id: "Id",
            fields: {
                kd_cb: { type: "string" },
                kd_rk: { type: "string" },
                masa_ptg: { type: "numeric" },
                usia_awal: { type: "numeric" },
                usia_akhr: { type: "numeric" },
                pst_rate: { type: "numeric" },
                stn_rate: { type: "numeric" },
                jns_program: { type: "numeric" },
            }
        }
    });
}

function saveTabelRate(e){
    e.model.dirty = false;
    showProgressOnGrid("#TabelRateGrid");
    
    var data = {}
    
    data.kd_cb = e.model.kd_cb;
    data.kd_rk = e.model.kd_rk;
    data.masa_ptg = e.model.masa_ptg;
    data.usia_awal = e.model.usia_awal;
    data.usia_akhr = e.model.usia_akhr;
    data.pst_rate = e.model.pst_rate;
    data.stn_rate = e.model.stn_rate.Value === undefined ? e.model.stn_rate : e.model.stn_rate.Value;
    data.jns_program = e.model.jns_program.Value === undefined ? e.model.jns_program : e.model.jns_program.Value;

    var stringifyData = JSON.stringify(data);

    ajaxPost("/Rekanan/SaveTabelRate", stringifyData, function (response) {
        closeProgressOnGrid("#TabelRateGrid");
        if (response.Result == "OK") {
            showMessage('Success', response.Message);
            $("#TabelRateGrid").getKendoGrid().saveChanges();
        }
        else if (response.Result == "ERROR")
            showMessage('Error', response.Message);
        else
            $("#tabTabelRate").html(response);

        ajaxGet(`/Rekanan/GetTabelRates?kd_rk=${kd_rk}&kd_cb=${kd_cb}`,
            function (response) {
                $('#tabelRateDS').val(JSON.stringify(response.Data));
            }
        );
    })
}

function deleteTabelRate(event){
    var dataItem = $("#TabelRateGrid").getKendoGrid().dataItem($(event.target).parent().parent()[0])
    showConfirmation('Confirmation', `Are you sure you want to delete this data?`,
        function () {
            showProgressOnGrid("#TabelRateGrid")
            
            var kd_cb = $("#kd_cb_hidden").val();
            var kd_rk = $("#kd_rk_hidden").val();

            var form = {};

            form.kd_cb = kd_cb;
            form.kd_rk = kd_rk;
            form.masa_ptg = dataItem.masa_ptg;
            form.usia_awal = dataItem.usia_awal;
            form.usia_akhr = dataItem.usia_akhr;

            var data = JSON.stringify(form);

            ajaxPost("/Rekanan/DeleteTabelRate", data,
                function (response) {
                    if (response.Result == "OK")
                        showMessage('Success', response.Message);
                    else
                        showMessage('Error', response.Message);

                    closeProgressOnGrid("#TabelRateGrid");

                    ajaxGet(`/Rekanan/GetTabelRates?kd_rk=${kd_rk}&kd_cb=${kd_cb}`,
                        function (response) {
                            $('#tabelRateDS').val(JSON.stringify(response.Data));
                            $("#TabelRateGrid").getKendoGrid().dataSource.remove(dataItem);
                        }
                    );
                }
            );
        }
    );
}