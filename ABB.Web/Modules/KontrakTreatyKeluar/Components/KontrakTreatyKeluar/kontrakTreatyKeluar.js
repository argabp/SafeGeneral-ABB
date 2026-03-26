$(document).ready(async function () {
    btnSaveKontrakTreatyKeluar_Click();
    btnNextKontrakTreatyKeluar();
    await setKontrakTreatyKeluarEditedValue();
});

function btnNextKontrakTreatyKeluar(){
    $('#btn-next-kontrakTreatyKeluar').click(function () {
        $("#kontrakTreatyKeluarTab").getKendoTabStrip().select(1);
    });
}

async function setKontrakTreatyKeluarEditedValue(){
    showProgress('#KontrakTreatyKeluarWindow');
    if($("#IsEdit").val() === 'True')
    {
        $("#kd_cb").getKendoDropDownList().readonly(true);
        $("#kd_jns_sor").getKendoDropDownList().readonly(true);
    }

    closeProgress('#KontrakTreatyKeluarWindow');
}

function btnSaveKontrakTreatyKeluar_Click() {
    $('#btn-save-kontrakTreatyKeluar').click(function () {
        showProgress('#KontrakTreatyKeluarWindow');
        setTimeout(function () {
            saveKontrakTreatyKeluar('/KontrakTreatyKeluar/SaveKontrakTreatyKeluar')
        }, 500);
    });
}

function saveKontrakTreatyKeluar(url){
    var form = getFormData($('#KontrakTreatyKeluarForm'));

    var grid = $("#DetailKontrakTreatyKeluarGrid").data("kendoGrid");
    var details = grid.dataSource.data();
    form.Details = details;
    
    if(details.length == 0){
        showMessage('Validation Error', "Detail Wajib diisi");
        closeProgress("#KontrakTreatyKeluarWindow");
        return;
    }

    var totalShare = 0;
    for (var i = 0; i < details.length; i++) {
        // Ensure we are adding numbers, default to 0 if null/undefined
        totalShare += parseFloat(details[i].pst_share || 0);
    }

    if (Math.abs(totalShare - 100) > 0.001) {
        showMessage('Validation Error', "Total Share (%) harus berjumlah 100. Total saat ini: " + totalShare.toFixed(2) + "%");
        closeProgress("#KontrakTreatyKeluarWindow");
        return;
    }
    
    ajaxPost(url, JSON.stringify(form),
        function (response) {
            refreshGrid("#KontrakTreatyKeluarGrid");
            if (response.Result == "OK") {
                showMessage('Success', response.Message);

                $("#btn-next-kontrakTreatyKeluar").prop("disabled", false);
                var tabstrip = $('#kontrakTreatyKeluarTab').data("kendoTabStrip");
                tabstrip.enable(tabstrip.items()[1]);
                tabstrip.enable(tabstrip.items()[2]);
                tabstrip.enable(tabstrip.items()[3]);
                tabstrip.enable(tabstrip.items()[4]);
                tabstrip.enable(tabstrip.items()[5]);

                setTimeout(() => {
                    refreshGrid("#DetailKontrakTreatyKeluarGrid");
                }, 2500)
                
                if (response.Model != undefined) {
                    setKontrakTreatyKeluarModel(response.Model);
                }
            }
            else if (response.Result == "ERROR") {
                showMessage('Error', response.Message);
            }
            else {
                $("#KontrakTreatyKeluarWindow").html(response);
            }

            closeProgress("#KontrakTreatyKeluarWindow");
        }
    );
}

function OnKodeCabangChange(e){
    var value = e.sender._cascadedValue;
    $("#temp_kd_cb").val(value);
}

function setKontrakTreatyKeluarModel(data){
    $("#kd_tty_pps").getKendoTextBox().value(data.kd_tty_pps);
    $("#kd_cb").getKendoDropDownList().readonly(true);
    $("#kd_jns_sor").getKendoDropDownList().readonly(true);
}

function OnDetailKontrakTreatyKeluarDataBound(e){
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

function openDetailKontrakTreatyKeluarWindow(url, title) {
    openWindow('#DetailKontrakTreatyKeluarWindow', url, title);
}

function onAddDetailKontrakTreatyKeluar() {
    openDetailKontrakTreatyKeluarWindow(`/KontrakTreatyKeluar/AddDetail?kd_cb=${$("#kd_cb").val()}&kd_jns_sor=${$("#kd_jns_sor").val()}&kd_tty_npps=${$("#kd_tty_npps").val()}`, 'Add Detail');
}

function onEditDetailKontrakTreatyKeluar(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    console.log('dataItem', dataItem);
    openDetailKontrakTreatyKeluarWindow(`/KontrakTreatyKeluar/EditDetail?kd_grp_pas=${dataItem.kd_grp_pas}&kd_rk_pas=${dataItem.kd_rk_pas}&kd_grp_sb_bis=${dataItem.kd_grp_sb_bis}&kd_rk_sb_bis=${dataItem.kd_rk_sb_bis}&pst_com=${dataItem.pst_com}&pst_share=${dataItem.pst_share}`, 'Edit Detail');
}

function onDeleteDetailKontrakTreatyKeluar(e){
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    showConfirmation('Confirmation', `Are you sure you want to delete?`,
        function () {

            var grid = $("#DetailKontrakTreatyKeluarGrid").data("kendoGrid");
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

function onSaveDetailKontrakTreatyKeluar(e) {
    var model = e.model;

    // Example validation: pst_ang must be > 0
    if (model.pst_ang <= 0) {
        alert("Nilai Angsuran (pst_ang) harus lebih besar dari 0.");
        e.preventDefault(); // Cancel the save, keep editing
    }
}

function OnTahunTreatyChange(e){
    var value = e.sender.value();
    var ddlJenisSor = $("#kd_jns_sor").data("kendoDropDownList");
    var ddlKodeCOB = $("#kd_cob").data("kendoDropDownList");
    ajaxGet(`/KontrakTreatyKeluar/GetNmTtyNpps?kd_cob=${ddlKodeCOB.text()}&nm_jns_sor=${ddlJenisSor.text()}&thn_tty_pps=${value}`, (returnValue) => {
        if(returnValue) {
            var strings = returnValue.split(",");
            $("#nm_tty_pps").getKendoTextBox().value(strings[1]);
        }
    });
}

function OnKodeCOBChange(e){
    var value = e.sender.text();
    var ddlJenisSor = $("#kd_jns_sor").data("kendoDropDownList");
    ajaxGet(`/KontrakTreatyKeluar/GetNmTtyNpps?kd_cob=${value}&nm_jns_sor=${ddlJenisSor.text()}&thn_tty_pps=${$("#thn_tty_pps").val()}`, (returnValue) => {
        if(returnValue) {
            var strings = returnValue.split(",");
            $("#nm_tty_pps").getKendoTextBox().value(strings[1]);
        }
    });
}