$(document).ready(function () {
    btnPreviousAlokasi();
    searchKeywordAlokasi_OnKeyUp();
    btnAddAkseptasiAlokasi_Click();
});

function btnPreviousAlokasi(){
    $('#btn-previous-akseptasiResikoAlokasi').click(function () {
        $("#resikoTab").getKendoTabStrip().select(2);
    });
}

function searchKeywordAlokasi_OnKeyUp() {
    $('#SearchKeywordAlokasi').keyup(function () {
        refreshGrid("#AlokasiGrid");
    });
}

function openAkseptasiAlokasiWindow(url, title) {
    openWindow('#AlokasiWindow', url, title);
}


function btnAddAkseptasiAlokasi_Click() {
    $('#btnAddNewAkseptasiAlokasi').click(function () {
        openAkseptasiAlokasiWindow(`/Akseptasi/AddDetailAlokasi?kd_cb=${$("#kd_cb").val().trim()}&kd_cob=${$("#kd_cob").val().trim()}&kd_scob=${$("#kd_scob").val().trim()}&kd_thn=${$("#kd_thn").val().trim()}&no_rsk=${resiko.no_rsk}`, 'Add New Alokasi');
    });
}
function btnEditAkseptasiAlokasi_OnClick(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    console.log('dataItem', dataItem);
    openAkseptasiAlokasiWindow(`/Akseptasi/EditDetailAlokasi?kd_cb=${dataItem.kd_cb}&kd_cob=${dataItem.kd_cob}
                                    &kd_scob=${dataItem.kd_scob}&kd_thn=${dataItem.kd_thn}
                                    &no_pol=${dataItem.no_pol}&no_updt=${dataItem.no_updt}
                                    &no_rsk=${dataItem.no_rsk}&kd_endt=${dataItem.kd_endt}
                                    &no_updt_reas=${dataItem.no_updt_reas}&kd_jns_sor=${dataItem.kd_jns_sor}
                                    &kd_grp_sor=${dataItem.kd_grp_sor}&kd_rk_sor=${dataItem.kd_rk_sor}
                                    &kd_grp_sb_bis=${dataItem.kd_grp_sb_bis}`, 'Edit Alokasi');
}
function btnDeleteAkseptasiAlokasi_OnClick(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    showConfirmation('Confirmation', `Are you sure you want to delete Akseptasi Alokasi?`,
        function () {
            showProgressOnGrid('#AlokasiGrid');
            setTimeout(function () { deleteAkseptasiAlokasi(dataItem); }, 500);
        }
    );
}
function searchFilterAlokasi() {
    return {
        searchkeyword: $("#SearchKeywordAlokasi").val(),
        kd_cb: $("#kd_cb").val(),
        kd_cob: $("#kd_cob").val(),
        kd_scob: $("#kd_scob").val(),
        kd_thn: $("#kd_thn").val(),
        no_pol: $("#no_aks").val(),
        no_updt: $("#no_updt").val(),
        no_rsk: resiko?.no_rsk,
        kd_endt: resiko?.kd_endt
    }
}

function deleteAkseptasiAlokasi(dataItem) {
    ajaxGet(`/Akseptasi/DeleteDetailAlokasi?kd_cb=${dataItem.kd_cb}&kd_cob=${dataItem.kd_cob}
                &kd_scob=${dataItem.kd_scob}&kd_thn=${dataItem.kd_thn}&no_pol=${dataItem.no_pol}
                &no_updt=${dataItem.no_updt}&no_rsk=${dataItem.no_rsk}&kd_endt=${dataItem.kd_endt}
                &no_updt_reas=${dataItem.no_updt_reas}&kd_jns_sor=${dataItem.kd_jns_sor}
                &kd_grp_sor=${dataItem.kd_grp_sor}&kd_rk_sor=${dataItem.kd_rk_sor}
                &kd_grp_sb_bis=${dataItem.kd_grp_sb_bis}`, function (response) {
        if (response.Result) {
            showMessage('Success', 'Data has been deleted');
            refreshGrid("#AlokasiGrid");
        }
        else if (!response.Result) {
            showMessage('Error', 'Delete data is failed, this data is already used');
            refreshGrid("#AlokasiGrid");
        }
        else
            $("#AlokasiWindow").html(response);

        closeProgressOnGrid('#AlokasiGrid');
    }, AjaxContentType.URLENCODED);
}

// Format totalNilaiAng as currency (money format)
var currencyFormatter = new Intl.NumberFormat('en-US', {
    style: 'decimal',
    minimumFractionDigits: 3,
    maximumFractionDigits: 3
});

function OnAkseptasiAlokasiDataBound(e) {
    // Get the grid instance
    var grid = e.sender;

    // Find the total rows
    var totalRow = $("#totalAlokasi");

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
        '<h2 class="col-sm-7">Total Share & Premi: ' + currencyFormatter.format(totalPtgReas) + '</h2>' +
        '<h2 class="col-sm-2">' + totalPstShare.toFixed(2) + "%" + '</h2>' +
        '<h2 class="col-sm-2">' + currencyFormatter.format(totalPremi) + '</h2>' +
        '</div>';

    // Update the content of the totalRow with the formatted HTML
    totalRow.html(formattedHTML);
}
