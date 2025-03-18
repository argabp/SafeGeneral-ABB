$(document).ready(function () {
    btnCancelPopUpApproval();
    refreshGrid("#" + gridName);
});

function btnCancelPopUpApproval(){
    $('#btn-cancelPopUp').click(function () {
        showConfirmation('Cancel', `Are you sure you want to cancel payment?`,
            function () {
                showProgress('#CancelWindow');
                setTimeout(function () {
                    cancelPopUp();
                }, 500);
            }
        );
    });
}

function cancelPopUp(){
    var form = new FormData();

    form.append("kd_cb", dataItem.kd_cb);
    form.append("kd_product", dataItem.kd_product);
    form.append("no_sppa", dataItem.no_sppa);
    form.append("kd_rk", dataItem.kd_rk);
    form.append("kd_thn", dataItem.kd_thn);
    form.append("no_updt", dataItem.no_updt);
    form.append("kd_approval", dataItem.kd_approval);
    form.append("kd_user_status", dataItem.kd_user_status);
    form.append("kd_action", 20);
    form.append("tgl_status", Date());
    form.append("keterangan", $("#keteranganCancelPopUp").val());
    form.append("nomor_sppa", dataItem.nomor_sppa);
    
    $("#uploadCancelPopUp").getKendoUpload().getFiles().forEach((data, index) => {
        form.append("Attachments", data.rawFile);
    });

    ajaxUpload("/ProsesRekonsiliasi/Cancel", form, function (response) {
        closeProgress("#CancelWindow");
        if (response.Result == "OK") {
            refreshGrid("#" + gridName);
            showMessage('Success', response.Message);
            closeWindow("#CancelWindow");
        }
        else if (response.Result == "ERROR"){
            refreshGrid("#" + gridName);
            showMessage('Error', response.Message);
            closeWindow("#CancelWindow");
        }
        else
            $("#CancelPopUpWindow").html(response);

        refreshGrid("#" + gridName);
    })
}