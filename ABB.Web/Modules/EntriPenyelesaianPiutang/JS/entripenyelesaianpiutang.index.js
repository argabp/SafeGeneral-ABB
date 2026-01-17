function btnEntriPenyelesaian_OnClick(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    
    // Ambil composite key dari dataItem
    var kodeCabang = dataItem.KodeCabang;
    var nomorBukti = dataItem.NomorBukti;

    var window = $("#EntriPenyelesaianPiutangWindow").data("kendoWindow");

    window.one("refresh", function() {
        attachChangeEvents();
        // Setelah window refresh, panggil clearForm untuk memastikan form dalam kondisi Add
        clearPaymentForm(); 
    });
    
    openWindow(
        '#EntriPenyelesaianPiutangWindow', 
        `/EntriPenyelesaianPiutang/Add?kodeCabang=${kodeCabang}&nomorBukti=${nomorBukti}`, 
        'Entri Penyelesaian Piutang'
    );
}

function btnLihatPembayaranPiutang_OnClick(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    
    // Ambil composite key dari dataItem
    var kodeCabang = dataItem.KodeCabang;
    var nomorBukti = dataItem.NomorBukti;

    var window = $("#EntriPenyelesaianPiutangLihatWindow").data("kendoWindow");

    window.one("refresh", function() {
        attachChangeEvents();
        // Setelah window refresh, panggil clearForm untuk memastikan form dalam kondisi Add
        clearFinalPaymentForm(); 
    });
    
    openWindow(
        '#EntriPenyelesaianPiutangLihatWindow', 
        `/EntriPenyelesaianPiutang/Lihat?kodeCabang=${kodeCabang}&nomorBukti=${nomorBukti}`, 
        'Lihat Penyelesaian Piutang'
    );
}
// Ganti fungsi lama Anda dengan yang ini
// GANTI FUNGSI LAMA ANDA DENGAN YANG INI
function onSaveHeaderAndProceed() {
    // 1. Kumpulkan data header
    var tanggal = $("#PenyelesaianHeader_Tanggal").data("kendoDatePicker").value();
    var mataUang = $("#PenyelesaianHeader_MataUang").data("kendoComboBox").value();
    var totalOrg = $("#PenyelesaianHeader_TotalOrg").data("kendoNumericTextBox").value();
    var kodeAkun = $("#PenyelesaianHeader_KodeAkun").data("kendoComboBox").value();

    // 2. Validasi Header Sederhana
    // (Anda bisa tambahkan field lain jika wajib)
    if (!tanggal || !mataUang || (totalOrg === null || totalOrg <= 0) || !kodeAkun) {
        showMessage('warning', 'Harap lengkapi data Header (Tanggal, Mata Uang, Total, Kode Akun) sebelum menyimpan.');
        // Mengembalikan promise yang sudah gagal agar .done() tidak berjalan
        return $.Deferred().reject({ error: "Header form is incomplete." }).promise();
    }

    var headerData = {
        KodeCabang: $("#PenyelesaianHeader_KodeCabang").data("kendoComboBox").value().trim(),
        JenisPenyelesaian: $("#PenyelesaianHeader_JenisPenyelesaian").val(),
        NomorBukti: $("#PenyelesaianHeader_NomorBukti").val(),
        KodeVoucherAcc: $("#PenyelesaianHeader_KodeVoucherAcc").val(),
        Tanggal: tanggal,
        MataUang: mataUang,
        KodeAkun: kodeAkun,
        TotalOrg: totalOrg,
        TotalRp: $("#PenyelesaianHeader_TotalRp").data("kendoNumericTextBox").value(),
        Keterangan: $("#PenyelesaianHeader_Keterangan").val(),
        DebetKredit: $("#PenyelesaianHeader_DebetKredit").data("kendoDropDownList").value()
    };

    console.log("Saving Header:", headerData);
    

    // 3. KEMBALIKAN promise dari $.ajax
    return $.ajax({
        type: "POST",
        url: "/EntriPenyelesaianPiutang/SaveHeader",
        contentType: "application/json",
        data: JSON.stringify(headerData),
        success: function (response) {
            if (response.success) {
                showMessage('Success', 'Data Header Penyelesaian Utang Piutang Berhasil Disimpan!');
                var nomorBukti = response.nomorBukti;
                console.log("Header save success, NoBukti:", nomorBukti);
                // Set nomor bukti untuk form detail & grid
                $('#NewPaymentForm input[name="NoBukti"]').val(nomorBukti);
                $("#PenyelesaianHeader_NomorBukti").val(nomorBukti);
                $("#BelumFinalGrid").data("kendoGrid").dataSource.read();
                $("#SudahFinalGrid").data("kendoGrid").dataSource.read();
           
                // --- PERBAIKAN DI SINI ---
                var $footer = $(".window-footer");
                
                // 1. Ambil nilai TotalOrg LANGSUNG DARI INPUT KENDO
                var totalOrgInput = $("#PenyelesaianHeader_TotalOrg").data("kendoNumericTextBox");
                var totalOrgValue = totalOrgInput ? totalOrgInput.value() : 0;
                
                // 2. Update span #voucherTotal dengan nilai baru
                $("#voucherTotal").text(kendo.toString(totalOrgValue, "n0"));
                
                // 3. Update data-attribute di footer dengan nilai baru
                $footer
                .attr("data-total-penyelesaian-original", totalOrgValue)
                .data("total-penyelesaian-original", totalOrgValue);
                    
                // JANGAN tampilkan swal di sini, biarkan pemanggil (onSavePembayaran) yang urus
            } else {
                 // Error akan ditangani oleh .fail()
                console.log("Header save failed:", response.message);
            }
        }
    });

}

