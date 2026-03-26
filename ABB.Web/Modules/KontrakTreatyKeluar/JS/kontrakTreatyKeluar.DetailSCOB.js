$(document).ready(async function () {
    btnSaveDetailKontrakTreatyKeluarSCOB_Click();
});

function btnSaveDetailKontrakTreatyKeluarSCOB_Click() {
    $('#btn-save-detailKontrakTreatyKeluarSCOB').click(function () {
        showProgress('#DetailKontrakTreatyKeluarWindow');
        setTimeout(function () {
            saveDetailKontrakTreatyKeluarSCOB()
        }, 500);
    });
}

function saveDetailKontrakTreatyKeluarSCOB(){    
    var kd_cb = $("#kd_cb").val().trim();
    var kd_jns_sor = $("#kd_jns_sor").val().trim();
    var kd_tty_pps = $("#kd_tty_pps").val().trim();
    var kd_cob = $("#kd_cob").val().trim();
    var kd_scob = $("#kd_scob").val().trim();

    var form = {
        kd_cb: kd_cb,
        kd_jns_sor: kd_jns_sor,
        kd_tty_pps: kd_tty_pps,
        kd_cob: kd_cob,
        kd_scob: kd_scob
    }
    
    ajaxPost("/KontrakTreatyKeluar/SaveDetailKontrakTreatyKeluarSCOB", JSON.stringify(form),
        function (response) {
            if (response.Result == "OK") {
                showMessage('Success', response.Message);
            }
            else if (response.Result == "ERROR") {
                showMessage('Error', response.Message);
            }
            else {
                $("#DetailKontrakTreatyKeluarWindow").html(response);
            }

            refreshGrid("#DetailKontrakTreatyKeluarSCOBGrid");
            closeProgress("#DetailKontrakTreatyKeluarWindow");
            closeWindow("#DetailKontrakTreatyKeluarWindow");
        }
    );
}

function dataKodeSCOBDropDown(){
    return {
        kd_cob: $("#kd_cob").val().trim()
    }
}