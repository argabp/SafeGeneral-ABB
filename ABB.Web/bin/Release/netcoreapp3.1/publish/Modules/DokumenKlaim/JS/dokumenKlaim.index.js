function onSaveDokumenKlaim(){    
    var url = "/DokumenKlaim/Save";

    var data = {
        kd_cob: $("#kd_cob").val(),
        kd_dok: $("#kd_dok").val(),
        nm_dok: $("#nm_dok").val()
    }
    showProgress('#DokumenKlaimWindow');
    
    ajaxPost(url, JSON.stringify(data),
        function (response) {
            refreshGrid("#DokumenKlaimGrid");
            if (response.Result == "OK") {
                showMessage('Success', response.Message);
                closeWindow("#DokumenKlaimWindow");
            }
            else if (response.Result == "ERROR")
                showMessage('Error', response.Message);
            else
                $("#DokumenKlaimWindow").html(response);

            closeProgress('#DokumenKlaimWindow');
        }
    );
}

function onDeleteDokumenKlaim(e){
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    showConfirmation('Confirmation', `Are you sure you want to delete?`,
        function () {
            showProgressOnGrid('#DokumenKlaimGrid');

            ajaxGet(`/DokumenKlaim/DeleteDokumenKlaim?kd_cob=${dataItem.kd_cob.trim()}&kd_dok=${dataItem.kd_dok.trim()}`,
                function (response) {
                    if (response.Result == "OK") {
                        showMessage('Success', response.Message);
                    } else
                        showMessage('Error', response.Message);
                    
                    refreshGrid("#DokumenKlaimGrid");
                    closeProgressOnGrid('#DokumenKlaimGrid');
                }
            );
        }
    );
}

function btnAddDokumenKlaim_OnClick() {
    openWindow('#DokumenKlaimWindow', `/DokumenKlaim/Add`, 'Add');
}

function btnEditDokumenKlaim_OnClick(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    console.log('dataItem', dataItem);
    openWindow('#DokumenKlaimWindow', `/DokumenKlaim/Edit?kd_cob=${dataItem.kd_cob.trim()}&kd_dok=${dataItem.kd_dok.trim()}`, 'Edit');
}