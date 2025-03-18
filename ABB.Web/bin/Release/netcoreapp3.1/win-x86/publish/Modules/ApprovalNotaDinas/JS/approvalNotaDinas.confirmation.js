$(document).ready(function () {
    btnConfirmationApproval();
    keteranganTextArea = "#keteranganConfirmation";
    kodeLookUp = "15";
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
        openWindow('#LookUpWindow', `/ApprovalNotaDinas/LookUp`, 'Look Up');
    });
}

function confirmationApproval(){
    var form = new FormData();
    
    form.append("id_nds", dataItem.id_nds);
    form.append("kd_user_status", dataItem.kd_user_status);
    form.append("kd_action", 2);
    form.append("tgl_status", Date());
    form.append("keterangan", $("#keteranganConfirmation").val());
    
    $("#uploadConfirmation").getKendoUpload().getFiles().forEach((data, index) => {
        form.append("Attachments", data.rawFile);
    });

    ajaxUpload("/ApprovalNotaDinas/Confirmation", form, function (response) {
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