function onSaveTarifKendaraanOJK(){
    var url = "/TarifKendaraanOJK/Save";

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
    
    showProgress('#TarifKendaraanOJKWindow');

    ajaxPost(url, JSON.stringify(data),
        function (response) {
            refreshGrid("#TarifKendaraanOJKGrid" + data.kd_kategori.trim());
            if (response.Result == "OK") {
                showMessage('Success', response.Message);
                closeWindow("#TarifKendaraanOJKWindow");
            }
            else if (response.Result == "ERROR")
                showMessage('Error', response.Message);
            else
                $("#TarifKendaraanOJKWindow").html(response);

            closeProgress('#TarifKendaraanOJKWindow');
        }
    );
}

function onDeleteTarifKendaraanOJK(e){
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    showConfirmation('Confirmation', `Are you sure you want to delete?`,
        function () {
            showProgressOnGrid('#TarifKendaraanOJKGrid' + dataItem.kd_kategori.trim());

            ajaxGet(`/TarifKendaraanOJK/Delete?kd_kategori=${dataItem.kd_kategori.trim()}&kd_jns_ptg=${dataItem.kd_jns_ptg.trim()}&kd_wilayah=${dataItem.kd_wilayah.trim()}&no_kategori=${dataItem.no_kategori}`,
                function (response) {
                    if (response.Result == "OK") {
                        showMessage('Success', response.Message);
                    } else
                        showMessage('Error', response.Message);

                    refreshGrid("#TarifKendaraanOJKGrid" + dataItem.kd_kategori.trim());
                    closeProgressOnGrid('#TarifKendaraanOJKGrid' + dataItem.kd_kategori.trim());
                }
            );
        }
    );
}

function btnAddTarifKendaraanOJK_OnClick(kd_kategori) {
    openWindow('#TarifKendaraanOJKWindow', `/TarifKendaraanOJK/Add?kd_kategori=${kd_kategori}`, 'Add');
}

function btnEditTarifKendaraanOJK_OnClick(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    console.log('dataItem', dataItem);
    openWindow('#TarifKendaraanOJKWindow', `/TarifKendaraanOJK/Edit?kd_kategori=${dataItem.kd_kategori.trim()}&kd_jns_ptg=${dataItem.kd_jns_ptg.trim()}&kd_wilayah=${dataItem.kd_wilayah.trim()}&no_kategori=${dataItem.no_kategori}`, 'Edit');
}