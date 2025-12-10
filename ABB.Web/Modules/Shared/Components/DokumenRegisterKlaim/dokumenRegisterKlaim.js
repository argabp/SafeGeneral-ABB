$(document).ready(function () {
    getDocumentNames();
    initDokumenRegisterKlaimGrid();
    loadDokumenRegisterKlaimDS();
    btnPreviousDokumenRegisterKlaim();
    btnNextDokumenRegisterKlaim();
});

function btnPreviousDokumenRegisterKlaim(){
    $('#btn-previous-dokumenRegisterKlaim').click(function () {
        $("#RegisterKlaimTab").getKendoTabStrip().select(0);
    });
}

function btnNextDokumenRegisterKlaim(){
    $('#btn-next-dokumenRegisterKlaim').click(function () {
        $("#RegisterKlaimTab").getKendoTabStrip().select(2);
    });
}

function parameterDokumen() {
    return {
        kd_cb: $("#kd_cb").val(),
        kd_cob: $("#kd_cob").val(),
        kd_scob: $("#kd_scob").val(),
        kd_thn: $("#kd_thn").val(),
        no_kl: $("#no_kl").val()
    };
}

var documentNames;

function getDocumentNames() {
    ajaxGet(`/RegisterKlaim/GetDocumentNames?kd_cob=${$("#kd_cob").val()}`, function (result) {
        documentNames = result;
    });
}

function initDokumenRegisterKlaimGrid() {
    $("#DokumenRegisterKlaimGrid").kendoGrid({
        pageable: true,
        save: saveDokumenRegisterKlaim,
        toolbar: ["create"],
        columns: [
            {
                command: ["edit"], title: "&nbsp;", width: "100px"
            },
            {
                command: { text: "Delete", click: deleteDokumen, iconClass: "k-icon k-i-delete" }, title: "&nbsp;", width: "85px"
            },
            { 
                field: "kd_dok", title: "Nama Dokumen", width: 150,
                editor: function (container, options) {
                    if(options.model.kd_dok === undefined)
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
                field: "link_file", title: "File",
                editor: uploadEditor,
                template: function (dataItem) {
                    var nomor_register = `${$("#kd_cb").val().trim()}${$("#kd_cob").val().trim()}${$("#kd_scob").val().trim()}${$("#kd_thn").val().trim()}${$("#no_kl").val().trim()}`
                    return `<a href="/dokumen-register-klaim/${nomor_register.replaceAll("/", "")}/${dataItem.link_file}" target="_blank">${dataItem.link_file}</a>`
                },
                width: "450px"
            },
            {
                field: "flag_dok", title: "Berkas Dokumen",
                editor: function (container, options) {
                    // Generate unique IDs for each row to avoid conflicts
                    var uniqueId = options.model.uid || 'temp_' + Date.now();

                    // Create radio group with iCheck structure
                    var radioGroup = $(`
                        <div class="form-group clearfix">
                            <div class="icheck-primary d-inline">
                                <input type="radio" id="${options.field}_ada_${uniqueId}" name="${options.field}" value="Y">
                                <label for="${options.field}_ada_${uniqueId}">Ada</label>
                            </div>
                            <div class="icheck-primary d-inline">
                                <input type="radio" id="${options.field}_tidak_ada_${uniqueId}" name="${options.field}" value="N">
                                <label for="${options.field}_tidak_ada_${uniqueId}">Tidak Ada</label>
                            </div>
                        </div>
                    `).appendTo(container);

                    // Set the initial value based on the model
                    var currentValue = options.model[options.field];
                    if (currentValue === "Y") {
                        radioGroup.find(`#${options.field}_ada_${uniqueId}`).prop('checked', true);
                    } else {
                        radioGroup.find(`#${options.field}_tidak_ada_${uniqueId}`).prop('checked', true);
                    }
                },
                template: function (dataItem) {
                    var result;
                    if (dataItem.flag_dok === "Y") {
                        result = `<p>Ada</p>`;
                    } else {
                        result = `<p>Tidak Ada</p>`;
                    }
                    return result;
                },
                width: "450px"
            }
        ],
        editable: {
            mode: "inline",
        },
        cancel: function(e) {
            var grid = $("#DokumenRegisterKlaimGrid").data("kendoGrid");
            grid.dataSource.read();
        }
    });
}

function loadDokumenRegisterKlaimDS() {
    var dokumenRegisterKlaim = JSON.parse($('#dokumenRegisterKlaimDS').val());
    loadInlineGridDS({
        gridId: '#DokumenRegisterKlaimGrid',
        arrayObj: dokumenRegisterKlaim,
        fieldKey: "kd_dok",
        model: {
            id: "kd_dok",
            fields: {
                kd_dok: { type: "numeric" },
                link_file: { type: "string" },
                flag_dok: { type: "string" },
            }
        }
    });
}

function uploadEditor(container, options){
    $('<input name="' + options.field + '" id="' + options.field + '" type="file" multiple="false">')
        .appendTo(container)
}

function saveDokumenRegisterKlaim(e){
    showProgressOnGrid("#DokumenRegisterKlaimGrid");
    
    var kd_cb = $("#kd_cb").val();
    var kd_cob = $("#kd_cob").val();
    var kd_scob = $("#kd_scob").val();
    var kd_thn = $("#kd_thn").val();
    var no_kl = $("#no_kl").val();
    var kd_dok = e.model.kd_dok?.Value === undefined ?  e.model.kd_dok : e.model.kd_dok.Value;
    var file = $("#link_file")[0].files[0];
    var flag_dok = e.model.flag_dok;
    
    var form = new FormData();

    form.append("kd_cb", kd_cb);
    form.append("kd_cob", kd_cob);
    form.append("kd_scob", kd_scob);
    form.append("kd_thn", kd_thn);
    form.append("no_kl", no_kl);
    form.append("kd_dok", kd_dok);
    form.append("File", file);
    form.append("flag_dok", flag_dok);
    
    ajaxUpload("/RegisterKlaim/SaveDokumenRegisterKlaim", form, function (response) {
        closeProgressOnGrid("#DokumenRegisterKlaimGrid");
        if (response.Result == "OK") {
            showMessage('Success', response.Message);
            refreshGridDokumenRegisterKlaim();
        }
        else if (response.Result == "ERROR")
            showMessage('Error', response.Message);
        else
            $("#tabDokumenRegisterKlaim").html(response);
    })
}

function deleteDokumen(event){
    var dataItem = $("#DokumenRegisterKlaimGrid").getKendoGrid().dataItem($(event.target).parent().parent()[0])
    showConfirmation('Confirmation', `Are you sure you want to delete this data?`,
        function () {
            showProgressOnGrid("#DokumenRegisterKlaimGrid")

            var kd_cb = $("#kd_cb").val();
            var kd_cob = $("#kd_cob").val();
            var kd_scob = $("#kd_scob").val();
            var kd_thn = $("#kd_thn").val();
            var no_kl = $("#no_kl").val();

            var form = {};

            form.kd_cb = kd_cb;
            form.kd_cob = kd_cob;
            form.kd_scob = kd_scob;
            form.kd_thn = kd_thn;
            form.no_kl = no_kl;
            form.kd_dok = dataItem.kd_dok;

            var data = JSON.stringify(form);

            ajaxPost("/RegisterKlaim/DeleteDokumenRegisterKlaim", data,
                function (response) {
                    if (response.Result == "OK")
                        showMessage('Success', response.Message);
                    else
                        showMessage('Error', response.Message);
                    
                    closeProgressOnGrid("#DokumenRegisterKlaimGrid")
                    refreshGridDokumenRegisterKlaim();
                }
            );
        }
    );
}