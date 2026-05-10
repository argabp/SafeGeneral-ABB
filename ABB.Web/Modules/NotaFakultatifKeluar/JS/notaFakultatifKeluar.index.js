$(document).ready(function () {
    searchKeyword_OnKeyUp();
});

function searchFilterNotaFakultatifKeluar(e) {
    const gridReq = buildGridRequest(e, "SearchKeyword");

    return {
        grid: gridReq
    };
}

function searchFilterDetailNotaFakultatifKeluar(e) {
    const gridReq = buildGridRequest(e);

    return {
        grid: gridReq,
        kd_cb: $("#kd_cb").val(),
        jns_tr: $("#jns_tr").val(),
        jns_nt_msk: $("#jns_nt_msk").val(),
        kd_thn: $("#kd_thn").val(),
        kd_bln: $("#kd_bln").val(),
        no_nt_msk: $("#no_nt_msk").val(),
        jns_nt_kel: $("#jns_nt_kel").val(),
        no_nt_kel: $("#no_nt_kel").val()
    };
}

function searchKeyword_OnKeyUp() {
    $('#SearchKeyword').keyup(function () {
        refreshGrid("#NotaFakultatifKeluarGrid");
    });
}

function openNotaFakultatifKeluarWindow(url, title) {
    openWindow('#NotaFakultatifKeluarWindow', url, title);
}

function onEditNotaFakultatifKeluar(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    console.log('dataItem', dataItem);
    openNotaFakultatifKeluarWindow(`/NotaFakultatifKeluar/Edit?kd_cb=${dataItem.kd_cb}&jns_tr=${dataItem.jns_tr}&jns_nt_msk=${dataItem.jns_nt_msk}&kd_thn=${dataItem.kd_thn}&kd_bln=${dataItem.kd_bln}&no_nt_msk=${dataItem.no_nt_msk}&jns_nt_kel=${dataItem.jns_nt_kel}&no_nt_kel=${dataItem.no_nt_kel}`, 'Edit');
}

function onViewNotaFakultatifKeluar(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    console.log('dataItem', dataItem);
    openNotaFakultatifKeluarWindow(`/NotaFakultatifKeluar/View?kd_cb=${dataItem.kd_cb}&jns_tr=${dataItem.jns_tr}&jns_nt_msk=${dataItem.jns_nt_msk}&kd_thn=${dataItem.kd_thn}&kd_bln=${dataItem.kd_bln}&no_nt_msk=${dataItem.no_nt_msk}&jns_nt_kel=${dataItem.jns_nt_kel}&no_nt_kel=${dataItem.no_nt_kel}`, 'View');
}

function onNotaFakultatifKeluarDataBound(e) {
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