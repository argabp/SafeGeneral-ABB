function btnEntriPembayaran_OnClick(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    var noVoucher = dataItem.NoVoucher;
    
    // 1. Ambil referensi ke Kendo Window
    var window = $("#EntriPembayaranBankWindow").data("kendoWindow");

    // 2. Pasang "pendengar" event 'open'
    //    Ini akan berjalan SETELAH window terbuka sepenuhnya
    window.one("open", function() {
        // Ambil referensi ke grid di dalam window
        var grid = $("#DetailPembayaranGrid").data("kendoGrid");
        if (grid) {
            // Perintahkan grid untuk memuat datanya
            grid.dataSource.read();
        }
    });

     window.one("refresh", function () {
        attachChangeEvents();
    });

    // 3. Buka window seperti biasa
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
});


function onSavePembayaran() {
    var form = $("#NewPaymentForm");
    var flagPembayaran = $("#FlagPembayaran").data("kendoDropDownList").value();

    var kodeAkunValue = null;
    var noNota4Value = null;

    // 3. Isi variabel berdasarkan pilihan FlagPembayaran
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
        TotalDlmRupiah: $("#TotalDlmRupiah").val(),
        KodeAkun: kodeAkunValue,
        NoNota4: noNota4Value
    };
    $.ajax({
        type: "POST",
        url: "/EntriPembayaranBank/Save",
        contentType: "application/json",
        data: JSON.stringify(data),
        success: function (response) {
            if (response.success) {
                showMessage('Success', data.No ? 'Data berhasil diperbarui.' : 'Data berhasil disimpan.');

                $("#DetailPembayaranGrid").data("kendoGrid").dataSource.read();
                
                // Mengosongkan form input setelah berhasil
                $("#KodeAkun").val("");
                $("#TotalBayar").data("kendoNumericTextBox").value(null);
                $("#FlagPembayaran").val("");
                $("#NoNota4").data("kendoTextBox").value("");
                 $("#DebetKredit").data("kendoDropDownList").value(null);
                $("#KodeMataUang").data("kendoComboBox").value("");
                $("#btn-cancel-edit").hide();
                $("#FlagPembayaran").data("kendoDropDownList").readonly(false);
                
            }else {
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
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
     if (!dataItem) {
        showMessage('Error', 'Data pembayaran tidak ditemukan.');
        return;
    }
    $("#FlagPembayaran").data("kendoDropDownList").readonly(true);    
    // Isi form input di atas dengan data dari baris yang dipilih
    $("#TotalBayar").data("kendoNumericTextBox").value(dataItem.TotalBayar);
    $("#DebetKredit").data("kendoDropDownList").value(dataItem.DebetKredit);
    $("#NoVoucher").val(dataItem.NoVoucher);
    
    $("#FlagPembayaran").data("kendoDropDownList").value(dataItem.FlagPembayaran);
    if (dataItem.FlagPembayaran && dataItem.FlagPembayaran.toUpperCase() === "AKUN") {
        $("#akunField").show();
        $("#notaField").hide();
        $("#KodeAkun").data("kendoTextBox").value(dataItem.KodeAkun);
        
    } else if (dataItem.FlagPembayaran && dataItem.FlagPembayaran.toUpperCase() === "NOTA") {
        $("#notaField").show();
        $("#akunField").hide();
        $("#NoNota4").data("kendoTextBox").value(dataItem.NoNota4);
    }
    $("#KodeMataUang").data("kendoComboBox").value(dataItem.KodeMataUang);
   
    // Simpan juga nomor urut (No) di elemen tersembunyi agar bisa di-update
    // Anda perlu menambahkan <input type="hidden" id="currentEditNo" /> di form
     if ($("#No").length === 0) {
        // kalau belum ada hidden input No, buat baru
        $("<input>").attr({
            type: "hidden",
            id: "No",
            name: "No",
            value: dataItem.No
        }).appendTo("#NewPaymentKasForm");
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
    var window = $("#PilihNotaWindow").data("kendoWindow");
    // Muat konten dari action PilihNota
    window.refresh({ url: "/EntriPembayaranBank/PilihNota" });
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

// sweetalert
const Toast = Swal.mixin({
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

// Fungsi helper baru untuk menampilkan pesan
function showSwal(type, message) {
    Toast.fire({
        icon: type, // 'success', 'error', 'warning', 'info'
        title: message
    });
}

function clearPaymentForm() {
    $("#KodeAkun").val("");
    $("#TotalBayar").data("kendoNumericTextBox").value(null);
    $("#FlagPembayaran").val("");
    $("#NoNota4").data("kendoTextBox").value("");
        $("#DebetKredit").data("kendoDropDownList").value(null);
    $("#KodeMataUang").data("kendoComboBox").value("");
        if ($("#No").length > 0) {
        $("#No").val(0);
    }

    // Aktifkan kembali field yang mungkin di-readonly
    $("#FlagPembayaran").data("kendoDropDownList").readonly(false);
  
    // ---> TAMBAHKAN BARIS INI <---
    $("#btn-cancel-edit").hide(); // Sembunyikan kembali tombol Cancel
}

// fungsi total pembayaran
function updateGridFooter() {
    var grid = $("#DetailPembayaranGrid").data("kendoGrid");
    if (!grid) return;

    var data = grid.dataSource.data();
    var totalPembayaran = 0;

    // Loop melalui setiap baris data di grid
    for (var i = 0; i < data.length; i++) {
        var item = data[i];
        if (item.TotalBayar) {
            // --- INI LOGIKA BARUNYA ---
            // Cek status Debet atau Kredit
            if (item.DebetKredit && item.DebetKredit.toUpperCase() === 'K') {
                totalPembayaran -= item.TotalBayar; // Jika Kredit, kurangi
            } else {
                totalPembayaran += item.TotalBayar; // Jika Debit (atau kosong), tambahkan
            }
            // -------------------------
        }
    }

    // Update teks di footer dengan format angka
    $("#pembayaranTotal").text(kendo.toString(totalPembayaran, "n0"));
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

