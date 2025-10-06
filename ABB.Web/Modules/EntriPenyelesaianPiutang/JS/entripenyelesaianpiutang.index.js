function btnEntriPenyelesaian_OnClick(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    
    // Ambil composite key dari dataItem
    var kodeCabang = dataItem.KodeCabang;
    var nomorBukti = dataItem.NomorBukti;

    var window = $("#EntriPenyelesaianPiutangWindow").data("kendoWindow");

    window.one("refresh", function() {
        // Setelah window refresh, panggil clearForm untuk memastikan form dalam kondisi Add
        clearPaymentForm(); 
    });
    
    openWindow(
        '#EntriPenyelesaianPiutangWindow', 
        `/EntriPenyelesaianPiutang/Add?kodeCabang=${kodeCabang}&nomorBukti=${nomorBukti}`, 
        'Entri Penyelesaian Piutang'
    );
}
// Ganti fungsi lama Anda dengan yang ini
function onSaveHeaderAndProceed() {
    // Kumpulkan semua data dari form header
    var headerData = {
        KodeCabang: $("#PenyelesaianHeader_KodeCabang").data("kendoComboBox").value(),
        JenisPenyelesaian: $("#PenyelesaianHeader_JenisPenyelesaian").val(),
        NomorBukti: $("#PenyelesaianHeader_NomorBukti").val(),
        KodeVoucherAcc: $("#PenyelesaianHeader_KodeVoucherAcc").val(),
        Tanggal: $("#PenyelesaianHeader_Tanggal").data("kendoDatePicker").value(),
        MataUang: $("#PenyelesaianHeader_MataUang").data("kendoComboBox").value(),
        KodeAkun: $("#PenyelesaianHeader_KodeAkun").data("kendoComboBox").value(),
        TotalOrg: $("#PenyelesaianHeader_TotalOrg").data("kendoNumericTextBox").value(),
        TotalRp: $("#PenyelesaianHeader_TotalRp").data("kendoNumericTextBox").value(),
        Keterangan: $("#PenyelesaianHeader_Keterangan").val()
    };

    $.ajax({
        type: "POST",
        url: "/EntriPenyelesaianPiutang/SaveHeader",
        contentType: "application/json",
        data: JSON.stringify(headerData),
        success: function (response) {
            if (response.success) {
                var nomorBukti = response.nomorBukti;

                // Set nomor bukti untuk form detail & grid
                $('#NewPaymentForm input[name="NoBukti"]').val(nomorBukti);
                $("#PenyelesaianHeader_NomorBukti").val(nomorBukti);

                // Kunci form header & tombolnya
                $("#PenyelesaianPiutangForm :input").prop("disabled", true);
                $("#btn-save-header").hide();

                // Tampilkan & aktifkan bagian detail
                $("#detailSection").show();
                
                showSwal('success', 'Header berhasil disimpan. Silakan lanjutkan ke detail.');

            } else {
                showSwal('error', 'Gagal menyimpan header.');
            }
        }
    });
}

function btnAddPenyelesaianPiutang_OnClick() {
    var window = $("#EntriPenyelesaianPiutangWindow").data("kendoWindow");

    // Pasang event listener untuk mereset form saat window dibuka
    window.one("refresh", function() {
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
        $("#EntriPenyelesaianPiutangGrid").data("kendoGrid").dataSource.read();
    });
});


function onSavePembayaran() {
    var form = $("#NewPaymentForm");
    var flagPembayaran = $("#FlagPembayaran").data("kendoDropDownList").value();

    var kodeAkunValue = null;
    var NoNotaValue = null;

    // 3. Isi variabel berdasarkan pilihan FlagPembayaran
    if (flagPembayaran && flagPembayaran.toUpperCase() === "AKUN") {
        kodeAkunValue = $("#KodeAkun").data("kendoComboBox").value();
    } else if (flagPembayaran && flagPembayaran.toUpperCase() === "NOTA") {
        NoNotaValue = $("#NoNota").val();
    }
     var data = {
        No: $("#No").val() ? parseInt($("#No").val()) : 0,
         NoBukti: $("#NomorBuktiHeader").val(),
        TotalBayarOrg: $("#TotalBayarOrg").data("kendoNumericTextBox").value(),
        TotalBayarRp: $("#TotalBayarRp").data("kendoNumericTextBox").value(),
        FlagPembayaran: flagPembayaran,
        KodeMataUang: $("#KodeMataUang").data("kendoComboBox").value(),
        DebetKredit: $("#DebetKredit").data("kendoDropDownList").value(),
        
        KodeAkun: kodeAkunValue,
        NoNota: NoNotaValue
    };
    $.ajax({
        type: "POST",
        url: "/EntriPenyelesaianPiutang/Save",
        contentType: "application/json",
        data: JSON.stringify(data),
        success: function (response) {
            if (response.success) {
                $("#DetailPembayaranGrid").data("kendoGrid").dataSource.read();
                
                showSwal('success', 'Data berhasil disimpan!');
                // Mengosongkan form input setelah berhasil
                $("#KodeAkun").val("");
                $("#TotalBayarOrg").data("kendoNumericTextBox").value(null);
                $("#TotalBayarRp").data("kendoNumericTextBox").value(null);
                $("#NoNota").data("kendoTextBox").value("");
                 $("#DebetKredit").data("kendoDropDownList").value(null);
                $("#KodeMataUang").data("kendoComboBox").value("");
                $("#btn-cancel-edit").hide();
                $("#FlagPembayaran").data("kendoDropDownList").readonly(false);
                
            }else {
                showSwal('error', 'Gagal menyimpan data.');
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
    $("#TotalBayarOrg").data("kendoNumericTextBox").value(dataItem.TotalBayarOrg);
    $("#TotalBayarRp").data("kendoNumericTextBox").value(dataItem.TotalBayarRp);
    $("#DebetKredit").data("kendoDropDownList").value(dataItem.DebetKredit);
    $("#NoBukti").val(dataItem.NoBukti);
    
    $("#FlagPembayaran").data("kendoDropDownList").value(dataItem.FlagPembayaran);
    if (dataItem.FlagPembayaran && dataItem.FlagPembayaran.toUpperCase() === "AKUN") {
        $("#akunField").show();
        $("#notaField").hide();
        $("#KodeAkun").data("kendoTextBox").value(dataItem.KodeAkun);
        
    } else if (dataItem.FlagPembayaran && dataItem.FlagPembayaran.toUpperCase() === "NOTA") {
        $("#notaField").show();
        $("#akunField").hide();
        $("#NoNota").data("kendoTextBox").value(dataItem.NoNota);
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
            data: JSON.stringify({ NomorBukti: dataItem.NomorBukti, No: dataItem.No }),
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
    $("#KodeAkun").val("");
    
    // Reset ID
    if ($("#No").length > 0) {
        $("#No").val(0);
    }

    // Aktifkan kembali field
    if (flagPembayaran) flagPembayaran.readonly(false);
    
    // Sembunyikan tombol Cancel
    $("#btn-cancel-edit").hide();
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
        if (item.TotalBayarOrg) {
            // --- INI LOGIKA BARUNYA ---
            // Cek status Debet atau Kredit
            if (item.DebetKredit && item.DebetKredit.toUpperCase() === 'K') {
                totalPembayaran -= item.TotalBayarOrg; // Jika Kredit, kurangi
            } else {
                totalPembayaran += item.TotalBayarOrg; // Jika Debit (atau kosong), tambahkan
            }
            // -------------------------
        }
    }

    // Update teks di footer dengan format angka
    $("#pembayaranTotal").text(kendo.toString(totalPembayaran, "n0"));
}