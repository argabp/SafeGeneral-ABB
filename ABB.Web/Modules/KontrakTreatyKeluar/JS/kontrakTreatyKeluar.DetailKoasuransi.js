$(document).ready(async function () {
    btnSaveDetailKontrakTreatyKeluarKoasuransi_Click();
});

function btnSaveDetailKontrakTreatyKeluarKoasuransi_Click() {
    $('#btn-save-detailKontrakTreatyKeluarKoasuransi').click(function () {
        showProgress('#DetailKontrakTreatyKeluarWindow');
        setTimeout(function () {
            saveDetailKontrakTreatyKeluarKoasuransi()
        }, 500);
    });
}

function saveDetailKontrakTreatyKeluarKoasuransi(){    
    var kd_cb = $("#kd_cb").val().trim();
    var kd_jns_sor = $("#kd_jns_sor").val().trim();
    var kd_tty_pps = $("#kd_tty_pps").val().trim();
    var no_urut = $("#no_urut").val();
    var pst_share_mul = $("#pst_share_mul").val();
    var pst_share_akh = $("#pst_share_akh").val();
    var pst_bts_koas = $("#pst_bts_koas").val();

    var form = {
        kd_cb: kd_cb,
        kd_jns_sor: kd_jns_sor,
        kd_tty_pps: kd_tty_pps,
        no_urut: no_urut,
        pst_share_mul: pst_share_mul,
        pst_share_akh: pst_share_akh,
        pst_bts_koas: pst_bts_koas
    }
    
    ajaxPost("/KontrakTreatyKeluar/SaveDetailKontrakTreatyKeluarKoasuransi", JSON.stringify(form),
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

            refreshGrid("#DetailKontrakTreatyKeluarKoasuransiGrid");
            closeProgress("#DetailKontrakTreatyKeluarWindow");
            closeWindow("#DetailKontrakTreatyKeluarWindow");
        }
    );
}