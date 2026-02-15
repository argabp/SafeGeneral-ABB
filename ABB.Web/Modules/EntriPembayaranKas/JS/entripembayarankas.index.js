function btnEntriPembayaranKas_OnClick(e) {
    e.preventDefault();
    // var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    // var noVoucher = dataItem.NoVoucher;

    // // Tutup SEMUA window Kendo yang terbuka
    // $(".k-window-content").each(function () {
    //     var wnd = $(this).data("kendoWindow");
    //     if (wnd) wnd.close();
    // });

    // // Ambil window untuk Entri/Edit
    // var windowEdit = $("#EntriPembayaranKasWindow").data("kendoWindow");

    // windowEdit.one("open", function () {
    //     var grid = $("#DetailPembayaranGrid").data("kendoGrid");
    //     if (grid) grid.dataSource.read();
    // });

    // windowEdit.one("refresh", function () {
    //     attachChangeEvents();
    // });

    // // 🔹 Muat halaman Edit dan buka modal
    // windowEdit.refresh(`/EntriPembayaranKas/Add?noVoucher=${noVoucher}`);
    // windowEdit.title("Edit Pembayaran Kas");
    // windowEdit.center().open();

   
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    var noVoucher = dataItem.NoVoucher;
    
    // 1. Ambil referensi ke Kendo Window
    var window = $("#EntriPembayaranKasWindow").data("kendoWindow");
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
     openWindow(
        '#EntriPembayaranKasWindow', 
        `/EntriPembayaranKas/Add?noVoucher=${noVoucher}`, 
        'Entri Pembayaran'
    );
}

$(document).ready(function () {
   $("#SearchKeyword").on("keyup", function () {
        // 1. Cek Tab mana yang sedang aktif
        var tabStrip = $("#tabStrip").data("kendoTabStrip");
        
        // Jaga-jaga jika tabstrip belum ter-init
        if (!tabStrip) return; 

        var activeTabIndex = tabStrip.select().index();

        // 2. Refresh Grid sesuai Tab yang aktif
        if (activeTabIndex === 0) {
            // Tab 1: Dalam Proses
            var gridBelum = $("#BelumFinalGrid").data("kendoGrid");
            if (gridBelum) gridBelum.dataSource.read();
        } else {
            // Tab 2: Sudah Final
            var gridSudah = $("#SudahFinalGrid").data("kendoGrid");
            if (gridSudah) gridSudah.dataSource.read();
        }
    });

     $(document).on("keyup", "#PilihNotaSearchKeyword", function() {
        var grid = $("#PilihNotaGrid").data("kendoGrid");
        if (grid) {
            grid.dataSource.read();
        }
    });
});

function getSearchFilter() {
    return {
        searchKeyword: $("#SearchKeyword").val()
    };
}

// fungsi harus di global scope, jangan di dalam $(document).ready


function btnCetakPembayaranKas_OnClick(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    var noVoucher = dataItem.NoVoucher;

    // buka tab baru (tanpa auto-print)
    window.open(`/EntriPembayaranKas/Cetak?noVoucher=${noVoucher}`, "_blank");
}

function onSavePembayaranKas() {
    var form = $("#NewPaymentKasForm");
    var data = {
        NoVoucher: form.find("#NoVoucher").val(),
        No: form.find("#No").val() || 0, // cek hidden input No
        KodeAkun: $("#KodeAkun").val(),
        TotalBayar: $("#TotalBayar").data("kendoNumericTextBox").value(),
        FlagPembayaran: $("#FlagPembayaran").val(),
        NoNota4: $("#NoNota4").val(),
        DebetKredit: $("#DebetKredit").val(),
        KodeMataUang: $("#KodeMataUang").val(),
        TotalDlmRupiah: $("#TotalDlmRupiah").val(),
        Kurs: $("#Kurs").val(),
    };

    $.ajax({
        type: "POST",
        url: "/EntriPembayaranKas/Save",
        contentType: "application/json",
        data: JSON.stringify(data),
        success: function (response) {
            if (response.success) {
                showMessage('Success', data.No ? 'Data berhasil diperbarui.' : 'Data berhasil disimpan.');

                $("#DetailPembayaranGrid").data("kendoGrid").dataSource.read();
                clearPaymentForm()
                // reset form setelah simpan
                form.find("#No").remove(); // buang id detail biar balik mode tambah baru
                // $("#FlagPembayaran").data("kendoDropDownList").value(null);
                // $("#NoNota4").data("kendoTextBox").value("");
                // $("#KodeAkun").data("kendoTextBox").value("");
                // $("#TotalBayar").data("kendoNumericTextBox").value(null);
                // $("#DebetKredit").data("kendoDropDownList").value(null);
                // $("#KodeMataUang").data("kendoComboBox").value("");
                // $("#TotalDlmRupiah").data("kendoNumericTextBox").value(null);
               // $("#akunField").hide();
               // $("#notaField").hide();
            } else {
                showMessage('Error', 'Gagal menyimpan data.');
            }
        },
        error: function() {
            showMessage('Error', 'Terjadi kesalahan saat menyimpan data.');
        }
    });
}


