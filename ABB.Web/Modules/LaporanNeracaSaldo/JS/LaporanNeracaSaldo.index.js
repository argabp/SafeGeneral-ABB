function onTipeAkunAwalChange() {
    var comboAwal = $("#TipeAkunAwal").data("kendoComboBox");
    var comboAkhir = $("#TipeAkunAkhir").data("kendoComboBox");
    
    if (comboAwal && comboAkhir) {
        var valAwal = comboAwal.value();
        // Cek jika awal diisi, tapi akhir masih kosong
        if (valAwal && !comboAkhir.value()) {
            comboAkhir.value(valAwal);
            comboAkhir.text(comboAwal.text()); // Set labelnya juga
        }
    }
}
function getTipeAkunParam() {
    return {
        text: $(document.activeElement).val()
    };
}

function onSearchClick() {
    var bulan = $("#Bulan").data("kendoComboBox").value();
    var tahun = $("#Tahun").data("kendoDropDownList").value();
    // Ambil kedua nilainya
    var tipeAwal = $("#TipeAkunAwal").data("kendoComboBox").value();
    var tipeAkhir = $("#TipeAkunAkhir").data("kendoComboBox").value();

    if (!bulan || !tahun) {
        alert("Silakan pilih bulan dan tahun dengan lengkap.");
        return;
    }

    var formData = {
        Bulan: bulan,
        Tahun: tahun,
        TipeAkunAwal: tipeAwal,   // <-- Baru
        TipeAkunAkhir: tipeAkhir
    };
    console.log(formData)

    $.ajax({
        url: '/LaporanNeracaSaldo/GenerateReport',
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify(formData),
        success: function (response) {
            if (response.Status === "OK") {
                window.open("/Reports/" + response.Data + "/LaporanNeracaSaldo.pdf", '_blank');
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
    var bulan = $("#Bulan").data("kendoComboBox").value();
    var tahun = $("#Tahun").data("kendoDropDownList").value();
    var tipeAwal = $("#TipeAkunAwal").data("kendoComboBox").value();
    var tipeAkhir = $("#TipeAkunAkhir").data("kendoComboBox").value();

    if (!bulan || !tahun) {
        alert("Silakan pilih bulan dan tahun dengan lengkap.");
        return;
    }

    var formData = {
        Bulan: bulan,
        Tahun: tahun,
        TipeAkunAwal: tipeAwal,   // <-- Baru
        TipeAkunAkhir: tipeAkhir  // <-- Baru
    };

    // Ubah text tombol biar user tau lagi loading
    var btn = $("#btnExcel").data("kendoButton");
    btn.enable(false);

    $.ajax({
        url: '/LaporanNeracaSaldo/GenerateExcel', // Nembak ke fungsi baru
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