$(document).ready(function () {
    searchKeyword_OnKeyUp();
});

function searchFilterMutasiKlaim(e) {
    const gridReq = buildGridRequest(e, "SearchKeyword");

    return {
        grid: gridReq
    };
}

function searchKeyword_OnKeyUp() {
    $('#SearchKeyword').keyup(function () {
        refreshGrid("#MutasiKlaimGrid");
    });
}

function openKlaimAlokasiReasuransiWindow(url, title) {
    openWindow('#KlaimAlokasiReasuransiWindow', url, title);
}

function OnClickViewMutasiKlaim(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    console.log('dataItem', dataItem);
    
    showProgressByElement($("#KlaimAlokasiReasuransiWindow"));
    
    openKlaimAlokasiReasuransiWindow(`/KlaimAlokasiReasuransi/View?kd_cb=${dataItem.kd_cb}&kd_cob=${dataItem.kd_cob}&kd_scob=${dataItem.kd_scob}&kd_thn=${dataItem.kd_thn}&no_kl=${dataItem.no_kl}&no_mts=${dataItem.no_mts}`, 'View');
}

function OnClickEditMutasiKlaim(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    console.log('dataItem', dataItem);

    showProgressByElement($("#KlaimAlokasiReasuransiWindow"));

    openKlaimAlokasiReasuransiWindow(`/KlaimAlokasiReasuransi/Edit?kd_cb=${dataItem.kd_cb}&kd_cob=${dataItem.kd_cob}&kd_scob=${dataItem.kd_scob}&kd_thn=${dataItem.kd_thn}&no_kl=${dataItem.no_kl}&no_mts=${dataItem.no_mts}`, 'Edit');
}

function OnClickEditSOL(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    console.log('dataItem', dataItem);
    
    showProgressByElement($("#KlaimAlokasiReasuransiWindow"));
    
    openKlaimAlokasiReasuransiWindow(`/KlaimAlokasiReasuransi/EditSOL?kd_cb=${dataItem.kd_cb}&kd_cob=${dataItem.kd_cob}&kd_scob=${dataItem.kd_scob}&kd_thn=${dataItem.kd_thn}&no_kl=${dataItem.no_kl}&no_mts=${dataItem.no_mts}&kd_jns_sor=${dataItem.kd_jns_sor}&kd_rk_sor=${dataItem.kd_rk_sor}&kd_grp_sor=${dataItem.kd_grp_sor}`, 'Edit SOL');
}

function OnClickEditSOLXOL(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    console.log('dataItem', dataItem);
    
    showProgressByElement($("#KlaimAlokasiReasuransiWindow"));
    
    openKlaimAlokasiReasuransiWindow(`/KlaimAlokasiReasuransi/EditSOLXOL?kd_cb=${dataItem.kd_cb}&kd_cob=${dataItem.kd_cob}&kd_scob=${dataItem.kd_scob}&kd_thn=${dataItem.kd_thn}&no_kl=${dataItem.no_kl}&no_mts=${dataItem.no_mts}&kd_jns_sor=${dataItem.kd_jns_sor}&kd_kontr=${dataItem.kd_kontr}`, 'Edit SOL XOL');
}

function onDeleteSOL(e){
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
                kd_jns_sor: dataItem.kd_jns_sor,
                kd_grp_sor: dataItem.kd_grp_sor,
                kd_rk_sor: dataItem.kd_rk_sor
            };
            
            var parentId =
                dataItem.kd_cb.trim() + "-" +
                dataItem.kd_cob.trim() + "-" +
                dataItem.kd_scob.trim() + "-" +
                dataItem.kd_thn.trim() + "-" +
                dataItem.no_kl.trim() + "-" +
                dataItem.no_mts;
            
            showProgressOnGrid('#grid_sol_' + parentId);

            ajaxPost("/KlaimAlokasiReasuransi/DeleteSOL", JSON.stringify(payload),
                function (response) {
                    if (response.Result == "OK") {
                        showMessage('Success', response.Message);
                    } else
                        showMessage('Error', response.Message);

                    refreshGrid('#grid_sol_' + parentId);
                    closeProgressOnGrid('#grid_sol_' + parentId);
                }
            );
        }
    );
}

