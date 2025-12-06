var _isClosingProgrammatically = false;

function btnEntriPembayaran_OnClick(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    var noVoucher = dataItem.NoVoucher;
    
    // 1. Ambil referensi ke Kendo Window
    var window = $("#EntriPembayaranBankWindow").data("kendoWindow");
    window.one("refresh", function () {
        // 'refresh' event berjalan SETELAH konten Add.cshtml dimuat
        // dan SEMUA elemen (termasuk #VoucherDK) sudah ada.
        
        // 1. Pasang event listener untuk kalkulator kurs
        attachChangeEvents();
        
        // 2. Ambil grid SEKARANG (setelah ada) dan refresh
        var grid = $("#DetailPembayaranGrid").data("kendoGrid");
        if (grid) {
            // .read() akan memicu dataBound, yang akan memanggil updateGridFooter
            grid.dataSource.read(); 
        }
    });

   

    // Buka window (ini akan memicu 'refresh')
    openWindow(
        '#EntriPembayaranBankWindow', 
        `/EntriPembayaranBank/Add?noVoucher=${noVoucher}`, 
        'Entri Pembayaran'
    );
   
}
// Tambahkan juga fungsi search
function getSearchFilter() {
    return {
        searchKeyword: $("#SearchKeyword").val()
    };
}
$(document).ready(function () {
    $("#SearchKeyword").on("keyup", function() {
        $("#EntriPembayaranBankGrid").data("kendoGrid").dataSource.read();
    });

    // $("#PilihNotaSearchKeyword").on("keyup", function() {
    //     $("#PilihNotaGrid").data("kendoGrid").dataSource.read();
    // });

    $(document).on("keyup", "#PilihNotaSearchKeyword", function() {
        var grid = $("#PilihNotaGrid").data("kendoGrid");
        if (grid) {
            grid.dataSource.read();
        }
    });
});
// Ganti fungsi onSavePembayaran Anda dengan yang ini
function onSavePembayaran() {
    var form = $("#NewPaymentForm");
    var flagPembayaran = $("#FlagPembayaran").data("kendoDropDownList").value();

    var kodeAkunValue = null;
    var noNota4Value = null;

    if (flagPembayaran && flagPembayaran.toUpperCase() === "AKUN") {
        kodeAkunValue = $("#KodeAkun").data("kendoComboBox").value();
    } else if (flagPembayaran && flagPembayaran.toUpperCase() === "NOTA") {
        noNota4Value = $("#NoNota4").val();
    }

    var data = {
        No: $("#No").val() ? parseInt($("#No").val()) : 0,
        NoVoucher: form.find("#NoVoucher").val(),
        TotalBayar: $("#TotalBayar").data("kendoNumericTextBox").value(),
        FlagPembayaran: flagPembayaran,
        KodeMataUang: $("#KodeMataUang").data("kendoComboBox").value(),
        DebetKredit: $("#DebetKredit").data("kendoDropDownList").value(),
        TotalDlmRupiah: $("#TotalDlmRupiah").data("kendoNumericTextBox").value(), // Ambil dari widget
        KodeAkun: kodeAkunValue,
        NoNota4: noNota4Value,
        Kurs: $("#Kurs").val(),
    };
    console.log(data)

    // LANGSUNG SIMPAN (AJAX KEDUA ANDA) TANPA VALIDASI
    $.ajax({
        type: "POST",
        url: "/EntriPembayaranBank/Save", // Ini memanggil Create/Update PembayaranBankCommand
        contentType: "application/json",
        data: JSON.stringify(data),
        success: function (response) {
            if (response.success) {
                showMessage('Success', data.No ? 'Data berhasil diperbarui.' : 'Data berhasil disimpan.');
                $("#DetailPembayaranGrid").data("kendoGrid").dataSource.read(); // Ini akan memicu updateGridFooter
                clearPaymentForm();
            } else {
                showMessage('Error', 'Gagal menyimpan data.');
            }
        },
        error: function() {
            showMessage('Error', 'Gagal menyimpan data.');
        }
    });
}

