$(document).ready(function () {
    btnRejectedApproval();
    keteranganTextArea = "#keteranganRejected";
    kodeLookUp = "12";
    btnLookUp();
});

function btnRejectedApproval(){
    $('#btn-rejected-approval').click(function () {
        showProgress('#RejectedApprovalWindow');
        setTimeout(function () {
            rejectedApproval();
        }, 500);
    });
}

function btnLookUp(){
    $('#btn-rejected-lookUp').click(function () {
        openWindow('#LookUpWindow', `/Approval/LookUp`, 'Look Up');
    });
}

function rejectedApproval(){
    var form = new FormData();

    form.append("kd_cb", dataItem.kd_cb);
    form.append("kd_product", dataItem.kd_product);
    form.append("no_sppa", dataItem.no_sppa);
    form.append("kd_rk", dataItem.kd_rk);
    form.append("kd_thn", dataItem.kd_thn);
    form.append("no_updt", dataItem.no_updt);
    form.append("kd_approval", dataItem.kd_approval);
    form.append("kd_user_status", dataItem.kd_user_status);
    form.append("kd_action", 3);
    form.append("tgl_status", Date());
    form.append("keterangan", $("#keteranganRejected").val());
    
    $("#uploadRejected").getKendoUpload().getFiles().forEach((data, index) => {
        form.append("Attachments", data.rawFile);
    });

    ajaxUpload("/Approval/Rejected", form, function (response) {
        closeProgress("#RejectedApprovalWindow");
        if (response.Result == "OK") {
            showMessage('Success', response.Message);
            closeWindow("#RejectedApprovalWindow");
        }
        else if (response.Result == "ERROR"){
            showMessage('Error', response.Message);
            closeWindow("#RejectedApprovalWindow");
        }
        else
            $("#RejectedApprovalWindow").html(response);

        refreshGrid("#ApprovalGrid");
    })
}