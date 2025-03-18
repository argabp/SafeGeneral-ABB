$(document).ready(function () {
    getDocumentNames();
    initDataBackupDokumenGrid();
    loadDataBackupDokumenDS();
    btnPreviousDataBackupDokumen();
    btnCloseDataBackupDokumen();
});

function btnPreviousDataBackupDokumen(){
    $('#btn-previous-dataBackup').click(function () {
        $("#dataBackupTab").getKendoTabStrip().select(0);
    });
}

function btnCloseDataBackupDokumen(){
    $('#btn-close-dataBackupDokumen').click(function () {
        closeWindow("#DataBackupWindow");
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
    ajaxGet(`/Tracking/GetDocumentNames`, function (result) {
        documentNames = result;
    });
}

function initDataBackupDokumenGrid() {
    $("#DataBackupDokumenGrid").kendoGrid({
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
                template: function (data) {
                    return `<a href="/peserta-data-backup/${dataItem.nomor_sppa.replaceAll("/", "")}/${data.dokumen}" target="_blank">${data.dokumen}</a>`
                },
                width: "450px"
            }
        ],
        editable: {
            mode: "inline",
        }
    });
}

function loadDataBackupDokumenDS() {
    var dataBackupDokumenDS = JSON.parse($('#dataBackupDokumenDS').val());
    loadInlineGridDS({
        gridId: '#DataBackupDokumenGrid',
        arrayObj: dataBackupDokumenDS,
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