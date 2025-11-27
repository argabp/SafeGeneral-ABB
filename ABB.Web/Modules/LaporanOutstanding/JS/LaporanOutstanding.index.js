function onSearchClick() {

    var dateObj_tgl1 = $("#tgl1").data("kendoDatePicker").value();
    var dateObj_tgl2 = $("#tgl2").data("kendoDatePicker").value();
    var dateObj_tglpelunasan = $("#tglpelunasan").data("kendoDatePicker").value();

    var tgl1 = kendo.toString(dateObj_tgl1, "yyyy-MM-dd");
    var tgl2 = kendo.toString(dateObj_tgl2, "yyyy-MM-dd");
    var tglpelunasan = kendo.toString(dateObj_tglpelunasan, "yyyy-MM-dd");

    var kodeCabang = $("#KodeCabang").data("kendoComboBox").value().trim();
    // var JenisTransaksi = $('input[name="JenisTransaksi"]:checked').val();

    if (!kodeCabang) {
        alert("Silakan pilih lokasi terlebih dahulu.");
        return;
    }

    // ⬇️ Data yang dikirim harus sesuai dengan nilai yang Anda punya
    var formData = {
        KodeCabang: kodeCabang,
        TglProduksiAwal: tgl1,
        TglProduksiAkhir: tgl2,
        TglPelunasan: tglpelunasan
        // JenisTransaksi: JenisTransaksi
    };

    console.log(formData);

    $.ajax({
        url: '/LaporanOutstanding/GenerateReport',
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify(formData),
        success: function (response) {
            if (response.Status === "OK") {
                window.open("/Reports/" + response.Data + "/LaporanOutstanding.pdf", '_blank');
            } else {
                alert("Gagal membuat laporan: " + response.Message);
            }
        },
        error: function () {
            alert("Terjadi kesalahan saat membuat laporan.");
        }
    });
}