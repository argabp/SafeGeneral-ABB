function disableGridTextboxWhenEdit(dataItem){
    return !!dataItem.isNew();
}

function onSaveCoverage(dataItem){
    var url = "/Coverage/SaveCoverage";

    var data = {
        kd_cvrg: dataItem.model.kd_cvrg,
        nm_cvrg: dataItem.model.nm_cvrg,
        nm_cvrg_ing: dataItem.model.nm_cvrg_ing
    }

    ajaxPost(url, JSON.stringify(data),
        function (response) {
            refreshGrid("#CoverageGrid");
            if (response.Result == "OK") {
                showMessage('Success', response.Message);
            } else {
                var errors = Object.keys(response.Message).map(k => response.Message[k]);
                errors.forEach((error)=> toastr.error(error))
            }
        }
    );
}

function onDeleteCoverage(e){
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    showConfirmation('Confirmation', `Are you sure you want to delete?`,
        function () {
            showProgressOnGrid("#CoverageGrid");

            var data = {
                kd_cvrg: dataItem.kd_cvrg
            }

            ajaxPost("/Coverage/DeleteCoverage", JSON.stringify(data),
                function (response) {
                    if (response.Result == "OK") {
                        showMessage('Success', response.Message);
                    } else
                        showMessage('Error', response.Message);

                    refreshGrid("#CoverageGrid");
                    closeProgressOnGrid("#CoverageGrid");
                }
            );
        }
    );
}
