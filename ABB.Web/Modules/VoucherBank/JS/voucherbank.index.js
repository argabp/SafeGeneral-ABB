// Fungsi untuk membuka window Tambah Data
function btnAddVoucherBank_OnClick() {
    var window = $("#VoucherBankWindow").data("kendoWindow");
    // MEMASANG "PENDENGAR": Jalankan 'attachChangeEvents' setelah konten window selesai di-refresh
    window.one("refresh", attachChangeEvents); 
    openWindow('#VoucherBankWindow', '/VoucherBank/Add', 'Add New Voucher Bank');
}
$(document).on('click', '#flagPosting-checkbox-ui', function() {
    var icon = $(this).find('i'); // Ambil elemen ikon di dalam tombol
    var hiddenInput = $("#FlagPosting"); // Ambil input tersembunyi

    // Cek kondisi saat ini dan ubah
    if (hiddenInput.val() === "true") {
        // Jika sedang true, ubah ke false
        hiddenInput.val("false");
        icon.removeClass('fa-check-square').addClass('fa-square'); // Ganti ikon
    } else {
        // Jika sedang false atau kosong, ubah ke true
        hiddenInput.val("true");
        icon.removeClass('fa-square').addClass('fa-check-square'); // Ganti ikon
    }
});
// Fungsi untuk membuka window Edit Data
function btnEditVoucherBank_OnClick(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    var window = $("#VoucherBankWindow").data("kendoWindow");
    // MEMASANG "PENDENGAR": Jalankan 'attachChangeEvents' setelah konten window selesai di-refresh
    window.one("refresh", attachChangeEvents); 
    openWindow('#VoucherBankWindow', `/VoucherBank/Edit?id=${dataItem.NoVoucher.trim()}`, 'Edit Voucher Bank');
}

// Fungsi untuk menyimpan data (dari form Add atau Edit)
function onSaveVoucherBank() {
    var comboCabang = $("#KodeCabang").data("kendoComboBox");
    var kodeCabangValue = comboCabang
    ? comboCabang.value().trim()
    : ($("#KodeCabang").val()?.split("-")[0].trim() || "");
    var url = "/VoucherBank/Save";
   
    var data = {
        KodeCabang: kodeCabangValue,
        JenisVoucher: $("#JenisVoucher").val(),
        DebetKredit: $("#DebetKredit").data("kendoDropDownList").value(), // Cara aman ambil value dropdown
        
        NoVoucher: $("#NoVoucher").val(),
        
        KodeAkun: $("#KodeAkun").val(),
        DiterimaDari: $("#DiterimaDari").val(),
        TanggalVoucher: $("#TanggalVoucher").data("kendoDatePicker").value(), // Cara aman ambil value datepicker
        KodeMataUang: $("#KodeMataUang").val(),
        TotalVoucher: $("#TotalVoucher").data("kendoNumericTextBox").value(), // Cara aman ambil value numeric
        TotalDalamRupiah: $("#TotalDalamRupiah").data("kendoNumericTextBox").value(),
        KeteranganVoucher: $("#KeteranganVoucher").val(),
        FlagPosting: $("#FlagPosting").val(), // Asumsi ini textbox biasa
        KodeBank: $("#KodeBank").val(),
        JenisPembayaran: $("#JenisPembayaran").data("kendoDropDownList").value(),
        NoBank: $("#NoBank").val()
    };
    console.log(data)
    showProgress('#VoucherBankWindow');

    $.ajax({
        type: "POST",
        url: url,
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(data),
        
        success: function (response) {
            
            closeProgress('#VoucherBankWindow');
            if (response && response.success) {
                showMessage('Success', 'Data berhasil disimpan.');
                closeWindow("#VoucherBankWindow");
                refreshGrid("#VoucherBankGrid");
            } else {
                showMessage('Error', 'Gagal menyimpan data.');
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            closeProgress('#VoucherBankWindow');
            if (jqXHR.status === 400) {
                var errorData = jqXHR.responseJSON;
                var errorMessage = "Terdapat error validasi:\n";
                for (var key in errorData.errors) {
                    errorMessage += "- " + errorData.errors[key][0] + "\n";
                }
                alert(errorMessage);
            } else {
                showMessage('Error', 'Tidak dapat terhubung ke server. Status: ' + jqXHR.status);
            }
        }
    });
}

// Fungsi untuk menghapus data
function onDeleteVoucherBank(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));

    // Tampilkan dialog konfirmasi
    showConfirmation('Konfirmasi Hapus', `Apakah Anda yakin ingin menghapus data dengan No. Voucher ${dataItem.NoVoucher}?`,
        function () {
            showProgressOnGrid('#VoucherBankGrid');

            // Kirim request hapus ke Controller
            ajaxGet(`/VoucherBank/Delete?id=${dataItem.NoVoucher.trim()}`,
                function (response) {
                    if (response.success) {
                        showMessage('Success', 'Data berhasil dihapus.');
                    } else {
                        showMessage('Error', 'Gagal menghapus data.');
                    }
                    
                    refreshGrid("#VoucherBankGrid");
                    closeProgressOnGrid('#VoucherBankGrid');
                }
            );
        }
    );
}