function onSavePembayaranLihat() {
    var form = $("#NewPaymentForm");
    var flagPembayaran = $("#FlagPembayaran").data("kendoDropDownList").value();

    var kodeAkunValue = null;
    var noNota4Value = null;

    if (flagPembayaran && flagPembayaran.toUpperCase() === "AKUN") {
        kodeAkunValue = $("#KodeAkun").data("kendoComboBox").value();
    } else if (flagPembayaran && flagPembayaran.toUpperCase() === "NOTA") {
        noNota4Value = $("#NoNota4").val();
    }

    var data = {
        No: $("#No").val() ? parseInt($("#No").val()) : 0,
        NoVoucher: form.find("#NoVoucher").val(),
        TotalBayar: $("#TotalBayar").data("kendoNumericTextBox").value(),
        FlagPembayaran: flagPembayaran,
        KodeMataUang: $("#KodeMataUang").data("kendoComboBox").value(),
        DebetKredit: $("#DebetKredit").data("kendoDropDownList").value(),
        TotalDlmRupiah: $("#TotalDlmRupiah").data("kendoNumericTextBox").value(), // Ambil dari widget
        KodeAkun: kodeAkunValue,
        NoNota4: noNota4Value,
        Kurs: $("#Kurs").val(),
    };
    console.log(data)

    // LANGSUNG SIMPAN (AJAX KEDUA ANDA) TANPA VALIDASI
    $.ajax({
        type: "POST",
        url: "/EntriPembayaranBank/SaveLihat", // Ini memanggil Create/Update PembayaranBankCommand
        contentType: "application/json",
        data: JSON.stringify(data),
        success: function (response) {
            if (response.success) {
                showMessage('Success', data.No ? 'Data berhasil diperbarui.' : 'Data berhasil disimpan.');
                $("#DetailPembayaranLihatGrid").data("kendoGrid").dataSource.read(); // Ini akan memicu updateGridFooter
                clearPaymentForm();
            } else {
                showMessage('Error', 'Gagal menyimpan data.');
            }
        },
        error: function() {
            showMessage('Error', 'Gagal menyimpan data.');
        }
    });
}

// edit pembayaran
function onEditPembayaran(e) {
    e.preventDefault();
    $("#btn-cancel-edit").show();
    $("#btn-save-pembayaran").show();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
     if (!dataItem) {
        showMessage('Error', 'Data pembayaran tidak ditemukan.');
        return;
    }
    $("#FlagPembayaran").data("kendoDropDownList").readonly(true);    
    $("#KodeMataUang").data("kendoComboBox").value(dataItem.KodeMataUang);
    // Isi form input di atas dengan data dari baris yang dipilih
    $("#TotalBayar").data("kendoNumericTextBox").value(dataItem.TotalBayar);
    $("#TotalDlmRupiah").data("kendoNumericTextBox").value(dataItem.TotalDlmRupiah);
    $("#DebetKredit").data("kendoDropDownList").value(dataItem.DebetKredit);
    $("#NoVoucher").val(dataItem.NoVoucher);
    $("#buttonselectnota").attr("disabled", true);
    $(".detail-payment-field").show();
    
    $("#FlagPembayaran").data("kendoDropDownList").value(dataItem.FlagPembayaran);
    if (dataItem.FlagPembayaran && dataItem.FlagPembayaran.toUpperCase() === "AKUN") {
        $("#akunField").show();
        $("#notaField").hide();
        $("#KodeAkun").data("kendoComboBox").value(dataItem.KodeAkun);
        
    } else if (dataItem.FlagPembayaran && dataItem.FlagPembayaran.toUpperCase() === "NOTA") {
        $("#notaField").show();
        $("#akunField").hide();
         $("#KodeAkun").data("kendoComboBox").value(dataItem.KodeAkun);
        $("#NoNota4").data("kendoTextBox").value(dataItem.NoNota4);
    }
    $("#KodeMataUang").data("kendoComboBox").value(dataItem.KodeMataUang);
   
    // ⬇️ --- TAMBAHKAN KODE INI --- ⬇️
    // Simpan nilai ASLI (TotalBayar) ke form untuk referensi validasi
    $("#NewPaymentForm").data("original-bayar", dataItem.TotalBayar || 0);
    $("#NewPaymentForm").data("original-dk", dataItem.DebetKredit);
    // ⬆️ --- BATAS TAMBAHAN --- ⬆️
    // Simpan juga nomor urut (No) di elemen tersembunyi agar bisa di-update
    // Anda perlu menambahkan <input type="hidden" id="currentEditNo" /> di form
     if ($("#No").length === 0) {
        // kalau belum ada hidden input No, buat baru
        $("<input>").attr({
            type: "hidden",
            id: "No",
            name: "No",
            value: dataItem.No
        }).appendTo("#NewPaymentForm");
    } else {
        $("#No").val(dataItem.No);
    }
      
    attachChangeEvents();
    // Nonaktifkan juga tombol select nota
   
}

// Fungsi ini berjalan saat tombol 'Delete' di grid detail diklik
function onDeletePembayaran(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    
    showConfirmation("Konfirmasi Hapus", "Anda yakin ingin menghapus data pembayaran ini?", function() {
        $.ajax({
            type: "POST", // Sebaiknya gunakan POST atau DELETE untuk operasi hapus
            url: "/EntriPembayaranBank/DeleteDetail",
            contentType: "application/json",
            data: JSON.stringify({ NoVoucher: dataItem.NoVoucher, No: dataItem.No }),
            success: function(response) {
                if (response.success) {
                    showMessage('Success', 'Data berhasil dihapus.');
                    clearPaymentForm();
                    $("#DetailPembayaranGrid").data("kendoGrid").dataSource.read(); // Refresh grid
                } else {
                    showMessage('Error', 'Gagal menghapus data.');
                }
            }
        });
    });
}

