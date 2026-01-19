$(document).ready(function () {
    btnUpdateKlaimClosed();
});

function btnUpdateKlaimClosed(){
    $('#btn-updateKlaim-closed').click(function () {

        if($("#keterangan").val().trim() === ""){
            showMessage('Error', "Keterangan Wajib Diisi");
            return;
        }

        showProgress('#UpdateKlaimWindow');
        setTimeout(function () {
            updateMutasiKlaimClosed();
        }, 500);
    });
}

function updateMutasiKlaimClosed(){
    var data = {
        kd_cb: dataItem.kd_cb,
        kd_cob: dataItem.kd_cob,
        kd_scob: dataItem.kd_scob,
        kd_thn: dataItem.kd_thn,
        no_kl: dataItem.no_kl,
        no_mts: dataItem.no_mts,
        kd_user_status: dataItem.kd_user_status,
        nomor_berkas: dataItem.register_klaim,
        kd_status: 8,
        tgl_status: new Date().toISOString(),
        keterangan: $("#keterangan").val(),
        status_name: "Closed"
    };

    var jsonString = JSON.stringify(data);

    ajaxPost("/UpdateKlaim/UpdateKlaim", jsonString, function (response) {
        closeProgress("#UpdateKlaimWindow");
        if (response.Result == "OK") {
            showMessage('Success', response.Message);
            closeWindow("#UpdateKlaimWindow");
        }
        else if (response.Result == "ERROR"){
            showMessage('Error', response.Message);
            closeWindow("#UpdateKlaimWindow");
        }
        else
            $("#UpdateKlaimWindow").html(response);

        refreshGrid("#UpdateKlaimGrid");
    })
}