$(document).ready(function () {
    $("#SearchKeyword").on("keyup", function() {
        $("#VoucherBankGrid").data("kendoGrid").dataSource.read();
    });
});

function getSearchFilter() {
    return {
        searchKeyword: $("#SearchKeyword").val()
    };
}

// Fungsi kalkulator kurs ke rupiah
function onTanggalVoucherChange() {
    hitungTotalRupiah();
    generateNoVoucher();
}



// Fungsi untuk membuka window Tambah Data
function btnAddVoucherBank_OnClick() {
    var window = $("#VoucherBankWindow").data("kendoWindow");
    window.one("refresh", attachChangeEvents); 
    openWindow('#VoucherBankWindow', '/VoucherBank/Add', 'Add New Voucher Bank');
}

// Fungsi untuk membuka window Edit Data
function btnEditVoucherBank_OnClick(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    var window = $("#VoucherBankWindow").data("kendoWindow");
    window.one("refresh", attachChangeEvents); 
    openWindow('#VoucherBankWindow', `/VoucherBank/Edit?id=${dataItem.NoVoucher.trim()}`, 'Edit Voucher Bank');
}

// Fungsi gabungan untuk event 'change' pada tanggal
function onTanggalVoucherChange() {
    hitungTotalRupiah();
    generateNoVoucher();
}

// Fungsi untuk memasang "kabel" event
function attachChangeEvents() {
    // Panggil semua fungsi sekali saat form pertama kali dimuat
   
    // var isEditMode = $("#NoVoucher").val() !== "";
    //  if (!isEditMode) {
    //     generateNoVoucher();
    // }

    
    // Dapatkan referensi ke setiap widget Kendo
    var jenisVoucher = $("#JenisVoucher");
    var debetKredit = $("#DebetKredit").data("kendoDropDownList");
    var kodeCabang = $("#KodeCabang").data("kendoComboBox");
    var kodeBank = $("#KodeBank").data("kendoComboBox");
    var mataUang = $("#KodeMataUang").data("kendoComboBox");
    var totalVoucher = $("#TotalVoucher").data("kendoNumericTextBox");
    var tanggalVoucher = $("#TanggalVoucher").data("kendoDatePicker");


    if (tanggalVoucher) tanggalVoucher.bind("change", generateNoVoucher);
    // --- Pasang pendengar ke fungsi yang sesuai ---

    // Pemicu untuk generator nomor voucher
    jenisVoucher.on("change", generateNoVoucher);
    if (kodeCabang) kodeCabang.bind("change", generateNoVoucher);
    
    // Pemicu untuk kalkulator kurs
    if (mataUang) mataUang.bind("change", hitungTotalRupiah);
    if (totalVoucher) totalVoucher.bind("change", hitungTotalRupiah);
    if (tanggalVoucher) tanggalVoucher.bind("change", hitungTotalRupiah);
    
    // Pemicu gabungan saat Kode Bank berubah
    if (kodeBank) {
        kodeBank.bind("change", onKodeBankChange); 
    }
    // Pemicu gabungan saat Debet/Kredit berubah
    if (debetKredit) {
        debetKredit.bind("change", onDebetKreditChange);
    }
    updateDynamicLabels();
}