function openPilihNotaWindow() {
    var noVoucher = $("#NoVoucher").val();
    if (!noVoucher) {
        showMessage('Error', 'Nomor Voucher tidak ditemukan di form utama.');
        return;
    }
    var window = $("#PilihNotaWindow").data("kendoWindow");
    // Muat konten dari action PilihNota
    window.refresh({ 
        url: `/EntriPembayaranBank/PilihNota?noVoucher=${noVoucher}` 
    });
    window.center().open();
}

function onNotaProduksiSelect(e) {
    // Ambil data dari baris yang dipilih
    var selectedRow = this.dataItem(this.select());

    if (selectedRow) {
        // "Replace" nilai di form utama dengan data dari pop-up
        $("#NoNota4").val(selectedRow.no_nd);
        // Anda juga bisa mengisi field lain jika perlu, contoh:
        // $("#TotalBayar").data("kendoNumericTextBox").value(selectedRow.Premi);
        
        // Tutup window pop-up setelah dipilih
        $("#PilihNotaWindow").data("kendoWindow").close();
    }
}

function clearPaymentForm() {
     $("#KodeAkun").data("kendoComboBox").value(null);
    $("#buttonselectnota").attr("disabled", false);
    $("#TotalBayar").data("kendoNumericTextBox").value(null);
    $("#FlagPembayaran").data("kendoDropDownList").value("");
    $("#NoNota4").data("kendoTextBox").value("");
        $("#DebetKredit").data("kendoDropDownList").value(null);
    $("#KodeMataUang").data("kendoComboBox").value("");
        if ($("#No").length > 0) {
        $("#No").val(0);
    }

    // ⬇️ --- TAMBAHKAN KODE INI --- ⬇️
    // Bersihkan data nilai asli saat form di-clear
     $("#TotalDlmRupiah").data("kendoNumericTextBox").value(null);
    $("#NewPaymentForm").data("original-bayar", 0);
    $("#NewPaymentForm").data("original-dk", "");
    // ⬆️ --- BATAS TAMBAHAN --- ⬆️

  
    // ---> TAMBAHKAN BARIS INI <---
    $("#btn-cancel-edit").hide(); // Sembunyikan kembali tombol Cancel

    var ddl = $("#FlagPembayaran").data("kendoDropDownList");
    ddl.readonly(false);
    ddl.trigger("change");
}

// fungsi total pembayaran
function updateGridFooter() {
    var grid = $("#DetailPembayaranGrid").data("kendoGrid");
    if (!grid) return;
     var voucherDK = $("#VoucherDK").val();
    // Ambil noVoucher dari data grid
    var dataForGrid = getNoVoucherForDetailGrid(); 
    
    if (dataForGrid && dataForGrid.noVoucher) {
        var noVoucher = dataForGrid.noVoucher;
        
        // Panggil endpoint untuk mendapatkan total terbaru dari tabel TEMP
        $.ajax({
            type: "GET",
            url: `/EntriPembayaranBank/GetTotalPembayaran?noVoucher=${noVoucher}&voucherDK=${voucherDK}`,
            success: function (response) {
                var totalPembayaran = response.totalPembayaran || 0;
                var $pembayaranTotalSpan = $("#pembayaranTotal");
                var $sisaPembayaranSpan = $("#sisapembayaranTotal");

                // Ambil total voucher asli dari header
                var $footer = $pembayaranTotalSpan.closest(".window-footer");
                var totalVoucherAsli = parseFloat($footer.data("total-voucher-original")) || 0;

                var sisaPembayaran = totalVoucherAsli - totalPembayaran;

                // Update teks total pembayaran
                $pembayaranTotalSpan.text(kendo.toString(totalPembayaran, "n0"));
                
                $sisaPembayaranSpan.text(kendo.toString(sisaPembayaran, "n0"));

                // Ambil referensi ke tombol Final Pembayaran
                var $btnFinal = $("#btn-save-pembayaran-final"); // <-- Beri ID ini pada tombol Anda

                // --- INI LOGIKA BARUNYA ---
                // Kita pakai toleransi 1 untuk mengatasi masalah pembulatan desimal
                if (Math.abs(totalPembayaran - totalVoucherAsli) < 1) {
                    // 1. JIKA BALANCE
                    $pembayaranTotalSpan.css("color", "green"); // Warna hijau
                    $btnFinal.prop("disabled", false); // AKTIFKAN tombol
                    $btnFinal.removeClass("k-disabled");
                } else {
                    // 2. JIKA TIDAK BALANCE
                    $pembayaranTotalSpan.css("color", "red"); // Warna merah
                    $btnFinal.prop("disabled", true); // NONAKTIFKAN tombol
                    $btnFinal.addClass("k-disabled");
                }
                if (Math.abs(sisaPembayaran) < 1) { // Ganti logika ke sisaPembayaran
                    $sisaPembayaranSpan.css("color", "green"); // <-- Tambahkan ini
                   
                } else {
                    $sisaPembayaranSpan.css("color", "red"); // <-- Tambahkan ini
                }
            }
        });
    }
}

