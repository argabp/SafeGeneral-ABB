$(document).ready(function () {
    searchKeyword_OnKeyUp();
});

function searchKeyword_OnKeyUp() {
    $('#SearchKeyword').keyup(function () {
        refreshGrid("#LKSGrid");
    });
}

function openLKSWindow(url, title) {
    openWindow('#LKSWindow', url, title);
}

function onEditLKS(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    console.log('dataItem', dataItem);
    openLKSWindow(`/LKS/Edit?kd_cb=${dataItem.kd_cb}&kd_cob=${dataItem.kd_cob}&kd_scob=${dataItem.kd_scob}&kd_thn=${dataItem.kd_thn}&no_kl=${dataItem.no_kl}&no_mts=${dataItem.no_mts}&st_tipe_pla=${dataItem.st_tipe_pla}&no_pla=${dataItem.no_pla}`, 'Edit LKS');
}

function onViewLKS(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    console.log('dataItem', dataItem);
    openLKSWindow(`/LKS/View?kd_cb=${dataItem.kd_cb}&kd_cob=${dataItem.kd_cob}&kd_scob=${dataItem.kd_scob}&kd_thn=${dataItem.kd_thn}&no_kl=${dataItem.no_kl}&no_mts=${dataItem.no_mts}&st_tipe_pla=${dataItem.st_tipe_pla}&no_pla=${dataItem.no_pla}`, 'View LKS');
}

function dataKodePasDropDown(){
    return {
        kd_grp_pas: $("#kd_grp_pas").val().trim()
    }
}

function onLKSDataBound(e) {
    var grid = $("#LKSGrid").data("kendoGrid");
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