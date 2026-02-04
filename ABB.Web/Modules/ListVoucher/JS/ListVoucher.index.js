// Fungsi untuk mengirim Tipe="BANK" ke dataSource KodeBank
  
function getTipeBank() {
    return {
        tipe: "BANK"
    };
}

function getTipeKas() {
    return {
        tipe: "KAS"
    };
}

// Fungsi untuk show/hide dropdown Kode Bank
function onTipeVoucherChange() {
    var tipe = $("#TipeVoucher").data("kendoDropDownList").value();
    console.log("Tipe Voucher:", tipe);

    if (tipe === "BANK") {
        $("#kodeBankSection").show();
        $("#kodeKasSection").hide();
    }
    else if (tipe === "KAS") {
        $("#kodeKasSection").show();
        $("#kodeBankSection").hide();
    }
    else {
        $("#kodeBankSection").hide();
        $("#kodeKasSection").hide();
    }
}


// Ini adalah FUNGSI LAMA ANDA (onSearchClick) yang sudah digabungkan
// dan diganti namanya menjadi onCetakClick
// function onCetakClick() {
//     // 1. Ambil SEMUA nilai filter
//     var tipe = $("#TipeVoucher").data("kendoDropDownList").value();
//     var kodeBank = (tipe === "BANK") ? $("#KodeBank").data("kendoComboBox").value() : "";
//     var tglAwal = $("#TanggalAwal").data("kendoDatePicker").value();
//     var tglAkhir = $("#TanggalAkhir").data("kendoDatePicker").value();

//     // 2. Validasi
//     if (!tglAwal || !tglAkhir) {
//         alert("Silakan pilih tanggal awal dan tanggal akhir.");
//         return;
//     }

//     // 3. Format tanggal
//     var awal = kendo.toString(tglAwal, "yyyy-MM-dd");
//     var akhir = kendo.toString(tglAkhir, "yyyy-MM-dd");

//     // 4. Bangun URL dengan parameter LENGKAP
//     var url = `/ListVoucher/CetakVoucher`
//         + `?tipeVoucher=${encodeURIComponent(tipe)}`
//         + `&kodeBank=${encodeURIComponent(kodeBank || '')}`
//         + `&tanggalAwal=${encodeURIComponent(awal)}`
//         + `&tanggalAkhir=${encodeURIComponent(akhir)}`;
    
//     // 5. Gunakan logika AJAX dan window.open LAMA Anda
//     $.ajax({
//         url: url,
//         method: 'GET',
//         dataType: 'html',
//         success: function (html) {
//             var printWindow = window.open('', '_blank');
//             if (!printWindow) {
//                 alert("Popup diblokir. Izinkan popup untuk menampilkan cetakan.");
//                 return;
//             }

//             printWindow.document.open();
//             printWindow.document.write(html);
//             printWindow.document.close();

//             // Tunggu sebentar sebelum cetak
//             setTimeout(function () {
//                 printWindow.focus();
//                 printWindow.print();
//             }, 500);
//         },
//         error: function () {
//             alert("Gagal memuat data cetak. Coba lagi.");
//         }
//     });
// }

function onCetakClick() {
    // 1. Ambil SEMUA nilai filter
    var tipe = $("#TipeVoucher").data("kendoDropDownList").value();
    var kodeBank = (tipe === "BANK") ? $("#KodeBank").data("kendoComboBox").value() : "";
    var tglAwal = $("#TanggalAwal").data("kendoDatePicker").value();
    var tglAkhir = $("#TanggalAkhir").data("kendoDatePicker").value();
    // var kodeCabang = $("#KodeCabang").data("kendoComboBox").value().trim();

    // 2. Validasi
    if (!tglAwal || !tglAkhir) {
        alert("Silakan pilih tanggal awal dan tanggal akhir.");
        return;
    }
    

    var kodeBank = "";
    var namaBank = "";
    var kodeKas = "";
    var namaKas = "";

    if (tipe === "BANK") {
        var cbBank = $("#KodeBank").data("kendoComboBox");
        var bankItem = cbBank.dataItem();

        if (!bankItem) {
            alert("Silakan pilih Bank.");
            return;
        }

        kodeBank = cbBank.value();
        namaBank = bankItem.Keterangan; // pastikan field ini ada
    }

    if (tipe === "KAS") {
        var cbKas = $("#KodeKas").data("kendoComboBox");
        var kasItem = cbKas.dataItem();

        if (!kasItem) {
            alert("Silakan pilih Kas.");
            return;
        }

        kodeKas = cbKas.value();
        namaKas = kasItem.Keterangan;
    }

    // 3. Format tanggal
    var awal = kendo.toString(tglAwal, "yyyy-MM-dd");
    var akhir = kendo.toString(tglAkhir, "yyyy-MM-dd");

    var formData = {
        tipe: tipe,
        kodeBank: kodeBank,
        keterangan: namaBank,
        kodeKas: kodeKas,
        keteranganKas: namaKas,
        tglAwal: awal,
        tglAkhir: akhir
        // KodeCabang: kodeCabang
    };
    console.log(namaBank)

      $.ajax({
        url: '/ListVoucher/GenerateReport',
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify(formData),
        success: function (response) {
            if (response.Status === "OK") {
                window.open("/Reports/" + response.Data + "/ListVoucher.pdf", '_blank');
            } else {
                alert("Gagal membuat List: " + response.Message);
            }
        },
        error: function () {
            alert("Terjadi kesalahan saat membuat List.");
        }
    });
}

// Panggil sekali saat load dan pasang event listener
$(document).ready(function() {
    onTipeVoucherChange(); // Set tampilan awal
    setTimeout(function() {
        onTipeVoucherChange(); // Set tampilan awal
    }, 1);
    // Pasang event listener untuk tombol Cetak
    $("#btn-cetak").click(onCetakClick);
});

