$(document).ready(function () {
    btnEndorsAlokasi_Click();
});

function btnEndorsAlokasi_Click() {
    $('#btn-endors-alokasi').click(function () {
        showProgress('#EndorsWindow');
        setTimeout(function () {
            endorsAlokasi('/Alokasi/EndorsAlokasi')
        }, 500);
    });
}

function endorsAlokasi(url) {
    var form = {
        kd_cb: alokasi.kd_cb,
        kd_cob: alokasi.kd_cob,
        kd_scob: alokasi.kd_scob,
        kd_thn: alokasi.kd_thn,
        no_pol: alokasi.no_pol,
        no_updt: alokasi.no_updt,
        no_rsk: alokasi.no_rsk,
        kd_endt: alokasi.kd_endt,
        no_updt_reas: alokasi.no_updt_reas,
        ket_endt: $("#ket_endt").val(),
    }

    ajaxPost(url, JSON.stringify(form), function (response) {
        if (response.Result == "OK") {
            showMessage('Success', response.Message);
            refreshGrid("#SORGrid");
            closeWindow('#EndorsWindow')
        }
        else if (response.Result == "ERROR")
            $("#EndorsWindow").html(response.Message);
        else
            $("#EndorsWindow").html(response);

        closeProgress('#EndorsWindow');
    })
}

