$(document).ready(function () {
    btnPengajuanAkseptasiSubmit();
    btnLookUp();
});

function btnPengajuanAkseptasiSubmit(){
    $('#btn-pengajuanAkseptasi-submit').click(function () {
        showProgress('#SubmitWindow');
        setTimeout(function () {
            pengajuanAkseptasiSubmit();
        }, 500);
    });
}

function pengajuanAkseptasiSubmit(){
    var form = new FormData();

    form.append("kd_cb", dataItem.kd_cb);
    form.append("kd_cob", dataItem.kd_cob);
    form.append("kd_scob", dataItem.kd_scob);
    form.append("kd_thn", dataItem.kd_thn);
    form.append("no_aks", dataItem.no_aks);
    form.append("no_updt", dataItem.no_updt);
    form.append("kd_user_status", dataItem.kd_user_status);
    form.append("nomor_pengajuan", dataItem.nomor_pengajuan);
    form.append("kd_status", 2);
    form.append("tgl_status", Date());
    form.append("keterangan", $("#keteranganSubmit").val());
    
    $("#uploadSubmit").getKendoUpload().getFiles().forEach((data, index) => {
        form.append("Files", data.rawFile);
    });

    ajaxUpload("/PengajuanAkseptasi/Submit", form, function (response) {
        closeProgress("#SubmitWindow");
        if (response.Result == "OK") {
            showMessage('Success', response.Message);
            closeWindow("#SubmitWindow");
        }
        else if (response.Result == "ERROR"){
            showMessage('Error', response.Message);
            closeWindow("#SubmitWindow");
        }
        else
            $("#SubmitWindow").html(response);

        refreshGrid("#PengajuanAkseptasiGrid");
    })
}