// hitung kurs
function hitungTotalRupiah() {
  
    var kodeMtu = $("#KodeMataUang").data("kendoComboBox").value();
    var tanggal = $("#TanggalVoucher").data("kendoDatePicker").value();
    var totalVoucher = $("#TotalVoucher").data("kendoNumericTextBox").value();

  
    if (kodeMtu && tanggal && totalVoucher) {
        
        var formattedDate = kendo.toString(tanggal, "yyyy-MM-dd");
        var url = `/VoucherBank/GetKurs?kodeMataUang=${kodeMtu}&tanggalVoucher=${formattedDate}`;
     
        $.ajax({
            type: "GET",
            url: url,
            success: function (response) {
                console.log("4. AJAX berhasil! Respons dari server:", response); // <-- LOG 5
                if (response && response.nilai_kurs) {
                    var kurs = response.nilai_kurs;
                    var totalRupiah = totalVoucher * kurs;
                    $("#TotalDalamRupiah").data("kendoNumericTextBox").value(totalRupiah);
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.error("4. AJAX GAGAL!", jqXHR); // <-- LOG 6 (ini akan merah jika error)
            }
        });
    } else {
        console.log("3. Salah satu data kosong, kalkulasi dibatalkan.");
    }
}

// logika untuk generate voucher
function generateNoVoucher() {
    // Ambil nilai dari komponen form
    var jenisVoucher = $("#JenisVoucher").val() || "";
    var debetKredit = $("#DebetKredit").data("kendoDropDownList") ? $("#DebetKredit").data("kendoDropDownList").value() : "";
    var kodeCabang = $("#KodeCabang").data("kendoComboBox") ? $("#KodeCabang").data("kendoComboBox").value() : "";
    var kodeBank = $("#KodeBank").data("kendoComboBox") ? $("#KodeBank").data("kendoComboBox").value() : "";
    var tanggalVoucher = $("#TanggalVoucher").data("kendoDatePicker");
    var tanggal = tanggalVoucher ? tanggalVoucher.value() : null;


     if (!tanggal) {
        $("#NoVoucher").val("");
        return;
    }

    // Panggil AJAX untuk mendapatkan nomor urut berikutnya dari server
    $.ajax({
        type: "GET",
        url: "/VoucherBank/GetNextVoucherNumber",
        data: {
           tanggalVoucher: kendo.toString(tanggal, "yyyy-MM-dd")
        },
        success: function (response) {
            if (response && response.success) {
                var noUrut = response.nextNumber; // Nomor urut dari server

                // --- LOGIKA BARU SESUAI PERMINTAAN ---
                
                // 1. Ambil HANYA ANGKA dari Kode Cabang
                var bagian1 = "";
                if (kodeCabang) {
                    bagian1 = kodeCabang.replace(/[^0-9]/g, ''); // Hapus semua yang bukan angka
                }

                // 2. Nomor Urut dari server
                var bagian2 = noUrut;
                
                // 3. Jenis Voucher (Bank/Kas + Debit/Kredit)
                var bagian3 = "";
                if (jenisVoucher.toUpperCase() === "BANK") bagian3 = "B";
                if (debetKredit.toUpperCase() === "D") bagian3 += "D";
                else if (debetKredit.toUpperCase() === "K") bagian3 += "K";
                
                // 4. Kode Bank
                var bagian4 = kodeBank;
                
                // 5. Bulan dan Tahun hari ini
                var bulan = ('0' + (tanggal.getMonth() + 1)).slice(-2);
                var tahun = tanggal.getFullYear().toString().slice(-2);
                var bagian5 = `${bulan}/${tahun}`;

                // Gabungkan jika semua bagian penting sudah terisi
                if (bagian1 && bagian2 && bagian3 && bagian4 && bagian5) {
                    var noVoucherFinal = `${bagian1}/${bagian2}/${bagian3}/${bagian4}/${bagian5}`;
                    $("#NoVoucher").val(noVoucherFinal);
                }
            }
        }
    });
}

function onKodeBankChange() {
    var kodeBank = $("#KodeBank").data("kendoComboBox").value();

    if (kodeBank) {
        // Panggil action GetAkunByBank di Controller
        $.ajax({
            type: "GET",
            url: `/VoucherBank/GetAkunByBank?kodeBank=${kodeBank}`,
            success: function(response) {
                if (response && response.success) {
                    // Jika berhasil, isi nilai KodeAkun
                    $("#KodeAkun").data("kendoComboBox").value(response.kodeAkun);
                }
            }
        });
    }
    // Panggil juga generator nomor voucher agar ikut ter-update
    generateNoVoucher();
}

function updateDynamicLabels() {
    var debetKredit = $("#DebetKredit").data("kendoDropDownList") ? $("#DebetKredit").data("kendoDropDownList").value() : "";
    var labelDiterimaDari = $("#LabelDiterimaDari");
    var labelJenisPembayaran = $("#LabelJenisPembayaran");

    if (debetKredit.toUpperCase() === "K") { // KREDIT
        labelDiterimaDari.text("Dibayarkan Kepada");
        labelJenisPembayaran.text("Jenis Pembayaran");
    } else { // DEBIT atau lainnya
        labelDiterimaDari.text("Diterima Dari");
        labelJenisPembayaran.text("Jenis Penerimaan");
    }
}
function onDebetKreditChange() {
    generateNoVoucher();
    updateDynamicLabels();
}
