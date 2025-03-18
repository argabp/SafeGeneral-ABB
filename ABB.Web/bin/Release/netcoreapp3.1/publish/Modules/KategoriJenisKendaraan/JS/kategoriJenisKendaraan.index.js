function onSaveKategoriJenisKendaraan(){    
    var url = "/KategoriJenisKendaraan/Save";

    var data = {
        kd_grp_rsk: $("#kd_grp_rsk").val(),
        kd_rsk: $("#kd_rsk").val(),
        desk_rsk: $("#desk_rsk").val(),
        kd_ref: $("#kd_ref").val(),
        kd_ref1: $("#kd_ref1").val()
    }
    showProgress('#KategoriJenisKendaraanWindow');
    
    ajaxPost(url, JSON.stringify(data),
        function (response) {
            refreshGrid("#KategoriJenisKendaraanGrid");
            if (response.Result == "OK") {
                showMessage('Success', response.Message);
                closeWindow("#KategoriJenisKendaraanWindow");
            }
            else if (response.Result == "ERROR")
                showMessage('Error', response.Message);
            else
                $("#KategoriJenisKendaraanWindow").html(response);

            closeProgress('#KategoriJenisKendaraanWindow');
        }
    );
}

function onDeleteKategoriJenisKendaraan(e){
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    showConfirmation('Confirmation', `Are you sure you want to delete?`,
        function () {
            showProgressOnGrid('#KategoriJenisKendaraanGrid');

            ajaxGet(`/KategoriJenisKendaraan/Delete?kd_grp_rsk=${dataItem.kd_grp_rsk.trim()}&kd_rsk=${dataItem.kd_rsk.trim()}`,
                function (response) {
                    if (response.Result == "OK") {
                        showMessage('Success', response.Message);
                    } else
                        showMessage('Error', response.Message);
                    
                    refreshGrid("#KategoriJenisKendaraanGrid");
                    closeProgressOnGrid('#KategoriJenisKendaraanGrid');
                }
            );
        }
    );
}

function btnAddKategoriJenisKendaraan_OnClick() {
    openWindow('#KategoriJenisKendaraanWindow', `/KategoriJenisKendaraan/Add`, 'Add');
}

function btnEditKategoriJenisKendaraan_OnClick(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    console.log('dataItem', dataItem);
    openWindow('#KategoriJenisKendaraanWindow', `/KategoriJenisKendaraan/Edit?kd_grp_rsk=${dataItem.kd_grp_rsk.trim()}&kd_rsk=${dataItem.kd_rsk.trim()}`, 'Edit');
}