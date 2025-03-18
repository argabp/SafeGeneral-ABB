$(document).ready(function () {
    btnConfirmationApproval();
    keteranganTextArea = "#keteranganConfirmation";
    kodeLookUp = "11";
    btnLookUp();
});

function btnConfirmationApproval(){
    $('#btn-confirmation-approval').click(function () {
        showProgress('#ConfirmationApprovalWindow');
        setTimeout(function () {
            confirmationApproval();
        }, 500);
    });
}

function btnLookUp(){
    $('#btn-confirmation-lookUp').click(function () {
        openWindow('#LookUpWindow', `/Approval/LookUp`, 'Look Up');
    });
}

function confirmationApproval(){
    var form = new FormData();

    form.append("kd_cb", dataItem.kd_cb);
    form.append("kd_product", dataItem.kd_product);
    form.append("no_sppa", dataItem.no_sppa);
    form.append("kd_rk", dataItem.kd_rk);
    form.append("kd_thn", dataItem.kd_thn);
    form.append("no_updt", dataItem.no_updt);
    form.append("kd_approval", dataItem.kd_approval);
    form.append("kd_user_status", dataItem.kd_user_status);
    form.append("kd_action", 2);
    form.append("tgl_status", Date());
    form.append("keterangan", $("#keteranganConfirmation").val());
    
    $("#uploadConfirmation").getKendoUpload().getFiles().forEach((data, index) => {
        form.append("Attachments", data.rawFile);
    });

    ajaxUpload("/Approval/Confirmation", form, function (response) {
        closeProgress("#ConfirmationApprovalWindow");
        if (response.Result == "OK") {
            showMessage('Success', response.Message);
            closeWindow("#ConfirmationApprovalWindow");
        }
        else if (response.Result == "ERROR"){
            showMessage('Error', response.Message);
            closeWindow("#ConfirmationApprovalWindow");
        }
        else
            $("#ConfirmationApprovalWindow").html(response);

        refreshGrid("#ApprovalGrid");
    })
}