$(document).ready(function () {
    getDocumentNames();
    initLampiranPengajuanAkseptasiGrid();
    loadLampiranPengajuanAkseptasiDS();
    btnPreviousLampiranPengajuanAkseptasi();
    // btnFinishPesertaLampiran();
    // btnClosePesertaLampiran();
});

function btnPreviousLampiranPengajuanAkseptasi(){
    $('#btn-previous-lampiranPengajuanAkseptasi').click(function () {
        $("#PengajuanAkseptasiTab").getKendoTabStrip().select(0);
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
        kd_cob: $("#kd_cob").val(),
        kd_scob: $("#kd_scob").val(),
        kd_thn: $("#kd_thn").val(),
        no_aks: $("#no_aks").val()
    };
}

var documentNames;

function getDocumentNames() {
    ajaxGet(`/PengajuanAkseptasi/GetDocumentNames`, function (result) {
        documentNames = result;
    });
}

function initLampiranPengajuanAkseptasiGrid() {
    $("#LampiranPengajuanAkseptasiGrid").kendoGrid({
        pageable: false,
        save: saveLampiranPengajuanAkseptasi,
        toolbar: ["create"],
        columns: [
            {
                command: ["edit"], title: "&nbsp;", width: "100px"
            },
            {
                command: { text: "Delete", click: deleteLampiran, iconClass: "k-icon k-i-delete" }, title: "&nbsp;", width: "85px"
            },
            { 
                field: "kd_dokumen", title: "Nama Dokumen", width: 150,
                editor: function (container, options) {
                    if(options.model.no_dokumen === undefined)
                        dropDownEditor({
                            container: container,
                            options: options,
                            data: documentNames,
                            valueField: "Value",
                            textField: "Text"
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
                field: "nm_dokumen", title: "File",
                editor: uploadEditor,
                template: function (dataItem) {
                    var nomor_pengajuan = $("#nomor_pengajuan").val();
                    return `<a href="/pengajuan-akseptasi-attachment/${nomor_pengajuan.replaceAll("/", "")}/${dataItem.nm_dokumen}" target="_blank">${dataItem.nm_dokumen}</a>`
                },
                width: "450px"
            }
        ],
        editable: {
            mode: "inline",
        }
    });
}

function loadLampiranPengajuanAkseptasiDS() {
    var lampiranPengajuanAkseptasi = JSON.parse($('#lampiranPengajuanAkseptasiDS').val());
    loadInlineGridDS({
        gridId: '#LampiranPengajuanAkseptasiGrid',
        arrayObj: lampiranPengajuanAkseptasi,
        fieldKey: "kd_jns_dokumen",
        model: {
            id: "kd_jns_dokumen",
            fields: {
                kd_dokumen: { type: "numeric" },
                nm_dokumen: { type: "string" }
            }
        }
    });
}

function uploadEditor(container, options){
    $('<input name="' + options.field + '" id="' + options.field + '" type="file" multiple="false">')
        .appendTo(container)
}

function saveLampiranPengajuanAkseptasi(e){
    showProgressOnGrid("#LampiranPengajuanAkseptasiGrid");
    
    var kd_cb = $("#kd_cb").val();
    var kd_cob = $("#kd_cob").val();
    var kd_scob = $("#kd_scob").val();
    var kd_thn = $("#kd_thn").val();
    var no_aks = $("#no_aks").val();
    var kd_jns_dokumen = 1;
    var kd_dokumen = e.model.kd_dokumen.Value === undefined ?  e.model.kd_dokumen : parseInt(e.model.kd_dokumen.Value);
    var file = $("#nm_dokumen")[0].files[0];
    
    var form = new FormData();

    form.append("kd_cb", kd_cb);
    form.append("kd_cob", kd_cob);
    form.append("kd_scob", kd_scob);
    form.append("kd_thn", kd_thn);
    form.append("no_aks", no_aks);
    form.append("kd_jns_dokumen", kd_jns_dokumen);
    form.append("kd_dokumen", kd_dokumen);
    form.append("File", file);
    
    ajaxUpload("/PengajuanAkseptasi/SaveLampiranPengajuanAkseptasi", form, function (response) {
        closeProgressOnGrid("#LampiranPengajuanAkseptasiGrid");
        if (response.Result == "OK") {
            showMessage('Success', response.Message);
            refreshGridLampiranPengajuanAkseptasi();
        }
        else if (response.Result == "ERROR")
            showMessage('Error', response.Message);
        else
            $("#tabPengajuanAkseptasi").html(response);
    })
}

function deleteLampiran(event){
    var dataItem = $("#LampiranPengajuanAkseptasiGrid").getKendoGrid().dataItem($(event.target).parent().parent()[0])
    showConfirmation('Confirmation', `Are you sure you want to delete this data?`,
        function () {
            showProgressOnGrid("#LampiranPengajuanAkseptasiGrid")

            var kd_cb = $("#kd_cb").val();
            var kd_cob = $("#kd_cob").val();
            var kd_scob = $("#kd_scob").val();
            var kd_thn = $("#kd_thn").val();
            var no_aks = $("#no_aks").val();

            var form = {};

            form.kd_cb = kd_cb;
            form.kd_cob = kd_cob;
            form.kd_scob = kd_scob;
            form.kd_thn = kd_thn;
            form.no_aks = no_aks;
            form.kd_jns_dokumen = dataItem.kd_jns_dokumen;
            form.kd_dokumen = dataItem.kd_dokumen;

            var data = JSON.stringify(form);

            ajaxPost("/PengajuanAkseptasi/DeleteLampiranPengajuanAkseptasi", data,
                function (response) {
                    if (response.Result == "OK")
                        showMessage('Success', response.Message);
                    else
                        showMessage('Error', response.Message);
                    
                    closeProgressOnGrid("#LampiranPengajuanAkseptasiGrid")
                    refreshGridLampiranPengajuanAkseptasi();
                }
            );
        }
    );
}