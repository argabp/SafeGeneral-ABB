$(document).ready(function () {
    searchKeyword_OnKeyUp();
});

function searchKeyword_OnKeyUp() {
    $('#SearchKeyword').keyup(function () {
        refreshGrid("#MutasiKlaimGrid");
    });
}

function openMutasiKlaimWindow(url, title) {
    openWindow('#MutasiKlaimWindow', url, title);
}

function OnClickInsertDetailMutasiKlaim(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    console.log('dataItem', dataItem);
    openMutasiKlaimWindow(`/MutasiKlaim/Insert?kd_cb=${dataItem.kd_cb}&kd_cob=${dataItem.kd_cob}&kd_scob=${dataItem.kd_scob}&kd_thn=${dataItem.kd_thn}&no_kl=${dataItem.no_kl}`, 'Insert');
}

function OnClickViewMutasiKlaim(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    console.log('dataItem', dataItem);
    openMutasiKlaimWindow(`/MutasiKlaim/View?kd_cb=${dataItem.kd_cb}&kd_cob=${dataItem.kd_cob}&kd_scob=${dataItem.kd_scob}&kd_thn=${dataItem.kd_thn}&no_kl=${dataItem.no_kl}&no_mts=${dataItem.no_mts}`, 'View');
}

function OnMutasiChange(e) {
    var grid = e.sender;
    var selected = grid.dataItem(grid.select());
    if (!selected) return;

    // Build the same id as in your C# template
    var parentId =
        selected.kd_cb.trim() +
        selected.kd_cob.trim() +
        selected.kd_scob.trim() +
        selected.kd_thn.trim() +
        selected.no_kl.trim()
    
    $("#tipe_mts" + parentId).val(selected.tipe_mts);
    
    $(`#btnAddAlokasi${parentId}`).prop('disabled', false);
    OnClickAddMutasiKlaimAlokasi(selected);

    var alokasiGridName = "grid_alokasi_" + parentId;
    var alokasiElement = $("#" + alokasiGridName);

    alokasiElement.data("kendoGrid").dataSource.read({
        kd_cb: selected.kd_cb.trim(),
        kd_cob: selected.kd_cob.trim(),
        kd_scob: selected.kd_scob.trim(),
        kd_thn: selected.kd_thn.trim(),
        no_kl: selected.no_kl.trim(),
        no_mts: selected.no_mts,
    })
    
    ajaxGet(`/MutasiKlaim/ObyekView?kd_cb=${selected.kd_cb}&kd_cob=${selected.kd_cob}&kd_scob=${selected.kd_scob}&kd_thn=${selected.kd_thn}&no_kl=${selected.no_kl}&no_mts=${selected.no_mts}&tipe_mts=${selected.tipe_mts}&kd_mtu=${selected.kd_mtu}`,
        (returnView) => {
            $("#obyekGrid" + parentId).html(returnView);
        });
}

function saveMutasiKlaimObyek(e){
    var grid = e.sender;
    var model = e.model;

    // Extract the Mutasi data keys (we can also use grid.dataSource.transport.options.read.data)
    var parentParams = grid.dataSource.transport.options.read.data;

    // Prepare data to send
    var payload = {
        kd_cb: parentParams.kd_cb,
        kd_cob: parentParams.kd_cob,
        kd_scob: parentParams.kd_scob,
        kd_thn: parentParams.kd_thn,
        no_kl: parentParams.no_kl,
        no_mts: parentParams.no_mts,
        ...model.toJSON() // include edited/created data
    };

    showProgressOnGrid("#" + grid.element.attr("id"));

    ajaxPost("/MutasiKlaim/" + grid.dataSource.transport.options.create.url, payload, function (response) {
        closeProgressOnGrid("#" + grid.element.attr("id"));
        if (response.Result == "OK") {
            showMessage('Success', response.Message);
        }
        else
            showMessage('Error', response.Message);
    })
}

