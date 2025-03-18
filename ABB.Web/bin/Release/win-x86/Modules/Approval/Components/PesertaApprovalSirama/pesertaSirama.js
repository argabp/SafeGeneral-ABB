$(document).ready(function () {
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