$(document).ready(async function () {
    btnSaveDetailKontrakTreatyKeluarExclude_Click();
});

function btnSaveDetailKontrakTreatyKeluarExclude_Click() {
    $('#btn-save-detailKontrakTreatyKeluarExclude').click(function () {
        showProgress('#DetailKontrakTreatyKeluarWindow');
        setTimeout(function () {
            saveDetailKontrakTreatyKeluarExclude()
        }, 500);
    });
}

function saveDetailKontrakTreatyKeluarExclude(){    
    var kd_cb = $("#kd_cb").val().trim();
    var kd_jns_sor = $("#kd_jns_sor").val().trim();
    var kd_tty_pps = $("#kd_tty_pps").val().trim();
    var kd_okup = $("#kd_okup").val().trim();

    var form = {
        kd_cb: kd_cb,
        kd_jns_sor: kd_jns_sor,
        kd_tty_pps: kd_tty_pps,
        kd_okup: kd_okup
    }
    
    ajaxPost("/KontrakTreatyKeluar/SaveDetailKontrakTreatyKeluarExclude", JSON.stringify(form),
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

            refreshGrid("#DetailKontrakTreatyKeluarExcludeGrid");
            closeProgress("#DetailKontrakTreatyKeluarWindow");
            closeWindow("#DetailKontrakTreatyKeluarWindow");
        }
    );
}