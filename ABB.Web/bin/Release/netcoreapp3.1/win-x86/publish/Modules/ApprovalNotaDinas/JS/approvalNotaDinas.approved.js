$(document).ready(function () {
    btnApprovedApproval();
    keteranganTextArea = "#keteranganApproved";
    kodeLookUp = "16";
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
        openWindow('#LookUpWindow', `/ApprovalNotaDinas/LookUp`, 'Look Up');
    });
}

function approvedApproval(){
    var form = new FormData();

    form.append("id_nds", dataItem.id_nds);
    form.append("kd_user_status", dataItem.kd_user_status);
    form.append("kd_action", 4);
    form.append("tgl_status", Date());
    form.append("keterangan", $("#keteranganApproved").val());
    
    $("#uploadApproved").getKendoUpload().getFiles().forEach((data, index) => {
        form.append("Attachments", data.rawFile);
    });

    ajaxUpload("/ApprovalNotaDinas/Approved", form, function (response) {
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