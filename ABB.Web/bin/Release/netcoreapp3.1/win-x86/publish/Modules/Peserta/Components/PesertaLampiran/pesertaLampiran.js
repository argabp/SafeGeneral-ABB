$(document).ready(function () {
    getDocumentNames();
    initPesertaLampiranGrid();
    loadPesertaLampiranDS();
    btnPreviousPesertaLampiran();
    btnFinishPesertaLampiran();
    btnClosePesertaLampiran();
});

function btnPreviousPesertaLampiran(){
    $('#btn-previous-pesertaLampiran').click(function () {
        $("#pesertaTab").getKendoTabStrip().select(2);
    });
}

function btnFinishPesertaLampiran(){
    $('#btn-finish-pesertaLampiran').click(function () {
        showProgress("#PesertaWindow")
        
        var kd_cb = $("#kd_cb").val();
        var kd_product = $("#kd_product").val();
        var no_sppa = $("#no_sppa").val();
        var kd_rk = $("#kd_rk").val();
        var kd_thn = $("#kd_thn").val();
        var no_updt = $("#no_updt").val();

        var form = {};

        form.kd_cb = kd_cb;
        form.kd_product = kd_product;
        form.no_sppa = no_sppa;
        form.kd_rk = kd_rk;
        form.kd_thn = kd_thn;
        form.no_updt = no_updt;

        var data = JSON.stringify(form);

        ajaxPost("/Peserta/FinishPeserta", data,
            function (response) {
                if (response.Result == "ERROR")
                    showMessage('Error', response.Message);
                else
                    closeWindow("#PesertaWindow");

                refreshGrid("#PesertaGrid");
                closeProgress("#PesertaWindow")
            }
        );
    });
}

function btnClosePesertaLampiran(){
    $('#btn-close-pesertaLampiran').click(function () {
        closeWindow("#PesertaWindow");
    });
}

function parameterLampiran() {
    return {
        kd_cb: $("#kd_cb").val(),
        kd_product: $("#kd_product").val(),
        no_sppa: $("#no_sppa").val(),
        kd_rk: $("#kd_rk").val(),
        kd_thn: $("#kd_thn").val(),
        no_updt: $("#no_updt").val()
    };
}

var documentNames;

function getDocumentNames() {
    ajaxGet(`/Peserta/GetDocumentNames`, function (result) {
        documentNames = result;
    });
}

function initPesertaLampiranGrid() {
    $("#PesertaLampiranGrid").kendoGrid({
        pageable: false,
        save: savePesertaLampiran,
        toolbar: ["create"],
        columns: [
            {
                command: ["edit"], title: "&nbsp;", width: "100px"
            },
            {
                command: { text: "Delete", click: deleteLampiran, iconClass: "k-icon k-i-delete" }, title: "&nbsp;", width: "85px"
            },
            { 
                field: "no_dokumen", title: "Nama Dokumen", width: 150,
                editor: function (container, options) {
                    if(options.model.no_dokumen === undefined)
                        dropDownEditor({
                            container: container,
                            options: options,
                            data: documentNames,
                            valueField: "no_lookup",
                            textField: "nm_detail_lookup"
                        });
                    else
                        $('<p>' + options.model.dokumenName +  '</p>')
                            .appendTo(container);
                },
                template: function (dataItem) {
                    return dataItem.dokumenName;
                },
            },
            {
                field: "dokumen", title: "File",
                editor: uploadEditor,
                template: function (dataItem) {
                    var noSppa = $("#no_sppa").val();
                    return `<a href="/peserta-attachment/${noSppa}/${dataItem.dokumen}" target="_blank">${dataItem.dokumen}</a>`
                },
                width: "450px"
            }
        ],
        editable: {
            mode: "inline",
        }
    });
}

function loadPesertaLampiranDS() {
    var pesertaLampiranDS = JSON.parse($('#pesertaLampiranDS').val());
    loadInlineGridDS({
        gridId: '#PesertaLampiranGrid',
        arrayObj: pesertaLampiranDS,
        fieldKey: "Id",
        model: {
            id: "Id",
            fields: {
                no_dokumen: { type: "numeric" },
                dokumen: { type: "string" }
            }
        }
    });
}

function uploadEditor(container, options){
    $('<input name="' + options.field + '" id="' + options.field + '" type="file" multiple="false">')
        .appendTo(container)
}

function savePesertaLampiran(e){
    showProgressOnGrid("#PesertaLampiranGrid");
    
    var kd_cb = $("#kd_cb").val();
    var kd_product = $("#kd_product").val();
    var no_sppa = $("#no_sppa").val();
    var kd_rk = $("#kd_rk").val();
    var kd_thn = $("#kd_thn").val();
    var no_updt = $("#no_updt").val();
    var no_dokumen = e.model.no_dokumen.no_lookup === undefined ?  e.model.no_dokumen : parseInt(e.model.no_dokumen.no_lookup);
    var file = $("#dokumen")[0].files[0];
    
    var form = new FormData();

    form.append("kd_cb", kd_cb);
    form.append("kd_product", kd_product);
    form.append("no_sppa", no_sppa);
    form.append("kd_rk", kd_rk);
    form.append("kd_thn", kd_thn);
    form.append("no_updt", no_updt);
    form.append("no_dokumen", no_dokumen);
    form.append("File", file);
    
    ajaxUpload("/Peserta/SavePesertaLampiran", form, function (response) {
        closeProgressOnGrid("#PesertaLampiranGrid");
        if (response.Result == "OK") {
            showMessage('Success', response.Message);
            refreshGridPesertaLampiran();
            refreshGrid("#PesertaGrid");
        }
        else if (response.Result == "ERROR")
            showMessage('Error', response.Message);
        else
            $("#tabPeserta").html(response);
    })
}

function deleteLampiran(event){
    var dataItem = $("#PesertaLampiranGrid").getKendoGrid().dataItem($(event.target).parent().parent()[0])
    showConfirmation('Confirmation', `Are you sure you want to delete this data?`,
        function () {
            showProgressOnGrid("#PesertaLampiranGrid")

            var kd_cb = $("#kd_cb").val();
            var kd_product = $("#kd_product").val();
            var no_sppa = $("#no_sppa").val();
            var kd_rk = $("#kd_rk").val();
            var kd_thn = $("#kd_thn").val();
            var no_updt = $("#no_updt").val();

            var form = {};

            form.kd_cb = kd_cb;
            form.kd_product = kd_product;
            form.no_sppa = no_sppa;
            form.kd_rk = kd_rk;
            form.kd_thn = kd_thn;
            form.no_updt = no_updt;
            form.no_dokumen = dataItem.no_dokumen;

            var data = JSON.stringify(form);

            ajaxPost("/Peserta/DeletePesertaLampiran", data,
                function (response) {
                    if (response.Result == "OK")
                        showMessage('Success', response.Message);
                    else
                        showMessage('Error', response.Message);
                    
                    closeProgressOnGrid("#PesertaLampiranGrid")
                    refreshGridPesertaLampiran();
                    refreshGrid("#PesertaGrid");
                }
            );
        }
    );
}