function onSaveBiayaPerSubCOB(){    
    var url = "/BiayaPerSubCOB/Save";

    var data = {
        kd_mtu: $("#kd_mtu").val(),
        kd_cob: $("#kd_cob").val(),
        kd_scob: $("#kd_scob").val(),
        nilai_min_prm: $("#nilai_min_prm").val(),
        nilai_bia_pol: $("#nilai_bia_pol").val(),
        nilai_bia_adm: $("#nilai_bia_adm").val(),
        nilai_min_form: $("#nilai_min_form").val(),
        nilai_maks_plafond: $("#nilai_maks_plafond").val(),
    }
    showProgress('#BiayaPerSubCOBWindow');
    
    ajaxPost(url, JSON.stringify(data),
        function (response) {
            refreshGrid("#BiayaPerSubCOBGrid");
            if (response.Result == "OK") {
                showMessage('Success', response.Message);
                closeWindow("#BiayaPerSubCOBWindow");
            }
            else if (response.Result == "ERROR")
                showMessage('Error', response.Message);
            else
                $("#BiayaPerSubCOBWindow").html(response);

            closeProgress('#BiayaPerSubCOBWindow');
        }
    );
}

function onDeleteBiayaPerSubCOB(e){
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    showConfirmation('Confirmation', `Are you sure you want to delete?`,
        function () {
            showProgressOnGrid('#BiayaPerSubCOBGrid');

            ajaxGet(`/BiayaPerSubCOB/Delete?kd_mtu=${dataItem.kd_mtu.trim()}&kd_cob=${dataItem.kd_cob.trim()}&kd_scob=${dataItem.kd_scob.trim()}`,
                function (response) {
                    if (response.Result == "OK") {
                        showMessage('Success', response.Message);
                    } else
                        showMessage('Error', response.Message);
                    
                    refreshGrid("#BiayaPerSubCOBGrid");
                    closeProgressOnGrid('#BiayaPerSubCOBGrid');
                }
            );
        }
    );
}

function btnAddBiayaPerSubCOB_OnClick() {
    openWindow('#BiayaPerSubCOBWindow', `/BiayaPerSubCOB/Add`, 'Add');
}

function btnEditBiayaPerSubCOB_OnClick(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    console.log('dataItem', dataItem);
    openWindow('#BiayaPerSubCOBWindow', `/BiayaPerSubCOB/Edit?kd_mtu=${dataItem.kd_mtu.trim()}&kd_cob=${dataItem.kd_cob.trim()}&kd_scob=${dataItem.kd_scob.trim()}`, 'Edit');
}