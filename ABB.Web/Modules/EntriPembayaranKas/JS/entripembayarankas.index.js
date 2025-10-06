function btnEntriPembayaranKas_OnClick(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    var noVoucher = dataItem.NoVoucher;

    var window = $("#EntriPembayaranKasWindow").data("kendoWindow");

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

    openWindow(
        '#EntriPembayaranKasWindow',
        `/EntriPembayaranKas/Add?noVoucher=${noVoucher}`,
        'Entri Pembayaran Kas'
    );
}

$(document).ready(function () {
    $("#SearchKeyword").on("keyup", function () {
        $("#EntriPembayaranKasGrid").data("kendoGrid").dataSource.read();
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
        TotalDlmRupiah: $("#TotalDlmRupiah").val()
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

                // reset form setelah simpan
                form.find("#No").remove(); // buang id detail biar balik mode tambah baru
                $("#FlagPembayaran").data("kendoDropDownList").value(null);
                $("#NoNota4").data("kendoTextBox").value("");
                $("#KodeAkun").data("kendoTextBox").value("");
                $("#TotalBayar").data("kendoNumericTextBox").value(null);
                $("#DebetKredit").data("kendoDropDownList").value(null);
                $("#KodeMataUang").data("kendoComboBox").value("");
                $("#TotalDlmRupiah").data("kendoNumericTextBox").value(null);
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
        $("#KodeAkun").data("kendoTextBox").value(dataItem.KodeAkun);
    } else if (dataItem.FlagPembayaran && dataItem.FlagPembayaran.toUpperCase() === "NOTA") {
        $("#notaField").show();
        $("#akunField").hide();
        $("#NoNota4").data("kendoTextBox").value(dataItem.NoNota4);
    }

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
}
function openPilihNotaWindow() {
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




