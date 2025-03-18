$(document).ready(function () {
    btnSaveAsuransi_Click();
    btnPreviousPesertaSirama();
    btnNextPesertaSirama();
    calculateMasaPinjaman();
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
            }
            else if (response.Result == "ERROR")
                showMessage('Error', response.Message);
            else
                $("#tabAsuransi").html(response);

            closeProgress('#PesertaWindow');
        }
    );
}

function calculateMasaPinjaman(){
    var mulai = $("#tgl_mulai").getKendoDatePicker().value();
    var akhir = $("#tgl_akh").getKendoDatePicker().value();

    var startDate = moment(mulai);
    var endDate = moment(akhir);

    endDate = endDate.add(1, "d");
    
    var different = moment.duration(endDate.diff(startDate));

    $("#tahunMasaPinjaman").text(different._data.years);
    $("#bulanMasaPinjaman").text(different._data.months);
}

function onChangeMasaPinjaman(e){
    calculateMasaPinjaman();
    
    var id = e.sender.element[0].id;
    
    if(id === "tgl_akh")
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
        tgl_pk: $("#tgl_pk").getKendoDatePicker().value(),
        tgl_awal: $("#tgl_mulai").getKendoDatePicker().value(),
        tgl_akhir: $("#tgl_akh").getKendoDatePicker().value()
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
    });
}