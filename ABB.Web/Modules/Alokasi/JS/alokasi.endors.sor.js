$(document).ready(function () {
    btnEndorsSOR_Click();
});

function btnEndorsSOR_Click() {
    $('#btn-endors-sor').click(function () {
        showProgress('#EndorsWindow');
        setTimeout(function () {
            endorsSOR('/Alokasi/EndorsSOR')
        }, 500);
    });
}

function endorsSOR(url) {
    var form = {
        kd_cb: sorData.kd_cb,
        kd_cob: sorData.kd_cob,
        kd_scob: sorData.kd_scob,
        kd_thn: sorData.kd_thn,
        no_pol: sorData.no_pol,
        no_updt: sorData.no_updt,
        no_rsk: sorData.no_rsk,
        kd_endt: sorData.kd_endt,
        no_updt_reas: sorData.no_updt_reas,
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

