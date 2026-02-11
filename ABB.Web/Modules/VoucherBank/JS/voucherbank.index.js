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
    openWindow('#VoucherBankWindow', `/VoucherBank/Edit?id=${dataItem.Id}`, 'Edit Voucher Bank');
}

// Fungsi untuk menyimpan data (dari form Add atau Edit)
function onSaveVoucherBank() {
    // 1. Ambil Komponen
    var comboCabang = $("#KodeCabang").data("kendoComboBox");
    var kodeCabangValue = comboCabang
        ? comboCabang.value().trim()
        : ($("#KodeCabang").val()?.split("-")[0].trim() || "");
    
    var url = "/VoucherBank/Save";

    // 2. Ambil Status & Nilai Textbox
    var isSementara = $("#FlagSementara").is(":checked");
    var noVoucherUtama = $("#NoVoucher").val(); // Pastikan ini terambil
    var noVoucherSmt = $("#NoVoucherSementara").val();

    // 3. LOGIKA PENENTUAN NO VOUCHER (FINAL)
    var finalNoVoucher = null;

    if (isSementara) {
        // --- KASUS SEMENTARA ---
        // Validasi: No Smt wajib ada
        if (!noVoucherSmt) {
            alert("Nomor Voucher Sementara belum terbentuk!");
            return;
        }
        // No Voucher Utama dikosongkan (null) agar tidak bentrok
        finalNoVoucher = null; 
    } else {
        // --- KASUS TETAP (ORIGINAL) ---
        // Validasi: No Utama wajib ada
        if (!noVoucherUtama) {
            alert("Nomor Voucher belum terbentuk!");
            return;
        }
        // Kirim No Voucher Utama
        finalNoVoucher = noVoucherUtama;
    }
    var data = {
        Id: $("#Id").val(),
        KodeCabang: kodeCabangValue,
        JenisVoucher: $("#JenisVoucher").val(),
        DebetKredit: $("#DebetKredit").data("kendoDropDownList").value(),
        
        // Logic Nomor
        NoVoucher: finalNoVoucher,
        FlagSementara: isSementara,
        NoVoucherSementara: noVoucherSmt, // Selalu kirim SMT sebagai history

        KodeBank: $("#KodeBank").data("kendoComboBox").value().trim(),
        KodeAkun: $("#KodeAkun").val(), // Ambil dari input/combo
        NoBank: $("#NoBank").val(),
        
        DiterimaDari: $("#DiterimaDari").val(),
        TanggalVoucher: kendo.toString($("#TanggalVoucher").data("kendoDatePicker").value(), "yyyy-MM-dd"),
        
        KodeMataUang: $("#KodeMataUang").val(),
        TotalVoucher: $("#TotalVoucher").data("kendoNumericTextBox").value(),
        TotalDalamRupiah: $("#TotalDalamRupiah").data("kendoNumericTextBox").value(),
        
        JenisPembayaran: $("#JenisPembayaran").data("kendoDropDownList").value(),
        KeteranganVoucher: $("#KeteranganVoucher").val(),
        
        FlagPosting: $("#FlagPosting").val() === "true"
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
            ajaxGet(`/VoucherBank/Delete?id=${dataItem.Id}`,
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



// Fungsi gabungan untuk event 'change' pada tanggal
function onTanggalVoucherChange() {
    hitungTotalRupiah();
    generateNoVoucher();
}

function checkVoucherGeneration() {
    var isSementara = $("#FlagSementara").is(":checked");
    var txtSmt = $("#NoVoucherSementara");
    var txtUtama = $("#NoVoucher");

    if (isSementara) {
        // --- MODE: SEMENTARA (Checked) ---
        
        // Cek apakah ada nilai original (saat edit)?
        var originalSmt = txtSmt.data("original-value");
        
        // Kalau ada nilai lama, kembalikan. Kalau kosong, generate baru.
        if (originalSmt) {
            txtSmt.val(originalSmt);
        } else {
            // Cek biar gak generate berulang kalau udah ada isinya
            if(!txtSmt.val()){
                 generateNoVoucherSementara();
            }
        }
        
        // Kosongkan field Utama (Visual only) -> Karena belum jadi voucher tetap
        txtUtama.val(""); 

    } else {
        // --- MODE: TETAP / ORIGINAL (Unchecked) ---
        
        generateNoVoucher(); 
        
        // [PERBAIKAN PENTING DI SINI]
        // Jangan hapus No Voucher Sementara jika ini adalah data hasil EDIT (punya original value)
        // Biarkan saja dia tertulis di situ sebagai histori.
        
        var originalSmt = txtSmt.data("original-value");
        if (originalSmt) {
            txtSmt.val(originalSmt); // Pastikan nilainya tetap ada
        } else {
            // Kalau data baru (Add), boleh dikosongkan biar bersih
            txtSmt.val(""); 
        }
    }
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

    // 1. EVENT: Checkbox Flag Sementara
        var txtSmt = $("#NoVoucherSementara");
            // Simpan nilai awal ke dalam memori elemen (data attribute)
            if (txtSmt.val()) {
                txtSmt.data("original-value", txtSmt.val());
            }
        // 1. EVENT: Checkbox Flag Sementara
        $("#FlagSementara").on("change", function() {
            checkVoucherGeneration(); 
        });

    if (tanggalVoucher) tanggalVoucher.bind("change", generateNoVoucher);
    // --- Pasang pendengar ke fungsi yang sesuai ---

    // Pemicu untuk generator nomor voucher
    jenisVoucher.on("change", generateNoVoucher);
    if (kodeCabang) kodeCabang.bind("change", checkVoucherGeneration);
    jenisVoucher.on("change", checkVoucherGeneration);
    
    // Pemicu untuk kalkulator kurs
    if (mataUang) mataUang.bind("change", hitungTotalRupiah);
    if (totalVoucher) totalVoucher.bind("change", hitungTotalRupiah);
    if (tanggalVoucher) {
                tanggalVoucher.bind("change", function() {
                    hitungTotalRupiah();
                    checkVoucherGeneration(); // Generate Nomor sesuai checkbox
                });
            }
    
    // Pemicu gabungan saat Kode Bank berubah
    if (kodeBank) {
        kodeBank.bind("change", function() {
            onKodeBankChange(); 
            // Jangan panggil generateNoVoucher() langsung!
            checkVoucherGeneration(); 
        });
    }
    // Pemicu gabungan saat Debet/Kredit berubah
    if (debetKredit) {
        debetKredit.bind("change", function() {
            onDebetKreditChange(); // Label update
            checkVoucherGeneration(); // Cek mau generate yg mana
        });
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
     var kodeCabangText = $("#KodeCabang").data("kendoComboBox")?.input.val() || "";
        var kodeCabang = kodeCabangText.split(" - ")[0].trim(); 
    var kodeBank = $("#KodeBank").data("kendoComboBox") ? $("#KodeBank").data("kendoComboBox").value() : "";
    var tanggalVoucher = $("#TanggalVoucher").data("kendoDatePicker");
    var tanggal = tanggalVoucher ? tanggalVoucher.value() : null;


     if (!tanggal) {
        tanggal = new Date(); 
    }
    console.log("Kode Cabang:", kodeCabang);
    if (!kodeCabang) return;

    // harusnya bekerja di prod yaa
    var formattedDateForAPI = kendo.toString(tanggal, "yyyy-MM-dd");
    // Panggil AJAX untuk mendapatkan nomor urut berikutnya dari server
    $.ajax({
        type: "GET",
        url: "/VoucherBank/GetNextVoucherNumber",
        data: {
           tanggalVoucher: kendo.toString(tanggal, "yyyy-MM-dd"),
           kodeCabang: kodeCabang // [PENTING] Filter Global per Cabang
        },
        success: function (response) {
            if (response && response.success) {
                var noUrut = response.nextNumber;

                // --- RAKIT STRING ---
                // Format: 50/BD01/02/2026/001
                
                // 1. Cabang (2 digit belakang)
                var bagian1 = kodeCabang.slice(-2);
                
                // 2. Prefix (B + D/K + KodeBank)
                var prefixTipe = "B"; 
                if (debetKredit.toUpperCase() === "D") prefixTipe += "D";
                else if (debetKredit.toUpperCase() === "K") prefixTipe += "K";
                var bagian2 = prefixTipe + kodeBank;
                
                // 3. Periode
                var bulan = ('0' + (tanggal.getMonth() + 1)).slice(-2);
                var tahun = tanggal.getFullYear().toString();
                var bagian3 = `${bulan}/${tahun}`;
                
                // 4. Urut
                var bagian4 = noUrut;

                if (bagian1 && bagian2 && bagian3 && bagian4) {
                     var noVoucherFinal = `${bagian1}/${bagian2}/${bagian3}/${bagian4}`;
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
    // generateNoVoucher();
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

function btnCetakPembayaranBank_OnClick(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    var noVoucher = dataItem.NoVoucher;

    // buka tab baru (tanpa auto-print)
    window.open(`Cetak?id=${dataItem.Id}`, "_blank");
}

function onDebetKreditChange() {
    updateDynamicLabels();
}
function generateNoVoucherSementara() {
    var kodeCabangText = $("#KodeCabang").data("kendoComboBox")?.input.val() || "";
    var kodeCabang = kodeCabangText.split(" - ")[0].trim(); 
    console.log(kodeCabang)
    var kodeBank = $("#KodeBank").data("kendoComboBox")?.value() || "";
    var debetKredit = $("#DebetKredit").data("kendoDropDownList")?.value() || "";

    var kendoDatePicker = $("#TanggalVoucher").data("kendoDatePicker");
    var tanggalSelected = kendoDatePicker ? kendoDatePicker.value() : null;
     if (!tanggalSelected) {
            tanggalSelected = new Date(); 
        }
    var formattedDate = kendo.toString(tanggalSelected, "yyyy-MM-dd");

    // Hanya generate jika data pendukung lengkap
    if (kodeCabang && kodeBank && debetKredit) {
        $.ajax({
            type: "GET",
            url: "/VoucherBank/GetNextVoucherSementara",
            data: {
                kodeCabang: kodeCabang,
                kodeBank: kodeBank,
                tanggalVoucher: formattedDate,
                debetKredit: debetKredit 
            },
            success: function (response) {
                if (response.success) {
                    $("#NoVoucherSementara").val(response.noVoucherSmt);
                }
            }
        });
    }
}