$(document).ready(function () {
    searchKeyword_OnKeyUp();
});

function searchKeyword_OnKeyUp() {
    $('#SearchKeyword').keyup(function () {
        refreshGrid("#EntriNotaGrid");
    });
}

function openEntriNotaWindow(url, title) {
    openWindow('#EntriNotaWindow', url, title);
}

function onEditEntriNota(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    console.log('dataItem', dataItem);
    openEntriNotaWindow(`/EntriNota/Edit?kd_cb=${dataItem.kd_cb}&jns_tr=${dataItem.jns_tr}&jns_nt_msk=${dataItem.jns_nt_msk}&kd_thn=${dataItem.kd_thn}&kd_bln=${dataItem.kd_bln}&no_nt_msk=${dataItem.no_nt_msk}&jns_nt_kel=${dataItem.jns_nt_kel}&no_nt_kel=${dataItem.no_nt_kel}`, 'Edit Nota Tertanggung');
}

function onViewEntriNota(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    console.log('dataItem', dataItem);
    openEntriNotaWindow(`/EntriNota/View?kd_cb=${dataItem.kd_cb}&jns_tr=${dataItem.jns_tr}&jns_nt_msk=${dataItem.jns_nt_msk}&kd_thn=${dataItem.kd_thn}&kd_bln=${dataItem.kd_bln}&no_nt_msk=${dataItem.no_nt_msk}&jns_nt_kel=${dataItem.jns_nt_kel}&no_nt_kel=${dataItem.no_nt_kel}`, 'View Nota Tertanggung');
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

    if (dataItem.container.find("input[name='pst_ang']").length) {
        var numericTextbox = dataItem.container.find("input[name='pst_ang']").data("kendoNumericTextBox");
        if (numericTextbox) {
            numericTextbox.bind("change", function (e) {
                onNotaTertanggungDetailPstAngChange(e);
            });
        }
    }

    if (dataItem.container.find("input[name='nilai_ang']").length) {
        var numericTextbox = dataItem.container.find("input[name='nilai_ang']").data("kendoNumericTextBox");
        if (numericTextbox) {
            numericTextbox.bind("change", function () {
                onNotaTertanggungDetailNilaiAngChange();
            });
        }
    }
}

function onNotaTertanggungDetailPstAngChange(data) {
    var totalAng = 0;
    var totalPstAng = 0;
    var grid = $("#DetailEntriNotaGrid").getKendoGrid();

    var row = $(data.sender.element).closest("tr");
    var dataItem = grid.dataItem(row);
    
    ajaxGet(`/EntriNota/GenerateNilaiAng?pst_ang=${data.sender.value()}&nilai_nt=${$("#nilai_nt").val()}&nilai_ppn=${$("#nilai_ppn").val()}&nilai_pph=${$("#nilai_pph").val()}`, (returnValue) => {
        dataItem.set("nilai_ang", returnValue.split(",")[1]);
        
        grid.dataSource.view().forEach(function(dataItem) {
            totalPstAng += dataItem.pst_ang || 0;  // Ensure we sum the value or add 0 if undefined
            totalAng += dataItem.nilai_ang || 0;
        });

        $("#totalAngsuran").text(currencyFormatter.format(totalAng));
        $("#totalPersentaseAngsuran").text(totalPstAng.toFixed(2));
    });
}

function onNotaTertanggungDetailNilaiAngChange() {
    var totalAng = 0;
    var grid = $("#DetailEntriNotaGrid").getKendoGrid();
    
    grid.dataSource.view().forEach(function(dataItem) {
        totalAng += dataItem.nilai_ang || 0;
    });

    $("#totalAngsuran").text(currencyFormatter.format(totalAng));
}

function onSaveDetailNota(e) {
    var model = e.model;

    // Example validation: pst_ang must be > 0
    if (model.pst_ang <= 0) {
        alert("Nilai Angsuran (pst_ang) harus lebih besar dari 0.");
        e.preventDefault(); // Cancel the save, keep editing
        return;
    }

    // Add other validation rules as needed
    // Example: nilai_ang must be >= pst_ang
    if (model.nilai_ang < model.pst_ang) {
        alert("Jumlah Angsuran (nilai_ang) harus lebih besar atau sama dengan Nilai Angsuran (pst_ang).");
        e.preventDefault();
        return;
    }

    // If all validations pass, the save will proceed normally
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
        if (dataItem.flag_posting == "N") {
            $(this).find("a[title='View']").hide();
        }
    });
}

function OnKodeRkTTJChange(e){
    ajaxGet(`/EntriNota/GenerateEntriNotaData?kd_rk=${e.sender.value()}&kd_cb=${$("#kd_cb").val()}&kd_grp_rk=${$("#kd_grp_ttj").val()}`, (returnValue) => {
        $("#almt_ttj").getKendoTextArea().value(returnValue.split(",")[1]);
        $("#kt_ttj").getKendoTextBox().value(returnValue.split(",")[4]);
    });
}