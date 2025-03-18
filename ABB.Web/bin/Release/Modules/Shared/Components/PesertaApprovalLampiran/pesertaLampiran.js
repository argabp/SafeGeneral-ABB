$(document).ready(function () {
    initPesertaLampiranGrid();
    loadPesertaLampiranDS();
    btnPreviousPesertaLampiran();
    btnClosePesertaLampiran();
});

function btnPreviousPesertaLampiran(){
    $('#btn-previous-pesertaLampiran').click(function () {
        $("#pesertaTab").getKendoTabStrip().select(2);
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

function initPesertaLampiranGrid() {
    $("#PesertaLampiranGrid").kendoGrid({
        pageable: false,
        columns: [
            { 
                field: "no_dokumen", title: "Nama Dokumen", width: 150,
                template: function (dataItem) {
                    return dataItem.dokumenName;
                },
            },
            {
                field: "dokumen", title: "File",
                template: function (dataItem) {
                    var nomor_sppa = $("#nomor_sppa_hidden").val();
                    return `<a href="/peserta-attachment/${nomor_sppa}/${dataItem.dokumen}" target="_blank">${dataItem.dokumen}</a>`
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