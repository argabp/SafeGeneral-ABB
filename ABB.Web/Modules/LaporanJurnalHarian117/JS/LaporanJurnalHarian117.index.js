function onSearchClick() {

    var dateObj_tgl1 = $("#tgl1").data("kendoDatePicker").value();
    var dateObj_tgl2 = $("#tgl2").data("kendoDatePicker").value();
   

    var tgl1 = kendo.toString(dateObj_tgl1, "yyyy-MM-dd");
    var tgl2 = kendo.toString(dateObj_tgl2, "yyyy-MM-dd");
   
    var kodeCabang = $("#KodeCabang").data("kendoComboBox").value().trim();
    var jenisTransaksi = $("input[name='JenisTransaksi']:checked").val();

    if (!kodeCabang) {
        alert("Silakan pilih lokasi terlebih dahulu.");
        return;
    }

    // ⬇️ Data yang dikirim harus sesuai dengan nilai yang Anda punya
    var formData = {
        KodeCabang: kodeCabang,
        PeriodeAwal: tgl1,
        PeriodeAkhir: tgl2,
        JenisTransaksi: $("#JenisTransaksi").val()
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