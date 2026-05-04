$(document).ready(function () {
    searchKeyword_OnKeyUp();
});

function searchFilterNotaPremiTreatyKeluar(e) {
    const gridReq = buildGridRequest(e, "SearchKeyword");

    return {
        grid: gridReq
    };
}

function searchKeyword_OnKeyUp() {
    $('#SearchKeyword').keyup(function () {
        refreshGrid("#NotaPremiTreatyKeluarGrid");
    });
}

function openNotaPremiTreatyKeluarWindow(url, title) {
    openWindow('#NotaPremiTreatyKeluarWindow', url, title);
}

function onEditNotaPremiTreatyKeluar(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    console.log('dataItem', dataItem);
    openNotaPremiTreatyKeluarWindow(`/NotaPremiTreatyKeluar/Edit?kd_cb=${dataItem.kd_cb}&jns_tr=${dataItem.jns_tr}&jns_nt_msk=${dataItem.jns_nt_msk}&kd_thn=${dataItem.kd_thn}&kd_bln=${dataItem.kd_bln}&no_nt_msk=${dataItem.no_nt_msk}&jns_nt_kel=${dataItem.jns_nt_kel}&no_nt_kel=${dataItem.no_nt_kel}`, 'Edit');
}

function onViewNotaPremiTreatyKeluar(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    console.log('dataItem', dataItem);
    openNotaPremiTreatyKeluarWindow(`/NotaPremiTreatyKeluar/View?kd_cb=${dataItem.kd_cb}&jns_tr=${dataItem.jns_tr}&jns_nt_msk=${dataItem.jns_nt_msk}&kd_thn=${dataItem.kd_thn}&kd_bln=${dataItem.kd_bln}&no_nt_msk=${dataItem.no_nt_msk}&jns_nt_kel=${dataItem.jns_nt_kel}&no_nt_kel=${dataItem.no_nt_kel}`, 'View');
}

function dataKodePershDropDown(){
    return {
        kd_grp_pas: $("#kd_grp_pas").val().trim(),
        kd_cb: $("#kd_cb").val().trim()
    }
}

function dataKodeTreatyDropDown(){
    return {
        kd_jns_tty: $("#kd_jns_tty").val().trim(),
        kd_cb: $("#kd_cb").val().trim()
    }
}

function onNotaPremiTreatyKeluarDataBound(e) {
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
            if (dataItem.flag_posting == "Y") {
                // Find the "Closing" button and hide it
                buttonContainer.find("a[title='Edit']").hide();
            }
            if (dataItem.flag_posting == "N") {
                // Find the "Closing" button and hide it
                buttonContainer.find("a[title='View']").hide();
            }
        }
    });

    gridAutoFit(grid);
}