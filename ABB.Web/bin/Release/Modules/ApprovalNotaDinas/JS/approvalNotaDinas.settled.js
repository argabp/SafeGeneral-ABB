$(document).ready(function () {
    btnSettledApproval();
});

function btnSettledApproval(){
    $('#btn-settled-approval').click(function () {
        showProgress('#SettledApprovalWindow');
        setTimeout(function () {
            settledApproval();
        }, 500);
    });
}

function settledApproval(){
    var form = new FormData();

    form.append("id_nds", dataItem.id_nds);
    form.append("kd_user_status", dataItem.kd_user_status);
    form.append("kd_action", 7);
    form.append("tgl_status", $("#tgl_status_settled").val());
    form.append("keterangan", $("#keteranganSettled").val());
    
    $("#uploadSettled").getKendoUpload().getFiles().forEach((data, index) => {
        form.append("Attachments", data.rawFile);
    });

    ajaxUpload("/ApprovalNotaDinas/Settled", form, function (response) {
        closeProgress("#SettledApprovalWindow");
        if (response.Result == "OK") {
            showMessage('Success', response.Message);
            closeWindow("#SettledApprovalWindow");
        }
        else if (response.Result == "ERROR"){
            showMessage('Error', response.Message);
            closeWindow("#SettledApprovalWindow");
        }
        else
            $("#settledApprovalWindow").html(response);

        refreshGrid("#ApprovalGrid");
    })
}