// Fungsi untuk membuka window Tambah Data
function btnAddVoucherKas_OnClick() {
    var window = $("#VoucherKasWindow").data("kendoWindow");
    window.one("refresh", attachChangeEvents); 
    openWindow('#VoucherKasWindow', '/VoucherKas/Add', 'Add New Voucher Kas');
}

function onKodeKasChange(e) {
        // 'this' mengacu pada widget Kendo ComboBox
        var kodeKas = this.value(); 
        console.log("Kode Kas Changed:", kodeKas); // Cek console browser

        if (kodeKas) {
            $.ajax({
                url: '/VoucherKas/GetAkunByKas',
                type: 'GET',
                data: { kodeKas: kodeKas },
                success: function (data) {
                    if (data.success) {
                        var cbAkun = $("#KodeAkun").data("kendoComboBox");
                        if (cbAkun) {
                            cbAkun.value(data.kodeAkun);
                            cbAkun.trigger("change"); // Trigger change biar validasi jalan (opsional)
                        }
                    } else {
                        console.warn("Gagal mengambil Kode Akun");
                    }
                },
                error: function(err) {
                    console.error("Error AJAX:", err);
                }
            });
        } else {
            // Opsional: Kosongkan Kode Akun jika Kode Kas dihapus
            var cbAkun = $("#KodeAkun").data("kendoComboBox");
            if (cbAkun) cbAkun.value(""); 
        }
    }

