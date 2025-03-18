$(document).ready(function () {
    calculatePremi();
    resetCalculate();
});

function setAge(){
    var birthday = $("#tgl_lahir").val();

    ajaxGet("/Kalkulator/GetUsia?tgl_lahir=" + birthday, (result) => {
        $("#usia").val(result);
    })
}

function onGetTanggalAkhir(e){
    var data = {
        tgl_mulai: $("#tgl_mulai").getKendoDatePicker().value(),
        periode_asuransi: $("#periode_asuransi").val()
    }

    var jsonData = JSON.stringify(data);

    ajaxPost("/Kalkulator/GetTanggalAkhir", jsonData, (result) => {
        $("#tgl_akhir").getKendoDatePicker().value(result);
    });
}

function resetCalculate(){
    $('#btn-reset').click(function () {
        var todayDate = kendo.toString(kendo.parseDate(new Date()), 'MM/dd/yyyy');
        $("#tgl_lahir").getKendoDatePicker().value(todayDate);
        $("#usia").getKendoTextBox().value(0);
        $("#nilai_ptg").getKendoNumericTextBox().value(0);
        $("#periode_asuransi").getKendoNumericTextBox().value(0);
        $("#jns_program").getKendoDropDownList().value(-1);
        $("#kd_kategori").getKendoDropDownList().value(-1);
        $("#stn_rate").getKendoDropDownList().value(-1);
        $("#pst_rate").getKendoNumericTextBox().value(0);
        $("#nilai_prm").getKendoNumericTextBox().value(0);
        $("#tgl_mulai").getKendoDatePicker().value(todayDate);
        $("#tgl_akhir").getKendoDatePicker().value(todayDate);
    });
}

function calculatePremi(){
    $('#btn-hitung').click(function () {
        var data = {
            usia: $("#usia").val(),
            nilai_ptg: $("#nilai_ptg").getKendoNumericTextBox().value(),
            tgl_akhir: $("#tgl_akhir").val(),
            tgl_awal: $("#tgl_mulai").val(),
            periode_asuransi: $("#periode_asuransi").val()
        }

        var jsonData = JSON.stringify(data);

        ajaxPost("/Kalkulator/Calculate", jsonData, (result) => {
            $("#jns_program").getKendoDropDownList().value(result.Model.jns_program);
            $("#kd_kategori").getKendoDropDownList().value(result.Model.kd_kategori);
            $("#stn_rate").getKendoDropDownList().value(result.Model.stn_rate);
            $("#pst_rate").getKendoNumericTextBox().value(result.Model.pst_rate);
            $("#nilai_prm").getKendoNumericTextBox().value(result.Model.nilai_prm);
        });
    });
}