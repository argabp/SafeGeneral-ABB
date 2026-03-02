function onSearchClick() {

    // 1. Ambil Tanggal dari Kendo DatePicker
    var dateObj_tgl1 = $("#tgl1").data("kendoDatePicker").value();
    var dateObj_tgl2 = $("#tgl2").data("kendoDatePicker").value();

    if (!dateObj_tgl1 || !dateObj_tgl2) {
        alert("Periode tanggal harus diisi lengkap.");
        return;
    }

    // Format Tanggal (yyyy-MM-dd)
    var tgl1 = kendo.toString(dateObj_tgl1, "yyyy-MM-dd");
    var tgl2 = kendo.toString(dateObj_tgl2, "yyyy-MM-dd");

    // 2. Ambil Kode Cabang
    // Catatan: Sesuai view Index.cshtml sebelumnya, KodeCabang pakai hidden input
    var kodeCabang = $("#KodeCabang").data("kendoComboBox").value(); 
    
    // Jika nanti kamu ubah view-nya jadi Kendo ComboBox seperti jurnal harian, ganti jadi:
    // var kodeCabang = $("#KodeCabang").data("kendoComboBox").value();

    if (!kodeCabang) {
        alert("Silakan pilih cabang terlebih dahulu.");
        return;
    }

    // 3. Ambil Kode Akun (Range)
    var akunAwal = $("#AkunAwal").data("kendoComboBox").value();
    var akunAkhir = $("#AkunAkhir").data("kendoComboBox").value();

    // 4. Susun Data (Sesuai LaporanBukuBesarFilterDto di Controller)
    var formData = {
        KodeCabang: kodeCabang,
        PeriodeAwal: tgl1,
        PeriodeAkhir: tgl2,
        AkunAwal: akunAwal, // Ganti JenisTransaksi jadi Akun
        AkunAkhir: akunAkhir
    };

    console.log("Mengirim Data:", formData);

    // 5. Kirim ke Controller
    $.ajax({
        url: '/LaporanBukuBesar/GenerateReport',
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify(formData),
        success: function (response) {
            if (response.Status === "OK") {
                // Buka PDF di tab baru
                // Pastikan nama file di Controller adalah "LaporanBukuBesar.pdf"
                window.open("/Reports/" + response.Data + "/LaporanBukuBesar.pdf", '_blank');
            } else {
                alert("Gagal membuat laporan: " + response.Message);
            }
        },
        error: function () {
            alert("Terjadi kesalahan saat membuat laporan.");
        }
    });
}

function onAkunAwalChange() {
    var valAwal = $("#AkunAwal").data("kendoComboBox").value();
    var comboAkhir = $("#AkunAkhir").data("kendoComboBox");
    
    // Otomatis isi Akun Akhir sama dengan Akun Awal pas pertama pilih
    if(comboAkhir && !comboAkhir.value()) {
        comboAkhir.value(valAwal);
    }
}

// Mengirim kode cabang yang terpilih ke Controller GetCoaList
// Fungsi ini otomatis dipanggil Kendo pas user ngetik di ComboBox mana pun yang pake .Data() ini
function getKodeCabangParam() {
    var comboCabang = $("#KodeCabang").data("kendoComboBox");
    // Kita ambil aja teks dari input yang lagi "fokus" diketik user
    var activeText = $(document.activeElement).val();

    return {
        kodeCabangDropdown: comboCabang ? comboCabang.value() : "",
        text: activeText
    };
}
// Me-refresh isi combobox AkunAwal & AkunAkhir setiap kali Cabang diubah
function onCabangChange() {
    var cbAwal = $("#AkunAwal").data("kendoComboBox");
    var cbAkhir = $("#AkunAkhir").data("kendoComboBox");

    if (cbAwal) {
        cbAwal.value("");
        cbAwal.dataSource.read();
    }
    if (cbAkhir) {
        cbAkhir.value("");
        cbAkhir.dataSource.read();
    }
}

function onExcelClick() {
    var formData = {
        KodeCabang: $("#KodeCabang").data("kendoComboBox").value(),
        PeriodeAwal: kendo.toString($("#tgl1").data("kendoDatePicker").value(), "yyyy-MM-dd"),
        PeriodeAkhir: kendo.toString($("#tgl2").data("kendoDatePicker").value(), "yyyy-MM-dd"),
        AkunAwal: $("#AkunAwal").data("kendoComboBox").value(),
        AkunAkhir: $("#AkunAkhir").data("kendoComboBox").value()
    };

    var btn = $("#btnExcel").data("kendoButton");
    btn.enable(false);

    $.ajax({
        url: '/LaporanBukuBesar/GenerateExcel',
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