function btnAddPenyelesaianPiutang_OnClick() {
    var window = $("#EntriPenyelesaianPiutangWindow").data("kendoWindow");

    // Pasang event listener untuk mereset form saat window dibuka
    window.one("refresh", function() {
        attachChangeEvents();
        clearPaymentForm();
    });
    
    // Buka window dengan action Add TANPA parameter (untuk membuat data baru)
    openWindow(
        '#EntriPenyelesaianPiutangWindow', 
        `/EntriPenyelesaianPiutang/Add`, 
        'Entri Penyelesaian Piutang Baru'
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
        var keyword = $(this).val(); // Opsional: jika ingin log keyword

        // 1. Refresh Grid Belum Final (Jika ada)
        var gridBelum = $("#BelumFinalGrid").data("kendoGrid");
        if (gridBelum) {
            gridBelum.dataSource.read();
        }

        // 2. Refresh Grid Sudah Final (Jika ada)
        var gridSudah = $("#SudahFinalGrid").data("kendoGrid");
        if (gridSudah) {
            gridSudah.dataSource.read();
        }
        
        // Catatan: Jika ID grid utama Anda memang #EntriPenyelesaianPiutangGrid,
        // pastikan ID di HTML View (.cshtml) sudah sesuai.
        // Tapi melihat kode lain, sepertinya Anda menggunakan 2 grid terpisah.
    });

    $(document).on("keyup", "#PilihNotaSearchKeyword", function() {
        var grid = $("#PilihNotaGrid").data("kendoGrid");
        if (grid) {
            grid.dataSource.read();
        }
    });
});

function onSavePembayaran() {
    
    // --- 1. VALIDASI FORM DETAIL (NewPaymentForm) ---
    var flagPembayaran = $("#FlagPembayaran").data("kendoDropDownList").value();
    var totalBayarOrg = $("#TotalBayarOrg").data("kendoNumericTextBox").value();
    var kodeMataUang = $("#KodeMataUang").data("kendoComboBox").value();
    var debetKredit = $("#DebetKredit").data("kendoDropDownList").value();
    var kodeAkunValue = null;
    var NoNotaValue = null;

    if (!flagPembayaran) {
        showMessage('warning', 'Silakan pilih Flag Pembayaran terlebih dahulu.');
        return; // Stop
    }

    if (!kodeMataUang) {
        showMessage('warning', 'Silakan pilih Kode Mata Uang.');
        return; // Stop
    }
    
    if (totalBayarOrg === null || totalBayarOrg <= 0) {
        showMessage('warning', 'Total Original harus diisi dan lebih besar dari 0.');
        return; // Stop
    }
    
    if (!debetKredit) {
        showMessage('warning', 'Silakan pilih Debet/Kredit.');
        return; // Stop
    }

    // Validasi spesifik berdasarkan FlagPembayaran
    if (flagPembayaran.toUpperCase() === "AKUN") {
        kodeAkunValue = $("#KodeAkun").data("kendoComboBox").value();
        if (!kodeAkunValue) {
            showMessage('warning', 'Silakan pilih Kode Akun.');
            return; // Stop
        }
    } else if (flagPembayaran.toUpperCase() === "NOTA") {
        NoNotaValue = $("#NoNota").val();
        if (!NoNotaValue || NoNotaValue.trim() === "") {
            showMessage('warning', 'Silakan pilih Nomor Nota.');
            return; // Stop
        }
    }
    // --- AKHIR VALIDASI DETAIL ---


    // --- 2. JIKA DETAIL VALID, SIMPAN HEADER (YANG JUGA DIVALIDASI) ---
    // onSaveHeaderAndProceed() sekarang mengembalikan promise
    
    onSaveHeaderAndProceed().done(function(headerResponse) {
        
        // Cek apakah response header sukses
        if (headerResponse.success) {
            
            // --- 3. JIKA HEADER SUKSES, LANJUT SIMPAN DETAIL ---
            
            // Ambil NoBukti yang *pasti* sudah di-set oleh success callback header
            var noBukti = $("#PenyelesaianHeader_NomorBukti").val(); 
            
            if (!noBukti) {
                 showMessage('error', 'Gagal mendapatkan Nomor Bukti dari header.');
                 return;
            }

            var data = {
                No:  $("#No").val() ? parseInt($("#No").val()) : 0,
                NoBukti: noBukti, // Gunakan noBukti yang sudah pasti
                TotalBayarOrg: totalBayarOrg,
                TotalBayarRp: $("#TotalBayarRp").data("kendoNumericTextBox").value(),
                FlagPembayaran: flagPembayaran,
                KodeMataUang: kodeMataUang,
                DebetKredit: debetKredit,
                KodeAkun: kodeAkunValue,
                NoNota: NoNotaValue
            };
        
            // --- 4. AJAX CALL UNTUK SIMPAN DETAIL ---
            $.ajax({
                type: "POST",
                url: "/EntriPenyelesaianPiutang/Save",
                contentType: "application/json",
                data: JSON.stringify(data),
                success: function (response) {
                    if (response.success) {
                        // Gabungkan pesan sukses
                        showMessage('success', 'Data berhasil diperbarui.'); // Pakai showMessage
                        updateGridFooter()
                        $("#TempDetailPembayaranGrid").data("kendoGrid").dataSource.read();
                        $("#BelumFinalGrid").data("kendoGrid").dataSource.read();
                        $("#SudahFinalGrid").data("kendoGrid").dataSource.read();
           
                        clearPaymentForm(); // Panggil fungsi clear yang sudah ada
                    }else {
                       showMessage('error', 'Gagal menyimpan data detail: ' + (response.message || ''));
                    }
                },
                error: function() {
                    showMessage('error', 'Gagal menyimpan data detail.');
                }
            });

        } else {
            // Ini jika header save me-return success: false
            showMessage('error', 'Gagal menyimpan header: ' + (headerResponse.message || ''));
        }

    }).fail(function(jqXHR, textStatus, errorThrown) {
        // Ini jika AJAX header-nya gagal (error 500, 404, atau dari validasi manual)
        // Jika bukan dari validasi manual, tampilkan error
        if (!jqXHR.error) {
             showMessage('error', 'Gagal menghubungi server untuk menyimpan header.');
        }
    });
}

