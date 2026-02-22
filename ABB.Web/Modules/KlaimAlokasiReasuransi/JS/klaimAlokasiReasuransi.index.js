$(document).ready(function () {
    searchKeyword_OnKeyUp();
});
var mutasiKlaim;

var processData;

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
    
    showConfirmation('Confirmation', `Are you sure you want to closing SOL?`,
        function () {
            showProgressOnGrid('#MutasiKlaimGrid');
            setTimeout(function () { closingMutasiKlaim(dataItem); }, 500);
        }
    );
}

function closingMutasiKlaim(dataItem){    
    var form = {};

    form.kd_cb = dataItem.kd_cb;
    form.kd_cob = dataItem.kd_cob;
    form.kd_scob = dataItem.kd_scob;
    form.kd_thn = dataItem.kd_thn;
    form.no_kl = dataItem.no_kl;
    form.no_mts = dataItem.no_mts;
    // form.tgl_closing = kendo.toString(dataItem.tgl_closing, "MM/dd/yyyy");

    var data = JSON.stringify(form);
    ajaxPost(`/KlaimAlokasiReasuransi/ClosingKlaimAlokasiReasuransi`, data,  function (response) {
        if (response.Result === "OK") {
            showMessage('Closing Successfully', response.Message);
        }
        else {
            showMessage('Error', 'Clossing is failed, ' + response.Message);
        };

        refreshGrid("#MutasiKlaimGrid");
        closeProgressOnGrid('#MutasiKlaimGrid');
    });
}

function OnClickAlokasiReasuransi(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    
    var parentId =
        dataItem.kd_cb.trim() + "-" + 
        dataItem.kd_cob.trim() + "-" + 
        dataItem.kd_scob.trim() + "-" + 
        dataItem.kd_thn.trim() + "-" + 
        dataItem.no_kl.trim()+ "-" + 
        dataItem.no_mts;
    
    showConfirmation('Confirmation', `Are you sure you want to Auto SOL?`,
        function () {
            showProgressOnGrid('#grid_sol_' + parentId);
            setTimeout(function () { alokasiReasuransi(dataItem); }, 500);
        }
    );
}

function alokasiReasuransi(dataItem){
    var parentId =
        dataItem.kd_cb.trim() + "-" + 
        dataItem.kd_cob.trim() + "-" + 
        dataItem.kd_scob.trim() + "-" + 
        dataItem.kd_thn.trim() + "-" + 
        dataItem.no_kl.trim()+ "-" + 
        dataItem.no_mts;
    
    var form = {};

    form.kd_cb = dataItem.kd_cb;
    form.kd_cob = dataItem.kd_cob;
    form.kd_scob = dataItem.kd_scob;
    form.kd_thn = dataItem.kd_thn;
    form.no_kl = dataItem.no_kl;
    form.no_mts = dataItem.no_mts;
    // form.tgl_closing = kendo.toString(dataItem.tgl_closing, "MM/dd/yyyy");

    var data = JSON.stringify(form);
    ajaxPost(`/KlaimAlokasiReasuransi/ClosingKlaimAlokasiReasuransi`, data,  function (response) {
        if (response.Result === "OK") {
            showMessage('Closing Successfully', response.Message);
        }
        else {
            showMessage('Error', 'Clossing is failed, ' + response.Message);
        };

        refreshGrid("#grid_sol_" + parentId);
        closeProgressOnGrid('#grid_sol_' + parentId);
    });
}

function OnKlaimAlokasiReasuransiDataBound(e){
    var grid = this;

    // grid.tbody.find("tr").each(function(e, element) {
    //     var dataItem = grid.dataItem(this);
    //     var uid = $(this).data("uid");

    //     // Find button container - try locked column first, then regular
    //     var buttonContainer = grid.element.find(".k-grid-content-locked tr[data-uid='" + uid + "'] .k-command-cell");
    //     if (!buttonContainer.length) {
    //         buttonContainer = $(this).find(".k-command-cell");
    //     }
        
    //     if (buttonContainer.length) {
    //         if (dataItem.flag_closing == "Y") {
    //             // Find the "Closing" button and hide it
    //             buttonContainer.find("a[title='Edit']").hide();
    //             buttonContainer.find("a[title='Delete']").hide();
    //             buttonContainer.find("a[title='Closing']").hide();
    //         }
            
    //         if (dataItem.tipe_mts == "D") {
    //             // Find the "Closing" button and hide it
    //             buttonContainer.find("a[title='Closing']").hide();

    //             if(dataItem.flag_proses != "0"){
    //                 buttonContainer.find("a[title='Process']").hide();
    //             }
    //         } else {
    //             buttonContainer.find("a[title='Process']").hide();
    //         }
    //     }
    // });

    gridAutoFit(grid);
}

function OnKlaimAlokasiReasuransiXLDataBound(e){
    var grid = this;

    // grid.tbody.find("tr").each(function(e, element) {
    //     var dataItem = grid.dataItem(this);
    //     var uid = $(this).data("uid");

    //     // Find button container - try locked column first, then regular
    //     var buttonContainer = grid.element.find(".k-grid-content-locked tr[data-uid='" + uid + "'] .k-command-cell");
    //     if (!buttonContainer.length) {
    //         buttonContainer = $(this).find(".k-command-cell");
    //     }

    //     if (buttonContainer.length) {
    //         if (mutasiKlaim.flag_closing == "Y") {
    //             // Find the "Closing" button and hide it
    //             buttonContainer.find("a[title='Edit']").hide();
    //             buttonContainer.find("a[title='Delete']").hide();
    //         }
    //     }
    // });

    gridAutoFit(grid);
}

function OnClickAddSOL(kd_cb, kd_cob, kd_scob, kd_thn, no_kl, no_mts) {
    showProgressByElement($("#KlaimAlokasiReasuransiWindow"));
    
    openKlaimAlokasiReasuransiWindow(`/KlaimAlokasiReasuransi/AddSOL?kd_cb=${kd_cb}&kd_cob=${kd_cob}&kd_scob=${kd_scob}&kd_thn=${kd_thn}&no_kl=${no_kl}&no_mts=${no_mts}`, "Add SOL");
}

function OnClickAddSOLXOL(kd_cb, kd_cob, kd_scob, kd_thn, no_kl, no_mts) {
    showProgressByElement($("#KlaimAlokasiReasuransiWindow"));
    
    openKlaimAlokasiReasuransiWindow(`/KlaimAlokasiReasuransi/AddSOLXOL?kd_cb=${kd_cb}&kd_cob=${kd_cob}&kd_scob=${kd_scob}&kd_thn=${kd_thn}&no_kl=${no_kl}&no_mts=${no_mts}`, "Add SOL");
}