function onDeleteSOLXOL(e){
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
                kd_jns_sor: dataItem.kd_jns_sor,
                kd_kontr: dataItem.kd_kontr
            };
            
            var parentId =
                dataItem.kd_cb.trim() + "-" +
                dataItem.kd_cob.trim() + "-" +
                dataItem.kd_scob.trim() + "-" +
                dataItem.kd_thn.trim() + "-" +
                dataItem.no_kl.trim() + "-" +
                dataItem.no_mts;
            
            showProgressOnGrid('#grid_sol_xol_' + parentId);

            ajaxPost("/KlaimAlokasiReasuransi/DeleteSOLXOL", JSON.stringify(payload),
                function (response) {
                    if (response.Result == "OK") {
                        showMessage('Success', response.Message);
                    } else
                        showMessage('Error', response.Message);

                    refreshGrid('#grid_sol_xol_' + parentId);
                    closeProgressOnGrid('#grid_sol_xol_' + parentId);
                }
            );
        }
    );
}

function OnClickClosingMutasiKlaim(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    openWindow("#ClosingMutasiKlaimiWindow", `/KlaimAlokasiReasuransi/Closing?kd_cb=${dataItem.kd_cb}&kd_cob=${dataItem.kd_cob}&kd_scob=${dataItem.kd_scob}&kd_thn=${dataItem.kd_thn}&no_kl=${dataItem.no_kl}&no_mts=${dataItem.no_mts}`, "Closing");
}

function OnClickAlokasiReasuransi(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    
    showConfirmation('Confirmation', `Are you sure you want to Auto SOL?`,
        function () {
            showProgressOnGrid('#MutasiKlaimGrid');
            setTimeout(function () { alokasiReasuransi(dataItem); }, 500);
        }
    );
}

function alokasiReasuransi(dataItem){
    var form = {};

    form.kd_cb = dataItem.kd_cb;
    form.kd_cob = dataItem.kd_cob;
    form.kd_scob = dataItem.kd_scob;
    form.kd_thn = dataItem.kd_thn;
    form.no_kl = dataItem.no_kl;
    form.no_mts = dataItem.no_mts;
    // form.tgl_closing = kendo.toString(dataItem.tgl_closing, "MM/dd/yyyy");

    var data = JSON.stringify(form);
    ajaxPost(`/KlaimAlokasiReasuransi/AlokasiReasuransi`, data,  function (response) {
        if (response.Result === "OK") {
            showMessage('Alokasi Reasuransi Success', 'Proses alokasi reasuransi selesai');
        }
        else {
            showMessage('Error', 'Alokasi Reasuransi is failed, ' + response.Message);
        };

        refreshGrid("#MutasiKlaimGrid");
        closeProgressOnGrid("#MutasiKlaimGrid");
    });
}

function OnMutasiKlaimDataBound(e){
    var grid = e.sender;

    grid.tbody.find("tr").each(function(e, element) {
        var dataItem = grid.dataItem(this);
        var uid = $(this).data("uid");

        // Find button container - try locked column first, then regular
        var buttonContainer = grid.element.find(".k-grid-content-locked tr[data-uid='" + uid + "'] .k-command-cell");
        if (!buttonContainer.length) {
            buttonContainer = $(this).find(".k-command-cell");
        }

        if (buttonContainer.length) {
            if (dataItem.flag_reas == "Y") {
                // Find the "Closing" button and hide it
                buttonContainer.find("a[title='Alokasi Reasuransi']").hide();
            }
        }
    });

    gridAutoFit(grid);
}