// edit pembayaran
function onEditPembayaran(e) {
    e.preventDefault();
    $("#btn-save-pembayaran").show();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
     if (!dataItem) {
        showMessage('Error', 'Data pembayaran tidak ditemukan.');
        return;
    }
    console.log(dataItem)
    $("#FlagPembayaran").data("kendoDropDownList").readonly(true);    
    // Isi form input di atas dengan data dari baris yang dipilih
    $("#TotalBayarOrg").data("kendoNumericTextBox").value(dataItem.TotalBayarOrg);
    $("#TotalBayarRp").data("kendoNumericTextBox").value(dataItem.TotalBayarRp);
    $("#DebetKredit").data("kendoDropDownList").value(dataItem.DebetKredit);
    $("#NoBukti").val(dataItem.NoBukti);
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
        $("#NoNota").data("kendoTextBox").value(dataItem.NoNota);
    }
    $("#KodeMataUang").data("kendoComboBox").value(dataItem.KodeMataUang);

    $("#NewPaymentForm").data("original-bayar", dataItem.TotalBayar || 0);
    $("#NewPaymentForm").data("original-dk", dataItem.DebetKredit);
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
      
    $("#btn-cancel-edit").show();
    // Nonaktifkan juga tombol select nota
   
}

// Fungsi ini berjalan saat tombol 'Delete' di grid detail diklik
function onDeletePembayaran(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    
    showConfirmation("Konfirmasi Hapus", "Anda yakin ingin menghapus data pembayaran ini?", function() {
        $.ajax({
            type: "POST", // Sebaiknya gunakan POST atau DELETE untuk operasi hapus
            url: "/EntriPenyelesaianPiutang/DeleteDetail",
            contentType: "application/json",
            data: JSON.stringify({ NoBukti: dataItem.NoBukti, No: dataItem.No }),
            success: function(response) {
                if (response.success) {
                    showMessage('Success', 'Data berhasil dihapus.');
                    clearPaymentForm();
                    $("#TempDetailPembayaranGrid").data("kendoGrid").dataSource.read();
                       $("#BelumFinalGrid").data("kendoGrid").dataSource.read();
                            $("#SudahFinalGrid").data("kendoGrid").dataSource.read(); // Refresh grid
                } else {
                    showMessage('Error', 'Gagal menghapus data.');
                }
            }
        });
    });
}

function openPilihNotaWindow() {
    var window = $("#PilihNotaWindow").data("kendoWindow");
    // Muat konten dari action PilihNota
    window.refresh({ url: "/EntriPenyelesaianPiutang/PilihNota" });
    window.center().open();
}

function onNotaProduksiSelect(e) {
    // Ambil data dari baris yang dipilih
    var selectedRow = this.dataItem(this.select());

    if (selectedRow) {
        // "Replace" nilai di form utama dengan data dari pop-up
        $("#NoNota").val(selectedRow.no_nd);
        // Anda juga bisa mengisi field lain jika perlu, contoh:
        // $("#TotalBayarOrg").data("kendoNumericTextBox").value(selectedRow.Premi);
        
        // Tutup window pop-up setelah dipilih
        $("#PilihNotaWindow").data("kendoWindow").close();
    }
}

// sweetalert
if (typeof Toast === "undefined") {
  window.Toast = Swal.mixin({
    toast: true,
    position: "top-end",
    showConfirmButton: false,
    timer: 3000,
    timerProgressBar: true,
    didOpen: (toast) => {
      toast.onmouseenter = Swal.stopTimer;
      toast.onmouseleave = Swal.resumeTimer;
    }
  });
}

// Fungsi helper baru untuk menampilkan pesan
// function showMessage(type, message) {
//     Toast.fire({
//         icon: type, // 'success', 'error', 'warning', 'info'
//         title: message
//     });
// }


function clearPaymentForm() {
    // Cek dan reset setiap komponen Kendo
    var flagPembayaran = $("#FlagPembayaran").data("kendoDropDownList");
    if (flagPembayaran) flagPembayaran.value("");

    var TotalBayarOrg = $("#TotalBayarOrg").data("kendoNumericTextBox");
    if (TotalBayarOrg) TotalBayarOrg.value(null);

    var TotalBayarRp = $("#TotalBayarRp").data("kendoNumericTextBox");
    if (TotalBayarRp) TotalBayarRp.value(null);
    
    var debetKredit = $("#DebetKredit").data("kendoDropDownList");
    if (debetKredit) debetKredit.value("");

    var kodeMataUang = $("#KodeMataUang").data("kendoComboBox");
    if (kodeMataUang) kodeMataUang.value("");

    // Reset textbox biasa
    $("#NoNota").val("");
    var kodeAkun = $("#KodeAkun").data("kendoComboBox");
    if (kodeAkun) kodeAkun.value("");
    // Reset ID
    if ($("#No").length > 0) {
        $("#No").val(0);
    }

    // Aktifkan kembali field
    if (flagPembayaran) flagPembayaran.readonly(false);
    
    // Sembunyikan tombol Cancel
    $("#btn-cancel-edit").hide();
     $("#NewPaymentForm").show();
}

