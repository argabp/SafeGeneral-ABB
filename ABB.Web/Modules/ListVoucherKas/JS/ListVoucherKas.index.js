function onSearchClick() {
    var tglAwal = $("#StartDate").data("kendoDatePicker").value();
    var tglAkhir = $("#EndDate").data("kendoDatePicker").value();

    if (!tglAwal || !tglAkhir) {
        alert("Silakan pilih tanggal awal dan tanggal akhir.");
        return;
    }

    // Format ISO (server-friendly)
    var awal = kendo.toString(tglAwal, "yyyy-MM-dd");
    var akhir = kendo.toString(tglAkhir, "yyyy-MM-dd");

    // Panggil URL yang mengembalikan HTML cetak
    var url = '/ListVoucherKas/CetakVoucherKas?tanggalAwal=' + encodeURIComponent(awal) + '&tanggalAkhir=' + encodeURIComponent(akhir);

    // Ambil HTML via AJAX, buka window dan tulis hasilnya
    $.ajax({
        url: url,
        method: 'GET',
        dataType: 'html',
        success: function (html) {
            // Buka jendela baru (tab) untuk cetak
            var printWindow = window.open('', '_blank');
            if (!printWindow) {
                alert("Popup diblokir. Izinkan popup untuk menampilkan cetakan.");
                return;
            }

            // tulis HTML ke jendela baru
            printWindow.document.open();
            printWindow.document.write(html);
            printWindow.document.close();

            // Tunggu sedikit biar resource (font/css) ter-load, lalu panggil print
            //  (0.5s biasanya cukup; kalau ada gambar/generasi berat, tingkatkan)
            setTimeout(function () {
                printWindow.focus();
                printWindow.print();
                // window.close() jika mau menutup otomatis setelah print (opsional)
                // printWindow.close();
            }, 500);
        },
        error: function () {
            alert("Gagal memuat data cetak. Coba lagi.");
        }
    });
}