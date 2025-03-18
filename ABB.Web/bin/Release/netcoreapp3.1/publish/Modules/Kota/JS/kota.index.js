function disableGridTextboxWhenEdit(dataItem){
    return !!dataItem.isNew();
}

function onSaveKota(dataItem){
    var url = "/Kota/SaveKota";

    var data = {
        kd_kota: dataItem.model.kd_kota,
        nm_kota: dataItem.model.nm_kota
    }

    ajaxPost(url, JSON.stringify(data),
        function (response) {
            refreshGrid("#KotaGrid");
            if (response.Result == "OK") {
                showMessage('Success', response.Message);
            } else {
                var errors = Object.keys(response.Message).map(k => response.Message[k]);
                errors.forEach((error)=> toastr.error(error))
            }
        }
    );
}

function onDeleteKota(e){
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    showConfirmation('Confirmation', `Are you sure you want to delete?`,
        function () {
            showProgressOnGrid("#KotaGrid");

            var data = {
                kd_kota: dataItem.kd_kota,
                nm_kota: dataItem.nm_kota
            }

            ajaxPost("/Kota/DeleteKota", JSON.stringify(data),
                function (response) {
                    if (response.Result == "OK") {
                        showMessage('Success', response.Message);
                    } else
                        showMessage('Error', response.Message);

                    refreshGrid("#KotaGrid");
                    closeProgressOnGrid("#KotaGrid");
                }
            );
        }
    );
}