function GetObyekGridConfig(data) {
    var tipe = data.tipe_mts;
    var readUrl = "";
    var saveUrl = "";
    var deleteUrl = "";
    var columns = [];
    var modelFields = {};

    switch (tipe) {
        case "P":
        case "D":
            readUrl = "/MutasiKlaim/GetMutasiKlaimObyeks";
            saveUrl = "/MutasiKlaim/SaveMutasiKlaimObyek";
            deleteUrl = "/MutasiKlaim/DeleteMutasiKlaimObyek";
            columns = [
                { field: "no_oby", title: "Nomor Obyek", width: "50px"},
                { field: "nm_oby", title: "Nama Obyek", width: "50px" },
                {
                    field: "nilai_ttl_ptg",
                    title: "Nilai Pertanggungan",
                    format: "{0:n2}",
                    attributes: { class: "text-right" },
                    width: "50px"
                },
                {
                    field: "nilai_kl",
                    title: "Nilai Klaim",
                    format: "{0:n2}",
                    attributes: { class: "text-right" },
                    width: "50px"
                }
            ];
            modelFields = {
                no_oby: { type: "int", editable: false },
                nm_oby: { type: "string" },
                nilai_ttl_ptg: { type: "number", defaultValue: 0 },
                nilai_kl: { type: "number", defaultValue: 0 }
            };
            break;

        case "R":
            
            
            readUrl = "/MutasiKlaim/GetMutasiKlaimBebans";
            saveUrl = "/MutasiKlaim/SaveMutasiKlaimBeban";
            deleteUrl = "/MutasiKlaim/DeleteMutasiKlaimBeban";
            columns = [
                { field: "st_jns", title: "Status", editable: false, width: "150px" },
                { field: "ket_jns", title: "Keterangan", width: "150px" },
                { field: "kd_mtu", title: "Mata Uang", editable: false, width: "150px" },
                {
                    field: "nilai_jns_org",
                    title: "Nilai Recovery",
                    format: "{0:n2}",
                    attributes: { class: "text-right" },
                    width: 150
                }
            ];
            modelFields = {
                st_jns: { type: "string", defaultValue: data.nm_tipe_mts || "" },
                ket_jns: { type: "string" },
                kd_mtu: { type: "string", defaultValue: data.nm_kd_mtu || "" },
                nilai_jns_org: { type: "number", defaultValue: 0 }
            };
            break;
        case "B":
            readUrl = "/MutasiKlaim/GetMutasiKlaimBebans";
            saveUrl = "/MutasiKlaim/SaveMutasiKlaimBeban";
            deleteUrl = "/MutasiKlaim/DeleteMutasiKlaimBeban";
            columns = [
                { field: "st_jns", title: "Status", editable: false, width: "150px" },
                { field: "ket_jns", title: "Keterangan", width: "150px" },
                { field: "kd_mtu", title: "Mata Uang", editable: false, width: "150px" },
                {
                    field: "nilai_jns_org",
                    title: "Nilai Beban",
                    format: "{0:n2}",
                    attributes: { class: "text-right" },
                    width: 150
                }
            ];
            modelFields = {
                st_jns: { type: "string", defaultValue: data.nm_tipe_mts || "" },
                ket_jns: { type: "string" },
                kd_mtu: { type: "string", defaultValue: data.nm_kd_mtu || "" },
                nilai_jns_org: { type: "number", defaultValue: 0 }
            };
            break;

        default:
            readUrl = "/MutasiKlaim/GetMutasiKlaimBebans";
            saveUrl = "/MutasiKlaim/SaveMutasiKlaimBeban";
            deleteUrl = "/MutasiKlaim/DeleteMutasiKlaimBeban";
            columns = [{ field: "fieldX", title: "Default Field" }];
            modelFields = { fieldX: { type: "string" } };
            break;
    }

    return {
        readUrl: readUrl,
        saveUrl: saveUrl,
        deleteUrl: deleteUrl,
        parameters: {
            kd_cb: data.kd_cb.trim(),
            kd_cob: data.kd_cob.trim(),
            kd_scob: data.kd_scob.trim(),
            kd_thn: data.kd_thn.trim(),
            no_kl: data.no_kl.trim(),
            no_mts: parseInt(data.no_mts)
        },
        columns: columns,
        modelFields: modelFields
    };
}

function OnClickAddMutasiKlaimAlokasi(dataItem) {
    var parentId =
        dataItem.kd_cb.trim() +
        dataItem.kd_cob.trim() +
        dataItem.kd_scob.trim() +
        dataItem.kd_thn.trim() +
        dataItem.no_kl.trim()
    $(`#btnAddAlokasi${parentId}`).click(function () {
        openMutasiKlaimWindow(`/MutasiKlaim/Alokasi?kd_cb=${dataItem.kd_cb}&kd_cob=${dataItem.kd_cob}&kd_scob=${dataItem.kd_scob}&kd_thn=${dataItem.kd_thn}&no_kl=${dataItem.no_kl}&no_mts=${dataItem.no_mts}`, 'Add Alokasi');
    });
}

function OnEditAlokasi(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    console.log('dataItem', dataItem);
    openMutasiKlaimWindow(`/MutasiKlaim/EditAlokasi?kd_cb=${dataItem.kd_cb}&kd_cob=${dataItem.kd_cob}&kd_scob=${dataItem.kd_scob}&kd_thn=${dataItem.kd_thn}&no_kl=${dataItem.no_kl}&no_mts=${dataItem.no_mts}&kd_rk_pas=${dataItem.kd_rk_pas}&kd_grp_pas=${dataItem.kd_grp_pas}`, 'Edit Alokasi');
}

