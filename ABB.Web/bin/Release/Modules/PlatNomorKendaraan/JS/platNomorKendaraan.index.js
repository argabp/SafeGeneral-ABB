function onSavePlatNomorKendaraan(){    
    var url = "/PlatNomorKendaraan/Save";

    var data = {
        kd_grp_rsk: $("#kd_grp_rsk").val(),
        kd_rsk: $("#kd_rsk").val(),
        desk_rsk: $("#desk_rsk").val(),
        kd_ref: $("#kd_ref").val(),
        kd_ref1: $("#kd_ref1").val()
    }
    showProgress('#PlatNomorKendaraanWindow');
    
    ajaxPost(url, JSON.stringify(data),
        function (response) {
            refreshGrid("#PlatNomorKendaraanGrid");
            if (response.Result == "OK") {
                showMessage('Success', response.Message);
                closeWindow("#PlatNomorKendaraanWindow");
            }
            else if (response.Result == "ERROR")
                showMessage('Error', response.Message);
            else
                $("#PlatNomorKendaraanWindow").html(response);

            closeProgress('#PlatNomorKendaraanWindow');
        }
    );
}

function onDeletePlatNomorKendaraan(e){
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    showConfirmation('Confirmation', `Are you sure you want to delete?`,
        function () {
            showProgressOnGrid('#PlatNomorKendaraanGrid');

            ajaxGet(`/PlatNomorKendaraan/Delete?kd_grp_rsk=${dataItem.kd_grp_rsk.trim()}&kd_rsk=${dataItem.kd_rsk.trim()}`,
                function (response) {
                    if (response.Result == "OK") {
                        showMessage('Success', response.Message);
                    } else
                        showMessage('Error', response.Message);
                    
                    refreshGrid("#PlatNomorKendaraanGrid");
                    closeProgressOnGrid('#PlatNomorKendaraanGrid');
                }
            );
        }
    );
}

function btnAddPlatNomorKendaraan_OnClick() {
    openWindow('#PlatNomorKendaraanWindow', `/PlatNomorKendaraan/Add`, 'Add');
}

function btnEditPlatNomorKendaraan_OnClick(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    console.log('dataItem', dataItem);
    openWindow('#PlatNomorKendaraanWindow', `/PlatNomorKendaraan/Edit?kd_grp_rsk=${dataItem.kd_grp_rsk.trim()}&kd_rsk=${dataItem.kd_rsk.trim()}`, 'Edit');
}