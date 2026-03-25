$(document).ready(async function () {
    btnSaveKontrakTreatyKeluarXOL_Click();
});

function btnSaveKontrakTreatyKeluarXOL_Click() {
    $('#btn-save-kontrakTreatyKeluarXOL').click(function () {
        showProgress('#KontrakTreatyKeluarXOLWindow');
        setTimeout(function () {
            saveKontrakTreatyKeluarXOL('/KontrakTreatyKeluarXOL/SaveKontrakTreatyKeluarXOL')
        }, 500);
    });
}

function saveKontrakTreatyKeluarXOL(url){
    var form = getFormData($('#KontrakTreatyKeluarXOLForm'));

    var grid = $("#DetailKontrakTreatyKeluarXOLGrid").data("kendoGrid");
    var details = grid.dataSource.data();
    form.Details = details;
    
    if(details.length == 0){
        showMessage('Validation Error', "Detail Wajib diisi");
        closeProgress("#KontrakTreatyKeluarXOLWindow");
        return;
    }

    var totalShare = 0;
    for (var i = 0; i < details.length; i++) {
        // Ensure we are adding numbers, default to 0 if null/undefined
        totalShare += parseFloat(details[i].pst_share || 0);
    }

    if (Math.abs(totalShare - 100) > 0.001) {
        showMessage('Validation Error', "Total Share (%) harus berjumlah 100. Total saat ini: " + totalShare.toFixed(2) + "%");
        closeProgress("#KontrakTreatyKeluarXOLWindow");
        return;
    }
    
    ajaxPost(url, JSON.stringify(form),
        function (response) {
            refreshGrid("#KontrakTreatyKeluarXOLGrid");
            if (response.Result == "OK") {
                showMessage('Success', response.Message);
                closeWindow("#KontrakTreatyKeluarXOLWindow")
            }
            else if (response.Result == "ERROR") {
                showMessage('Error', response.Message);
            }
            else {
                $("#KontrakTreatyKeluarXOLWindow").html(response);
            }

            closeProgress("#KontrakTreatyKeluarXOLWindow");
        }
    );
}

function OnDetailKontrakTreatyKeluarXOLDataBound(e){
    // Get the grid instance
    var grid = e.sender;
    var totalPstShare = 0;

    grid.dataSource.view().forEach(function(dataItem) {
        totalPstShare += dataItem.pst_share || 0;  // Ensure we sum the value or add 0 if undefined
    });

    $("#totalPersentaseShare").text(totalPstShare.toFixed(2));
}

function OnKodePasChange(e){
    var kd_rk_pas = $("#kd_rk_pas").data("kendoDropDownList");
    kd_rk_pas.dataSource.read({
        kd_grp : e.sender._cascadedValue,
        kd_cb : $("#kd_cb").val().trim(),
    });
}

function OnKodeSbBisChange(e){
    var kd_rk_sb_bis = $("#kd_rk_sb_bis").data("kendoDropDownList");
    kd_rk_sb_bis.dataSource.read({
        kd_grp : e.sender._cascadedValue,
        kd_cb : $("#kd_cb").val().trim(),
    });
}

function OnKodeCabangChange(e){
    var value = e.sender._cascadedValue;
    $("#temp_kd_cb").val(value);
}

function dataKodePasDropDown(){
    return {
        kd_grp: $("#kd_grp_pas").val().trim(),
        kd_cb: $("#kd_cb").val().trim(),
    }
}

function dataKodeSbBisDropDown(){
    return {
        kd_grp: $("#kd_grp_sb_bis").val().trim(),
        kd_cb: $("#kd_cb").val().trim(),
    }
}

function openDetailKontrakTreatyKeluarXOLWindow(url, title) {
    openWindow('#DetailKontrakTreatyKeluarXOLWindow', url, title);
}

function onAddDetailKontrakTreatyKeluarXOL() {
    openDetailKontrakTreatyKeluarXOLWindow(`/KontrakTreatyKeluarXOL/AddDetail?kd_cb=${$("#kd_cb").val()}&kd_jns_sor=${$("#kd_jns_sor").val()}&kd_tty_npps=${$("#kd_tty_npps").val()}`, 'Add Detail');
}

