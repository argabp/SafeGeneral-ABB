function onSearchClick() {
    var kodeCabang = $("#KodeCabang").data("kendoComboBox").value().trim();
    var jenisAwal = $("#JenisAsset").data("kendoComboBox").value();
    var jenisAkhir = $("#JenisAssetSD").data("kendoComboBox").value();
    var bulanAwal = $("#BulanAwal").data("kendoComboBox").value();
    var bulanAkhir = $("#BulanAkhir").data("kendoComboBox").value();
    var tahun = $("#Tahun").data("kendoDropDownList").value();

    // if (!kodeCabang) {
    //     alert("Silakan pilih lokasi terlebih dahulu.");
    //     return;
    // }
    if (!bulanAwal || !bulanAkhir || !tahun) {
        alert("Silakan pilih bulan dan tahun dengan lengkap.");
        return;
    }

    var formData = {
        KodeCabang: kodeCabang,
        JenisAwal: jenisAwal,
        JenisAkhir: jenisAkhir,
        BulanAwal: bulanAwal,
        BulanAkhir: bulanAkhir,
        Tahun: tahun
    };
    console.log(formData)

    $.ajax({
        url: '/LaporanPelunasan/GenerateReport',
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify(formData),
        success: function (response) {
            if (response.Status === "OK") {
                window.open("/Reports/" + response.Data + "/LaporanPelunasan.pdf", '_blank');
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
    var comboCabang = $("#KodeCabang").data("kendoComboBox");
    var kodeCabang = comboCabang.value().trim();
    
    // Ambil Teks Cabangnya (Misal: "10 - Cabang Jakarta 1")
    var namaCabangLengkap = comboCabang.text().trim();

    var jenisAwal = $("#JenisAsset").data("kendoComboBox").value();
    var jenisAkhir = $("#JenisAssetSD").data("kendoComboBox").value();
    var bulanAwal = $("#BulanAwal").data("kendoComboBox").value();
    var bulanAkhir = $("#BulanAkhir").data("kendoComboBox").value();
    var tahun = $("#Tahun").data("kendoDropDownList").value();

    // if (!kodeCabang) {
    //     alert("Silakan pilih lokasi terlebih dahulu.");
    //     return;
    // }
    if (!bulanAwal || !bulanAkhir || !tahun) {
        alert("Silakan pilih bulan dan tahun dengan lengkap.");
        return;
    }

    var formData = {
        KodeCabang: kodeCabang,
        JenisAwal: jenisAwal,
        JenisAkhir: jenisAkhir,
        BulanAwal: bulanAwal,
        BulanAkhir: bulanAkhir,
        NamaCabang: namaCabangLengkap,
        Tahun: tahun
    };

    // Ubah text tombol biar user tau lagi loading
    var btn = $("#btnExcel").data("kendoButton");
    btn.enable(false);

    $.ajax({
        url: '/LaporanPelunasan/GenerateExcel', // Nembak ke fungsi baru
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify(formData),
        success: function (response) {
            btn.enable(true);
            
            if (response.Status === "OK") {
                // Trik download file dari Base64 tanpa harus save ke server
                var link = document.createElement('a');
                link.href = "data:application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;base64," + response.FileData;
                link.download = response.FileName;
                link.click();
            } else {
                alert("Gagal membuat laporan: " + response.Message);
            }
        },
        error: function () {
            btn.enable(true);
            alert("Terjadi kesalahan saat membuat file Excel.");
        }
    });
}