$(document).ready(function () {
    btnApprovedApproval();
    keteranganTextArea = "#keteranganApproved";
    kodeLookUp = "13";
    btnLookUp();
});

function btnApprovedApproval(){
    $('#btn-approved-approval').click(function () {
        showProgress('#ApprovedApprovalWindow');
        setTimeout(function () {
            approvedApproval();
        }, 500);
    });
}

function btnLookUp(){
    $('#btn-approved-lookUp').click(function () {
        openWindow('#LookUpWindow', `/Approval/LookUp`, 'Look Up');
    });
}

function approvedApproval(){
    var form = new FormData();

    form.append("kd_cb", dataItem.kd_cb);
    form.append("kd_product", dataItem.kd_product);
    form.append("no_sppa", dataItem.no_sppa);
    form.append("kd_rk", dataItem.kd_rk);
    form.append("kd_thn", dataItem.kd_thn);
    form.append("no_updt", dataItem.no_updt);
    form.append("kd_approval", dataItem.kd_approval);
    form.append("kd_user_status", dataItem.kd_user_status);
    form.append("kd_action", 4);
    form.append("tgl_status", Date());
    form.append("keterangan", $("#keteranganApproved").val());
    
    $("#uploadApproved").getKendoUpload().getFiles().forEach((data, index) => {
        form.append("Attachments", data.rawFile);
    });

    ajaxUpload("/Approval/Approved", form, function (response) {
        closeProgress("#ApprovedApprovalWindow");
        if (response.Result == "OK") {
            showMessage('Success', response.Message);
            closeWindow("#ApprovedApprovalWindow");
        }
        else if (response.Result == "ERROR"){
            showMessage('Error', response.Message);
            closeWindow("#ApprovedApprovalWindow");
        }
        else
            $("#ApprovedApprovalWindow").html(response);

        refreshGrid("#ApprovalGrid");
    })
}