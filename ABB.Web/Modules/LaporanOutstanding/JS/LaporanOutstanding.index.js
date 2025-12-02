function onSearchClick() {

    var dateObj_tgl1 = $("#tgl1").data("kendoDatePicker").value();
    var dateObj_tgl2 = $("#tgl2").data("kendoDatePicker").value();
    var dateObj_tglpelunasan = $("#tglpelunasan").data("kendoDatePicker").value();

    var tgl1 = kendo.toString(dateObj_tgl1, "yyyy-MM-dd");
    var tgl2 = kendo.toString(dateObj_tgl2, "yyyy-MM-dd");
    var tglpelunasan = kendo.toString(dateObj_tglpelunasan, "yyyy-MM-dd");

    var kodeCabang = $("#KodeCabang").data("kendoComboBox").value().trim();
    var jenisTransaksi = $("input[name='JenisTransaksi']:checked").val();
        var selectedCodes = [];

        // Pilih container yang aktif
        var containerSelector = "";
        if (jenisTransaksi === "Piutang") {
            containerSelector = "#checkboxContainerPiutang";
        } else {
            containerSelector = "#checkboxContainerHutang";
        }

        // Loop checkbox yang dicentang di dalam container aktif
        $(containerSelector + " input[type='checkbox']:checked").each(function () {
            // Name format: Check_A1 atau Check_H_A1
            // Kita ambil bagian paling belakang (A1, B1, dst)
            var fullName = $(this).attr("name");
            var parts = fullName.split('_');
            var code = parts[parts.length - 1]; // Selalu ambil bagian terakhir
            
            selectedCodes.push(code);
        });

        // Validasi minimal 1 checkbox
        if (selectedCodes.length === 0) {
            kendo.alert("Mohon pilih minimal satu jenis transaksi (Checkbox).");
            return;
        }

    if (!kodeCabang) {
        alert("Silakan pilih lokasi terlebih dahulu.");
        return;
    }

    // ⬇️ Data yang dikirim harus sesuai dengan nilai yang Anda punya
    var formData = {
        KodeCabang: kodeCabang,
        TglProduksiAwal: tgl1,
        TglProduksiAkhir: tgl2,
        TglPelunasan: tglpelunasan,
        JenisLaporan: $("#JenisLaporan").val(),
        JenisTransaksi: jenisTransaksi,
        SelectedCodes: selectedCodes
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