function onSaveKapasitasCabang(){    
    var url = "/KapasitasCabang/Save";

    var data = {
        kd_cb: $("#kd_cb").val(),
        kd_cob: $("#kd_cob").val(),
        kd_scob: $("#kd_scob").val(),
        thn: parseInt($("#thn").val()),
        nilai_kapasitas: $("#nilai_kapasitas").val(),
        tgl_input: $("#tgl_input").val(),
        kd_usr_input: $("#kd_usr_input").val(),
        nilai_kl: $("#nilai_kl").val()
    }
    showProgress('#KapasitasCabangWindow');
    
    ajaxPost(url, JSON.stringify(data),
        function (response) {
            refreshGrid("#KapasitasCabangGrid");
            if (response.Result == "OK") {
                showMessage('Success', response.Message);
                closeWindow("#KapasitasCabangWindow");
            }
            else if (response.Result == "ERROR")
                showMessage('Error', response.Message);
            else
                $("#KapasitasCabangWindow").html(response);

            closeProgress('#KapasitasCabangWindow');
        }
    );
}

function onDeleteKapasitasCabang(e){
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    showConfirmation('Confirmation', `Are you sure you want to delete?`,
        function () {
            showProgressOnGrid('#KapasitasCabangGrid');

            ajaxGet(`/KapasitasCabang/Delete?kd_cb=${dataItem.kd_cb.trim()}&kd_cob=${dataItem.kd_cob.trim()}&kd_scob=${dataItem.kd_scob.trim()}&thn=${dataItem.thn}`,
                function (response) {
                    if (response.Result == "OK") {
                        showMessage('Success', response.Message);
                    } else
                        showMessage('Error', response.Message);
                    
                    refreshGrid("#KapasitasCabangGrid");
                    closeProgressOnGrid('#KapasitasCabangGrid');
                }
            );
        }
    );
}

function btnAddKapasitasCabang_OnClick() {
    openWindow('#KapasitasCabangWindow', `/KapasitasCabang/Add`, 'Add');
}

function btnEditKapasitasCabang_OnClick(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    console.log('dataItem', dataItem);
    openWindow('#KapasitasCabangWindow', `/KapasitasCabang/Edit?kd_cb=${dataItem.kd_cb.trim()}&kd_cob=${dataItem.kd_cob.trim()}&kd_scob=${dataItem.kd_scob.trim()}&thn=${dataItem.thn}`, 'Edit');
}