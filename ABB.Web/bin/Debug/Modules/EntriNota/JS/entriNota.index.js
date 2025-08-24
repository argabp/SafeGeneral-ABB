function openEntriNotaWindow(url, title) {
    openWindow('#EntriNotaWindow', url, title);
}

function onEditEntriNota(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    console.log('dataItem', dataItem);
    openEntriNotaWindow(`/EntriNota/Edit?kd_cb=${dataItem.kd_cb}&jns_tr=${dataItem.jns_tr}&jns_nt_msk=${dataItem.jns_nt_msk}&kd_thn=${dataItem.kd_thn}&kd_bln=${dataItem.kd_bln}&no_nt_msk=${dataItem.no_nt_msk}&jns_nt_kel=${dataItem.jns_nt_kel}&no_nt_kel=${dataItem.no_nt_kel}`, 'Edit Nota Tertanggung');
}

function dataKodeTertujuDropDown(){
    return {
        kd_grp_ttj: $("#kd_grp_ttj").val().trim(),
        kd_cb: $("#kd_cb").val().trim(),
        kd_cob: $("#kd_cob").val().trim(),
        kd_scob: $("#kd_scob").val().trim(),
        kd_thn: $("#kd_thn").val().trim(),
        no_pol: $("#no_pol").val().trim(),
        no_updt: $("#no_updt").val()
    }
}

function onEditDetailNota(dataItem){
    if(dataItem.model.isNew()) {
        dataItem.model.no_ang = $("#DetailEntriNotaGrid").getKendoGrid().dataSource.data().reduce((max, current) => {
            return (current.no_ang > max.no_ang) ? current : max;
        }).no_ang;
        dataItem.model.kd_cb = $("#kd_cb").val().trim();
        dataItem.model.jns_tr = $("#jns_tr").val().trim();
        dataItem.model.jns_nt_msk = $("#jns_nt_msk").val().trim();
        dataItem.model.kd_thn = $("#kd_thn").val().trim();
        dataItem.model.kd_bln = $("#kd_bln").val().trim();
        dataItem.model.no_nt_msk = $("#no_nt_msk").val().trim();
        dataItem.model.jns_nt_kel = $("#jns_nt_kel").val().trim();
        dataItem.model.no_nt_kel = $("#no_nt_kel").val().trim();
    }
}

function onDeleteDetailNota(e){
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    showConfirmation('Confirmation', `Are you sure you want to delete?`,
        function () {

            var grid = $("#DetailEntriNotaGrid").data("kendoGrid");
            var datas = grid.dataSource.data();
            for (var data of datas) {

                if (data.no_ang == dataItem.no_ang) {
                    datas.remove(data);
                    break;
                }
            }
        }
    );
}

function onEntriNotaDataBound(e) {
    var grid = $("#EntriNotaGrid").data("kendoGrid");
    grid.tbody.find("tr").each(function() {
        var dataItem = grid.dataItem(this);
        if (dataItem.flag_posting == "Y") {
            $(this).find("a[title='Edit']").hide();
        }
    });
}

function OnKodeRkTTJChange(e){
    ajaxGet(`/EntriNota/GenerateEntriNotaData?kd_rk=${e.sender.value()}&kd_cb=${$("#kd_cb").val()}&kd_grp_rk=${$("#kd_grp_ttj").val()}`, (returnValue) => {
        $("#almt_ttj").getKendoTextArea().value(returnValue.split(",")[1]);
        $("#kt_ttj").getKendoTextBox().value(returnValue.split(",")[4]);
    });
}