$(document).on('click', '#flagPosting-checkbox-ui', function() {
   var icon = $(this).find('i'); 
    var hiddenInput = $("#FlagPosting");
    
    // Ambil widget DatePicker
    var tglVoucherPicker = $("#TanggalVoucher").data("kendoDatePicker"); 

    // Cek kondisi saat ini dan ubah
    if (hiddenInput.val() === "true") {
        // --- PROSES UN-POST (Batal Posting) ---
        hiddenInput.val("false");
        icon.removeClass('fa-check-square').addClass('fa-square'); // Jadi kotak kosong
        
        // HAPUS LOGICA DATEPICKER DISINI

    } else {
        // --- PROSES POSTING ---
        hiddenInput.val("true");
        icon.removeClass('fa-square').addClass('fa-check-square'); // Jadi centang
        
        // HAPUS LOGICA DATEPICKER DISINI
    }
});
function onSaveVoucherKas() {
    var errors = [];
    
    // --- 1. AMBIL VALUE KOMPONEN ---
    var debetKredit = $("#DebetKredit").data("kendoDropDownList").value();
    var kodeKas = $("#KodeKas").data("kendoComboBox").value();
    var tanggalVoucher = $("#TanggalVoucher").data("kendoDatePicker").value();
    var totalVoucher = $("#TotalVoucher").data("kendoNumericTextBox").value();
    var kodeMataUang = $("#KodeMataUang").data("kendoComboBox").value();
    var jenisPembayaran = $("#JenisPembayaran").data("kendoDropDownList").value();
    
    // Ambil Flag & Nomor
    var isSementara = $("#FlagSementara").is(":checked");
    var noVoucherUtama = $("#NoVoucher").val();
    var noVoucherSmt = $("#NoVoucherSementara").val();
    
    // --- 2. VALIDASI DASAR ---
    if (!debetKredit) errors.push("- Debet/Kredit harus dipilih");
    if (!kodeKas) errors.push("- Kode Kas harus dipilih");
    if (!tanggalVoucher) errors.push("- Tanggal Voucher harus diisi");
    if (!kodeMataUang) errors.push("- Kode Mata Uang harus dipilih");
    if (totalVoucher === null || totalVoucher <= 0) errors.push("- Total Voucher harus lebih dari 0");
    if (!jenisPembayaran) errors.push("- Jenis Pembayaran harus dipilih");

    // Validasi Nomor Voucher (Sesuai Flag)
    if (isSementara) {
        if (!noVoucherSmt) errors.push("- Nomor Voucher Sementara belum terbentuk");
    } else {
        if (!noVoucherUtama) errors.push("- Nomor Voucher belum terbentuk");
    }

    if (errors.length > 0) {
        alert("Mohon lengkapi data berikut:\n" + errors.join("\n"));
        return; 
    }

    // --- 3. PERSIAPAN DATA ---
    var comboCabang = $("#KodeCabang").data("kendoComboBox");
    var kodeCabangValue = comboCabang
        ? comboCabang.value().trim()
        : ($("#KodeCabang").val()?.split("-")[0].trim() || "");

    // Tentukan Primary Key yang akan dikirim
    // Jika Sementara = TRUE, PK-nya adalah No Smt. Jika FALSE, PK-nya No Utama.
    var finalNoVoucher = isSementara ? null : noVoucherUtama;

    var data = {
        Id: $("#Id").val(),
        KodeCabang: kodeCabangValue,
        JenisVoucher: $("#JenisVoucher").val(),
        DebetKredit: debetKredit,
        KodeKas: kodeKas,
        
        // --- PENTING: Primary Key ---
       NoVoucher: finalNoVoucher,
        // --- TAMBAHAN BARU ---
        FlagSementara: isSementara,
        NoVoucherSementara: noVoucherSmt, // Kirim null jika tidak sementara (opsional)
        // --------------------

        KodeAkun: $("#KodeAkun").data("kendoComboBox").value(), // Pake value() lebih aman buat combo
        DibayarKepada: $("#DibayarKepada").val(),
        TanggalVoucher: kendo.toString(tanggalVoucher, "yyyy-MM-dd"), // Format date manual biar aman
        TotalVoucher: totalVoucher,
        KodeMataUang: kodeMataUang,
        KeteranganVoucher: $("#KeteranganVoucher").val(),
        TotalDalamRupiah: $("#TotalDalamRupiah").data("kendoNumericTextBox").value(),
        JenisPembayaran: jenisPembayaran,
        FlagPosting: $("#FlagPosting").val() === "true" // Pastikan boolean
    };
    
    // --- 4. KIRIM AJAX ---
    showProgress('#VoucherKasWindow');

    $.ajax({
        type: "POST",
        url: "/VoucherKas/Save",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(data),
        success: function (response) {
            closeProgress('#VoucherKasWindow');
            if (response && response.success) {
                showMessage('Success', 'Data berhasil disimpan.');
                closeWindow("#VoucherKasWindow");
                refreshGrid("#VoucherKasGrid");
            } else {
                // Tampilkan pesan error spesifik dari server jika ada
                showMessage('Error', response.message || 'Gagal menyimpan data.');
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            closeProgress('#VoucherKasWindow');
            if (jqXHR.status === 400) {
                var errorData = jqXHR.responseJSON;
                var errorMessage = "Terdapat error validasi:\n";
                // Handle struktur error standar ASP.NET Core
                if (errorData.errors) {
                    for (var key in errorData.errors) {
                        errorMessage += "- " + errorData.errors[key][0] + "\n";
                    }
                } else {
                    errorMessage += "- Data tidak valid.";
                }
                alert(errorMessage);
            } else {
                showMessage('Error', 'Tidak dapat terhubung ke server. Status: ' + jqXHR.status);
            }
        }
    });
}

// Fungsi untuk membuka window Edit Data
function btnEditVoucherKas_OnClick(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));

    var window = $("#VoucherKasWindow").data("kendoWindow");
    window.one("refresh", attachChangeEvents); 
    
    // --- GANTI LOGIC URL ---
    // Gunakan ID (dataItem.Id) bukan NoVoucher
    // Pastikan Controller Edit menerima parameter (long id)
    openWindow('#VoucherKasWindow', `/VoucherKas/Edit?id=${dataItem.Id}`, 'Edit Voucher Kas');
}

// Fungsi untuk menghapus data
function onDeleteVoucherKas(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));

    // Tampilkan dialog konfirmasi (Text boleh tetap NoVoucher buat user lihat)
    showConfirmation('Konfirmasi Hapus', `Apakah Anda yakin ingin menghapus data dengan No. Voucher ${dataItem.NoVoucher}?`,
        function () {
            showProgressOnGrid('#VoucherKasGrid');

            // --- GANTI LOGIC URL ---
            // Kirim ID ke Controller Delete
            ajaxGet(`/VoucherKas/Delete?id=${dataItem.Id}`,
                function (response) {
                    if (response.success) {
                        showMessage('Success', 'Data berhasil dihapus.');
                    } else {
                        showMessage('Error', 'Gagal menghapus data.');
                    }
                    
                    refreshGrid("#VoucherKasGrid");
                    closeProgressOnGrid('#VoucherKasGrid');
                }
            );
        }
    );
}

