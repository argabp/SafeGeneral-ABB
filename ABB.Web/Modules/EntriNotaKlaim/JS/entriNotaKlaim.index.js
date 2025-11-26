$(document).ready(function () {
    searchKeyword_OnKeyUp();
});

function searchKeyword_OnKeyUp() {
    $('#SearchKeyword').keyup(function () {
        refreshGrid("#EntriNotaKlaimGrid");
    });
}

function openEntriNotaKlaimWindow(url, title) {
    openWindow('#EntriNotaKlaimWindow', url, title);
}

function onEditEntriNotaKlaim(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    console.log('dataItem', dataItem);
    openEntriNotaKlaimWindow(`/EntriNotaKlaim/Edit?kd_cb=${dataItem.kd_cb}&jns_tr=${dataItem.jns_tr}&jns_nt_msk=${dataItem.jns_nt_msk}&kd_thn=${dataItem.kd_thn}&kd_bln=${dataItem.kd_bln}&no_nt_msk=${dataItem.no_nt_msk}&jns_nt_kel=${dataItem.jns_nt_kel}&no_nt_kel=${dataItem.no_nt_kel}`, 'Edit Nota Klaim');
}

function onViewEntriNotaKlaim(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    console.log('dataItem', dataItem);
    openEntriNotaKlaimWindow(`/EntriNotaKlaim/View?kd_cb=${dataItem.kd_cb}&jns_tr=${dataItem.jns_tr}&jns_nt_msk=${dataItem.jns_nt_msk}&kd_thn=${dataItem.kd_thn}&kd_bln=${dataItem.kd_bln}&no_nt_msk=${dataItem.no_nt_msk}&jns_nt_kel=${dataItem.jns_nt_kel}&no_nt_kel=${dataItem.no_nt_kel}`, 'View Nota Klaim');
}

function dataKodeTertujuDropDown(){
    return {
        kd_grp_ttj: $("#kd_grp_ttj").val().trim()
    }
}

function onEntriNotaKlaimDataBound(e) {
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

function OnKodeRkTTJChange(e){
    ajaxGet(`/EntriNotaKlaim/GenerateEntriNotaKlaimData?kd_rk=${e.sender.value()}&kd_cb=${$("#kd_cb").val()}&kd_grp_rk=${$("#kd_grp_ttj").val()}`, (returnValue) => {
        $("#nm_ttj").getKendoTextBox().value(returnValue.split(",")[1]);
        $("#almt_ttj").getKendoTextArea().value(returnValue.split(",")[4]);
        $("#kt_ttj").getKendoTextBox().value(returnValue.split(",")[7]);
    });
}