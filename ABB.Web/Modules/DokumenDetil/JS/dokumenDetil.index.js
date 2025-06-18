function disableGridTextboxWhenEdit(dataItem){
    return !!dataItem.isNew();
}

function onSaveDokumenDetil(dataItem){
    var url = "/DokumenDetil/SaveDokumenDetil";

    var data = {
        kd_dokumen: dataItem.model.kd_dokumen,
        nm_dokumen: dataItem.model.nm_dokumen
    }

    ajaxPost(url, JSON.stringify(data),
        function (response) {
            refreshGrid("#DokumenDetilGrid");
            if (response.Result == "OK") {
                showMessage('Success', response.Message);
            } else {
                var errors = Object.keys(response.Message).map(k => response.Message[k]);
                errors.forEach((error)=> toastr.error(error))
            }
        }
    );
}

function onDeleteDokumenDetil(e){
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    showConfirmation('Confirmation', `Are you sure you want to delete?`,
        function () {
            showProgressOnGrid("#DokumenDetilGrid");
            
            var data = {
                kd_dokumen: dataItem.kd_dokumen,
                nm_dokumen: dataItem.nm_dokumen
            }

            ajaxPost("/DokumenDetil/DeleteDokumenDetil", JSON.stringify(data),
                function (response) {
                    if (response.Result == "OK") {
                        showMessage('Success', response.Message);
                    } else
                        showMessage('Error', response.Message);

                    refreshGrid("#DokumenDetilGrid");
                    closeProgressOnGrid("#DokumenDetilGrid");
                }
            );
        }
    );
}
