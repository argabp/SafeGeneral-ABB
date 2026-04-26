$(document).ready(function () {
    btnPreviousDetailAlokasi();
    searchKeywordDetailAlokasi_OnKeyUp();
    btnAddDetailAlokasi_Click();

    if($("#IsViewOnly").val() == "True") {
        $("#divAddDetailAlokasi").hide();
    } else{
        $("#divAddDetailAlokasi").show();
    }
});

function btnPreviousDetailAlokasi(){
    $('#btn-previous-detailAlokasi').click(function () {
        $("#AlokasiTab").getKendoTabStrip().select(0);
    });
}

function searchKeywordDetailAlokasi_OnKeyUp() {
    $('#SearchKeywordDetailAlokasi').keyup(function () {
        refreshGrid("#DetailAlokasiGrid");
    });
}

function openDetailAlokasiWindow(url, title) {
    openWindow('#FormAlokasiWindow', url, title);
}


function btnAddDetailAlokasi_Click() {
    $('#btnAddNewDetailAlokasi').click(function () {
        openDetailAlokasiWindow(`/Alokasi/AddDetailAlokasi?kd_cb=${alokasi?.kd_cb.trim()}&kd_cob=${alokasi?.kd_cob.trim()}&kd_scob=${alokasi?.kd_scob.trim()}&kd_thn=${alokasi?.kd_thn.trim()}&no_rsk=${alokasi?.kd_thn.no_rsk}`, 'Add New Alokasi');
    });
}

function btnEditDetailAlokasi_OnClick(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    console.log('dataItem', dataItem);
    openDetailAlokasiWindow(`/Alokasi/EditDetailAlokasi?kd_cb=${dataItem.kd_cb}&kd_cob=${dataItem.kd_cob}
                                    &kd_scob=${dataItem.kd_scob}&kd_thn=${dataItem.kd_thn}
                                    &no_pol=${dataItem.no_pol}&no_updt=${dataItem.no_updt}
                                    &no_rsk=${dataItem.no_rsk}&kd_endt=${dataItem.kd_endt}
                                    &no_updt_reas=${dataItem.no_updt_reas}&kd_jns_sor=${dataItem.kd_jns_sor}
                                    &kd_grp_sor=${dataItem.kd_grp_sor}&kd_rk_sor=${dataItem.kd_rk_sor}
                                    &kd_grp_sb_bis=${dataItem.kd_grp_sb_bis}`, 'Edit Detail Alokasi');
}

function btnViewDetailAlokasi_OnClick(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    console.log('dataItem', dataItem);
    openDetailAlokasiWindow(`/Alokasi/ViewDetailAlokasi?kd_cb=${dataItem.kd_cb}&kd_cob=${dataItem.kd_cob}
                                    &kd_scob=${dataItem.kd_scob}&kd_thn=${dataItem.kd_thn}
                                    &no_pol=${dataItem.no_pol}&no_updt=${dataItem.no_updt}
                                    &no_rsk=${dataItem.no_rsk}&kd_endt=${dataItem.kd_endt}
                                    &no_updt_reas=${dataItem.no_updt_reas}&kd_jns_sor=${dataItem.kd_jns_sor}
                                    &kd_grp_sor=${dataItem.kd_grp_sor}&kd_rk_sor=${dataItem.kd_rk_sor}
                                    &kd_grp_sb_bis=${dataItem.kd_grp_sb_bis}`, 'View Detail Alokasi');
}

function btnDeleteDetailAlokasi_OnClick(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    showConfirmation('Confirmation', `Are you sure you want to delete Detail Alokasi?`,
        function () {
            showProgressOnGrid('#DetailAlokasiGrid');
            setTimeout(function () { deleteDetailAlokasi(dataItem); }, 500);
        }
    );
}
function searchFilterDetailAlokasi(e) {
    const gridReq = buildGridRequest(e, "SearchKeywordDetailAlokasi");

    return {
        grid: gridReq,
        kd_cb: alokasi?.kd_cb,
        kd_cob: alokasi?.kd_cob,
        kd_scob: alokasi?.kd_scob,
        kd_thn: alokasi?.kd_thn,
        no_pol: alokasi?.no_pol,
        no_updt: alokasi?.no_updt,
        no_rsk: alokasi?.no_rsk,
        kd_endt: alokasi?.kd_endt
    };
}

