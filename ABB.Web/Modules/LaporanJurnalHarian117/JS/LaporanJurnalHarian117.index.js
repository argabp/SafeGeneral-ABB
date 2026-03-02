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
        url: '/LaporanJurnalHarian117/GenerateReport',
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify(formData),
        success: function (response) {
            if (response.Status === "OK") {
                window.open("/Reports/" + response.Data + "/LaporanJurnalHarian117.pdf", '_blank');
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
    var dateObj_tgl1 = $("#tgl1").data("kendoDatePicker").value();
    var dateObj_tgl2 = $("#tgl2").data("kendoDatePicker").value();
   
    var tgl1 = kendo.toString(dateObj_tgl1, "yyyy-MM-dd");
    var tgl2 = kendo.toString(dateObj_tgl2, "yyyy-MM-dd");
   
    var kodeCabang = $("#KodeCabang").data("kendoComboBox").value().trim();
    var comboJenis = $("#JenisTransaksi").data("kendoComboBox");
    var jenisTransaksi = comboJenis.value() ? comboJenis.value().trim() : "";

    if (!kodeCabang || !dateObj_tgl1 || !dateObj_tgl2) {
        alert("Silakan lengkapi filter Cabang dan Periode terlebih dahulu.");
        return;
    }

    var formData = {
        KodeCabang: kodeCabang,
        PeriodeAwal: tgl1,
        PeriodeAkhir: tgl2,
        JenisTransaksi: jenisTransaksi
    };

    // Loading state pada tombol
    var btn = $("#btnExcel").data("kendoButton");
    btn.enable(false);

    $.ajax({
        url: '/LaporanJurnalHarian117/GenerateExcel',
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify(formData),
        success: function (res) {
            btn.enable(true);
            if (res.Status === "OK") {
                var link = document.createElement('a');
                link.href = "data:application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;base64," + res.FileData;
                link.download = res.FileName || "LaporanJurnalHarian117.xlsx";
                link.click();
            } else {
                alert("Gagal: " + res.Message);
            }
        },
        error: function () {
            btn.enable(true);
            alert("Terjadi kesalahan saat generate Excel.");
        }
    });
}