function onDeleteAlokasi(e){
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    showConfirmation('Confirmation', `Are you sure you want to delete?`,
        function () {

            // Prepare data to send
            var payload = {
                kd_cb: dataItem.kd_cb,
                kd_cob: dataItem.kd_cob,
                kd_scob: dataItem.kd_scob,
                kd_thn: dataItem.kd_thn,
                no_kl: dataItem.no_kl,
                no_mts: dataItem.no_mts,
                kd_grp_pas: dataItem.kd_grp_pas,
                kd_rk_pas: dataItem.kd_rk_pas
            };
            
            var parentId =
                dataItem.kd_cb.trim() +
                dataItem.kd_cob.trim() +
                dataItem.kd_scob.trim() +
                dataItem.kd_thn.trim() +
                dataItem.no_kl.trim();
            
            showProgressOnGrid('#grid_alokasi_' + parentId);

            ajaxPost("/MutasiKlaim/DeleteMutasiKlaimAlokasi", JSON.stringify(payload),
                function (response) {
                    if (response.Result == "OK") {
                        showMessage('Success', response.Message);
                    } else
                        showMessage('Error', response.Message);

                    refreshGrid('#grid_alokasi_' + parentId);
                    closeProgressOnGrid('#grid_alokasi_' + parentId);
                }
            );
        }
    );
}

function onDeleteMutasiKlaim(e){
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    showConfirmation('Confirmation', `Are you sure you want to delete?`,
        function () {

            // Prepare data to send
            var payload = {
                kd_cb: dataItem.kd_cb,
                kd_cob: dataItem.kd_cob,
                kd_scob: dataItem.kd_scob,
                kd_thn: dataItem.kd_thn,
                no_kl: dataItem.no_kl,
                no_mts: dataItem.no_mts
            };
            
            var parentId =
                dataItem.kd_cb.trim() +
                dataItem.kd_cob.trim() +
                dataItem.kd_scob.trim() +
                dataItem.kd_thn.trim() +
                dataItem.no_kl.trim();
            
            showProgressOnGrid('#grid_mutasi_' + parentId);

            ajaxPost("/MutasiKlaim/DeleteMutasiKlaim", JSON.stringify(payload),
                function (response) {
                    if (response.Result == "OK") {
                        showMessage('Success', response.Message);
                    } else
                        showMessage('Error', response.Message);

                    refreshGrid('#grid_mutasi_' + parentId);
                    closeProgressOnGrid('#grid_mutasi_' + parentId);
                }
            );
        }
    );
}

// Called by the Kendo .Data("dataParameterAlokasi") on the Alokasi grid read
function dataParameterMutasi() {
    // find first mutasi grid that has a selected row
    var $mutasiGridElem = $(".k-grid[id^='grid_mutasi_']").filter(function () {
        var g = $(this).data("kendoGrid");
        return g && g.select && g.select().length > 0;
    }).first();

    if (!$mutasiGridElem || $mutasiGridElem.length === 0) return {};

    var mutasiGrid = $mutasiGridElem.data("kendoGrid");
    var selected = mutasiGrid.dataItem(mutasiGrid.select());
    if (!selected) return {};

    // suffix from the grid id
    var suffix = $mutasiGridElem.attr("id").replace("grid_mutasi_", "");

    return {
        kd_cb: (selected.kd_cb || "").toString().trim(),
        kd_cob: (selected.kd_cob || "").toString().trim(),
        kd_scob: (selected.kd_scob || "").toString().trim(),
        kd_thn: (selected.kd_thn || "").toString().trim(),
        no_kl: (selected.no_kl || "").toString().trim(),
        no_mts: (selected.no_mts == null) ? null : (isNaN(parseInt(selected.no_mts)) ? selected.no_mts : parseInt(selected.no_mts))
    };
}

function OnClickClosingMutasiKlaim(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    
    var parentId =
        dataItem.kd_cb.trim() +
        dataItem.kd_cob.trim() +
        dataItem.kd_scob.trim() +
        dataItem.kd_thn.trim() +
        dataItem.no_kl.trim()
    
    showConfirmation('Confirmation', `Are you sure you want to closing Mutasi Klaim?`,
        function () {
            showProgressOnGrid('#grid_mutasi_' + parentId);
            setTimeout(function () { closingMutasiKlaim(dataItem); }, 500);
        }
    );
}

function closingMutasiKlaim(dataItem){
    var parentId =
        dataItem.kd_cb.trim() +
        dataItem.kd_cob.trim() +
        dataItem.kd_scob.trim() +
        dataItem.kd_thn.trim() +
        dataItem.no_kl.trim()
    
    var form = {};

    form.kd_cb = dataItem.kd_cb;
    form.kd_cob = dataItem.kd_cob;
    form.kd_scob = dataItem.kd_scob;
    form.kd_thn = dataItem.kd_thn;
    form.no_kl = dataItem.no_kl;
    form.no_mts = dataItem.no_mts;
    form.tgl_closing = dataItem.tgl_mts;
    form.kd_usr_input = dataItem.kd_usr_input;

    var data = JSON.stringify(form);
    ajaxPost(`/MutasiKlaim/ClosingMutasiKlaim`, data,  function (response) {
        if (response.Result === "OK") {
            showMessage('Closing Successfully', response.Message);
        }
        else {
            showMessage('Error', 'Clossing is failed, ' + response.Message);
        };

        refreshGrid("#grid_mutasi_" + parentId);
        closeProgressOnGrid('#grid_mutasi_' + parentId);
    });
}