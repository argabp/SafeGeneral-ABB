$(document).ready(function () {
    btnApprovalMutasiKlaimRevised();
});

function btnApprovalMutasiKlaimRevised(){
    $('#btn-approvalMutasiKlaim-revised').click(function () {

        if($("#keterangan").val().trim() === ""){
            showMessage('Error', "Keterangan Wajib Diisi");
            return;
        }

        showProgress('#ApprovalWindow');
        setTimeout(function () {
            approvalMutasiKlaimRevised();
        }, 500);
    });
}

function approvalMutasiKlaimRevised(){
    var form = new FormData();

    form.append("kd_cb", dataItem.kd_cb);
    form.append("kd_cob", dataItem.kd_cob);
    form.append("kd_scob", dataItem.kd_scob);
    form.append("kd_thn", dataItem.kd_thn);
    form.append("no_kl", dataItem.no_kl);
    form.append("no_mts", dataItem.no_mts);
    form.append("kd_user_status", dataItem.kd_user_status);
    form.append("nomor_berkas", dataItem.nomor_berkas);
    form.append("kd_status", 10);
    form.append("tgl_status", Date());
    form.append("keterangan", $("#keterangan").val());
    form.append("status_name", "Revised");
    form.append("kd_user_sign", $("#kd_user_sign").val());
    
    $("#upload").getKendoUpload().getFiles().forEach((data, index) => {
        form.append("Files", data.rawFile);
    });

    ajaxUpload("/ApprovalMutasiKlaim/ApprovalMutasiKlaimRev", form, function (response) {
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

        refreshGrid("#ApprovalMutasiKlaimGrid");
    })
}

function dataUserSignRevised(){
    return {
        kd_cb: dataItem.kd_cb,
        kd_cob: dataItem.kd_cob,
        kd_scob: dataItem.kd_scob,
        kd_thn: dataItem.kd_thn,
        no_kl: dataItem.no_kl,
    }
}