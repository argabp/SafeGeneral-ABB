$(document).ready(function () {
    btnProcessMutasiKlaim();
});

function btnProcessMutasiKlaim(){
    $('#btn-mutasiKlaim-process').click(function () {

        if($("#keterangan").val().trim() === ""){
            showMessage('Error', "Keterangan Wajib Diisi");
            return;
        }

        showProgress('#ProcessMutasiKlaimWindow');
        setTimeout(function () {
            processMutasiKlaim();
        }, 500);
    });
}

function processMutasiKlaim(){
    var form = new FormData();

    form.append("kd_cb", processData.kd_cb);
    form.append("kd_cob", processData.kd_cob);
    form.append("kd_scob", processData.kd_scob);
    form.append("kd_thn", processData.kd_thn);
    form.append("no_kl", processData.no_kl);
    form.append("no_mts", processData.no_mts);
    form.append("kd_user_status", processData.kd_user_status);
    form.append("nomor_berkas", processData.nomor_berkas);
    form.append("kd_status", 3);
    form.append("tgl_status", Date());
    form.append("keterangan", $("#keterangan").val());
    form.append("status_name", "Process");
    
    const obj = Object.fromEntries(form.entries());
    
    // $("#upload").getKendoUpload().getFiles().forEach((data, index) => {
    //     form.append("Files", data.rawFile);
    // });

    ajaxPost("/MutasiKlaim/ProcessMutasiKlaim", JSON.stringify(obj), function (response) {
        closeProgress("#ProcessMutasiKlaimWindow");
        if (response.Result == "OK") {
            showMessage('Success', response.Message);
            closeWindow("#ProcessMutasiKlaimWindow");
        }
        else if (response.Result == "ERROR"){
            showMessage('Error', response.Message);
            closeWindow("#ProcessMutasiKlaimWindow");
        }
        else
            $("#ProcessMutasiKlaimWindow").html(response);
    })
}