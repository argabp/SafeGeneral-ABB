$(document).ready(function () {
    searchKeyword_OnKeyUp();
});

var processData;

function searchKeyword_OnKeyUp() {
    $('#SearchKeyword').keyup(function () {
        refreshGrid("#MutasiKlaimGrid");
    });
}

function openMutasiKlaimWindow(url, title) {
    openWindow('#MutasiKlaimWindow', url, title);
}

function openProcessMutasiKlaimWindow(url, title) {
    openWindow('#ProcessMutasiKlaimWindow', url, title);
}

function OnClickInsertDetailMutasiKlaim(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    console.log('dataItem', dataItem);

    showProgressByElement($("#MutasiKlaimWindow"));
    
    openMutasiKlaimWindow(`/MutasiKlaim/Insert?kd_cb=${dataItem.kd_cb}&kd_cob=${dataItem.kd_cob}&kd_scob=${dataItem.kd_scob}&kd_thn=${dataItem.kd_thn}&no_kl=${dataItem.no_kl}`, 'Insert');

}

function OnClickEditMutasiKlaim(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    console.log('dataItem', dataItem);
    
    showProgressByElement($("#MutasiKlaimWindow"));
    
    openMutasiKlaimWindow(`/MutasiKlaim/Edit?kd_cb=${dataItem.kd_cb}&kd_cob=${dataItem.kd_cob}&kd_scob=${dataItem.kd_scob}&kd_thn=${dataItem.kd_thn}&no_kl=${dataItem.no_kl}&no_mts=${dataItem.no_mts}`, 'Edit');

}

