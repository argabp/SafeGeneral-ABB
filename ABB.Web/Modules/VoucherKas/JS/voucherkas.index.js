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
    var errors = [];
    var debetKredit = $("#DebetKredit").data("kendoDropDownList").value();
    var kodeKas = $("#KodeKas").data("kendoComboBox").value();
    var tanggalVoucher = $("#TanggalVoucher").data("kendoDatePicker").value();
    var totalVoucher = $("#TotalVoucher").data("kendoNumericTextBox").value();
    var kodeMataUang = $("#KodeMataUang").data("kendoComboBox").value();
    var jenisPembayaran = $("#JenisPembayaran").data("kendoDropDownList").value();
    
    if (!debetKredit) errors.push("- Debet/Kredit harus dipilih");
    if (!kodeKas) errors.push("- Kode Kas harus dipilih");
    if (!tanggalVoucher) errors.push("- Tanggal Voucher harus diisi");
    if (!kodeMataUang) errors.push("- Kode Mata Uang harus dipilih");
    if (totalVoucher === null || totalVoucher <= 0) errors.push("- Total Voucher harus lebih dari 0");
    if (!jenisPembayaran) errors.push("- Jenis Pembayaran harus dipilih");

    // Jika ada error, tampilkan alert dan BERHENTI (return)
    if (errors.length > 0) {
        alert("Mohon lengkapi data berikut:\n" + errors.join("\n"));
        return; // <--- STOP DISINI, jangan lanjut AJAX
    }

    var comboCabang = $("#KodeCabang").data("kendoComboBox");
     var kodeCabangValue = comboCabang
    ? comboCabang.value().trim()
    : ($("#KodeCabang").val()?.split("-")[0].trim() || "");
    var url = "/VoucherKas/Save";

    

    var data = {
        KodeCabang: kodeCabangValue,
        JenisVoucher: $("#JenisVoucher").val(),
        DebetKredit: $("#DebetKredit").data("kendoDropDownList").value(), // Cara aman ambil value dropdown
        KodeKas: $("#KodeKas").val(), // Cara aman ambil value dropdown
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
        // --- TAMBAHAN BARU ---
        var kodeKasCombo = $("#KodeKas").data("kendoComboBox");
        // ---------------------
        
        $("#JenisVoucher").on("change", generateNoVoucher);

        if (debetKredit) {
            debetKredit.bind("change", generateNoVoucher);
            
            // Pindahkan logic label 'Dibayar Kepada' kesini juga biar rapi
            debetKredit.bind("change", function(e) {
                var val = this.value();
                if (val && val.toUpperCase() === "D") {
                    $("#labelDibayarKepada").text("Diterima Dari");
                } else {
                    $("#labelDibayarKepada").text("Dibayar Kepada");
                }
            });
        }
        if (kodeCabang) {
            kodeCabang.bind("change", generateNoVoucher);
        }

        // --- BINDING KODE KAS ---
        if (kodeKasCombo) {
            // 1. Panggil Generate No Voucher saat Kas berubah
            kodeKasCombo.bind("change", generateNoVoucher);
            
            // 2. Panggil Logic Get Akun Otomatis
            kodeKasCombo.bind("change", onKodeKasChangeInternal);
        }
        // ------------------------

        
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
                // UPDATED: Saat tanggal berubah, hitung Rupiah DAN Generate Nomor Voucher baru
                tanggalVoucher.bind("change", function() {
                    hitungTotalRupiah();
                    generateNoVoucher(); // <--- TAMBAHAN PENTING
                });
            }
    }
    function onKodeKasChangeInternal(e) {
        var kodeKas = this.value(); 
        console.log("Kode Kas Changed (Internal):", kodeKas);

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

        $.ajax({
            type: "GET",
            // 2. KIRIM TANGGAL KE CONTROLLER
            url: "/VoucherKas/GetNextVoucherNumber?tanggalVoucher=" + formattedDateForAPI,
            success: function (response) {
                if (response && response.success) {
                    var noUrut = response.nextNumber;

                    // 3. GUNAKAN TANGGAL SELECTED UNTUK BAGIAN BULAN/TAHUN
                    var bagian1 = kodeCabang.slice(-2);
                    var bagian3 = "";

                    if (jenisVoucher.toUpperCase() === "KAS") bagian3 = "K";
                    if (debetKredit.toUpperCase() === "D") bagian3 += "D";
                    else if (debetKredit.toUpperCase() === "K") bagian3 += "K";

                    // Ubah logic ini agar pake tanggalSelected
                    var bulan = ('0' + (tanggalSelected.getMonth() + 1)).slice(-2);
                    var tahun = tanggalSelected.getFullYear().toString().slice(-2);
                    var bagian5 = bulan + "/" + tahun;

                    if (noUrut && bagian3 && kodeKas && bagian5) {
                        var noVoucherFinal = `${bagian1}/${noUrut}/${bagian3}/${kodeKas}/${bagian5}`;
                        $("#NoVoucher").val(noVoucherFinal);
                    }
                }
            }
        });
    }
