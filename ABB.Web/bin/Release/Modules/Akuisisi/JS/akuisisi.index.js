function onSaveAkuisisi(){    
    var url = "/Akuisisi/Save";

    var data = {
        kd_mtu: $("#kd_mtu").val(),
        kd_cob: $("#kd_cob").val(),
        kd_scob: $("#kd_scob").val(),
        kd_thn: parseInt($("#kd_thn").val()),
        nilai_min_acq: $("#nilai_min_acq").val(),
        nilai_maks_acq: $("#nilai_maks_acq").val(),
        nilai_acq: $("#nilai_acq").val()
    }
    showProgress('#AkuisisiWindow');
    
    ajaxPost(url, JSON.stringify(data),
        function (response) {
            refreshGrid("#AkuisisiGrid");
            if (response.Result == "OK") {
                showMessage('Success', response.Message);
                closeWindow("#AkuisisiWindow");
            }
            else if (response.Result == "ERROR")
                showMessage('Error', response.Message);
            else
                $("#AkuisisiWindow").html(response);

            closeProgress('#AkuisisiWindow');
        }
    );
}

function onDeleteAkuisisi(e){
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    showConfirmation('Confirmation', `Are you sure you want to delete?`,
        function () {
            showProgressOnGrid('#AkuisisiGrid');

            ajaxGet(`/Akuisisi/Delete?kd_mtu=${dataItem.kd_mtu.trim()}&kd_cob=${dataItem.kd_cob.trim()}&kd_scob=${dataItem.kd_scob.trim()}&kd_thn=${dataItem.kd_thn}`,
                function (response) {
                    if (response.Result == "OK") {
                        showMessage('Success', response.Message);
                    } else
                        showMessage('Error', response.Message);
                    
                    refreshGrid("#AkuisisiGrid");
                    closeProgressOnGrid('#AkuisisiGrid');
                }
            );
        }
    );
}

function btnAddAkuisisi_OnClick() {
    openWindow('#AkuisisiWindow', `/Akuisisi/Add`, 'Add');
}

function btnEditAkuisisi_OnClick(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    console.log('dataItem', dataItem);
    openWindow('#AkuisisiWindow', `/Akuisisi/Edit?kd_mtu=${dataItem.kd_mtu.trim()}&kd_cob=${dataItem.kd_cob.trim()}&kd_scob=${dataItem.kd_scob.trim()}&kd_thn=${dataItem.kd_thn}`, 'Edit');
}