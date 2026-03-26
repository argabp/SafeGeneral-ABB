$(document).ready(async function () {
    btnSaveDetailKontrakTreatyKeluarTableOfLimit_Click();
});

function btnSaveDetailKontrakTreatyKeluarTableOfLimit_Click() {
    $('#btn-save-detailKontrakTreatyKeluarTableOfLimit').click(function () {
        showProgress('#DetailKontrakTreatyKeluarWindow');
        setTimeout(function () {
            saveDetailKontrakTreatyKeluarTableOfLimit()
        }, 500);
    });
}

function saveDetailKontrakTreatyKeluarTableOfLimit(){    
    var kd_cb = $("#kd_cb").val().trim();
    var kd_jns_sor = $("#kd_jns_sor").val().trim();
    var kd_tty_pps = $("#kd_tty_pps").val().trim();
    var kd_okup = $("#kd_okup").val().trim();
    var category_rsk = $("#category_rsk").val();
    var kd_kls_konstr = $("#kd_kls_konstr").val();
    var pst_bts_tty = $("#pst_bts_tty").val();

    var form = {
        kd_cb: kd_cb,
        kd_jns_sor: kd_jns_sor,
        kd_tty_pps: kd_tty_pps,
        kd_okup: kd_okup,
        category_rsk: category_rsk,
        kd_kls_konstr: kd_kls_konstr,
        pst_bts_tty: pst_bts_tty
    }
    
    ajaxPost("/KontrakTreatyKeluar/SaveDetailKontrakTreatyKeluarTableOfLimit", JSON.stringify(form),
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

            refreshGrid("#DetailKontrakTreatyKeluarTableOfLimitGrid");
            closeProgress("#DetailKontrakTreatyKeluarWindow");
            closeWindow("#DetailKontrakTreatyKeluarWindow");
        }
    );
}