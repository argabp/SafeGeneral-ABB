$(document).ready(async function () {
    btnSaveDetailKontrakTreatyKeluarCoverage_Click();
});

function btnSaveDetailKontrakTreatyKeluarCoverage_Click() {
    $('#btn-save-detailKontrakTreatyKeluarCoverage').click(function () {
        showProgress('#DetailKontrakTreatyKeluarWindow');
        setTimeout(function () {
            saveDetailKontrakTreatyKeluarCoverage()
        }, 500);
    });
}

function saveDetailKontrakTreatyKeluarCoverage(){    
    var kd_cb = $("#kd_cb").val().trim();
    var kd_jns_sor = $("#kd_jns_sor").val().trim();
    var kd_tty_pps = $("#kd_tty_pps").val().trim();
    var kd_cvrg = $("#kd_cvrg").val().trim();
    var pst_kms_reas = $("#coverage_pst_kms_reas").val();
    var max_limit_jktb = $("#max_limit_jktb").val();
    var max_limit_prov = $("#max_limit_prov").val();

    var form = {
        kd_cb: kd_cb,
        kd_jns_sor: kd_jns_sor,
        kd_tty_pps: kd_tty_pps,
        kd_cvrg: kd_cvrg,
        pst_kms_reas: pst_kms_reas,
        max_limit_jktb: max_limit_jktb,
        max_limit_prov: max_limit_prov
    }
    
    ajaxPost("/KontrakTreatyKeluar/SaveDetailKontrakTreatyKeluarCoverage", JSON.stringify(form),
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

            refreshGrid("#DetailKontrakTreatyKeluarCoverageGrid");
            closeProgress("#DetailKontrakTreatyKeluarWindow");
            closeWindow("#DetailKontrakTreatyKeluarWindow");
        }
    );
}