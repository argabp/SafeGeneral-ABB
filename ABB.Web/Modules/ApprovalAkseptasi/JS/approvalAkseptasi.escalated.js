$(document).ready(function () {
    btnApprovalAkseptasiEscalated();
});

function btnApprovalAkseptasiEscalated(){
    $('#btn-approvalAkseptasi-escalated').click(function () {

        if($("#keteranganApproval").val().trim() === ""){
            showMessage('Error', "Keterangan Wajib Diisi");
            return;
        }

        showProgress('#ApprovalWindow');
        setTimeout(function () {
            approvalAkseptasiEscalated();
        }, 500);
    });
}

function approvalAkseptasiEscalated(){
    var form = new FormData();

    form.append("kd_cb", dataItem.kd_cb);
    form.append("kd_cob", dataItem.kd_cob);
    form.append("kd_scob", dataItem.kd_scob);
    form.append("kd_thn", dataItem.kd_thn);
    form.append("no_aks", dataItem.no_aks);
    form.append("kd_user_status", dataItem.kd_user_status);
    form.append("nomor_pengajuan", dataItem.nomor_pengajuan);
    form.append("kd_status", 4);
    form.append("tgl_status", Date());
    form.append("keterangan", $("#keterangan").val());
    form.append("status_name", "Escalated");
    form.append("kd_user_sign", $("#kd_user_sign").val());
    
    $("#upload").getKendoUpload().getFiles().forEach((data, index) => {
        form.append("Files", data.rawFile);
    });

    ajaxUpload("/ApprovalAkseptasi/ApprovalAkseptasiEsc", form, function (response) {
        closeProgress("#ApprovalWindow");
        if (response.Result == "OK") {
            showMessage('Success', response.Message);
            closeWindow("#ApprovalWindow");
        }
        else if (response.Result == "ERROR"){
            showMessage('Error', response.Message);
            closeWindow("#ApprovalWindow");
        }
        else
            $("#ApprovalWindow").html(response);

        refreshGrid("#ApprovalAkseptasiGrid");
    })
}