function onSaveDetailSlikRetail(){
    showConfirmation('Confirmation', `Are you sure you want to save?`,
        function () {
            var form = getFormData($('#DetailSlikRetailForm'));
            form.kd_cb = selectedData.kd_cb;
            form.kd_grp_rk = selectedData.kd_grp_rk;
            form.kd_rk = selectedData.kd_rk;

            showProgress('#DetailRekananWindow');

            ajaxPost("/TertanggungPrincipal/SaveDetailSlikRetail", JSON.stringify(form),
                function (response) {
                    if (response.Result == "OK") {
                        showMessage('Success', response.Message);
                        closeWindow("#DetailRekananWindow");
                    } else if (response.Result == "ERROR") {
                        showMessage('Error', response.Message);
                    } else {
                        $("#DetailRekananWindow").html(response);
                    }

                    closeProgress("#DetailRekananWindow");
                }
            );
        })
}