$(document).ready(function () {
    $("#SearchKeyword").on("keyup", function() {
        $("#VoucherKasGrid").data("kendoGrid").dataSource.read();
    });
});
    function getSearchFilter() {
        return {
            searchKeyword: $("#SearchKeyword").val()
        };
    }

// Fungsi untuk memasang "kabel" event
   function attachChangeEvents() {
        var kodeKasCombo = $("#KodeKas").data("kendoComboBox");
        var tanggalVoucher = $("#TanggalVoucher").data("kendoDatePicker");
        var kodeCabang = $("#KodeCabang").data("kendoComboBox");
        var debetKredit = $("#DebetKredit").data("kendoDropDownList");
        var totalVoucherInput = $("#TotalVoucher").data("kendoNumericTextBox");
        var mataUangCombo = $("#KodeMataUang").data("kendoComboBox");

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

        // 2. EVENT: Komponen Pembentuk Nomor (Panggil Fungsi Pusat)
        if (kodeKasCombo) {
            kodeKasCombo.bind("change", function() {
                onKodeKasChangeInternal.call(this); // Ambil akun otomatis
                checkVoucherGeneration();           // Generate Nomor
            });
        }
        
        if (tanggalVoucher) {
            tanggalVoucher.bind("change", function() {
                hitungTotalRupiah();
                checkVoucherGeneration(); // Generate Nomor sesuai checkbox
            });
        }

        if (kodeCabang) kodeCabang.bind("change", checkVoucherGeneration);
        if (debetKredit) {
            debetKredit.bind("change", function() {
                // Update Label
                var val = this.value();
                if (val && val.toUpperCase() === "D") {
                    $("#labelDibayarKepada").text("Diterima Dari");
                } else {
                    $("#labelDibayarKepada").text("Dibayar Kepada");
                }
                // Generate Nomor
                checkVoucherGeneration();
            });
        }

        // 3. EVENT: Kalkulator Kurs
        if (mataUangCombo) mataUangCombo.bind("change", hitungTotalRupiah);
        if (totalVoucherInput) totalVoucherInput.bind("change", hitungTotalRupiah);
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
    function onKodeKasChangeInternal(e) {
        var kodeKas = this.value(); 
        var kodeCabang = $("#KodeCabang").data("kendoComboBox").value().trim();
        console.log("Kode Kas Changed (Internal):", kodeKas);

        if (kodeKas) {
            $.ajax({
                url: '/VoucherKas/GetAkunByKas',
                type: 'GET',
                data: { 
                    kodeKas: kodeKas,
                    KodeCabang : kodeCabang,
                    Tipe : 'KAS'
                },
                success: function (data) {
                    if (data.success) {
                        var cbAkun = $("#KodeAkun").data("kendoComboBox");
                        if (cbAkun) {
                            cbAkun.value(data.kodeAkun);
                            // Trigger change biar kalau ada validasi dia jalan
                            cbAkun.trigger("change"); 
                        }
                    }
                }
            });
        } else {
            // Kosongkan akun jika kas dihapus
            var cbAkun = $("#KodeAkun").data("kendoComboBox");
            if(cbAkun) cbAkun.value("");
        }
    }
    // hitung kurs
    function hitungTotalRupiah() {
    
        var kodeMtu = $("#KodeMataUang").data("kendoComboBox").value();
        var tanggal = $("#TanggalVoucher").data("kendoDatePicker").value();
        var totalVoucher = $("#TotalVoucher").data("kendoNumericTextBox").value();

    
        if (kodeMtu && tanggal && totalVoucher) {
            
            var formattedDate = kendo.toString(tanggal, "yyyy-MM-dd");
            var url = `/VoucherKas/GetKurs?kodeMataUang=${kodeMtu}&tanggalVoucher=${formattedDate}`;
        
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

    // --- LOGIC GENERATE NOMOR SEMENTARA (BARU) ---
    function generateNoVoucherSementara() {
        var kodeCabangText = $("#KodeCabang").data("kendoComboBox")?.input.val() || "";
        var kodeCabang = kodeCabangText.split(" - ")[0].trim(); 
        var kodeKas = $("#KodeKas").val() || ""; 
        
        // --- TAMBAHAN: AMBIL DEBET/KREDIT ---
        var debetKredit = $("#DebetKredit").data("kendoDropDownList")?.value() || "";

        var kendoDatePicker = $("#TanggalVoucher").data("kendoDatePicker");
        var tanggalSelected = kendoDatePicker ? kendoDatePicker.value() : null;

        // JIKA TANGGAL KOSONG, PAKAI HARI INI (DEFAULT)
        // Supaya nomor SMT langsung muncul tanpa harus pilih tanggal dulu
        if (!tanggalSelected) {
            tanggalSelected = new Date(); 
        }
        var formattedDate = kendo.toString(tanggalSelected, "yyyy-MM-dd");

        if (kodeCabang && kodeKas && debetKredit) {
            $.ajax({
                type: "GET",
                url: "/VoucherKas/GetNextVoucherSementara",
                data: {
                    kodeCabang: kodeCabang,
                    kodeKas: kodeKas,
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
    // logika untuk generate voucher
   function generateNoVoucher() {
        var jenisVoucher = $("#JenisVoucher").val() || "";
        var debetKredit = $("#DebetKredit").data("kendoDropDownList")?.value() || "";
        var kodeCabangText = $("#KodeCabang").data("kendoComboBox")?.input.val() || "";
        var kodeCabang = kodeCabangText.split(" - ")[0].trim(); 
        var kodeKas = $("#KodeKas").val() || "";

        // 1. AMBIL TANGGAL DARI INPUTAN
        var kendoDatePicker = $("#TanggalVoucher").data("kendoDatePicker");
        var tanggalSelected = kendoDatePicker ? kendoDatePicker.value() : null;

        // Jika belum pilih tanggal, pakai hari ini sebagai default (atau bisa return jika mau wajib pilih)
        if (!tanggalSelected) {
            tanggalSelected = new Date(); 
        }

        // Format tanggal untuk dikirim ke Controller (yyyy-MM-dd)
        var formattedDateForAPI = kendo.toString(tanggalSelected, "yyyy-MM-dd");

        console.log("Kode Cabang:", kodeCabang, "Tanggal:", formattedDateForAPI);
        if (!kodeCabang) return;

       $.ajax({
            type: "GET",
            url: "/VoucherKas/GetNextVoucherNumber",
            data: { 
                tanggalVoucher: formattedDateForAPI,
                kodeCabang: kodeCabang
                // Tidak perlu kirim DebetKredit/Kas buat filter angka urut
            },
            success: function (response) {
                if (response && response.success) {
                    var noUrut = response.nextNumber; // Dapat "002"

                    // --- RAKIT STRING TAMPILAN ---
                    // Format Request: Cabang/Tipe+KodeKas/Bulan/Tahun/NoUrut
                    
                    // 1. Cabang (Ambil 2 digit belakang, misal "50")
                    var bagian1 = kodeCabang.slice(-2); 
                    
                    // 2. Tipe (K + D/K + KodeKas)
                    // Logic: Huruf pertama 'K', huruf kedua D/K, lalu KodeKas
                    var prefixTipe = "K"; 
                    if (debetKredit.toUpperCase() === "D") prefixTipe += "D";
                    else if (debetKredit.toUpperCase() === "K") prefixTipe += "K";
                    
                    // Gabung jadi "KD01" atau "KK01"
                    var bagian2 = prefixTipe + kodeKas;

                    // 3. Bulan/Tahun
                    var bulan = ('0' + (tanggalSelected.getMonth() + 1)).slice(-2);
                    var tahun = tanggalSelected.getFullYear().toString();
                    var bagian3 = bulan + "/" + tahun;

                    // 4. Final String
                    // Contoh: 50/KD01/02/2026/002
                    var noVoucherFinal = `${bagian1}/${bagian2}/${bagian3}/${noUrut}`;
                    
                    $("#NoVoucher").val(noVoucherFinal);
                }
            }
        });
    }


function btnCetakPembayaranKas_OnClick(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    var noVoucher = dataItem.NoVoucher;
    var noVoucherSementara = dataItem.NoVoucherSementara;
    

    // buka tab baru (tanpa auto-print)
    window.open(`/VoucherKas/Cetak?id=${dataItem.Id}`, "_blank");
}