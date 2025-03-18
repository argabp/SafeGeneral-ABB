function disableGridTextboxWhenEdit(dataItem){
    return !!dataItem.isNew();
}

function onSaveKelasKonstruksi(dataItem){
    var url = "/KelasKonstruksi/SaveKelasKonstruksi";

    var data = {
        kd_kls_konstr: dataItem.model.kd_kls_konstr,
        nm_kls_konstr: dataItem.model.nm_kls_konstr
    }

    ajaxPost(url, JSON.stringify(data),
        function (response) {
            refreshGrid("#KelasKonstruksiGrid");
            if (response.Result == "OK") {
                showMessage('Success', response.Message);
            } else {
                var errors = Object.keys(response.Message).map(k => response.Message[k]);
                errors.forEach((error)=> toastr.error(error))
            }
        }
    );
}

function onDeleteKelasKonstruksi(e){
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    showConfirmation('Confirmation', `Are you sure you want to delete?`,
        function () {
            showProgressOnGrid("#KelasKonstruksiGrid");

            var data = {
                kd_kls_konstr: dataItem.kd_kls_konstr
            }

            ajaxPost("/KelasKonstruksi/DeleteKelasKonstruksi", JSON.stringify(data),
                function (response) {
                    if (response.Result == "OK") {
                        showMessage('Success', response.Message);
                    } else
                        showMessage('Error', response.Message);

                    refreshGrid("#KelasKonstruksiGrid");
                    closeProgressOnGrid("#KelasKonstruksiGrid");
                }
            );
        }
    );
}
