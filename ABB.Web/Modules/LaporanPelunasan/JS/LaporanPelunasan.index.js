function onSearchClick() {
    var kodeCabang = $("#KodeCabang").data("kendoComboBox").value().trim();
    var jenisAwal = $("#JenisAsset").data("kendoComboBox").value();
    var jenisAkhir = $("#JenisAssetSD").data("kendoComboBox").value();
    var bulanAwal = $("#BulanAwal").data("kendoComboBox").value();
    var bulanAkhir = $("#BulanAkhir").data("kendoComboBox").value();
    var tahun = $("#Tahun").data("kendoDropDownList").value();

    if (!kodeCabang) {
        alert("Silakan pilih lokasi terlebih dahulu.");
        return;
    }
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