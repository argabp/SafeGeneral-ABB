function onSavePertanggunganKendaraan(){    
    var url = "/PertanggunganKendaraan/Save";

    var data = {
        kd_cob: $("#kd_cob").val(),
        kd_scob: $("#kd_scob").val(),
        kd_jns_ptg: $("#kd_jns_ptg").val(),
        desk: $("#desk").val(),
        jml_hari: $("#jml_hari").val(),
        ket_klasula: $("#ket_klasula").val(),
        flag_tjh: $("#flag_tjh")[0].checked ? "1" : "0",
        flag_rscc: $("#flag_rscc")[0].checked ? "1" : "0",
        flag_banjir: $("#flag_banjir")[0].checked ? "1" : "0",
        flag_accessories: $("#flag_accessories")[0].checked ? "1" : "0",
        flag_lain_lain01: $("#flag_lain_lain01")[0].checked ? "1" : "0",
        flag_lain_lain02: $("#flag_lain_lain02")[0].checked ? "1" : '0'
    }
    
    showProgress('#PertanggunganKendaraanWindow');
    
    ajaxPost(url, JSON.stringify(data),
        function (response) {
            refreshGrid("#PertanggunganKendaraanGrid");
            if (response.Result == "OK") {
                showMessage('Success', response.Message);
                closeWindow("#PertanggunganKendaraanWindow");
            }
            else if (response.Result == "ERROR")
                showMessage('Error', response.Message);
            else
                $("#PertanggunganKendaraanWindow").html(response);

            closeProgress('#PertanggunganKendaraanWindow');
        }
    );
}

function onDeletePertanggunganKendaraan(e){
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    showConfirmation('Confirmation', `Are you sure you want to delete?`,
        function () {
            showProgressOnGrid('#PertanggunganKendaraanGrid');

            ajaxGet(`/PertanggunganKendaraan/Delete?kd_cob=${dataItem.kd_cob}&kd_scob=${dataItem.kd_scob}&kd_jns_ptg=${dataItem.kd_jns_ptg}`,
                function (response) {
                    if (response.Result == "OK") {
                        showMessage('Success', response.Message);
                    } else
                        showMessage('Error', response.Message);
                    
                    refreshGrid("#PertanggunganKendaraanGrid");
                    closeProgressOnGrid('#PertanggunganKendaraanGrid');
                }
            );
        }
    );
}

function onSaveDetailPertanggunganKendaraan(){
    var url = "/PertanggunganKendaraan/SaveDetail";

    var data = {
        kd_cob: $("#kd_cob_temp").val(),
        kd_scob: $("#kd_scob_temp").val(),
        kd_jns_ptg: $("#kd_jns_ptg_temp").val(),
        no_urut: $("#no_urut").val(),
        nilai_tsi_tjh_mul: $("#nilai_tsi_tjh_mul").val(),
        nilai_tsi_tjh_akh: $("#nilai_tsi_tjh_akh").val(),
        pst_rate_tjh: $("#pst_rate_tjh").val(),
        stn_rate_tjh: $("#stn_rate_tjh").val(),
        nilai_prm_tjh: $("#nilai_prm_tjh").val(),
        nilai_tsi_tjp: $("#nilai_tsi_tjp").val(),
        nilai_prm_tjp: $("#nilai_prm_tjp").val(),
        pst_rate_pad: $("#pst_rate_pad").val(),
        pst_rate_pap: $("#pst_rate_pap").val()
    }
    
    var gridId = data.kd_cob.trim() + data.kd_scob.trim() + data.kd_jns_ptg.trim();

    showProgress('#DetailPertanggunganKendaraanWindow');

    ajaxPost(url, JSON.stringify(data),
        function (response) {
            refreshGrid("#DetailPertanggunganKendaraanGrid" + gridId);
            if (response.Result == "OK") {
                showMessage('Success', response.Message);
                closeWindow("#DetailPertanggunganKendaraanWindow");
            }
            else if (response.Result == "ERROR")
                showMessage('Error', response.Message);
            else
                $("#DetailPertanggunganKendaraanWindow").html(response);

            closeProgress('#DetailPertanggunganKendaraanWindow');
        }
    );
}

function onDeleteDetailPertanggunganKendaraan(e){
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    showConfirmation('Confirmation', `Are you sure you want to delete?`,
        function () {
            var gridId = dataItem.kd_cob.trim() + dataItem.kd_scob.trim() + dataItem.kd_jns_ptg.trim();
            showProgressOnGrid('#DetailPertanggunganKendaraanGrid' + gridId);

            ajaxGet(`/PertanggunganKendaraan/DeleteDetail?kd_cob=${dataItem.kd_cob}&kd_scob=${dataItem.kd_scob}&kd_jns_ptg=${dataItem.kd_jns_ptg}&no_urut=${dataItem.no_urut}`,
                function (response) {
                    if (response.Result == "OK") {
                        showMessage('Success', response.Message);
                    } else
                        showMessage('Error', response.Message);

                    refreshGrid("#DetailPertanggunganKendaraanGrid" + gridId);
                    closeProgressOnGrid('#DetailPertanggunganKendaraanGrid' + gridId);
                }
            );
        }
    );
}

function btnAddPertanggunganKendaraan_OnClick() {
    openWindow('#PertanggunganKendaraanWindow', `/PertanggunganKendaraan/Add`, 'Add');
}

function btnEditPertanggunganKendaraan_OnClick(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    console.log('dataItem', dataItem);
    openWindow('#PertanggunganKendaraanWindow', `/PertanggunganKendaraan/Edit?kd_cob=${dataItem.kd_cob}&kd_scob=${dataItem.kd_scob}&kd_jns_ptg=${dataItem.kd_jns_ptg}`, 'Edit');
}



function btnAddDetailPertanggunganKendaraan_OnClick(kd_cob, kd_scob, kd_jns_ptg) {
    openWindow('#DetailPertanggunganKendaraanWindow', `/PertanggunganKendaraan/AddDetail?kd_cob=${kd_cob}&kd_scob=${kd_scob}&kd_jns_ptg=${kd_jns_ptg}`, 'Add Detail');
}

function btnEditDetailPertanggunganKendaraan_OnClick(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    console.log('dataItem', dataItem);
    openWindow('#DetailPertanggunganKendaraanWindow', `/PertanggunganKendaraan/EditDetail?kd_cob=${dataItem.kd_cob}&kd_scob=${dataItem.kd_scob}&kd_jns_ptg=${dataItem.kd_jns_ptg}&no_urut=${dataItem.no_urut}`, 'Edit Detail');
}