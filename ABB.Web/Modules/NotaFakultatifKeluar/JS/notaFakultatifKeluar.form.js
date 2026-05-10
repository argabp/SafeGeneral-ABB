$(document).ready(async function () {
    btnSaveNotaFakultatifKeluar_Click();
});

function btnSaveNotaFakultatifKeluar_Click() {
    $('#btn-save-notaFakultatifKeluar').click(function () {
        showProgress('#NotaFakultatifKeluarWindow');
        setTimeout(function () {
            saveNotaFakultatifKeluar('/NotaFakultatifKeluar/SaveNotaFakultatifKeluar')
        }, 500);
    });
}

function saveNotaFakultatifKeluar(url){
    var form = getFormData($('#NotaFakultatifKeluarForm'));

    var grid = $("#DetailNotaFakultatifKeluarGrid").data("kendoGrid");
    var details = grid.dataSource.data();
    form.Details = details;
    
    if(details.length == 0){
        showMessage('Validation Error', "Detail Wajib diisi");
        closeProgress("#NotaFakultatifKeluarWindow");
        return;
    }

    var totalAng = 0;
    for (var i = 0; i < details.length; i++) {
        // Ensure we are adding numbers, default to 0 if null/undefined
        totalAng += parseFloat(details[i].pst_ang || 0);
    }

    if (Math.abs(totalAng - 100) > 0.001) {
        showMessage('Validation Error', "Total Angsuran (%) harus berjumlah 100. Total saat ini: " + totalAng.toFixed(2) + "%");
        closeProgress("#NotaFakultatifKeluarWindow");
        return;
    }
    
    ajaxPost(url, JSON.stringify(form),
        function (response) {
            refreshGrid("#NotaFakultatifKeluarGrid");
            if (response.Result == "OK") {
                showMessage('Success', response.Message);
                closeWindow("#NotaFakultatifKeluarWindow")
            }
            else if (response.Result == "ERROR") {
                showMessage('Error', response.Message);
            }
            else {
                $("#NotaFakultatifKeluarWindow").html(response);
            }

            closeProgress("#NotaFakultatifKeluarWindow");
        }
    );
}

function OnDetailNotaFakultatifKeluarDataBound(e){
    // Get the grid instance
    var grid = e.sender;
    var totalPstAng = 0;
    var totalNilaiAng = 0;

    grid.dataSource.view().forEach(function(dataItem) {
        totalPstAng += dataItem.pst_ang || 0;  // Ensure we sum the value or add 0 if undefined
        totalNilaiAng += dataItem.nilai_ang || 0;  // Ensure we sum the value or add 0 if undefined
    });

    $("#totalPersentaseAngsuran").text(totalPstAng.toFixed(2));
    $("#totalNilaiAngsuran").text(totalNilaiAng.toFixed(2));
}

function dataKodePasDropDown(){
    return {
        kd_grp: $("#kd_grp_pas").val().trim(),
        kd_cb: $("#kd_cb").val().trim(),
    }
}

function dataKodeSumberBisnisDropDown(){
    return {
        kd_grp: $("#kd_grp_sb_bis").val().trim(),
        kd_cb: $("#kd_cb").val().trim(),
    }
}

function openDetailNotaFakultatifKeluarWindow(url, title) {
    openWindow('#DetailNotaFakultatifKeluarWindow', url, title);
}

function onAddDetailNotaFakultatifKeluar() {
    openDetailNotaFakultatifKeluarWindow(`/NotaFakultatifKeluar/AddDetail?kd_cb=${$("#kd_cb").val()}&kd_jns_sor=${$("#kd_jns_sor").val()}&kd_tty_npps=${$("#kd_tty_npps").val()}`, 'Add Detail');
}

function onEditDetailNotaFakultatifKeluar(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    console.log('dataItem', dataItem);
    openDetailNotaFakultatifKeluarWindow(`/NotaFakultatifKeluar/EditDetail?kd_grp_pas=${dataItem.kd_grp_pas}&kd_rk_pas=${dataItem.kd_rk_pas}&kd_grp_sb_bis=${dataItem.kd_grp_sb_bis}&kd_rk_sb_bis=${dataItem.kd_rk_sb_bis}&pst_com=${dataItem.pst_com}&pst_share=${dataItem.pst_share}`, 'Edit Detail');
}

function onDeleteDetailNotaFakultatifKeluar(e){
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    showConfirmation('Confirmation', `Are you sure you want to delete?`,
        function () {

            var grid = $("#DetailNotaFakultatifKeluarGrid").data("kendoGrid");
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

function onSaveDetailNotaFakultatifKeluar(e) {
    var model = e.model;

    // Example validation: pst_ang must be > 0
    if (model.pst_ang <= 0) {
        alert("Nilai Angsuran (pst_ang) harus lebih besar dari 0.");
        e.preventDefault(); // Cancel the save, keep editing
    }
}