function clearFinalPaymentForm() {
    // Cek dan reset setiap komponen Kendo
    var flagPembayaran = $("#FlagPembayaran_Lihat").data("kendoDropDownList");
    if (flagPembayaran) flagPembayaran.value("");

    var TotalBayarOrg = $("#TotalBayarOrg_Lihat").data("kendoNumericTextBox");
    if (TotalBayarOrg) TotalBayarOrg.value(null);

    var TotalBayarRp = $("#TotalBayarRp_Lihat").data("kendoNumericTextBox");
    if (TotalBayarRp) TotalBayarRp.value(null);
    
    var debetKredit = $("#DebetKredit_Lihat").data("kendoDropDownList");
    if (debetKredit) debetKredit.value("");

    var kodeMataUang = $("#KodeMataUang_Lihat").data("kendoComboBox");
    if (kodeMataUang) kodeMataUang.value("");

    // Reset textbox biasa
    $("#NoNota_Lihat").val("");
    var kodeAkun = $("#KodeAkun_Lihat").data("kendoComboBox");
    if (kodeAkun) kodeAkun.value("");
    // Reset ID
    if ($("#No").length > 0) {
        $("#No").val(0);
    }

    // Aktifkan kembali field
    if (flagPembayaran) flagPembayaran.readonly(false);
    
    // Sembunyikan tombol Cancel
    $("#btn-cancel-edit-final").hide();
     $("#NewPaymentFinalForm").hide();
}

// fungsi total pembayaran
function updateGridFooter() {
    console.log("Trigger updateGridFooter...");

    var grid = $("#TempDetailPembayaranGrid").data("kendoGrid"); // Pastikan ID Grid benar
    if (!grid) return;

    // Ambil NoBukti langsung dari input, karena getNoBuktiForDetailGrid mungkin mengembalikan object wrapper
    var noBukti = $("#PenyelesaianHeader_NomorBukti").val();
    
    // Ambil DebetKredit Header untuk filter perhitungan di server (jika diperlukan)
    var piutangDK = $("#PenyelesaianHeader_DebetKredit").data("kendoDropDownList").value();

    // Validasi: Jika NoBukti kosong, reset footer dan keluar
    if (!noBukti) {
        console.log("NoBukti kosong, reset footer.");
        $("#pembayaranTotal").text("0");
        $("#sisapembayaranTotal").text("0");
        return;
    }

    console.log("Fetching total for NoBukti:", noBukti, "DK:", piutangDK);

    $.ajax({
        type: "GET",
        // Pastikan parameter URL cocok dengan Controller (no_bukti vs NoBukti)
        url: `/EntriPenyelesaianPiutang/GetTotalPembayaran?no_bukti=${noBukti}&PiutangDK=${piutangDK}`,
        success: function (response) {
            console.log("Total Response:", response);

            var totalPembayaran = response.totalPembayaran || 0;
            var $pembayaranTotalSpan = $("#pembayaranTotal");
            var $sisaPembayaranSpan = $("#sisapembayaranTotal");
            var $btnFinal = $("#btn-save-pembayaran-final");
            $("#BelumFinalGrid").data("kendoGrid").dataSource.read();
             $("#SudahFinalGrid").data("kendoGrid").dataSource.read();
            // Ambil total header terbaru
            // PENTING: Ambil dari .data() yang sudah di-update di onSaveHeaderAndProceed
            var $footer = $(".window-footer");
            var totalHeader = parseFloat($footer.data("total-penyelesaian-original")) || 0;

            // Hitung Sisa
            var sisaPembayaran = totalHeader - totalPembayaran;

            // Update UI Text
            $pembayaranTotalSpan.text(kendo.toString(totalPembayaran, "n0"));
            $sisaPembayaranSpan.text(kendo.toString(sisaPembayaran, "n0"));

            // Logic Warna & Tombol Final
            // Toleransi perbedaan desimal kecil
            if (Math.abs(sisaPembayaran) < 1) {
                // BALANCE (Sisa 0)
                $pembayaranTotalSpan.css("color", "green");
                $sisaPembayaranSpan.css("color", "green");
                $btnFinal.prop("disabled", false).removeClass("k-disabled");
            } else {
                // TIDAK BALANCE
                $pembayaranTotalSpan.css("color", "red");
                $sisaPembayaranSpan.css("color", "red");
                $btnFinal.prop("disabled", true).addClass("k-disabled");
            }
        },
        error: function(err) {
            console.error("Gagal update footer:", err);
        }
    });
}


    function attachChangeEvents() {
        // Ambil referensi ke komponen Kendo di form HEADER
        var mataUangCombo = $("#PenyelesaianHeader_MataUang").data("kendoComboBox");
        var mataUangCombo2 = $("#KodeMataUang").data("kendoComboBox");
        var totalOrgInput = $("#PenyelesaianHeader_TotalOrg").data("kendoNumericTextBox");
        var totalOrgInput2 = $("#TotalBayarOrg").data("kendoNumericTextBox");
        var tanggalPicker = $("#PenyelesaianHeader_Tanggal").data("kendoDatePicker");
        console.log("test")
        // Pasang 'event listener' untuk setiap perubahan
        if (mataUangCombo) {
            mataUangCombo.bind("change", hitungTotalRupiah);
        }
        if (totalOrgInput) {
            totalOrgInput.bind("change", hitungTotalRupiah);
        }
        if (mataUangCombo2) {
            mataUangCombo2.bind("change", hitungTotalRupiah2);
        }
        if (totalOrgInput2) {
            totalOrgInput2.bind("change", hitungTotalRupiah2);
        }
        if (tanggalPicker) {
            tanggalPicker.bind("change", hitungTotalRupiah);
        }
    }

   function hitungTotalRupiah() {
        // Ambil nilai dari komponen di form HEADER
        var kodeMtu = $("#PenyelesaianHeader_MataUang").data("kendoComboBox").value();
        var tanggal = $("#PenyelesaianHeader_Tanggal").data("kendoDatePicker").value();
        var totalOrg = $("#PenyelesaianHeader_TotalOrg").data("kendoNumericTextBox").value();

        var $footer = $("#voucherTotal").closest(".window-footer");
        $("#voucherTotal").text(kendo.toString(totalOrg, "n0"));
        $footer.attr("data-total-penyelesaian-original", totalOrg).data("total-penyelesaian-original", totalOrg);
        
        // Jika mata uang adalah Rupiah (IDR, kode 001), kurs-nya 1
        if (kodeMtu === '001') {
            $("#PenyelesaianHeader_TotalRp").data("kendoNumericTextBox").value(totalOrg);
            return; // Hentikan fungsi
        }

        if (kodeMtu && tanggal && totalOrg) {
            // Format tanggal agar sesuai dengan yang diharapkan Controller (yyyy-MM-dd)
            var formattedDate = kendo.toString(tanggal, "yyyy-MM-dd");
            
            var url = `/EntriPenyelesaianPiutang/GetKurs?kodeMataUang=${kodeMtu}&tanggalVoucher=${formattedDate}`;
        
            $.ajax({
                type: "GET",
                url: url,
                success: function (response) {
                    if (response && response.nilai_kurs) {
                        var kurs = response.nilai_kurs;
                        var totalRupiah = totalOrg * kurs;
                        $("#PenyelesaianHeader_TotalRp").data("kendoNumericTextBox").value(totalRupiah);
                    } else {
                        // Jika kurs tidak ditemukan, kosongkan total rupiah
                        $("#PenyelesaianHeader_TotalRp").data("kendoNumericTextBox").value(null);
                    }
                }
            });
        }
    }
   function hitungTotalRupiah2() {
        // Ambil nilai dari komponen di form HEADER
        var kodeMtu = $("#KodeMataUang").data("kendoComboBox").value();
        var tanggal = $("#PenyelesaianHeader_Tanggal").data("kendoDatePicker").value();
        var totalOrg = $("#TotalBayarOrg").data("kendoNumericTextBox").value();

        // Jika mata uang adalah Rupiah (IDR, kode 001), kurs-nya 1
        if (kodeMtu === '001') {
            $("#TotalBayarRp").data("kendoNumericTextBox").value(totalOrg);
            return; // Hentikan fungsi
        }

        if (kodeMtu && tanggal && totalOrg) {
            // Format tanggal agar sesuai dengan yang diharapkan Controller (yyyy-MM-dd)
            var formattedDate = kendo.toString(tanggal, "yyyy-MM-dd");
            
            var url = `/EntriPenyelesaianPiutang/GetKurs?kodeMataUang=${kodeMtu}&tanggalVoucher=${formattedDate}`;
        
            $.ajax({
                type: "GET",
                url: url,
                success: function (response) {
                    if (response && response.nilai_kurs) {
                        var kurs = response.nilai_kurs;
                        var totalRupiah = totalOrg * kurs;
                        $("#TotalBayarRp").data("kendoNumericTextBox").value(totalRupiah);
                    } else {
                        // Jika kurs tidak ditemukan, kosongkan total rupiah
                        $("#TotalBayarRp").data("kendoNumericTextBox").value(null);
                    }
                }
            });
        }
    }

    