function OnKlaimAlokasiReasuransiDataBound(e){
    var grid = e.sender;
    var data = grid.dataSource.view(); // Gets only the data currently shown in the grid
    var gridElement = grid.element;

    // 1. Find the parent row of this detail grid
    var parentRow = gridElement.closest(".k-detail-row").prev(".k-master-row");
    var parentGrid = $("#MutasiKlaimGrid").data("kendoGrid");
    var parentDataItem = parentGrid.dataItem(parentRow);

    var totalShare = 0;
    var totalNilai = 0;

    // 1. Loop and increment
    for (var i = 0; i < data.length; i++) {
        totalShare += (data[i].pst_share || 0);
        totalNilai += (data[i].nilai_kl || 0);
    }

    // 2. Update the footer spans manually
    // We use .find() to ensure we only update the footer for THIS specific detail grid
    gridElement.find(".sum-pst_share").text(kendo.toString(totalShare, "n6"));
    gridElement.find(".sum-nilai_kl").text(kendo.toString(totalNilai, "n4"));
    
    if (parentDataItem && parentDataItem.flag_reas === "Y") {
        // 2. Hide the "Add" button container
        var parentId = parentDataItem.Id; // or whatever your PK is
        $("#add_btn_sol_" + parentId).hide();

        // 3. Hide the Edit and Delete buttons inside this specific grid
        gridElement.find(".k-grid-EditSOL").hide();
        gridElement.find(".k-grid-DeleteSOL").hide();

        // Optional: Hide the entire Action column header
        gridElement.find("th:eq(0)").hide(); // Assuming Action is the 1st column (index 0)
        gridElement.find("tr td:nth-child(1)").hide();
    }

    gridAutoFit(grid);
}

function OnKlaimAlokasiReasuransiXOLDataBound(e){
    var grid = e.sender;
    var gridElement = grid.element;
    var dataCount = grid.dataSource.total(); // Get total rows in this detail grid

    // 1. Find the parent row of this detail grid
    var parentRow = gridElement.closest(".k-detail-row").prev(".k-master-row");
    var parentGrid = $("#MutasiKlaimGrid").data("kendoGrid");
    var parentDataItem = parentGrid.dataItem(parentRow);
    
    var parentId =
        parentDataItem.kd_cb.trim() + "-" +
        parentDataItem.kd_cob.trim() + "-" +
        parentDataItem.kd_scob.trim() + "-" +
        parentDataItem.kd_thn.trim() + "-" +
        parentDataItem.no_kl.trim() + "-" +
        parentDataItem.no_mts;
    
    if (dataCount > 0) {
        $("#add_btn_xol_" + parentId).hide();
    } else {
        $("#add_btn_xol_" + parentId).show();
    }
    
    if (parentDataItem && parentDataItem.flag_reas === "Y") {
        // 2. Hide the "Add" button container
        var parentId = parentDataItem.Id; // or whatever your PK is
        $("#add_btn_xol_" + parentId).hide();

        // 3. Hide the Edit and Delete buttons inside this specific grid
        gridElement.find(".k-grid-EditSOLXOL").hide();
        gridElement.find(".k-grid-DeleteSOLXOL").hide();

        // Optional: Hide the entire Action column header
        gridElement.find("th:eq(0)").hide(); // Assuming Action is the 1st column (index 0)
        gridElement.find("tr td:nth-child(1)").hide();
    }

    gridAutoFit(grid);
}

function OnClickAddSOL(kd_cb, kd_cob, kd_scob, kd_thn, no_kl, no_mts, nilai_ttl_kl) {
    showProgressByElement($("#KlaimAlokasiReasuransiWindow"));
    
    openKlaimAlokasiReasuransiWindow(`/KlaimAlokasiReasuransi/AddSOL?kd_cb=${kd_cb}&kd_cob=${kd_cob}&kd_scob=${kd_scob}&kd_thn=${kd_thn}&no_kl=${no_kl}&no_mts=${no_mts}&nilai_ttl_kl=${nilai_ttl_kl}`, "Add SOL");
}

function OnClickAddSOLXOL(kd_cb, kd_cob, kd_scob, kd_thn, no_kl, no_mts) {
    showProgressByElement($("#KlaimAlokasiReasuransiWindow"));
    
    openKlaimAlokasiReasuransiWindow(`/KlaimAlokasiReasuransi/AddSOLXOL?kd_cb=${kd_cb}&kd_cob=${kd_cob}&kd_scob=${kd_scob}&kd_thn=${kd_thn}&no_kl=${no_kl}&no_mts=${no_mts}`, "Add SOL XOL");
}