function onEditDetailKontrakTreatyKeluarXOL(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    console.log('dataItem', dataItem);
    openDetailKontrakTreatyKeluarXOLWindow(`/KontrakTreatyKeluarXOL/EditDetail?kd_grp_pas=${dataItem.kd_grp_pas}&kd_rk_pas=${dataItem.kd_rk_pas}&kd_grp_sb_bis=${dataItem.kd_grp_sb_bis}&kd_rk_sb_bis=${dataItem.kd_rk_sb_bis}&pst_com=${dataItem.pst_com}&pst_share=${dataItem.pst_share}`, 'Edit Detail');
}

function onDeleteDetailKontrakTreatyKeluarXOL(e){
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    showConfirmation('Confirmation', `Are you sure you want to delete?`,
        function () {

            var grid = $("#DetailKontrakTreatyKeluarXOLGrid").data("kendoGrid");
            var datas = grid.dataSource.data();
            for (var data of datas) {

                if (data.Id == dataItem.Id) {
                    datas.remove(data);
                    break;
                }
            }
        }
    );
}

function onSaveDetailKontrakTreatyKeluarXOL(e) {
    var model = e.model;

    // Example validation: pst_ang must be > 0
    if (model.pst_ang <= 0) {
        alert("Nilai Angsuran (pst_ang) harus lebih besar dari 0.");
        e.preventDefault(); // Cancel the save, keep editing
    }
}

function OnKodeCOBChange(e){
    var value = e.sender.text();
    var ddlJenisSor = $("#kd_jns_sor").data("kendoDropDownList");
    ajaxGet(`/KontrakTreatyKeluarXOL/GetNmTtyNpps?kd_cob=${value}&nm_jns_sor=${ddlJenisSor.text()}&npps_layer=${$("#npps_layer").val()}&thn_tty_npps=${$("#thn_tty_npps").val()}`, (returnValue) => {
        if(returnValue) {
            var strings = returnValue.split(",");
            $("#nm_tty_npps").getKendoTextBox().value(strings[1]);
        }
    });
}

function OnJenisSorChange(e){
    var value = e.sender.text();
    var ddlKodeCOB = $("#kd_cob").data("kendoDropDownList");
    ajaxGet(`/KontrakTreatyKeluarXOL/GetNmTtyNpps?kd_cob=${ddlKodeCOB.text()}&nm_jns_sor=${value}&npps_layer=${$("#npps_layer").val()}&thn_tty_npps=${$("#thn_tty_npps").val()}`, (returnValue) => {
        if(returnValue) {
            var strings = returnValue.split(",");
            $("#nm_tty_npps").getKendoTextBox().value(strings[1]);
        }
    });
}

function OnLayerChange(e){
    var value = e.sender.value();
    var ddlJenisSor = $("#kd_jns_sor").data("kendoDropDownList");
    var ddlKodeCOB = $("#kd_cob").data("kendoDropDownList");
    ajaxGet(`/KontrakTreatyKeluarXOL/GetNmTtyNpps?kd_cob=${ddlKodeCOB.text()}&nm_jns_sor=${ddlJenisSor.text()}&npps_layer=${value}&thn_tty_npps=${$("#thn_tty_npps").val()}`, (returnValue) => {
        if(returnValue) {
            var strings = returnValue.split(",");
            $("#nm_tty_npps").getKendoTextBox().value(strings[1]);
        }
    });
}

function OnTahunTreatyChange(e){
    var value = e.sender.value();
    var ddlJenisSor = $("#kd_jns_sor").data("kendoDropDownList");
    var ddlKodeCOB = $("#kd_cob").data("kendoDropDownList");
    ajaxGet(`/KontrakTreatyKeluarXOL/GetNmTtyNpps?kd_cob=${ddlKodeCOB.text()}&nm_jns_sor=${ddlJenisSor.text()}&npps_layer=${$("#npps_layer").val()}&thn_tty_npps=${value}`, (returnValue) => {
        if(returnValue) {
            var strings = returnValue.split(",");
            $("#nm_tty_npps").getKendoTextBox().value(strings[1]);
        }
    });
}
