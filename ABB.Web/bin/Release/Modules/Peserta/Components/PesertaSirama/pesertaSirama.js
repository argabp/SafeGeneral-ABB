$(document).ready(function () {
    btnSaveAsuransi_Click();
    btnPreviousPesertaSirama();
    btnNextPesertaSirama();
});

function btnPreviousPesertaSirama(){
    $('#btn-previous-pesertaSirama').click(function () {
        $("#pesertaTab").getKendoTabStrip().select(0);
    });
}

function btnNextPesertaSirama(){
    $('#btn-next-pesertaSirama').click(function () {
        $("#pesertaTab").getKendoTabStrip().select(2);
    });
}

function btnSaveAsuransi_Click() {
    $('#btn-save-pesertaSirama').click(function () {
        showProgress('#PesertaWindow');
        setTimeout(function () {
            savePesertaSirama('/Peserta/SavePesertaSirama')
        }, 500);
    });
}

function savePesertaSirama(url) {
    var form = getFormData($('#PesertaSiramaForm'));
    var kd_cb = $("#kd_cb").val();
    var kd_product = $("#kd_product").val();
    var no_sppa = $("#no_sppa").val();
    var kd_rk = $("#kd_rk").val();
    var kd_thn = $("#kd_thn").val();
    var no_updt = $("#no_updt").val();

    form.kd_cb = kd_cb;
    form.kd_product = kd_product;
    form.no_sppa = no_sppa;
    form.kd_rk = kd_rk;
    form.kd_thn = kd_thn;
    form.no_updt = no_updt;

    var data = JSON.stringify(form);
    ajaxPost(url, data,
        function (response) {
            if (response.Result == "OK") {
                showMessage('Success', response.Message);
                refreshGridPesertaLampiran();
                refreshPesertaInfoKesehatan();
                refreshGrid("#PesertaGrid");

                $("#btn-next-pesertaSirama").prop("disabled", false);
                var tabstrip = $('#pesertaTab').data("kendoTabStrip");
                tabstrip.enable(tabstrip.items()[2]);
            }
            else if (response.Result == "ERROR")
                showMessage('Error', response.Message);
            else
                $("#tabAsuransi").html(response);

            closeProgress('#PesertaWindow');
        }
    );
}

function onChangeMasaPinjaman(e){
    generateRateSirama();
}

function generateRateSirama(){
    var data = {
        kd_cb: $("#kd_cb").val(),
        kd_product: $("#kd_product").val(),
        no_sppa: $("#no_sppa").val(),
        kd_rk: $("#kd_rk").val(),
        kd_thn: $("#kd_thn").val(),
        no_updt: $("#no_updt").val(),
        nilai_ptg: $("#nilai_ptg").getKendoNumericTextBox().value(),
        tgl_pk: $("#tgl_pk").val(),
        tgl_awal: $("#tgl_mulai").val(),
        periode_asuransi: $("#periode_asuransi").val()
    }
    
    var jsonData = JSON.stringify(data);
    
    ajaxPost("/Peserta/GenerateRateSirama", jsonData, (result) => {
        $("#jns_program").getKendoDropDownList().value(result.Model.jns_program);
        $("#kd_kategori").getKendoDropDownList().value(result.Model.kd_kategori);
        $("#stn_rate").getKendoDropDownList().value(result.Model.stn_rate);
        $("#pst_rate").getKendoNumericTextBox().value(result.Model.pst_rate);
        $("#nilai_prm").getKendoNumericTextBox().value(result.Model.nilai_prm);
        $("#loading_prm").getKendoNumericTextBox().value(result.Model.loading_prm);
        $("#total_prm").getKendoNumericTextBox().value(result.Model.total_prm);
        $("#tgl_akh").getKendoDatePicker().value(result.Model.tgl_akh);
    });
}