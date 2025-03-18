$(document).ready(function () {
    btnProcessApproval();
    keteranganTextArea = "#keteranganProcess";
    kodeLookUp = "10";
    btnLookUp();
});

function btnProcessApproval(){
    $('#btn-prcoess-approval').click(function () {
        showProgress('#ProcessApprovalWindow');
        setTimeout(function () {
            processApproval();
        }, 500);
    });
}

function btnLookUp(){
    $('#btn-process-lookUp').click(function () {
        openWindow('#LookUpWindow', `/Approval/LookUp`, 'Lookup Keterangan');
    });
}

function processApproval(){
    var form = new FormData();

    form.append("kd_cb", dataItem.kd_cb);
    form.append("kd_product", dataItem.kd_product);
    form.append("no_sppa", dataItem.no_sppa);
    form.append("kd_rk", dataItem.kd_rk);
    form.append("kd_thn", dataItem.kd_thn);
    form.append("no_updt", dataItem.no_updt);
    form.append("kd_approval", dataItem.kd_approval);
    form.append("kd_user_status", dataItem.kd_user_status);
    form.append("kd_action", 1);
    form.append("tgl_status", Date());
    form.append("keterangan", $("#keteranganProcess").val());
    
    $("#uploadProcess").getKendoUpload().getFiles().forEach((data, index) => {
        form.append("Attachments", data.rawFile);
    });

    ajaxUpload("/Approval/Process", form, function (response) {
        closeProgress("#ProcessApprovalWindow");
        if (response.Result == "OK") {
            showMessage('Success', response.Message);
            closeWindow("#ProcessApprovalWindow");
        }
        else if (response.Result == "ERROR"){
            showMessage('Error', response.Message);
            closeWindow("#ProcessApprovalWindow");
        }
        else
            $("#ProcessApprovalWindow").html(response);

        refreshGrid("#ApprovalGrid");
    })
}