function onNotaProduksiSelect(e) {
    // Ambil data dari baris yang dipilih
    var selectedRow = this.dataItem(this.select());

    if (selectedRow) {
        // "Replace" nilai di form utama dengan data dari pop-up
        $("#NoNota").val(selectedRow.no_nd);
        // Anda juga bisa mengisi field lain jika perlu, contoh:
        // $("#TotalBayar").data("kendoNumericTextBox").value(selectedRow.Premi);
        
        // Tutup window pop-up setelah dipilih
        $("#PilihNotaWindow").data("kendoWindow").close();
    }
}



function SimpanNota() {
    var grid = $("#PilihNotaGrid").data("kendoGrid");
    
    // --- [PERBAIKAN DISINI] ---
    // Coba ambil dari Header Input yang sudah pasti ada isinya setelah save header
    var noBukti = $("#PenyelesaianHeader_NomorBukti").val(); 

    // Backup: Kalau kosong, coba cari di hidden field form detail
    if (!noBukti) {
        noBukti = $('#NewPaymentForm input[name="NoBukti"]').val();
    }
    
    // Validasi akhir
    if (!noBukti) {
        showMessage("Warning", "Nomor Bukti belum terbentuk. Silakan simpan Header terlebih dahulu.");
        return;
    }
    // --------------------------

    var selectedData = [];

    grid.select().each(function () {
        var row = $(this);
        var dataItem = grid.dataItem(this);

        var totalOrg = parseFloat(row.find('.total-org-input').val()) || 0;
        var totalRp = parseFloat(row.find('.total-rp-input').val()) || 0;
        var totalRpInput = row.find('.total-rp-input');

        var akunOtomatis = row.find('.coa-input').val(); 
        var dkOtomatis = row.find('.dk-input').val();

        selectedData.push({
            NoNota: dataItem.no_nd,
            TotalBayarOrg: Math.floor(totalOrg),
            TotalBayarRp: totalRp,
            DebetKredit: dkOtomatis,
            KodeAkun: akunOtomatis,
            KodeMataUang: dataItem.kd_mtu,
            Kurs: parseFloat(totalRpInput.data('kurs')) || 1
        });
    });

    if (selectedData.length === 0) {
        showMessage("Silakan pilih minimal satu nota untuk disimpan.");
        return;
    }

    var payload = {
        NoBukti: noBukti, // Variable ini sekarang sudah aman
        Data: selectedData
    };

    $.ajax({
        url: '/EntriPenyelesaianPiutang/SimpanNota',
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
                
                // Refresh Grid Detail (pastikan ID grid benar)
                // Cek apakah pakai TempDetailPembayaranGrid atau DetailPembayaranGrid
                var detailGrid = $("#TempDetailPembayaranGrid").data("kendoGrid");
                if(detailGrid) detailGrid.dataSource.read();
                
                updateGridFooter(); // Update footer total
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

    // Ambil tanggal dari form utama (Kendo DatePicker)
    var tanggalVoucher = $("#PenyelesaianHeader_Tanggal").data("kendoDatePicker").value();

    // Jika belum diisi, hentikan
    if (!tanggalVoucher) {
        row.find('.total-rp-input').val('0.00');
        return;
    }

    // Ubah ke format yyyy-MM-dd
    var formattedDate = kendo.toString(tanggalVoucher, "yyyy-MM-dd");

    // Jika mata uang adalah Rupiah (001), kurs-nya 1
    if (kodeMtu && kodeMtu.trim() === '001') {
        row.find('.total-rp-input').val(totalOrg.toFixed(2));
        return;
    }

    // Hanya panggil AJAX jika semua data lengkap
    if (kodeMtu && formattedDate && totalOrg > 0) {
        var url = `/EntriPenyelesaianPiutang/GetKurs?kodeMataUang=${kodeMtu}&tanggalVoucher=${formattedDate}`;

        $.ajax({
            type: "GET",
            url: url,
            success: function (response) {
                if (response && response.nilai_kurs) {
                    var kurs = response.nilai_kurs;
                    var totalRupiah = totalOrg * kurs;
                    row.find('.total-rp-input').val(totalRupiah.toFixed(2));
                    row.find('.total-rp-input').data('kurs', kurs);
                }
            }
        });
    } else {
        row.find('.total-rp-input').val('0.00');
        row.find('.total-rp-input').data('kurs', 0);
    }
});

function onSavePembayaranPiutangFinal() {
    var form = $("#NewPaymentForm");
    var data = {
       NoBukti : $("#PenyelesaianHeader_NomorBukti").val()
    };

    if (!data.NoBukti) {
        showMessage("Warning", "Nomor bukti tidak boleh kosong.");
        return;
    }

    $.ajax({
        type: "POST",
        url: "/EntriPenyelesaianPiutang/SaveFinal",
        contentType: "application/json",
        data: JSON.stringify(data),
        success: function (response) {
            if (response.success) {
                showMessage("Success", "Data berhasil difinalkan.");

                // 1. Refresh Grid Internal (Detail di dalam modal)
                var tempGrid = $("#TempDetailPembayaranGrid").data("kendoGrid");
                if (tempGrid) {
                    tempGrid.dataSource.read();
                }
                
                clearPaymentForm();

                // 2. Refresh Grid "Belum Final" (Tab 1)
                var gridBelum = $("#BelumFinalGrid").data("kendoGrid");
                if (gridBelum) {
                    gridBelum.dataSource.read();
                }

                // 3. Refresh Grid "Sudah Final" (Tab 2)
                var gridSudah = $("#SudahFinalGrid").data("kendoGrid");
                if (gridSudah) {
                    // Jika grid sudah pernah dibuka, refresh datanya
                    gridSudah.dataSource.read();
                } 
                // Catatan: Jika gridSudah 'undefined' (user belum pernah buka tabnya), 
                // biarkan saja. Nanti pas user klik tabnya, dia otomatis load data terbaru.

                // 4. TUTUP MODAL
                var wnd = $("#EntriPenyelesaianPiutangWindow").data("kendoWindow");
                if (wnd) {
                    wnd.close();
                }
           
            } else {
                showMessage("Error", response.message || "Gagal memproses data.");
            }
        },
        error: function () {
            showMessage("Error", "Terjadi kesalahan saat menyimpan data.");
        },
    });
}

function onEditFinalPembayaran(e) {
    e.preventDefault();
     $("#NewPaymentFinalForm").show();
    $("#btn-save-pembayaran-final").show();
    $("#btn-cancel-edit-final").show();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
     if (!dataItem) {
        showMessage('Error', 'Data pembayaran tidak ditemukan.');
        return;
    }
    console.log(dataItem)
    $("#FlagPembayaran_Lihat").data("kendoDropDownList").readonly(true);    
    // Isi form input di atas dengan data dari baris yang dipilih
    $("#TotalBayarOrg_Lihat").data("kendoNumericTextBox").value(dataItem.TotalBayarOrg);
    $("#TotalBayarRp_Lihat").data("kendoNumericTextBox").value(dataItem.TotalBayarRp);
    $("#DebetKredit_Lihat").data("kendoDropDownList").value(dataItem.DebetKredit);
    $("#NoBukti_Lihat").val(dataItem.NoBukti);
     $("#buttonselectnota").attr("disabled", true);
    $(".detail-payment-field-final").show();
    
    $("#FlagPembayaran_Lihat").data("kendoDropDownList").value(dataItem.FlagPembayaran);
    if (dataItem.FlagPembayaran && dataItem.FlagPembayaran.toUpperCase() === "AKUN") {
        $("#akunField_Lihat").show();
        $("#notaField_Lihat").hide();
        $("#KodeAkun_Lihat").data("kendoComboBox").value(dataItem.KodeAkun);
        
    } else if (dataItem.FlagPembayaran && dataItem.FlagPembayaran.toUpperCase() === "NOTA") {
        $("#notaField_Lihat").show();
        $("#akunField_Lihat").hide();
        $("#NoNota_Lihat").data("kendoTextBox").value(dataItem.NoNota);
    }
    $("#KodeMataUang_Lihat").data("kendoComboBox").value(dataItem.KodeMataUang);

    $("#NewPaymentFinalForm").data("original-bayar", dataItem.TotalBayar || 0);
    $("#NewPaymentFinalForm").data("original-dk", dataItem.DebetKredit);
    // Simpan juga nomor urut (No) di elemen tersembunyi agar bisa di-update
    // Anda perlu menambahkan <input type="hidden" id="currentEditNo" /> di form
     if ($("#No").length === 0) {
        // kalau belum ada hidden input No, buat baru
        $("<input>").attr({
            type: "hidden",
            id: "No",
            name: "No",
            value: dataItem.No
        }).appendTo("#NewPaymentFinalForm");
    } else {
        $("#No").val(dataItem.No);
    }
      
    
    // Nonaktifkan juga tombol select nota
   
}



function onFinalSavePembayaran() {
    var isDetailMode = $("#NewPaymentFinalForm").is(":visible");
    if (isDetailMode) {
        var flagPembayaran = $("#FlagPembayaran_Lihat").data("kendoDropDownList").value();
        var totalBayarOrg = $("#TotalBayarOrg_Lihat").data("kendoNumericTextBox").value();
        var kodeMataUang = $("#KodeMataUang_Lihat").data("kendoComboBox").value();
        var debetKredit = $("#DebetKredit_Lihat").data("kendoDropDownList").value();
        var kodeAkunValue = null;
        var NoNotaValue = null;

        // 2. Validasi Detail
        if (!flagPembayaran) { showMessage('warning', 'Silakan pilih Flag Pembayaran.'); return; }
        if (!kodeMataUang) { showMessage('warning', 'Silakan pilih Kode Mata Uang.'); return; }
        if (totalBayarOrg === null || totalBayarOrg <= 0) { showMessage('warning', 'Total Original harus diisi > 0.'); return; }
        if (!debetKredit) { showMessage('warning', 'Silakan pilih Debet/Kredit.'); return; }

        if (flagPembayaran.toUpperCase() === "AKUN") {
            kodeAkunValue = $("#KodeAkun_Lihat").data("kendoComboBox").value();
            if (!kodeAkunValue) { showMessage('warning', 'Silakan pilih Kode Akun.'); return; }
        } else if (flagPembayaran.toUpperCase() === "NOTA") {
            NoNotaValue = $("#NoNota_Lihat").val().trim();
            if (!NoNotaValue) { showMessage('warning', 'Silakan pilih Nomor Nota.'); return; }
        }

        // 3. Eksekusi Simpan Header dulu -> lanjut Simpan Detail
        onSaveFinalHeaderAndProceed().done(function(headerResponse) {
            if (headerResponse.success) {
                var noBukti = $("#PenyelesaianHeader_NomorBukti_Lihat").val();
                
                var data = {
                    No: $("#No").val() ? parseInt($("#No").val()) : 0,
                    NoBukti: noBukti,
                    TotalBayarOrg: totalBayarOrg,
                    TotalBayarRp: $("#TotalBayarRp_Lihat").data("kendoNumericTextBox").value(),
                    FlagPembayaran: flagPembayaran,
                    KodeMataUang: kodeMataUang,
                    DebetKredit: debetKredit,
                    KodeAkun: kodeAkunValue,
                    NoNota: NoNotaValue
                };

                $.ajax({
                    type: "POST",
                    url: "/EntriPenyelesaianPiutang/UpdateFinal",
                    contentType: "application/json",
                    data: JSON.stringify(data),
                    success: function (response) {
                        if (response.success) {
                          showMessage('Success', 'Data Header & Detail berhasil disimpan.');
                            $("#TempDetailPembayaranGrid").data("kendoGrid").dataSource.read();
                            $("#BelumFinalGrid").data("kendoGrid").dataSource.read();
                            $("#SudahFinalGrid").data("kendoGrid").dataSource.read();
                            clearFinalPaymentForm(); // Form detail ditutup, tombol cancel hilang
                        } else {
                           showMessage('Error', 'Gagal detail: ' + response.message);
                        }
                    }
                });
            } else {
               showMessage('Error', 'Gagal menyimpan header: ' + headerResponse.message);
            }
        });
    }else{
        onSaveFinalHeaderAndProceed().done(function(response) {
            if (response.success) {
                showMessage('Success', 'Perubahan Header berhasil disimpan.');
                // Tidak perlu refresh grid atau clear form detail
            } else {
                showMessage('Error', 'Gagal menyimpan header: ' + response.message);
            }
        }).fail(function() {
             showMessage('Error', 'Terjadi kesalahan koneksi saat menyimpan header.');
        });
    }
   
}

function onSaveFinalHeaderAndProceed() {
    // 1. Kumpulkan data header
    var tanggal = $("#PenyelesaianHeader_Tanggal_Lihat").data("kendoDatePicker").value();
    var mataUang = $("#PenyelesaianHeader_MataUang_Lihat").data("kendoComboBox").value();
    var totalOrg = $("#PenyelesaianHeader_TotalOrg_Lihat").data("kendoNumericTextBox").value();
    var kodeAkun = $("#PenyelesaianHeader_KodeAkun_Lihat").data("kendoComboBox").value();

    // 2. Validasi Header Sederhana
    // (Anda bisa tambahkan field lain jika wajib)
    if (!tanggal || !mataUang || (totalOrg === null || totalOrg <= 0) || !kodeAkun) {
        showMessage('warning', 'Harap lengkapi data Header (Tanggal, Mata Uang, Total, Kode Akun) sebelum menyimpan.');
        // Mengembalikan promise yang sudah gagal agar .done() tidak berjalan
        return $.Deferred().reject({ error: "Header form is incomplete." }).promise();
    }
    var JenisP = "BM"

    var headerData = {
        KodeCabang: $("#PenyelesaianHeader_KodeCabang_Lihat").data("kendoComboBox").value().trim(),
        JenisPenyelesaian: JenisP,
        NomorBukti: $("#PenyelesaianHeader_NomorBukti_Lihat").val(),
        KodeVoucherAcc: $("#PenyelesaianHeader_KodeVoucherAcc_Lihat").val(),
        Tanggal: tanggal,
        MataUang: mataUang,
        KodeAkun: kodeAkun,
        TotalOrg: totalOrg,
        TotalRp: $("#PenyelesaianHeader_TotalRp_Lihat").data("kendoNumericTextBox").value(),
        Keterangan: $("#PenyelesaianHeader_Keterangan_Lihat").val(),
        DebetKredit: $("#PenyelesaianHeader_DebetKredit_Lihat").data("kendoDropDownList").value()
    };

    console.log("Saving Header:", headerData);
    

    // 3. KEMBALIKAN promise dari $.ajax
    return $.ajax({
        type: "POST",
        url: "/EntriPenyelesaianPiutang/SaveHeader",
        contentType: "application/json",
        data: JSON.stringify(headerData),
        success: function (response) {
            if (response.success) {
                
                var nomorBukti = response.nomorBukti;
                console.log("Header save success, NoBukti:", nomorBukti);
                // Set nomor bukti untuk form detail & grid
                $('#NewFinalPaymentForm input[name="NoBukti"]').val(nomorBukti);
                $("#PenyelesaianHeader_NomorBukti_Lihat").val(nomorBukti);
                $("#BelumFinalGrid").data("kendoGrid").dataSource.read();
                $("#SudahFinalGrid").data("kendoGrid").dataSource.read();
           
                // --- PERBAIKAN DI SINI ---
                var $footer = $(".window-footer");
                
                // 1. Ambil nilai TotalOrg LANGSUNG DARI INPUT KENDO
                var totalOrgInput = $("#PenyelesaianHeader_TotalOrg_Lihat").data("kendoNumericTextBox");
                var totalOrgValue = totalOrgInput ? totalOrgInput.value() : 0;
                
                // 2. Update span #voucherTotal dengan nilai baru
                $("#voucherTotal").text(kendo.toString(totalOrgValue, "n0"));
                
                // 3. Update data-attribute di footer dengan nilai baru
                $footer
                .attr("data-total-penyelesaian-original-final", totalOrgValue)
                .data("total-penyelesaian-original-final", totalOrgValue);
                    
                // JANGAN tampilkan swal di sini, biarkan pemanggil (onSavePembayaran) yang urus
            } else {
                 // Error akan ditangani oleh .fail()
                console.log("Header save failed:", response.message);
            }
        }
    });

}
function btnProsesPembayaranPiutang_OnClick(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    var NoBukti = dataItem.NomorBukti;
    console.log(NoBukti);

    // Tampilkan dialog konfirmasi
    showConfirmation('Konfirmasi Proses', `Apakah Anda yakin ingin Mengulang Proses Pembayaran dengan No. Bukti ${dataItem.NomorBukti}?`,
        function () {
            showProgressOnGrid('#SudahFinalGrid');

            // Kirim request hapus ke Controller
           ajaxGet(
                    `/EntriPenyelesaianPiutang/ProsesUlang?id=${NoBukti}`,
                    function (response) {
                        if (response.success) {
                            showMessage('Success', 'Data berhasil Di Proses Ulang.');
                            // 1. Ambil referensi grid
                            var gridBelum = $("#BelumFinalGrid").data("kendoGrid");
                            var gridSudah = $("#SudahFinalGrid").data("kendoGrid");

                            // 2. Baca ulang data
                            gridBelum.dataSource.read().then(function() {
                                // 3. SETELAH BACA SELESAI, Resize Grid!
                                // Ini kuncinya: Memaksa grid menghitung ulang lebar kolom & scrollbar
                                gridBelum.resize(); 
                            });

                            gridSudah.dataSource.read().then(function() {
                                gridSudah.resize();
                            });
                        } else {
                            showMessage('Error', 'Gagal Memproses data.');
                        }
                    }
                );
        }
    );
}

function onDeleteHeader_Click(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    
    showConfirmation("Konfirmasi Hapus", `Anda yakin ingin menghapus No. Bukti ${dataItem.NomorBukti}? Data detail juga akan terhapus permanen.`, function() {
        var payload = {
            KodeCabang: dataItem.KodeCabang,
            NomorBukti: dataItem.NomorBukti
        };

        $.ajax({
            type: "POST",
            url: "/EntriPenyelesaianPiutang/DeleteHeader",
            contentType: "application/json",
            data: JSON.stringify(payload),
            success: function(response) {
                if (response.success) {
                    showMessage('Success', 'Data berhasil dihapus.');
                    // Refresh Grid
                    $("#BelumFinalGrid").data("kendoGrid").dataSource.read();
                    $("#SudahFinalGrid").data("kendoGrid").dataSource.read();
                } else {
                    showMessage('Error', response.message);
                }
            },
            error: function() {
                showMessage('Error', 'Terjadi kesalahan saat menghapus data.');
            }
        });
    });
}