function onDeletePembayaranKas(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    
    showConfirmation("Konfirmasi Hapus", "Anda yakin ingin menghapus data pembayaran ini?", function() {
       $.ajax({
            type: "POST",
            url: "/EntriPembayaranKas/DeleteDetail",
            data: { NoVoucher: dataItem.NoVoucher, No: dataItem.No }, // kirim sebagai form data
            success: function(response) {
                if (response.success) {
                    showMessage('Success', 'Data berhasil dihapus.');
                    clearPaymentForm();
                    $("#DetailPembayaranGrid").data("kendoGrid").dataSource.read();
                } else {
                    showMessage('Error', 'Gagal menghapus data.');
                }
            }
        });
    });
}

function onEditPembayaran(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));

    if (!dataItem) {
        showMessage('Error', 'Data pembayaran tidak ditemukan.');
        return;
    }

    // Isi form dengan data yang dipilih
    $("#NoVoucher").val(dataItem.NoVoucher); // hidden field
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
     $("#buttonselectnota").attr("disabled", true);
     $("#btn-save-pembayaran").css("display", "flex");
    $(".detail-payment-field").show();
     $("#TotalDlmRupiah").data("kendoNumericTextBox").value(dataItem.TotalDlmRupiah);
   
    $("#KodeMataUang").data("kendoComboBox").value(dataItem.KodeMataUang);
    $("#TotalBayar").data("kendoNumericTextBox").value(dataItem.TotalBayar);
    $("#DebetKredit").data("kendoDropDownList").value(dataItem.DebetKredit);

    // Simpan juga ID detail (No) supaya saat simpan tahu ini edit
    if ($("#No").length === 0) {
        // kalau belum ada hidden input No, buat baru
       $("<input>").attr({
            type: "hidden",
            id: "No",
            name: "No",   // <-- penting supaya ikut terbaca di server
            value: dataItem.No
        }).appendTo("#NewPaymentKasForm");
    } else {
        $("#No").val(dataItem.No);
    }
    $("#btn-cancel-edit").show();
}
function openPilihNotaWindow() {
    var noVoucher = $("#NoVoucher").val(); 
    var window = $("#PilihNotaWindow").data("kendoWindow");
    // Muat konten dari action PilihNota
    window.refresh({ url: "/EntriPembayaranKas/PilihNota" });
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

function updateGridFooter() {
    var grid = $("#DetailPembayaranGrid").data("kendoGrid");
    if (!grid) return;
    var voucherDK = $("#VoucherDK").val();
    var dataForGrid = getNoVoucherForDetailGrid(); 
    
    if (dataForGrid && dataForGrid.noVoucher) {
        var noVoucher = dataForGrid.noVoucher;
        
        $.ajax({
            type: "GET",
            url: `/EntriPembayaranKas/GetTotalPembayaran?noVoucher=${noVoucher}&voucherDK=${voucherDK}`,
            success: function (response) {
                // Total Pembayaran dari JSON (biasanya sudah titik, jadi aman)
                var totalPembayaran = parseFloat(response.totalPembayaran) || 0;
                
                var $pembayaranTotalSpan = $("#pembayaranTotal");
                var $sisaPembayaranSpan = $("#sisapembayaranTotal");
                var $footer = $pembayaranTotalSpan.closest(".window-footer");
                
                // Ambil Total Voucher dari HTML (Sekarang sudah pakai titik berkat InvariantCulture)
                var rawTotalVoucher = $footer.data("total-voucher-original");
                var totalVoucherAsli = parseFloat(rawTotalVoucher) || 0;
                
                // Debugging (Cek di Console Browser kalau masih penasaran)
                console.log("Total Voucher Asli:", totalVoucherAsli); 
                console.log("Total Pembayaran:", totalPembayaran);

                // Hitung sisa dengan presisi integer (dikali 100)
                // Math.round mengatasi floating point error seperti .9900000001
                var sisaPembayaran = (Math.round(totalVoucherAsli * 100) - Math.round(totalPembayaran * 100)) / 100;

                // Update UI
                $pembayaranTotalSpan.text(kendo.toString(totalPembayaran, "n2"));
                $sisaPembayaranSpan.text(kendo.toString(sisaPembayaran, "n2"));

                var $btnFinal = $("#btn-save-pembayaran-final");

                // Logika Tombol Final
                if (Math.abs(sisaPembayaran) < 0.01) { 
                    $pembayaranTotalSpan.css("color", "green");
                    $sisaPembayaranSpan.css("color", "green");
                    // Paksa 0.00 jika sisa sangat kecil (misal 0.0000001)
                    $sisaPembayaranSpan.text("0.00"); 
                    $btnFinal.prop("disabled", false).removeClass("k-disabled");
                } else {
                    $pembayaranTotalSpan.css("color", "red");
                    $sisaPembayaranSpan.css("color", "red");
                    $btnFinal.prop("disabled", true).addClass("k-disabled");
                }
            }
        });
    }
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
        var tanggalRaw = $("#TangVoc").val(); // Sekarang nilainya pasti yyyy-MM-dd
        var totalVoucher = $("#TotalBayar").data("kendoNumericTextBox").value();
        var kursInput = $("#Kurs");

        if (kodeMtu && tanggalRaw && totalVoucher) {
            // PERBAIKAN: Beri tahu Kendo format pastinya (yyyy-MM-dd)
            var dateObj = kendo.parseDate(tanggalRaw, "yyyy-MM-dd");
            
            if (!dateObj) {
                // Jika masih gagal, kita coba cara kasar (string manipulation)
                if (tanggalRaw.includes("-")) {
                    var p = tanggalRaw.split("-");
                    dateObj = new Date(p[0], p[1] - 1, p[2]);
                }
            }

            if (!dateObj) {
                console.error("Format tanggal tetap tidak bisa dibaca:", tanggalRaw);
                return;
            }

            // Format standar untuk API
            var formattedDate = kendo.toString(dateObj, "yyyy-MM-dd");
            console.log("Tanggal yang dikirim ke API Kurs:", formattedDate);
            
            var url = `/EntriPembayaranKas/GetKurs?kodeMataUang=${kodeMtu}&tanggalVoucher=${formattedDate}`;
        
            $.ajax({
                type: "GET",
                url: url,
                success: function (response) {
                    if (response && response.nilai_kurs) {
                        var kurs = response.nilai_kurs;
                        kursInput.val(kurs);
                        var totalRupiah = totalVoucher * kurs;
                        $("#TotalDlmRupiah").data("kendoNumericTextBox").value(totalRupiah);
                    }
                },
                error: function (jqXHR) {
                    console.error("AJAX GAGAL!", jqXHR);
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
        var tanggalRaw = $("#TangVocFinal").val(); // Mengambil dari hidden input yyyy-MM-dd
        var totalVoucher = $("#TotalBayar_Lihat").data("kendoNumericTextBox").value();
        var kursInput = $("#Kurs_Lihat");

        if (kodeMtu && tanggalRaw && totalVoucher) {
            // PERBAIKAN: Beri tahu Kendo format pastinya (yyyy-MM-dd)
            var dateObj = kendo.parseDate(tanggalRaw, "yyyy-MM-dd");
            
            // Proteksi manual jika parsing gagal
            if (!dateObj && tanggalRaw.includes("-")) {
                var parts = tanggalRaw.split("-");
                dateObj = new Date(parts[0], parts[1] - 1, parts[2]);
            }

            if (!dateObj) {
                console.error("Format tanggal Lihat Kas tidak valid:", tanggalRaw);
                return;
            }

            var formattedDate = kendo.toString(dateObj, "yyyy-MM-dd");
            console.log("Tanggal Lihat Kas dikirim ke API Kurs:", formattedDate);
            
            // URL endpoint modul Kas
            var url = `/EntriPembayaranKas/GetKurs?kodeMataUang=${kodeMtu}&tanggalVoucher=${formattedDate}`;
        
            $.ajax({
                type: "GET",
                url: url,
                success: function (response) {
                    if (response && response.nilai_kurs) {
                        var kurs = response.nilai_kurs;
                        kursInput.val(kurs);
                        
                        var totalRupiah = totalVoucher * kurs;
                        $("#TotalDlmRupiah_Lihat").data("kendoNumericTextBox").value(totalRupiah);
                    }
                },
                error: function (jqXHR) {
                    console.error("AJAX GetKurs Lihat Kas Gagal!", jqXHR);
                }
            });
        }
    }




 function SimpanNota() {
    var grid = $("#PilihNotaGrid").data("kendoGrid");
    var selectedData = [];
    var noVoucher = $("#NoVoucher").val();

    var isValid = true;
    var errorMessage = "";

    grid.select().each(function () {
        var row = $(this);
        var dataItem = grid.dataItem(this);

        var totalOrg = parseFloat(row.find('.total-org-input').val()) || 0;
        var totalRp = parseFloat(row.find('.total-rp-input').val()) || 0;
        var totalRpInput = row.find('.total-rp-input');

        var akunOtomatis = row.find('.coa-input').val(); 
        var dkOtomatis = row.find('.dk-input').val();
        var saldo = parseFloat(dataItem.saldo) || 0;

        // 🔴 VALIDASI
        // if (totalOrg > saldo) {
        //     isValid = false;
        //    showMessage(
        //         'Error',
        //         "Jumlah dibayarkan melebihi saldo!<br><br>" +
        //         "<b>No Nota</b> : " + dataItem.no_nd + "<br>" +
        //         "<b>Saldo</b> : " + kendo.toString(saldo, "n2") + "<br>" +
        //         "<b>Input</b> : " + kendo.toString(totalOrg, "n2")
        //     );
        //     return false; // STOP LOOP
        // }

        selectedData.push({
            NoNota: dataItem.no_nd,
            TotalBayarOrg: totalOrg,
            TotalBayarRp: totalRp,
            DebetKredit: dkOtomatis,
            KodeAkun: akunOtomatis,
            KodeMataUang: dataItem.kd_mtu,
            Kurs: parseFloat(totalRpInput.data('kurs')) || 1
        });
    });

    // ❌ JIKA TIDAK VALID → STOP TOTAL
    if (!isValid) {
        return;
    }

    if (selectedData.length === 0) {
        alert("Silakan pilih minimal satu nota untuk disimpan.");
        return;
    }

    var payload = {
        NoVoucher: noVoucher,
        Data: selectedData
    };

    $.ajax({
        url: '/EntriPembayaranKas/SimpanNota',
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

                var detailGrid = $("#DetailPembayaranGrid").data("kendoGrid");
                if (detailGrid) {
                    detailGrid.dataSource.read();
                }
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

// function getNotaProduksiSearchFilter() {
//     return {
//         searchKeyword: $("#PilihNotaSearchKeyword").val(),
//         jenisAsset: $("#JenisAsset").data("kendoComboBox").value()
//     };
// }

 var totalBayarMap = {};

    function updateTotalBayar(noNota, value) {
        totalBayarMap[noNota] = parseFloat(value) || 0;
        console.log("Total bayar " + noNota + " = " + totalBayarMap[noNota]);
    }


$(document).on('change keyup', '#PilihNotaGrid .total-org-input', function () {
    var input = $(this);
    var row = input.closest('tr');
    var grid = $("#PilihNotaGrid").data("kendoGrid");
    var dataItem = grid.dataItem(row);

    var totalOrg = parseFloat(input.val()) || 0;
    var kodeMtu = dataItem.kd_mtu;
    var $inputRp = row.find('.total-rp-input');

    // --- [PERBAIKAN 1: Parsing Tanggal yang Aman] ---
    var rawDate = dataItem.tgl_prod || dataItem.date; 
    var dateObj = kendo.parseDate(rawDate); // Mencoba parse dari data grid
    
    var formattedDate = "";
    if (dateObj) {
        formattedDate = kendo.toString(dateObj, "yyyy-MM-dd");
    } else {
        // Fallback: Jika tanggal di grid aneh/kosong, pakai TangVoc yang sudah pasti yyyy-MM-dd
        var tangVocRaw = $("#TangVoc").val();
        var fallbackDate = kendo.parseDate(tangVocRaw, "yyyy-MM-dd");
        if (fallbackDate) {
            formattedDate = kendo.toString(fallbackDate, "yyyy-MM-dd");
        }
    }
    // ------------------------------------------------

    if (kodeMtu && kodeMtu.trim() === '001') {
        $inputRp.val(totalOrg.toFixed(2));
        $inputRp.data('kurs', 1);
        return; 
    }

    var cachedKurs = $inputRp.data('kurs');
    if (cachedKurs && cachedKurs > 0) {
        var totalRupiah = totalOrg * cachedKurs;
        $inputRp.val(totalRupiah.toFixed(2));
    } 
    else if (kodeMtu && formattedDate && totalOrg > 0) {
        // --- [PERBAIKAN 2: Pastikan URL Dinamis sesuai modul jika perlu] ---
        // Jika ini di modul Bank, ganti Kas menjadi Bank
        var url = `/EntriPembayaranKas/GetKurs?kodeMataUang=${kodeMtu}&tanggalVoucher=${formattedDate}`;
        
        $.ajax({
            type: "GET",
            url: url,
            success: function (response) {
                if (response && response.nilai_kurs) {
                    var kurs = response.nilai_kurs;
                    var totalRupiah = totalOrg * kurs;
                    $inputRp.val(totalRupiah.toFixed(2));
                    $inputRp.data('kurs', kurs);
                }
            },
            error: function() {
                console.error("Gagal mengambil kurs untuk nota: " + dataItem.no_nd);
            }
        });
    } else {
         $inputRp.val('0.00');
    }
});



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

    $("#NewPaymentForm").data("original-bayar", 0);
    $("#NewPaymentForm").data("original-dk", "");
     $("#TotalDlmRupiah").data("kendoNumericTextBox").value(null);
    // ---> TAMBAHKAN BARIS INI <---
    $("#btn-cancel-edit").hide(); // Sembunyikan kembali tombol Cancel

    var ddl = $("#FlagPembayaran").data("kendoDropDownList");
    ddl.readonly(false);
    ddl.trigger("change");
}

function onSavePembayaranKasFinal() {
    var form = $("#NewPaymentKasForm");
    var data = {
        NoVoucher: form.find("#NoVoucher").val(),
    };

    if (!data.NoVoucher) {
        showMessage("Warning", "Nomor voucher tidak boleh kosong.");
        return;
    }

    $.ajax({
        type: "POST",
        url: "/EntriPembayaranKas/SaveFinal",
        contentType: "application/json",
        data: JSON.stringify(data),
        success: function (response) {
            if (response.success) {
                showMessage("Success", "Data berhasil difinalkan.");

                // 1. Refresh Grid UTAMA (yang ada di Index) supaya statusnya terupdate
                // Pastikan ID grid utamanya benar, biasanya #EntriPembayaranKasGrid
                var mainGrid = $("#BelumFinalGrid").data("kendoGrid");
                if (mainGrid) {
                    mainGrid.dataSource.read();
                }
                var secondGrid = $("#SudahFinalGrid").data("kendoGrid");
                if (secondGrid) {
                    secondGrid.dataSource.read();
                }

                // 2. Tutup Modal (Window)
                var wnd = $("#EntriPembayaranKasWindow").data("kendoWindow");
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


function btnLihatPembayaranKas_OnClick(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    var noVoucher = dataItem.NoVoucher;

    // Tutup SEMUA window Kendo yang terbuka
    $(".k-window-content").each(function () {
        var wnd = $(this).data("kendoWindow");
        if (wnd) wnd.close();
    });

    // Ambil window untuk Lihat
    var windowView = $("#EntriPembayaranKasLihatWindow").data("kendoWindow");

    windowView.one("refresh", function () {
        var grid = $("#DetailPembayaranLihatGrid").data("kendoGrid");
        if (grid) grid.dataSource.read();
        attachChangeFinalEvents()
    });

    // 🔹 Muat halaman Lihat dan buka modal
    // windowView.refresh(`/EntriPembayaranKas/Lihat?noVoucher=${noVoucher}`);
    // windowView.title("Lihat Pembayaran Kas");
    // windowView.center().open();
    openWindow(
         '#EntriPembayaranKasLihatWindow', 
        `/EntriPembayaranKas/Lihat?noVoucher=${noVoucher}`, 
        'Lihat Pembayaran Kas'
    )
}


  function updateGridFooterLihat(e) {
        var grid = $("#DetailPembayaranLihatGrid").data("kendoGrid");
        var voucherDK = $("#VoucherDKFinal").val();
        var dataForGrid = getNoVoucherFinalForDetailGrid(); 

        if (dataForGrid && dataForGrid.noVoucher) {
            var noVoucher = dataForGrid.noVoucher;
            
            $.ajax({
                type: "GET",
                // Pastikan URL-nya benar ke method Final
                url: `/EntriPembayaranKas/GetTotalPembayaranFinal?noVoucher=${noVoucher}&voucherDK=${voucherDK}`,
                success: function (response) {
                    // 1. Ambil Total Pembayaran (Paksa float 2 desimal)
                    var totalPembayaran = parseFloat(parseFloat(response.totalPembayaran).toFixed(2)) || 0;
                    
                    var $pembayaranTotalSpan = $("#pembayaranTotalFinal");
                    var $sisaPembayaranSpan = $("#sisapembayaranTotalFinal");
                    var $footer = $pembayaranTotalSpan.closest(".window-footer");
                    
                    // 2. Ambil Total Voucher Asli dari HTML (Sekarang sudah format titik)
                    var rawTotalVoucher = $footer.data("total-voucher-original-final");
                    var totalVoucherAsli = parseFloat(rawTotalVoucher) || 0;
                    
                    // 3. Hitung sisa dengan presisi integer (dikali 100)
                    var sisaPembayaran = (Math.round(totalVoucherAsli * 100) - Math.round(totalPembayaran * 100)) / 100;

                    // 4. Update Teks UI
                    $pembayaranTotalSpan.text(kendo.toString(totalPembayaran, "n2"));
                    $sisaPembayaranSpan.text(kendo.toString(sisaPembayaran, "n2"));

                    // 5. Indikator Warna
                    if (Math.abs(sisaPembayaran) <= 0.01) { 
                        $pembayaranTotalSpan.css("color", "green");
                        $sisaPembayaranSpan.css("color", "green");
                        $sisaPembayaranSpan.text("0.00");
                    } else {
                        $pembayaranTotalSpan.css("color", "red");
                        $sisaPembayaranSpan.css("color", "red");
                    }
                }
            });
        }
    }


function onUpdateFinalPembayaranKas() {
    var form = $("#FinalPaymentKasForm"); // <-- ID form dari Lihat.cshtml

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
        url: "/EntriPembayaranKas/UpdateFinal", // <-- Targetkan endpoint FINAL
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

function onEditFinalPembayaran(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));

    if (!dataItem) {
        showMessage('Error', 'Data pembayaran tidak ditemukan.');
        return;
    }

    // 1. Dapatkan referensi form dan tombol cancel
    var form = $("#FinalPaymentKasForm");
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

/**
 * Membersihkan dan menyembunyikan form edit di window "Lihat" (Final).
 */
function clearFinalPaymentForm() {
    var form = $("#FinalPaymentKasForm");
    
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
function btnProsesPembayaranKas_OnClick(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));

    // Tampilkan dialog konfirmasi
    showConfirmation('Konfirmasi Proses', `Apakah Anda yakin ingin Mengulang Proses Pembayaran dengan No. Voucher ${dataItem.NoVoucher}?`,
        function () {
            showProgressOnGrid('#SudahFinalGrid');

            // Kirim request hapus ke Controller
            ajaxGet(`/EntriPembayaranKas/ProsesUlang?id=${dataItem.NoVoucher.trim()}`,
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