$(document).ready(function () {
    btnCancelPopUpApproval();
    btnCancelPopUpLookUp();
});

function btnCancelPopUpApproval(){
    $('#btn-cancelPopUp-approval').click(function () {
        showProgress('#CancelApprovalWindow');
        setTimeout(function () {
            cancelPopUpApproval();
        }, 500);
    });
}

function btnCancelPopUpLookUp(){
    $('#btn-cancelPopUp-lookUp').click(function () {
        openWindow('#LookUpWindow', `/PengajuanAkseptasi/LookUp`, 'Look Up');
    });
}

function cancelPopUpApproval(){
    var form = new FormData();

    form.append("kd_cb", dataItem.kd_cb);
    form.append("kd_product", dataItem.kd_product);
    form.append("no_sppa", dataItem.no_sppa);
    form.append("kd_rk", dataItem.kd_rk);
    form.append("kd_thn", dataItem.kd_thn);
    form.append("no_updt", dataItem.no_updt);
    form.append("kd_approval", dataItem.kd_approval);
    form.append("kd_user_status", dataItem.kd_user_status);
    form.append("kd_action", 6);
    form.append("tgl_status", Date());
    form.append("keterangan", $("#keteranganCancelPopUp").val());
    
    $("#uploadCancelPopUp").getKendoUpload().getFiles().forEach((data, index) => {
        form.append("Attachments", data.rawFile);
    });

    ajaxUpload("/PengajuanAkseptasi/CancelApproval", form, function (response) {
        closeProgress("#CancelApprovalWindow");
        if (response.Result == "OK") {
            showMessage('Success', response.Message);
            closeWindow("#CancelApprovalWindow");
        }
        else if (response.Result == "ERROR"){
            showMessage('Error', response.Message);
            closeWindow("#CancelApprovalWindow");
        }
        else
            $("#CancelPopUpApprovalWindow").html(response);

        if($("ApprovedApprovalGrid")[0] == undefined)
            refreshGrid("#ConfirmationApprovalGrid");
        else
            refreshGrid("#ApprovedApprovalGrid");
    })
}