function onSearchClick() {

    var dateObj_tgl1 = $("#tgl1").data("kendoDatePicker").value();
    var dateObj_tgl2 = $("#tgl2").data("kendoDatePicker").value();
    var dateObj_tglpelunasan = $("#tglpelunasan").data("kendoDatePicker").value();

    var tgl1 = kendo.toString(dateObj_tgl1, "yyyy-MM-dd");
    var tgl2 = kendo.toString(dateObj_tgl2, "yyyy-MM-dd");
    var tglpelunasan = kendo.toString(dateObj_tglpelunasan, "yyyy-MM-dd");
    var kodeCabang = $("#KodeCabang").data("kendoComboBox").value().trim();
    var JenisTransaksi = $('input[name="JenisTransaksi"]:checked').val();

    // Contoh penggunaan nilai (bisa Anda hapus jika hanya ingin deklarasi variabel)
    // console.log("Kode Cabang: " + kodeCabang);
    // console.log("Jenis Transaksi: " + JenisTransaksi);
    // console.log("Tanggal Produksi Awal (tgl1): " + tgl1);
    // console.log("Tanggal Produksi Akhir (tgl2): " + tgl2);
    // console.log("Tanggal Pelunasan (tglpelunasan): " + tglpelunasan);
    

    // if (!kodeCabang) {
    //     alert("Silakan pilih lokasi terlebih dahulu.");
    //     return;
    // }
    // if (!bulanAwal || !bulanAkhir || !tahun) {
    //     alert("Silakan pilih bulan dan tahun dengan lengkap.");
    //     return;
    // }

    // var formData = {
    //     KodeCabang: kodeCabang,
    //     JenisAwal: jenisAwal,
    //     JenisAkhir: jenisAkhir,
    //     BulanAwal: bulanAwal,
    //     BulanAkhir: bulanAkhir,
    //     Tahun: tahun
    // };
    // console.log(formData)

    // $.ajax({
    //     url: '/LaporanOutstanding/GenerateReport',
    //     type: 'POST',
    //     contentType: 'application/json',
    //     data: JSON.stringify(formData),
    //     success: function (response) {
    //         if (response.Status === "OK") {
    //             window.open("/Reports/" + response.Data + "/LaporanOutstanding.pdf", '_blank');
    //         } else {
    //             alert("Gagal membuat laporan: " + response.Message);
    //         }
    //     },
    //     error: function () {
    //         alert("Terjadi kesalahan saat membuat laporan.");
    //     }
    // });
}