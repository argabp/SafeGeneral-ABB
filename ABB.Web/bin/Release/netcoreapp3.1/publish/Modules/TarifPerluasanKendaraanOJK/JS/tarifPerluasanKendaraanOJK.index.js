function onSaveTarifPerluasanKendaraanOJK(){
    var url = "/TarifPerluasanKendaraanOJK/Save";

    var data = {
        kd_kategori: $("#kd_kategori").val(),
        kd_jns_ptg: $("#kd_jns_ptg").val(),
        kd_wilayah: $("#kd_wilayah").val(),
        no_kategori: parseInt($("#no_kategori").val()),
        nilai_ptg_mul: $("#nilai_ptg_mul").val(),
        nilai_ptg_akh: $("#nilai_ptg_akh").val(),
        stn_rate_prm: $("#stn_rate_prm").val(),
        pst_rate_prm_min: $("#pst_rate_prm_min").val(),
        pst_rate_prm_max: $("#pst_rate_prm_max").val()
    }
    
    showProgress('#TarifPerluasanKendaraanOJKWindow');

    ajaxPost(url, JSON.stringify(data),
        function (response) {
            refreshGrid("#TarifPerluasanKendaraanOJKGrid" + data.kd_kategori.trim());
            if (response.Result == "OK") {
                showMessage('Success', response.Message);
                closeWindow("#TarifPerluasanKendaraanOJKWindow");
            }
            else if (response.Result == "ERROR")
                showMessage('Error', response.Message);
            else
                $("#TarifPerluasanKendaraanOJKWindow").html(response);

            closeProgress('#TarifPerluasanKendaraanOJKWindow');
        }
    );
}

function onDeleteTarifPerluasanKendaraanOJK(e){
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    showConfirmation('Confirmation', `Are you sure you want to delete?`,
        function () {
            showProgressOnGrid('#TarifPerluasanKendaraanOJKGrid' + dataItem.kd_kategori.trim());

            ajaxGet(`/TarifPerluasanKendaraanOJK/Delete?kd_kategori=${dataItem.kd_kategori.trim()}&kd_jns_ptg=${dataItem.kd_jns_ptg.trim()}&kd_wilayah=${dataItem.kd_wilayah.trim()}&no_kategori=${dataItem.no_kategori}`,
                function (response) {
                    if (response.Result == "OK") {
                        showMessage('Success', response.Message);
                    } else
                        showMessage('Error', response.Message);

                    refreshGrid("#TarifPerluasanKendaraanOJKGrid" + dataItem.kd_kategori.trim());
                    closeProgressOnGrid('#TarifPerluasanKendaraanOJKGrid' + dataItem.kd_kategori.trim());
                }
            );
        }
    );
}

function btnAddTarifPerluasanKendaraanOJK_OnClick(kd_kategori) {
    openWindow('#TarifPerluasanKendaraanOJKWindow', `/TarifPerluasanKendaraanOJK/Add?kd_kategori=${kd_kategori}`, 'Add');
}

function btnEditTarifPerluasanKendaraanOJK_OnClick(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    console.log('dataItem', dataItem);
    openWindow('#TarifPerluasanKendaraanOJKWindow', `/TarifPerluasanKendaraanOJK/Edit?kd_kategori=${dataItem.kd_kategori.trim()}&kd_jns_ptg=${dataItem.kd_jns_ptg.trim()}&kd_wilayah=${dataItem.kd_wilayah.trim()}&no_kategori=${dataItem.no_kategori}`, 'Edit');
}