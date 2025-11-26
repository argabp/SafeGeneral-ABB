$(document).ready(function () {
    searchKeyword_OnKeyUp();
});

function searchKeyword_OnKeyUp() {
    $('#SearchKeyword').keyup(function () {
        refreshGrid("#LKTGrid");
    });
}

function openLKTWindow(url, title) {
    openWindow('#LKTWindow', url, title);
}

function onEditLKT(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    console.log('dataItem', dataItem);
    openLKTWindow(`/LKT/Edit?kd_cb=${dataItem.kd_cb}&kd_cob=${dataItem.kd_cob}&kd_scob=${dataItem.kd_scob}&kd_thn=${dataItem.kd_thn}&no_kl=${dataItem.no_kl}&no_mts=${dataItem.no_mts}&st_tipe_dla=${dataItem.st_tipe_dla}&no_dla=${dataItem.no_dla}`, 'Edit LKT');
}

function onViewLKT(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    console.log('dataItem', dataItem);
    openLKTWindow(`/LKT/View?kd_cb=${dataItem.kd_cb}&kd_cob=${dataItem.kd_cob}&kd_scob=${dataItem.kd_scob}&kd_thn=${dataItem.kd_thn}&no_kl=${dataItem.no_kl}&no_mts=${dataItem.no_mts}&st_tipe_dla=${dataItem.st_tipe_dla}&no_dla=${dataItem.no_dla}`, 'View LKT');
}

function dataKodePasDropDown(){
    return {
        kd_grp_pas: $("#kd_grp_pas").val().trim()
    }
}

function onLKTDataBound(e) {
    var grid = $("#LKTGrid").data("kendoGrid");
    grid.tbody.find("tr").each(function() {
        var dataItem = grid.dataItem(this);
        if (dataItem.flag_posting == "Y") {
            $(this).find("a[title='Edit']").hide();
        }
        if (dataItem.flag_posting == "N") {
            $(this).find("a[title='View']").hide();
        }
    });
}