function onSaveBiayaMaterai(){    
    var url = "/BiayaMaterai/Save";

    var data = {
        kd_mtu: $("#kd_mtu").val(),
        nilai_prm_mul: $("#nilai_prm_mul").val(),
        nilai_prm_akh: $("#nilai_prm_akh").val(),
        nilai_bia_mat: $("#nilai_bia_mat").val()
    }
    showProgress('#BiayaMateraiWindow');
    
    ajaxPost(url, JSON.stringify(data),
        function (response) {
            refreshGrid("#BiayaMateraiGrid");
            if (response.Result == "OK") {
                showMessage('Success', response.Message);
                closeWindow("#BiayaMateraiWindow");
            }
            else if (response.Result == "ERROR")
                showMessage('Error', response.Message);
            else
                $("#BiayaMateraiWindow").html(response);

            closeProgress('#BiayaMateraiWindow');
        }
    );
}

function onDeleteBiayaMaterai(e){
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    showConfirmation('Confirmation', `Are you sure you want to delete?`,
        function () {
            showProgressOnGrid('#BiayaMateraiGrid');

            ajaxGet(`/BiayaMaterai/Delete?kd_mtu=${dataItem.kd_mtu.trim()}&nilai_prm_akh=${dataItem.nilai_prm_akh}&nilai_prm_mul=${dataItem.nilai_prm_mul}`,
                function (response) {
                    if (response.Result == "OK") {
                        showMessage('Success', response.Message);
                    } else
                        showMessage('Error', response.Message);
                    
                    refreshGrid("#BiayaMateraiGrid");
                    closeProgressOnGrid('#BiayaMateraiGrid');
                }
            );
        }
    );
}

function btnAddBiayaMaterai_OnClick() {
    openWindow('#BiayaMateraiWindow', `/BiayaMaterai/Add`, 'Add');
}

function btnEditBiayaMaterai_OnClick(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    console.log('dataItem', dataItem);
    openWindow('#BiayaMateraiWindow', `/BiayaMaterai/Edit?kd_mtu=${dataItem.kd_mtu.trim()}&nilai_prm_akh=${dataItem.nilai_prm_akh}&nilai_prm_mul=${dataItem.nilai_prm_mul}`, 'Edit');
}