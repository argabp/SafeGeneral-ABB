function onSearchClick() {

    var dateObj_tgl1 = $("#tgl1").data("kendoDatePicker").value();
    var dateObj_tgl2 = $("#tgl2").data("kendoDatePicker").value();
   

    var tgl1 = kendo.toString(dateObj_tgl1, "yyyy-MM-dd");
    var tgl2 = kendo.toString(dateObj_tgl2, "yyyy-MM-dd");
   
    var kodeCabang = $("#KodeCabang").data("kendoComboBox").value().trim();
    var jenisTransaksi =  $("#JenisTransaksi").data("kendoComboBox").value().trim();

    if (!kodeCabang) {
        alert("Silakan pilih lokasi terlebih dahulu.");
        return;
    }

    // ⬇️ Data yang dikirim harus sesuai dengan nilai yang Anda punya
    var formData = {
        KodeCabang: kodeCabang,
        PeriodeAwal: tgl1,
        PeriodeAkhir: tgl2,
        JenisTransaksi: jenisTransaksi
    };

    console.log(formData);

    $.ajax({
        url: '/LaporanJurnalHarian/GenerateReport',
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify(formData),
        success: function (response) {
            if (response.Status === "OK") {
                window.open("/Reports/" + response.Data + "/LaporanJurnalHarian.pdf", '_blank');
            } else {
                alert("Gagal membuat laporan: " + response.Message);
            }
        },
        error: function () {
            alert("Terjadi kesalahan saat membuat laporan.");
        }
    });
}

function onExcelClick() {
    var formData = {
        KodeCabang: $("#KodeCabang").data("kendoComboBox").value(),
        PeriodeAwal: kendo.toString($("#tgl1").data("kendoDatePicker").value(), "yyyy-MM-dd"),
        PeriodeAkhir: kendo.toString($("#tgl2").data("kendoDatePicker").value(), "yyyy-MM-dd"),
        JenisTransaksi: $("#JenisTransaksi").data("kendoComboBox").value()
    };

    var btn = $("#btnSearch").data("kendoButton");
    btn.enable(false);

    $.ajax({
        url: '/LaporanJurnalHarian/GenerateExcel',
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify(formData),
        success: function (res) {
            btn.enable(true);
            if (res.Status === "OK") {
                var link = document.createElement('a');
                link.href = "data:application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;base64," + res.FileData;
                link.download = res.FileName;
                link.click();
            } else { alert(res.Message); }
        },
        error: function () { btn.enable(true); alert("Error!"); }
    });
}