function OnClickViewMutasiKlaim(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    console.log('dataItem', dataItem);
    
    showProgressByElement($("#MutasiKlaimWindow"));
    
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
        selected.no_kl.trim();
    
    ajaxGet(`/MutasiKlaim/ObyekView?kd_cb=${selected.kd_cb}&kd_cob=${selected.kd_cob}&kd_scob=${selected.kd_scob}&kd_thn=${selected.kd_thn}&no_kl=${selected.no_kl}&no_mts=${selected.no_mts}&tipe_mts=${selected.tipe_mts}&kd_mtu=${selected.kd_mtu}&flag_closing=${selected.flag_closing}&no_rsk=${selected.no_rsk}`,
        (returnView) => {
            $("#obyekGrid" + parentId).html(returnView);
        });
    
    ajaxGet(`/MutasiKlaim/CheckAlokasi?kd_cb=${selected.kd_cb}&kd_cob=${selected.kd_cob}&kd_scob=${selected.kd_scob}&kd_thn=${selected.kd_thn}&no_kl=${selected.no_kl}&no_mts=${selected.no_mts}&tipe_mts=${selected.tipe_mts}&kd_mtu=${selected.kd_mtu}&flag_closing=${selected.flag_closing}&flag_pol_lama=${selected.flag_pol_lama}&nilai_ttl_kl=${selected.nilai_ttl_kl}`,
        (returnView) => {
            $("#alokasiGrid" + parentId).html(returnView);
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
    
    var parentId =
        parentParams.kd_cb.trim() +
        parentParams.kd_cob.trim() +
        parentParams.kd_scob.trim() +
        parentParams.kd_thn.trim() +
        parentParams.no_kl.trim();

    showProgressOnGrid("#" + grid.element.attr("id"));

    ajaxPost("/MutasiKlaim/" + grid.dataSource.transport.options.create.url, payload, function (response) {
        closeProgressOnGrid("#" + grid.element.attr("id"));
        if (response.Result == "OK") {
            showMessage('Success', response.Message);
        }
        else
            showMessage('Error', response.Message);
        refreshGridWithSelection("#grid_mutasi_" + parentId)
    })
}

function OnEditAlokasi(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    console.log('dataItem', dataItem);
    
    showProgressByElement($("#MutasiKlaimWindow"));
    
    openMutasiKlaimWindow(`/MutasiKlaim/EditAlokasi?kd_cb=${dataItem.kd_cb}&kd_cob=${dataItem.kd_cob}&kd_scob=${dataItem.kd_scob}&kd_thn=${dataItem.kd_thn}&no_kl=${dataItem.no_kl}&no_mts=${dataItem.no_mts}&kd_rk_pas=${dataItem.kd_rk_pas}&kd_grp_pas=${dataItem.kd_grp_pas}`, 'Edit Alokasi');

}

function OnViewAlokasi(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    console.log('dataItem', dataItem);
    showProgressByElement($("#MutasiKlaimWindow"));
    
    openMutasiKlaimWindow(`/MutasiKlaim/ViewAlokasi?kd_cb=${dataItem.kd_cb}&kd_cob=${dataItem.kd_cob}&kd_scob=${dataItem.kd_scob}&kd_thn=${dataItem.kd_thn}&no_kl=${dataItem.no_kl}&no_mts=${dataItem.no_mts}&kd_rk_pas=${dataItem.kd_rk_pas}&kd_grp_pas=${dataItem.kd_grp_pas}`, 'View Alokasi');

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

function OnClickProcessMutasiKlaim(e) {
    e.preventDefault();
    processData = this.dataItem($(e.currentTarget).closest("tr"));

    openProcessMutasiKlaimWindow(`/MutasiKlaim/Process`, 'Process');
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

function OnMutasiKlaimDataBound(e){
    var grid = this;

    grid.tbody.find("tr").each(function(e, element) {
        var dataItem = grid.dataItem(this);
        var uid = $(this).data("uid");

        // Find button container - try locked column first, then regular
        var buttonContainer = grid.element.find(".k-grid-content-locked tr[data-uid='" + uid + "'] .k-command-cell");
        if (!buttonContainer.length) {
            buttonContainer = $(this).find(".k-command-cell");
        }
        
        if (buttonContainer.length) {
            if (dataItem.flag_closing == "Y") {
                // Find the "Closing" button and hide it
                buttonContainer.find("a[title='Edit']").hide();
                buttonContainer.find("a[title='Delete']").hide();
                buttonContainer.find("a[title='Closing']").hide();
            }
            
            if (dataItem.tipe_mts == "D") {
                // Find the "Closing" button and hide it
                buttonContainer.find("a[title='Closing']").hide();

                if(dataItem.flag_proses != "0"){
                    buttonContainer.find("a[title='Process']").hide();
                }
            } else {
                buttonContainer.find("a[title='Process']").hide();
            }
        }
    });

    gridAutoFit(grid);
}

function onEditObyek(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    console.log('dataItem', dataItem);
    
    showProgressByElement($("#MutasiKlaimWindow"));

    openMutasiKlaimWindow(`/MutasiKlaim/EditObyek?kd_cb=${dataItem.kd_cb}&kd_cob=${dataItem.kd_cob}&kd_scob=${dataItem.kd_scob}&kd_thn=${dataItem.kd_thn}&no_kl=${dataItem.no_kl}&no_mts=${dataItem.no_mts}&no_oby=${dataItem.no_oby}`, 'Edit Obyek');

}

function onViewObyek(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    console.log('dataItem', dataItem);
    
    showProgressByElement($("#MutasiKlaimWindow"));

    openMutasiKlaimWindow(`/MutasiKlaim/ViewObyek?kd_cb=${dataItem.kd_cb}&kd_cob=${dataItem.kd_cob}&kd_scob=${dataItem.kd_scob}&kd_thn=${dataItem.kd_thn}&no_kl=${dataItem.no_kl}&no_mts=${dataItem.no_mts}&no_oby=${dataItem.no_oby}`, 'View Obyek');

}

function onDeleteObyek(e){
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    showConfirmation('Confirmation', `Are you sure you want to delete?`,
        function () {

            var parentId =
                dataItem.kd_cb.trim() +
                dataItem.kd_cob.trim() +
                dataItem.kd_scob.trim() +
                dataItem.kd_thn.trim() +
                dataItem.no_kl.trim();

            var payload = {
                kd_cb: dataItem.kd_cb,
                kd_cob: dataItem.kd_cob,
                kd_scob: dataItem.kd_scob,
                kd_thn: dataItem.kd_thn,
                no_kl: dataItem.no_kl,
                no_mts: dataItem.no_mts,
                no_oby: dataItem.no_oby
            };

            showProgressOnGrid('#grid_obyek_' + parentId);

            ajaxPost("/MutasiKlaim/DeleteMutasiKlaimObyek", JSON.stringify(payload),
                function (response) {
                    if (response.Result == "OK") {
                        showMessage('Success', response.Message);
                    } else
                        showMessage('Error', response.Message);

                    refreshGrid('#grid_obyek_' + parentId);
                    closeProgressOnGrid('#grid_obyek_' + parentId);
                }
            );
        }
    );
}

function OnMutasiKlaimObyekDataBound(e){
    var grid = this;

    grid.tbody.find("tr").each(function(e, element) {
        var dataItem = grid.dataItem(this);
        var uid = $(this).data("uid");

        // Find button container - try locked column first, then regular
        var buttonContainer = grid.element.find(".k-grid-content-locked tr[data-uid='" + uid + "'] .k-command-cell");
        if (!buttonContainer.length) {
            buttonContainer = $(this).find(".k-command-cell");
        }

        if (buttonContainer.length) {
            if (dataItem.flag_closing == "Y") {
                // Find the "Closing" button and hide it
                buttonContainer.find("a[title='Edit']").hide();
                buttonContainer.find("a[title='Delete']").hide();
            }
        }
    });

    gridAutoFit(grid);
}

function OnMutasiKlaimAlokasiDataBound(e){
    var grid = this;
    
    var totalPercentage = 0;
    var totalPremi = 0;
    
    var id = undefined;
    
    grid.tbody.find("tr").each(function(e, element) {
        var dataItem = grid.dataItem(this);
        var uid = $(this).data("uid");

        // Find button container - try locked column first, then regular
        var buttonContainer = grid.element.find(".k-grid-content-locked tr[data-uid='" + uid + "'] .k-command-cell");
        if (!buttonContainer.length) {
            buttonContainer = $(this).find(".k-command-cell");
        }

        if (buttonContainer.length) {
            if (dataItem.flag_closing == "Y") {
                // Find the "Closing" button and hide it
                buttonContainer.find("a[title='Edit']").hide();
                buttonContainer.find("a[title='Delete']").hide();
            }
        }
        
        totalPercentage += dataItem.pst_share;
        totalPremi += dataItem.nilai_kl;
        
        if(id == undefined) {
            id = dataItem.kd_cb.trim() + dataItem.kd_cob.trim() +
                dataItem.kd_scob.trim() + dataItem.kd_thn.trim() + dataItem.no_kl.trim();
        }
    });

    $("#totalPercentageMutasiKlaim_" + id).text(totalPercentage.toFixed(2) + "%");
    $("#totalPremiMutasiKlaim_" + id).text( currencyFormatter.format(totalPremi));

    gridAutoFit(grid);
}

// Format totalNilaiAng as currency (money format)
var currencyFormatter = new Intl.NumberFormat('en-US', {
    style: 'decimal',
    minimumFractionDigits: 3,
    maximumFractionDigits: 3
});

function OnMutasiKlaimBebanDataBound(e){
    var grid = this;

    grid.tbody.find("tr").each(function(e, element) {
        var dataItem = grid.dataItem(this);
        var uid = $(this).data("uid");

        // Find button container - try locked column first, then regular
        var buttonContainer = grid.element.find(".k-grid-content-locked tr[data-uid='" + uid + "'] .k-command-cell");
        if (!buttonContainer.length) {
            buttonContainer = $(this).find(".k-command-cell");
        }

        if (buttonContainer.length) {
            if (dataItem.flag_closing == "Y") {
                // Find the "Closing" button and hide it
                buttonContainer.find("a[title='Edit']").hide();
                buttonContainer.find("a[title='Delete']").hide();
            }
        }
    });

    gridAutoFit(grid);
}


function OnClickAddMutasiKlaimRecovery(kd_cb, kd_cob, kd_scob, kd_thn, no_kl, no_mts, tipe_mts, kd_mtu) {
    showProgressByElement($("#MutasiKlaimWindow"));
    
    openMutasiKlaimWindow(`/MutasiKlaim/Recovery?kd_cb=${kd_cb}&kd_cob=${kd_cob}&kd_scob=${kd_scob}&kd_thn=${kd_thn}&no_kl=${no_kl}&no_mts=${no_mts}&tipe_mts=${tipe_mts}&kd_mtu=${kd_mtu}`, "Add Recovery");

}

function onEditRecovery(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    console.log('dataItem', dataItem);

    showProgressByElement($("#MutasiKlaimWindow"));
    
    openMutasiKlaimWindow(`/MutasiKlaim/EditRecovery?kd_cb=${dataItem.kd_cb}&kd_cob=${dataItem.kd_cob}&kd_scob=${dataItem.kd_scob}&kd_thn=${dataItem.kd_thn}&no_kl=${dataItem.no_kl}&no_mts=${dataItem.no_mts}&no_urut=${dataItem.no_urut}`, "Edit Recovery");

}

function onViewRecovery(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    console.log('dataItem', dataItem);
    
    showProgressByElement($("#MutasiKlaimWindow"));

    openMutasiKlaimWindow(`/MutasiKlaim/ViewRecovery?kd_cb=${dataItem.kd_cb}&kd_cob=${dataItem.kd_cob}&kd_scob=${dataItem.kd_scob}&kd_thn=${dataItem.kd_thn}&no_kl=${dataItem.no_kl}&no_mts=${dataItem.no_mts}&no_urut=${dataItem.no_urut}`, "View Recovery");

}

function onDeleteRecovery(e){
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    showConfirmation('Confirmation', `Are you sure you want to delete?`,
        function () {

            var parentId =
                dataItem.kd_cb.trim() +
                dataItem.kd_cob.trim() +
                dataItem.kd_scob.trim() +
                dataItem.kd_thn.trim() +
                dataItem.no_kl.trim();

            var payload = {
                kd_cb: dataItem.kd_cb,
                kd_cob: dataItem.kd_cob,
                kd_scob: dataItem.kd_scob,
                kd_thn: dataItem.kd_thn,
                no_kl: dataItem.no_kl,
                no_mts: dataItem.no_mts,
                no_urut: dataItem.no_urut
            };

            showProgressOnGrid('#grid_obyek_' + parentId);

            ajaxPost("/MutasiKlaim/DeleteMutasiKlaimBeban", JSON.stringify(payload),
                function (response) {
                    if (response.Result == "OK") {
                        showMessage('Success', response.Message);
                    } else
                        showMessage('Error', response.Message);

                    refreshGrid('#grid_obyek_' + parentId);
                    closeProgressOnGrid('#grid_obyek_' + parentId);
                }
            );
        }
    );
}


function OnClickAddMutasiKlaimAlokasi(kd_cb, kd_cob, kd_scob, kd_thn, no_kl, no_mts) {
    showProgressByElement($("#MutasiKlaimWindow"));
    
    openMutasiKlaimWindow(`/MutasiKlaim/Alokasi?kd_cb=${kd_cb}&kd_cob=${kd_cob}&kd_scob=${kd_scob}&kd_thn=${kd_thn}&no_kl=${no_kl}&no_mts=${no_mts}`, 'Add Alokasi');

}

function OnClickCopyAlokasi(kd_cb, kd_cob, kd_scob, kd_thn, no_kl, no_mts, nilai_ttl_kl) {
    showConfirmation('Confirmation', `Are you sure you want to copy alokasi koasuransi?`,
        function () {

            var parentId =
                kd_cb.trim() +
                kd_cob.trim() +
                kd_scob.trim() +
                kd_thn.trim() +
                no_kl.trim();

            var payload = {
                kd_cb: kd_cb,
                kd_cob: kd_cob,
                kd_scob: kd_scob,
                kd_thn: kd_thn,
                no_kl: no_kl,
                no_mts: no_mts,
                nilai_ttl_kl: nilai_ttl_kl
            };

            showProgressOnGrid('#grid_alokasi_' + parentId);

            ajaxPost("/MutasiKlaim/CopyAlokasi", JSON.stringify(payload),
                function (response) {
                    if (response.Result == "OK") {
                        showMessage('Success', "Berhasil Copy Alokasi");
                    } else
                        showMessage('Error', response.Message);

                    refreshGrid('#grid_alokasi_' + parentId);
                    closeProgressOnGrid('#grid_alokasi_' + parentId);
                    refreshGrid('#grid_mutasi_' + parentId);
                }
            );
        }
    );
}

function OnClickCopyAlokasiUnderwriting(kd_cb, kd_cob, kd_scob, kd_thn, no_kl, no_mts) {
    showConfirmation('Confirmation', `Are you sure you want to copy alokasi koasuransi underwriting?`,
        function () {

            var parentId =
                kd_cb.trim() +
                kd_cob.trim() +
                kd_scob.trim() +
                kd_thn.trim() +
                no_kl.trim();

            var payload = {
                kd_cb: kd_cb,
                kd_cob: kd_cob,
                kd_scob: kd_scob,
                kd_thn: kd_thn,
                no_kl: no_kl,
                no_mts: no_mts
            };

            showProgressOnGrid('#grid_alokasi_' + parentId);

            ajaxPost("/MutasiKlaim/CopyAlokasiUnderwriting", JSON.stringify(payload),
                function (response) {
                    if (response.Result == "OK") {
                        showMessage('Success', "Berhasil Copy Alokasi Underwriting");
                    } else
                        showMessage('Error', response.Message);

                    refreshGrid('#grid_alokasi_' + parentId);
                    closeProgressOnGrid('#grid_alokasi_' + parentId);
                    refreshGrid('#grid_mutasi_' + parentId);
                }
            );
        }
    );
}


function OnClickAddMutasiKlaimBeban(kd_cb, kd_cob, kd_scob, kd_thn, no_kl, no_mts, tipe_mts, kd_mtu) {
    showProgressByElement($("#MutasiKlaimWindow"));
    
    openMutasiKlaimWindow(`/MutasiKlaim/Beban?kd_cb=${kd_cb}&kd_cob=${kd_cob}&kd_scob=${kd_scob}&kd_thn=${kd_thn}&no_kl=${no_kl}&no_mts=${no_mts}&tipe_mts=${tipe_mts}&kd_mtu=${kd_mtu}`, "Add Beban");

}

function onEditBeban(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    console.log('dataItem', dataItem);

    showProgressByElement($("#MutasiKlaimWindow"));

    openMutasiKlaimWindow(`/MutasiKlaim/EditBeban?kd_cb=${dataItem.kd_cb}&kd_cob=${dataItem.kd_cob}&kd_scob=${dataItem.kd_scob}&kd_thn=${dataItem.kd_thn}&no_kl=${dataItem.no_kl}&no_mts=${dataItem.no_mts}&no_urut=${dataItem.no_urut}`, "Edit Beban");

}

function onViewBeban(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    console.log('dataItem', dataItem);

    showProgressByElement($("#MutasiKlaimWindow"));
    
    openMutasiKlaimWindow(`/MutasiKlaim/ViewBeban?kd_cb=${dataItem.kd_cb}&kd_cob=${dataItem.kd_cob}&kd_scob=${dataItem.kd_scob}&kd_thn=${dataItem.kd_thn}&no_kl=${dataItem.no_kl}&no_mts=${dataItem.no_mts}&no_urut=${dataItem.no_urut}`, "View Beban");

}

function onDeleteBeban(e){
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    showConfirmation('Confirmation', `Are you sure you want to delete?`,
        function () {

            var parentId =
                dataItem.kd_cb.trim() +
                dataItem.kd_cob.trim() +
                dataItem.kd_scob.trim() +
                dataItem.kd_thn.trim() +
                dataItem.no_kl.trim();

            var payload = {
                kd_cb: dataItem.kd_cb,
                kd_cob: dataItem.kd_cob,
                kd_scob: dataItem.kd_scob,
                kd_thn: dataItem.kd_thn,
                no_kl: dataItem.no_kl,
                no_mts: dataItem.no_mts,
                no_urut: dataItem.no_urut
            };

            showProgressOnGrid('#grid_obyek_' + parentId);

            ajaxPost("/MutasiKlaim/DeleteMutasiKlaimBeban", JSON.stringify(payload),
                function (response) {
                    if (response.Result == "OK") {
                        showMessage('Success', response.Message);
                    } else
                        showMessage('Error', response.Message);

                    refreshGrid('#grid_obyek_' + parentId);
                    closeProgressOnGrid('#grid_obyek_' + parentId);
                }
            );
        }
    );
}

function refreshGridWithSelection(gridSelector) {
    var grid = $(gridSelector).data("kendoGrid");

    if (grid) {
        // Save the selected row's data item ID
        var selectedRow = grid.select();
        var selectedDataItemId = null;

        if (selectedRow.length > 0) {
            var dataItem = grid.dataItem(selectedRow);
            selectedDataItemId = dataItem.id; // Assuming your data has an 'id' field
        }

        // Refresh the grid
        grid.dataSource.read();

        // After refresh, restore selection
        if (selectedDataItemId) {
            // Wait for data to load
            grid.dataSource.one("change", function() {
                var data = grid.dataSource.data();
                for (var i = 0; i < data.length; i++) {
                    if (data[i].id === selectedDataItemId) {
                        grid.select(grid.table.find("tr[data-uid='" + data[i].uid + "']"));
                        break;
                    }
                }
            });
        }
    }
}