function btnCetakPembayaranBank_OnClick(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    var noVoucher = dataItem.NoVoucher;

    // buka tab baru (tanpa auto-print)
    window.open(`Cetak?noVoucher=${noVoucher}`, "_blank");
}

function attachChangeEvents() {
        var mataUangCombo = $("#KodeMataUang").data("kendoComboBox");
        var totalVoucherInput = $("#TotalBayar").data("kendoNumericTextBox");
        var tanggalVoucher = $("#TangVoc").val();
        console.log(tanggalVoucher)
        // Pemicu untuk kalkulator kurs (tidak berubah)
        if (mataUangCombo) {
            mataUangCombo.bind("change", hitungTotalRupiah);
        }
        if (totalVoucherInput) {
            totalVoucherInput.bind("change", hitungTotalRupiah);
        }
        //if (tanggalVoucher) {
            // TanggalVoucher sekarang HANYA memicu kalkulator kurs
         //   tanggalVoucher.bind("change", hitungTotalRupiah);
      //  }
    }

    function hitungTotalRupiah() {
    
        var kodeMtu = $("#KodeMataUang").data("kendoComboBox").value();
        var tanggal = $("#TangVoc").val();
        var totalVoucher = $("#TotalBayar").val();
        console.log("total voucher: ",totalVoucher)
         var kursInput = $("#Kurs");
        if (kodeMtu && tanggal && totalVoucher) {
            
             var parts = tanggal.split(" ")[0].split("/"); 
           
            var day = parts[0];
            var month = parts[1];
            var year = parts[2];

            var formattedDate = `${year}-${month}-${day}`;
            console.log("Format: ", formattedDate);
            var url = `/EntriPembayaranKas/GetKurs?kodeMataUang=${kodeMtu}&tanggalVoucher=${formattedDate}`;
        
            $.ajax({
                type: "GET",
                url: url,
                success: function (response) {
                    console.log("4. AJAX berhasil! Respons dari server:", response); // <-- LOG 5
                    if (response && response.nilai_kurs) {
                        var kurs = response.nilai_kurs;
                        console.log("ini kurs", kurs)
                        kursInput.val(kurs);
                        var totalRupiah = totalVoucher * kurs;
                        console.log("naha Nan: ",totalRupiah)
                        $("#TotalDlmRupiah").data("kendoNumericTextBox").value(totalRupiah);
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
// Ganti fungsi SimpanNota Anda dengan yang ini
function SimpanNota() {
    var grid = $("#PilihNotaGrid").data("kendoGrid");
    var noVoucher = $("#NoVoucher").val(); // Pastikan ID ini benar di PilihNota.cshtml
    console.log(noVoucher)
    var selectedItems = grid.select().map(function() {
        var row = $(this);
        var dataItem = grid.dataItem(row);
        var rpInputElement = row.find('.total-rp-input');
        var totalOrg = parseFloat(row.find('.total-org-input').val()) || 0;
        var totalRp = parseFloat(row.find('.total-rp-input').val()) || 0;
        var kurs = parseFloat(rpInputElement.data('kurs')) || 1;
      
        
        console.log("--- DEBUGGING BARIS: " + dataItem.no_nd + " ---");
        
        // Tes 1: Apakah elemen input ditemukan?
        var inputElement = row.find('input.coa-combobox[data-role="combobox"]');
        var kendoCombo = inputElement.data("kendoComboBox");
        var kodeAkun = kendoCombo ? kendoCombo.value() : null; 

        return {
            NoNota: dataItem.no_nd,
            TotalBayarOrg: Math.floor(totalOrg),
            TotalBayarRp: totalRp,
            DebetKredit: row.find('.dk-input').val(),
            KodeMataUang: dataItem.kd_mtu,
            Kurs: Math.floor(kurs),
            KodeAkun: kodeAkun
        };
    }).get();
        console.log(selectedItems);
    if (selectedItems.length === 0) {
        showMessage("Silakan pilih minimal satu nota untuk disimpan.");
        return;
    }

    var payload = {
        NoVoucher: noVoucher,
        Data: selectedItems
    };

    // LANGSUNG SIMPAN (AJAX KEDUA ANDA) TANPA VALIDASI
    $.ajax({
        url: '/EntriPembayaranBank/SimpanNota', // Ini memanggil CreatePembayaranBankNotaCommand
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify(payload),
        success: function (response) {
            if (response.Status === "OK") {
                 showMessage('Success','Data berhasil disimpan.');
                var pilihNotaWindow = $("#PilihNotaWindow").data("kendoWindow");
                if (pilihNotaWindow) {
                    pilihNotaWindow.close();
                }
                clearPaymentForm()
                $("#DetailPembayaranGrid").data("kendoGrid").dataSource.read(); // Ini akan memicu updateGridFooter
            } else {
                alert("Error: " + (response.Message || "Gagal menyimpan data."));
            }
        },
        error: function () {
            alert("Terjadi kesalahan saat menyimpan data.");
        }
    });
}


function onSearchClick() {
    // Cukup perintahkan grid untuk membaca ulang datanya
    $("#PilihNotaGrid").data("kendoGrid").dataSource.read();
}

function getNotaProduksiSearchFilter() {
    return {
        searchKeyword: $("#PilihNotaSearchKeyword").val(),
        jenisAsset: $("#JenisAsset").data("kendoComboBox").value()
    };
}

$(document).on('change keyup', '#PilihNotaGrid .total-org-input', function () {
        var input = $(this);
        var row = input.closest('tr');
        var grid = $("#PilihNotaGrid").data("kendoGrid");
        var dataItem = grid.dataItem(row);

        var totalOrg = parseFloat(input.val()) || 0;
        var kodeMtu = dataItem.kd_mtu;

        // Ambil tanggal dari form UTAMA (di belakang pop-up)
        // Pastikan input Tanggal Voucher di Add.cshtml memiliki ID 'TangVoc'
        var tanggalVoucher = $("#TangVoc").val();
        var tanggalSaja = tanggalVoucher.split(' ')[0];
        // Jika mata uang adalah Rupiah (IDR, kode 001), kurs-nya 1
        if (kodeMtu.trim() === '001') {
            row.find('.total-rp-input').val(totalOrg.toFixed(2));
            return; // Hentikan fungsi
        }

        // Hanya panggil AJAX jika semua data lengkap
        if (kodeMtu && tanggalVoucher && totalOrg > 0) {
            // Format tanggal agar sesuai dengan yang diharapkan Controller (yyyy-MM-dd)
            var dateParts = tanggalSaja.split('/'); // Hasil: ["20", "09", "2025"]
            var formattedDate = dateParts[2] + '-' + dateParts[1] + '-' + dateParts[0]; // Hasil: "2025-09-20"

            var url = `/EntriPembayaranBank/GetKurs?kodeMataUang=${kodeMtu}&tanggalVoucher=${formattedDate}`;
        
            $.ajax({
                type: "GET",
                url: url,
                success: function (response) {
                    if (response && response.nilai_kurs) {
                        var kurs = response.nilai_kurs;
                        var totalRupiah = totalOrg * kurs;
                        // Isi nilai di baris yang sama
                        row.find('.total-rp-input').val(totalRupiah.toFixed(2));
                        row.find('.total-rp-input').data('kurs', kurs);
                    }
                }
            });
        } else {
             // Jika ada data yang kosong, set Total Rupiah menjadi 0
             row.find('.total-rp-input').val('0.00');
             row.find('.total-rp-input').data('kurs', 0);
        }
    });


function onEntriPembayaranWindowClose(e) {
    // 'this' adalah Kendo Window
    var window = this; 

    // 1. Cek flag. Jika kita menutupnya via kode, izinkan.
    if (_isClosingProgrammatically) {
        _isClosingProgrammatically = false; // Reset flag
        return; // Izinkan window tertutup
    }

    // 2. Ambil nilai pembanding
    // $(window.element) merujuk ke div window-nya
    var totalVoucherAsli = parseFloat($(window.element).find(".window-footer").data("total-voucher-original")) || 0;
    var noVoucher = $(window.element).find("#NoVoucher").val();
     var voucherDK = $("#VoucherDK").val();
    if (!noVoucher) {
        // Jika tidak ada NoVoucher, tidak bisa validasi, izinkan tutup.
        return; 
    }

    // 3. PENTING: Batalkan penutupan window untuk sementara
    e.preventDefault();

    // 4. Lakukan AJAX call untuk mendapatkan total pembayaran TERBARU
    $.ajax({
        type: "GET",
        url: `/EntriPembayaranBank/GetTotalPembayaran?noVoucher=${noVoucher}&voucherDK=${voucherDK}`,
        success: function (response) {
            var totalPembayaranAsli = response.totalPembayaran || 0;

            // 5. INI LOGIKA UTAMANYA: "Kalo kurang, gak bisa close"
            // Kita pakai toleransi 1, sama seperti logika save
            if (totalPembayaranAsli < (totalVoucherAsli - 1)) { 
                // Totalnya KURANG! Tampilkan pesan error dan JANGAN tutup window
                showMessage('Error', 
                    'Total pembayaran (Rp ' + kendo.toString(totalPembayaranAsli, 'n0') + 
                    ') masih kurang dari total voucher (Rp ' + kendo.toString(totalVoucherAsli, 'n0') + ').' +
                    ' Data belum balance.');
            } else {
                // Totalnya PAS (atau lebih, yg mana akan gagal disimpan). Izinkan tutup.
                _isClosingProgrammatically = true; // 1. Set flag
                window.close();                    // 2. Tutup window via kode
            }
        },
        error: function() {
            // Jika AJAX gagal, tampilkan error tapi jangan tutup window
            showMessage('Error', 'Gagal memvalidasi total. Tidak bisa menutup window.');
        }
    });
}


function onSavePembayaranBankFinal() {
    var form = $("#NewPaymentForm");
    var data = {
        NoVoucher: form.find("#NoVoucher").val(),
    };

    if (!data.NoVoucher) {
        showMessage("Warning", "Nomor voucher tidak boleh kosong.");
        return;
    }

    $.ajax({
        type: "POST",
        url: "/EntriPembayaranBank/SaveFinal",
        contentType: "application/json",
        data: JSON.stringify(data),
        success: function (response) {
            if (response.success) {

                // Pesan sukses
                showMessage("Success", "Pembayaran berhasil difinalkan.");

                // Tutup modal utama
                $("#EntriPembayaranBankWindow")
                    .data("kendoWindow")
                    .close();

                // Refresh grid "Dalam Proses"
                var grid = $("#BelumFinalGrid").data("kendoGrid");
                if (grid) grid.dataSource.read();

                // Refresh grid "Sudah Final"
                var grid2 = $("#SudahFinalGrid").data("kendoGrid");
                if (grid2) grid2.dataSource.read();

            } else {
                showMessage("Error", response.message || "Gagal memproses data.");
            }
        },
        error: function () {
            showMessage("Error", "Terjadi kesalahan saat menyimpan data.");
        }
    });
}


function btnLihatPembayaranBank_OnClick(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    var noVoucher = dataItem.NoVoucher;

    // Tutup SEMUA window Kendo yang terbuka
    $(".k-window-content").each(function () {
        var wnd = $(this).data("kendoWindow");
        if (wnd) wnd.close();
    });

    // Ambil window untuk Lihat
    var windowView = $("#EntriPembayaranBankLihatWindow").data("kendoWindow");

     windowView.one("refresh", function () {
        var grid = $("#DetailPembayaranLihatGrid").data("kendoGrid");
        if (grid) grid.dataSource.read();
        attachChangeFinalEvents()
    });

    openWindow(
         '#EntriPembayaranBankLihatWindow', 
        `/EntriPembayaranBank/Lihat?noVoucher=${noVoucher}`, 
        'Lihat Pembayaran Bank'
    )

}

  function updateGridFooterLihat(e) {
        // Ambil semua data yang tampil di grid
        var grid = $("#DetailPembayaranLihatGrid").data("kendoGrid");
         var dataForGrid = getNoVoucherFinalForDetailGrid(); 
        var voucherDK = $("#VoucherDKFinal").val();

        if (dataForGrid && dataForGrid.noVoucher) {
            var noVoucher = dataForGrid.noVoucher;
            
            // Panggil endpoint untuk mendapatkan total terbaru dari tabel TEMP
            $.ajax({
                type: "GET",
                url: `/EntriPembayaranBank/GetTotalPembayaranFinal?noVoucher=${noVoucher}&voucherDK=${voucherDK}`,
                success: function (response) {
                    var totalPembayaran = response.totalPembayaran || 0;
                    var $pembayaranTotalSpan = $("#pembayaranTotalFinal");
                    var $sisaPembayaranSpan = $("#sisapembayaranTotalFinal");
                    // Ambil total voucher asli dari header
                    var $footer = $pembayaranTotalSpan.closest(".window-footer");
                    var totalVoucherAsli = parseFloat($footer.data("total-voucher-original-final")) || 0;
                    
                    var sisaPembayaran = totalVoucherAsli - totalPembayaran;

                    // Update teks total pembayaran
                    $pembayaranTotalSpan.text(kendo.toString(totalPembayaran, "n0"));
                    $sisaPembayaranSpan.text(kendo.toString(sisaPembayaran, "n0"));

                    // Ambil referensi ke tombol Final Pembayaran
                    var $btnFinal = $("#btn-save-pembayaran-final"); // <-- Beri ID ini pada tombol Anda

                    // --- INI LOGIKA BARUNYA ---
                    // Kita pakai toleransi 1 untuk mengatasi masalah pembulatan desimal
                    if (Math.abs(totalPembayaran - totalVoucherAsli) < 1) {
                        // 1. JIKA BALANCE
                        $pembayaranTotalSpan.css("color", "green"); // Warna hijau
                        $btnFinal.prop("disabled", false); // AKTIFKAN tombol
                        $btnFinal.removeClass("k-disabled");
                    } else {
                        // 2. JIKA TIDAK BALANCE
                        $pembayaranTotalSpan.css("color", "red"); // Warna merah
                        $btnFinal.prop("disabled", true); // NONAKTIFKAN tombol
                        $btnFinal.addClass("k-disabled");
                    }
                    if (Math.abs(sisaPembayaran) < 1) { // Ganti logika ke sisaPembayaran
                        $sisaPembayaranSpan.css("color", "green"); // <-- Tambahkan ini
                    
                    } else {
                        $sisaPembayaranSpan.css("color", "red"); // <-- Tambahkan ini
                    }
                }
            });
        }
    }

    function attachChangeFinalEvents() {
        var mataUangCombo = $("#KodeMataUang_Lihat").data("kendoComboBox");
        var totalVoucherInput = $("#TotalBayar_Lihat").data("kendoNumericTextBox");
        var tanggalVoucher = $("#TangVocFinal").val();
        console.log(tanggalVoucher)
        // Pemicu untuk kalkulator kurs (tidak berubah)
        if (mataUangCombo) {
            mataUangCombo.bind("change", hitungTotalRupiahFinal);
        }
        if (totalVoucherInput) {
            totalVoucherInput.bind("change", hitungTotalRupiahFinal);
        }
        //if (tanggalVoucher) {
            // TanggalVoucher sekarang HANYA memicu kalkulator kurs
         //   tanggalVoucher.bind("change", hitungTotalRupiah);
      //  }
    }

    function hitungTotalRupiahFinal() {
    
        var kodeMtu = $("#KodeMataUang_Lihat").data("kendoComboBox").value();
        var tanggal = $("#TangVocFinal").val();
        var totalVoucher = $("#TotalBayar_Lihat").val();
        console.log("total voucher: ",totalVoucher)
        var kursInput = $("#Kurs_Lihat");

        if (kodeMtu && tanggal && totalVoucher) {
            
             var parts = tanggal.split(" ")[0].split("/"); 
           
            var day = parts[0];
            var month = parts[1];
            var year = parts[2];

            var formattedDate = `${year}-${month}-${day}`;
            console.log("Format: ", formattedDate);
            var url = `/EntriPembayaranBank/GetKurs?kodeMataUang=${kodeMtu}&tanggalVoucher=${formattedDate}`;
        
            $.ajax({
                type: "GET",
                url: url,
                success: function (response) {
                    console.log("4. AJAX berhasil! Respons dari server:", response); // <-- LOG 5
                    if (response && response.nilai_kurs) {
                        var kurs = response.nilai_kurs;
                        kursInput.val(kurs);
                        console.log("ini kurs", kurs)
                        var totalRupiah = totalVoucher * kurs;
                        console.log("naha Nan: ",totalRupiah)
                        $("#TotalDlmRupiah_Lihat").data("kendoNumericTextBox").value(totalRupiah);
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

    function onEditFinalPembayaran(e) {
        e.preventDefault();
        var dataItem = this.dataItem($(e.currentTarget).closest("tr"));

        if (!dataItem) {
            showMessage('Error', 'Data pembayaran tidak ditemukan.');
            return;
        }

        // 1. Dapatkan referensi form dan tombol cancel
        var form = $("#FinalPaymentBankForm");
        // Cari tombol cancel yang ada di div SETELAH form
        var cancelButton = form.next("div").find("#btn-cancel-edit"); 

        // 2. TAMPILKAN FORM FINAL (INI KUNCI UTAMA MENGHINDARI ERROR _move)
        form.show(); // <-- PENTING! Tampilkan form SEBELUM set value

        // 3. Isi form dengan data yang dipilih
        // (Asumsi window "Add" sudah ditutup, ID Kendo unik di DOM)
        
        form.find("#NoVoucher").val(dataItem.NoVoucher); 
        
        // Set nilai Kendo widgets SETELAH form terlihat
        $("#FlagPembayaran_Lihat").data("kendoDropDownList").value(dataItem.FlagPembayaran);
        $("#KodeAkun_Lihat").data("kendoComboBox").value(dataItem.KodeAkun);
        $("#NoNota4_Lihat").data("kendoTextBox").value(dataItem.NoNota4);
        $("#KodeMataUang_Lihat").data("kendoComboBox").value(dataItem.KodeMataUang);
        $("#TotalBayar_Lihat").data("kendoNumericTextBox").value(dataItem.TotalBayar);
        $("#TotalDlmRupiah_Lihat").data("kendoNumericTextBox").value(dataItem.TotalDlmRupiah);
        $("#DebetKredit_Lihat").data("kendoDropDownList").value(dataItem.DebetKredit);
        form.find("#Kurs").val(dataItem.Kurs); // Isi hidden field Kurs

        // 4. Simpan ID detail (No) supaya 'onUpdateFinalPembayaranKas' tahu ini mode edit
        form.find("#No").val(dataItem.No);

        // 5. Tampilkan tombol "Cancel"
        if (cancelButton.length) {
            cancelButton.show();
        }

        // 6. Trigger change di FlagPembayaran untuk memastikan UI show/hide konsisten
        $("#FlagPembayaran_Lihat").data("kendoDropDownList").trigger("change");
}

function onUpdateFinalPembayaranBank() {
    var form = $("#FinalPaymentBankForm"); // <-- ID form dari Lihat.cshtml

    // 1. Kumpulkan data dari form
    var data = {
        NoVoucher: $("#NoVoucherFinal").val(),
        No: form.find("#No").val() || 0, // <-- Ambil 'No' (ID detail) dari hidden field
        KodeAkun: $("#KodeAkun_Lihat").val(),
        TotalBayar: $("#TotalBayar_Lihat").data("kendoNumericTextBox").value(),
        FlagPembayaran: $("#FlagPembayaran_Lihat").val(),
        NoNota4: $("#NoNota4_Lihat").val().trim(),
        DebetKredit: $("#DebetKredit_Lihat").val(),
        KodeMataUang: $("#KodeMataUang_Lihat").val(),
        TotalDlmRupiah: $("#TotalDlmRupiah_Lihat").data("kendoNumericTextBox").value(), // Ambil data TotalDlmRupiah
        Kurs: $("#Kurs_Lihat").val(), // Ambil data Kurs
    };

    // 2. Validasi: Pastikan ini adalah mode 'Update' (No > 0)
    //    Ini sesuai dengan logic di controller Anda yang hanya memproses jika No > 0
    if (data.No <= 0) {
        showMessage('Warning', 'Tidak ada data pembayaran yang dipilih untuk diupdate. Silakan klik "Edit" pada grid terlebih dahulu.');
        return;
    }

    // 3. Kirim data ke action 'UpdateFinal'
    $.ajax({
        type: "POST",
        url: "/EntriPembayaranBank/SaveLihat", // <-- Targetkan endpoint FINAL
        contentType: "application/json",
        data: JSON.stringify(data),
        success: function (response) {
            if (response.success) {
                showMessage('Success', 'Data final berhasil diperbarui.');

                // 4. Refresh grid FINAL dan bersihkan form
                $("#DetailPembayaranLihatGrid").data("kendoGrid").dataSource.read(); // <-- Refresh grid 'Lihat'
                clearFinalPaymentForm(); // <-- Panggil helper untuk reset form
            } else {
                showMessage('Error', 'Gagal memperbarui data final.');
            }
        },
        error: function() {
            showMessage('Error', 'Terjadi kesalahan sistem saat memperbarui data.');
        }
    });
}

function clearFinalPaymentForm() {
    var form = $("#FinalPaymentBankForm");
    
    // 1. Sembunyikan form
    form.hide();

    // 2. Reset semua Kendo widget di dalam form
    // (Gunakan try-catch untuk jaga-jaga jika widget belum di-init)
    try {
        $("#FlagPembayaran_Lihat").data("kendoDropDownList").value("");
        $("#NoNota4_Lihat").data("kendoTextBox").value("");
        $("#KodeAkun_Lihat").data("kendoComboBox").value(null);
        $("#KodeMataUang_Lihat").data("kendoComboBox").value("");
        $("#TotalBayar_Lihat").data("kendoNumericTextBox").value(null);
        $("#TotalDlmRupiah_Lihat").data("kendoNumericTextBox").value(null);
        $("#DebetKredit_Lihat").data("kendoDropDownList").value(null);
    } catch (ex) {
        console.warn("Gagal reset Kendo widgets di clearFinalPaymentForm:", ex);
    }
    
    // 3. Reset hidden fields
    form.find("#No").val(0);
    form.find("#Kurs").val(null);

    // 4. Sembunyikan tombol "Cancel"
    form.next("div").find("#btn-cancel-edit").hide();
    
    // 5. Pastikan field nota/akun ter-reset
    $("#akunField_Lihat").hide();
    $("#notaField_Lihat").hide();
}

// Fungsi untuk menghapus data
function btnProsesPembayaranBank_OnClick(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));

    // Tampilkan dialog konfirmasi
    showConfirmation('Konfirmasi Proses', `Apakah Anda yakin ingin Mengulang Proses Pembayaran dengan No. Voucher ${dataItem.NoVoucher}?`,
        function () {
            showProgressOnGrid('#SudahFinalGrid');

            // Kirim request hapus ke Controller
            ajaxGet(`/EntriPembayaranBank/ProsesUlang?id=${dataItem.NoVoucher.trim()}`,
                function (response) {
                    if (response.success) {
                        showMessage('Success', 'Data berhasil Di Proses Ulang.');
                        setTimeout(function () {
                            location.reload(true); // reload dari server
                        }, 500);
                    } else {
                        showMessage('Error', 'Gagal Memproses data.');
                    }
                    
                }
            );
        }
    );
}

    

