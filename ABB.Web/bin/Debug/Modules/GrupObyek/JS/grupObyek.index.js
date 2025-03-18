function disableGridTextboxWhenEdit(dataItem){
    return !!dataItem.isNew();
}

function onSaveGrupObyek(dataItem){
    var url = "/GrupObyek/SaveGrupObyek";

    var data = {
        kd_grp_oby: dataItem.model.kd_grp_oby,
        nm_grp_oby: dataItem.model.nm_grp_oby
    }

    ajaxPost(url, JSON.stringify(data),
        function (response) {
            refreshGrid("#GrupObyekGrid");
            if (response.Result == "OK") {
                showMessage('Success', response.Message);
            } else {
                var errors = Object.keys(response.Message).map(k => response.Message[k]);
                errors.forEach((error)=> toastr.error(error))
            }
        }
    );
}

function onDeleteGrupObyek(e){
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    showConfirmation('Confirmation', `Are you sure you want to delete?`,
        function () {
            showProgressOnGrid("#GrupObyekGrid");

            var data = {
                kd_grp_oby: dataItem.kd_grp_oby
            }

            ajaxPost("/GrupObyek/DeleteGrupObyek", JSON.stringify(data),
                function (response) {
                    if (response.Result == "OK") {
                        showMessage('Success', response.Message);
                    } else
                        showMessage('Error', response.Message);

                    refreshGrid("#GrupObyekGrid");
                    closeProgressOnGrid("#GrupObyekGrid");
                }
            );
        }
    );
}