function deleteDetailAlokasi(dataItem) {
    ajaxGet(`/Alokasi/DeleteDetailAlokasi?kd_cb=${dataItem.kd_cb}&kd_cob=${dataItem.kd_cob}
                &kd_scob=${dataItem.kd_scob}&kd_thn=${dataItem.kd_thn}&no_pol=${dataItem.no_pol}
                &no_updt=${dataItem.no_updt}&no_rsk=${dataItem.no_rsk}&kd_endt=${dataItem.kd_endt}
                &no_updt_reas=${dataItem.no_updt_reas}&kd_jns_sor=${dataItem.kd_jns_sor}
                &kd_grp_sor=${dataItem.kd_grp_sor}&kd_rk_sor=${dataItem.kd_rk_sor}
                &kd_grp_sb_bis=${dataItem.kd_grp_sb_bis}`, function (response) {
        if (response.Result) {
            showMessage('Success', 'Data has been deleted');
            refreshGrid("#DetailAlokasiGrid");
        }
        else if (!response.Result) {
            showMessage('Error', 'Delete data is failed, this data is already used');
            refreshGrid("#DetailAlokasiGrid");
        }
        else
            $("#FormAlokasiWindow").html(response);

        closeProgressOnGrid('#DetailAlokasiGrid');
    }, AjaxContentType.URLENCODED);
}

// Format totalNilaiAng as currency (money format)
var currencyFormatter = new Intl.NumberFormat('en-US', {
    style: 'decimal',
    minimumFractionDigits: 3,
    maximumFractionDigits: 3
});

function OnDetailAlokasiDataBound(e) {
    // Get the grid instance
    var grid = e.sender;

    // Find the total rows
    var totalRow = $("#totalDetailAlokasi");

    // Calculate sum of `pst_ang` and `nilai_ang`
    var totalPtgReas = 0;
    var totalPstShare = 0;
    var totalPremi = 0;

    grid.dataSource.view().forEach(function(dataItem) {
        totalPtgReas += dataItem.nilai_ttl_ptg_reas || 0;  // Ensure we sum the value or add 0 if undefined
        totalPstShare += dataItem.pst_share || 0;
        totalPremi += dataItem.nilai_prm_reas || 0;
    });

    // Create the HTML content with two columns for percentage and money
    var formattedHTML = `<div class="row col-sm-12" id="totalAlokasi">` +
        '<h6 class="col-sm-7">Total Share & Premi: </h6>' +
        '<h6 class="col-sm-2">' + currencyFormatter.format(totalPtgReas)  + '</h6>' +
        '<h6 class="col-sm-1">' + totalPstShare.toFixed(2) + "%" + '</h6>' +
        '<h6 class="col-sm-2">' + currencyFormatter.format(totalPremi) + '</h6>' +
        '</div>';

    // Update the content of the totalRow with the formatted HTML
    totalRow.html(formattedHTML);

    grid.tbody.find("tr").each(function(e, element) {
        var uid = $(this).data("uid");

        // Find button container - try locked column first, then regular
        var buttonContainer = grid.element.find(".k-grid-content-locked tr[data-uid='" + uid + "'] .k-command-cell");
        if (!buttonContainer.length) {
            buttonContainer = $(this).find(".k-command-cell");
        }

        if (buttonContainer.length) {
            // Apply your business logic to hide buttons
            // if(userLogin != dataItem.kd_usr_input){
            //     buttonContainer.find(".k-grid-Edit").hide();
            //     buttonContainer.find(".k-grid-Delete").hide();
            // }

            if($("#IsViewOnly").val() == "True") {
                buttonContainer.find(".k-grid-EditDetailAlokasi").hide();
            } else{
                buttonContainer.find(".k-grid-ViewDetailAlokasi").hide();
            }
        }
    });

    gridAutoFit(grid);
}
