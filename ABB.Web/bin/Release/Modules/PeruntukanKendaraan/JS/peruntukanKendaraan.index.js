function disableGridTextboxWhenEdit(dataItem){
    return !!dataItem.isNew();
}

function onSavePeruntukanKendaraan(dataItem){
    var url = "/PeruntukanKendaraan/SavePeruntukanKendaraan";

    var data = {
        kd_utk: dataItem.model.kd_utk,
        nm_utk: dataItem.model.nm_utk,
        nm_utk_ing: dataItem.model.nm_utk_ing
    }

    ajaxPost(url, JSON.stringify(data),
        function (response) {
            refreshGrid("#PeruntukanKendaraanGrid");
            if (response.Result == "OK") {
                showMessage('Success', response.Message);
            } else {
                var errors = Object.keys(response.Message).map(k => response.Message[k]);
                errors.forEach((error)=> toastr.error(error))
            }
        }
    );
}

function onDeletePeruntukanKendaraan(e){
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    showConfirmation('Confirmation', `Are you sure you want to delete?`,
        function () {
            showProgressOnGrid("#PeruntukanKendaraanGrid");

            var data = {
                kd_utk: dataItem.kd_utk
            }

            ajaxPost("/PeruntukanKendaraan/DeletePeruntukanKendaraan", JSON.stringify(data),
                function (response) {
                    if (response.Result == "OK") {
                        showMessage('Success', response.Message);
                    } else
                        showMessage('Error', response.Message);

                    refreshGrid("#PeruntukanKendaraanGrid");
                    closeProgressOnGrid("#PeruntukanKendaraanGrid");
                }
            );
        }
    );
}
