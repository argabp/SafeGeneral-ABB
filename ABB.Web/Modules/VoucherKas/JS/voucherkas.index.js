// Fungsi untuk membuka window Tambah Data
function btnAddVoucherKas_OnClick() {
    var window = $("#VoucherKasWindow").data("kendoWindow");
    window.one("refresh", attachChangeEvents); 
    openWindow('#VoucherKasWindow', '/VoucherKas/Add', 'Add New Voucher Kas');
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

function onSaveVoucherKas() {
    var comboCabang = $("#KodeCabang").data("kendoComboBox");
     var kodeCabangValue = comboCabang
    ? comboCabang.value().trim()
    : ($("#KodeCabang").val()?.split("-")[0].trim() || "");
    var url = "/VoucherKas/Save";
    var data = {
        KodeCabang: kodeCabangValue,
        JenisVoucher: $("#JenisVoucher").val(),
        DebetKredit: $("#DebetKredit").data("kendoDropDownList").value(), // Cara aman ambil value dropdown
        NoVoucher: $("#NoVoucher").val(),
        KodeAkun: $("#KodeAkun").val(),
        DibayarKepada: $("#DibayarKepada").val(),
        TanggalVoucher: $("#TanggalVoucher").data("kendoDatePicker").value(),
        TotalVoucher: $("#TotalVoucher").data("kendoNumericTextBox").value(),
        KodeMataUang: $("#KodeMataUang").val(),
        KeteranganVoucher: $("#KeteranganVoucher").val(),
        TotalDalamRupiah: $("#TotalDalamRupiah").data("kendoNumericTextBox").value(),
        JenisPembayaran: $("#JenisPembayaran").data("kendoDropDownList").value(),
        FlagPosting: $("#FlagPosting").val()
    };
    
    showProgress('#VoucherKasWindow');

    $.ajax({
        type: "POST",
        url: url,
        // ---> DUA BARIS INI SANGAT PENTING <---
        contentType: "application/json; charset=utf-8", // 1. Beritahu server kita mengirim JSON
        data: JSON.stringify(data),                     // 2. Ubah objek JavaScript menjadi string JSON
        
        success: function (response) {
            closeProgress('#VoucherKasWindow');
            if (response && response.success) {
                console.log(response)
                showMessage('Success', 'Data berhasil disimpan.');
                closeWindow("#VoucherKasWindow");
                refreshGrid("#VoucherKasGrid");
            } else {
                showMessage('Error', 'Gagal menyimpan data.');
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            closeProgress('#VoucherKasWindow');
            if (jqXHR.status === 400) {
                // Menangani error validasi jika ada
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

// Fungsi untuk membuka window Edit Data
function btnEditVoucherKas_OnClick(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));

    var window = $("#VoucherKasWindow").data("kendoWindow");
    // MEMASANG "PENDENGAR": Jalankan 'attachChangeEvents' setelah konten window selesai di-refresh
    window.one("refresh", attachChangeEvents); 
    // Parameter 'id' harus cocok dengan yang ada di Controller (public IActionResult Edit(string id))
    openWindow('#VoucherKasWindow', `/VoucherKas/Edit?id=${dataItem.NoVoucher.trim()}`, 'Edit Voucher Kas');
}

// Fungsi untuk menghapus data
function onDeleteVoucherKas(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));

    // Tampilkan dialog konfirmasi
    showConfirmation('Konfirmasi Hapus', `Apakah Anda yakin ingin menghapus data dengan No. Voucher ${dataItem.NoVoucher}?`,
        function () {
            showProgressOnGrid('#VoucherKasGrid');

            // Kirim request hapus ke Controller
            ajaxGet(`/VoucherKas/Delete?id=${dataItem.NoVoucher.trim()}`,
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
        var mataUangCombo = $("#KodeMataUang").data("kendoComboBox");
        var totalVoucherInput = $("#TotalVoucher").data("kendoNumericTextBox");
        var tanggalVoucher = $("#TanggalVoucher").data("kendoDatePicker");
        var debetKredit = $("#DebetKredit").data("kendoDropDownList");
        var kodeCabang = $("#KodeCabang").data("kendoComboBox");

        $("#JenisVoucher").on("change", generateNoVoucher);

        if (debetKredit) {
            debetKredit.bind("change", generateNoVoucher);
        }
        if (kodeCabang) {
            kodeCabang.bind("change", generateNoVoucher);
        }
        // if (kodeBank) {
        //     kodeBank.bind("change", generateNoVoucher);
        // }

        // Pemicu untuk kalkulator kurs (tidak berubah)
        if (mataUangCombo) {
            mataUangCombo.bind("change", hitungTotalRupiah);
        }
        if (totalVoucherInput) {
            totalVoucherInput.bind("change", hitungTotalRupiah);
        }
        if (tanggalVoucher) {
            // TanggalVoucher sekarang HANYA memicu kalkulator kurs
            tanggalVoucher.bind("change", hitungTotalRupiah);
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

    // logika untuk generate voucher
   function generateNoVoucher() {
    var jenisVoucher = $("#JenisVoucher").val() || "";
    var debetKredit = $("#DebetKredit").data("kendoDropDownList")?.value() || "";
    var kodeCabangText = $("#KodeCabang").data("kendoComboBox")?.input.val() || "";
    var kodeCabang = kodeCabangText.split(" - ")[0].trim(); // ambil hanya kode
    var kodeKas = $("#KodeKas").val() || "";
    console.log("Kode Cabang:", kodeCabang);
    $.ajax({
        type: "GET",
        url: "/VoucherKas/GetNextVoucherNumber",
        success: function (response) {
            if (response && response.success) {
                var noUrut = response.nextNumber;
                var tanggalHariIni = new Date();

               var bagian1 = kodeCabang.slice(-2);
                var bagian3 = "";

                if (jenisVoucher.toUpperCase() === "KAS") bagian3 = "K";
                if (debetKredit.toUpperCase() === "D") bagian3 += "D";
                else if (debetKredit.toUpperCase() === "K") bagian3 += "K";

                var bulan = ('0' + (tanggalHariIni.getMonth() + 1)).slice(-2);
                var tahun = tanggalHariIni.getFullYear().toString().slice(-2);
                var bagian5 = bulan + "/" + tahun;

                if (noUrut && bagian3 && kodeKas && bagian5) {
                    var noVoucherFinal = `${bagian1}/${noUrut}/${bagian3}/${kodeKas}/${bagian5}`;
                    $("#NoVoucher").val(noVoucherFinal);
                }
            }
        }
    });
}

