function btnAddJurnalMamorial104_OnClick() {
    var window = $("#JurnalMemorial104Window").data("kendoWindow");
     window.one("refresh", function() {
        attachDetailEvents(); // Pasang event listener
        clearDetailForm();
    });
    
    window.refresh({
        url: "/JurnalMemorial104/Add"
    });
    window.title("Entri Jurnal Memorial 104");
    window.center().open();
}

function btnLihatJurnalMemorial104_OnClick(e) {
    e.preventDefault();
    // Ambil data dari grid (cara aman via onclick event)
    // Ingat: karena pakai client template onclick='...', 'this' bukanlah grid row
    // Gunakan parameter 'e' (event)
    
    // Cari row terdekat dari tombol yang diklik
    var row = $(e.target).closest("tr");
    // Ambil grid object
    var grid = $("#JurnalMemorial104Grid").data("kendoGrid");
    // Ambil dataItem
    var dataItem = grid.dataItem(row);

    var window = $("#JurnalMemorial104Window").data("kendoWindow");

    // Kita tidak perlu toggleFormState lagi!
    // Langsung refresh ke URL yang merender View Lihat.cshtml
    window.refresh({
        url: `/JurnalMemorial104/Lihat?kodeCabang=${dataItem.KodeCabang}&noVoucher=${dataItem.NoVoucher}`
    });

    window.title("Lihat Jurnal Memorial 104");
    window.center().open();
}

function onSaveHeader() {
    // 1. Ambil Data Header
    var headerData = {
        KodeCabang: $("#JurnalHeader_KodeCabang").data("kendoComboBox").value(),
        NoVoucher: $("#JurnalHeader_NoVoucher").val(),
        Tanggal: $("#JurnalHeader_Tanggal").data("kendoDatePicker").value(),
        Keterangan: $("#JurnalHeader_Keterangan").val(),
        // FlagPosting default false di controller
    };

    // 2. Validasi Sederhana
    if (!headerData.KodeCabang || !headerData.Tanggal) {
        Swal.fire('Warning', 'Kode Cabang dan Tanggal harus diisi.', 'warning');
        return;
    }

    // 3. AJAX Save
    $.ajax({
        type: "POST",
        url: "/JurnalMemorial104/SaveHeader",
        contentType: "application/json",
        data: JSON.stringify(headerData),
        success: function (response) {
            if (response.success) {
               showMessage('Success', 'Header berhasil disimpan!');
                
                // Update No Voucher di Form (jika Create Baru)
                $("#JurnalHeader_NoVoucher").val(response.noVoucher);
                
                // Tampilkan Form Detail
                $("#JurnalDetailForm").fadeIn();
                
                // Refresh Grid Utama
                $("#JurnalMemorial104Grid").data("kendoGrid").dataSource.read();
            } else {
               showMessage('Error', 'Gagal menyimpan header.');
            }
        },
        error: function(err) {
           showMessage('Error', 'Terjadi kesalahan server.');
        }
    });
}



function onSaveDetail() {
    var noVoucher = $("#JurnalHeader_NoVoucher").val();
    if(!noVoucher) {
        Swal.fire('Error', 'Simpan Header terlebih dahulu.', 'error');
        return;
    }

    var noUrut = $("#DetailNo").val(); // 0 = New, >0 = Edit
    
    var detailData = {
        NoVoucher: noVoucher,
            No: parseInt($("#DetailNo").val()),
            KodeAkun: $("#KodeAkun").data("kendoComboBox").value(),
            NoNota: $("#NoNota").val(),
            KodeMataUang: $("#KodeMataUang").data("kendoComboBox").value(),
            NilaiDebet: $("#NilaiDebet").data("kendoNumericTextBox").value() || 0,
            NilaiKredit: $("#NilaiKredit").data("kendoNumericTextBox").value() || 0,
            
            // --- TAMBAHKAN INI ---
            NilaiDebetRp: $("#NilaiDebetRp").data("kendoNumericTextBox").value() || 0,
            NilaiKreditRp: $("#NilaiKreditRp").data("kendoNumericTextBox").value() || 0,
            KeteranganDetail: $("#KeteranganDetail").val() || ""
            // ---------------------
    };
    console.log(detailData);

    // Validasi
    if(!detailData.KodeAkun || !detailData.KodeMataUang) {
        Swal.fire('Warning', 'Kode Akun dan Mata Uang harus diisi.', 'warning');
        return;
    }
    
    if(detailData.NilaiDebet === 0 && detailData.NilaiKredit === 0) {
        Swal.fire('Warning', 'Isi Nilai Debet atau Kredit.', 'warning');
        return;
    }

    $.ajax({
        type: "POST",
        url: "/JurnalMemorial104/SaveDetail",
        contentType: "application/json",
        data: JSON.stringify(detailData),
        success: function (response) {
            if (response.success) {
                // Swal.fire('Success', 'Detail berhasil disimpan.', 'success'); // Optional, biar cepat inputnya
                
                // Refresh Grid Detail
                $("#DetailJurnalGrid").data("kendoGrid").dataSource.read();
                
                // Reset Form Detail
                clearDetailForm();
                
            } else {
                Swal.fire('Error', 'Gagal menyimpan detail.', 'error');
            }
        }
    });
}

function updateFooterTotals() {
    var grid = $("#DetailJurnalGrid").data("kendoGrid");
    if (!grid) return;

    var dataSource = grid.dataSource;
    var data = dataSource.data(); // Mengambil seluruh baris data di memori
    
    var totalDebet = 0;
    var totalKredit = 0;

    // Hitung manual dari seluruh data (lintas halaman)
    for (var i = 0; i < data.length; i++) {
        totalDebet += data[i].NilaiDebetRp || 0;
        totalKredit += data[i].NilaiKreditRp || 0;
    }

    var balance = totalDebet - totalKredit;

    // Update Label dengan format ribuan yang cantik
    $("#lblTotalDebet").text(kendo.toString(totalDebet, "n2"));
    $("#lblTotalKredit").text(kendo.toString(totalKredit, "n2"));

    var lblBalance = $("#lblBalance");
    lblBalance.text(kendo.toString(balance, "n2"));

    // Toleransi pembulatan standar akuntansi (0.01)
    if (Math.abs(balance) < 0.01) {
        lblBalance.css("color", "green").text("0.00 (Balance)");
    } else {
        lblBalance.css("color", "red");
        // Opsional: Jika selisih besar, kasih warning ke user
        console.warn("Jurnal tidak balance sebesar: " + balance);
    }
}

function btnEditJurnalMemorial104_OnClick(e) {
    e.preventDefault();
    var row = $(e.target).closest("tr");
    var grid = $("#JurnalMemorial104Grid").data("kendoGrid");
    var dataItem = grid.dataItem(row);

    var window = $("#JurnalMemorial104Window").data("kendoWindow");
    
    // Pasang listener refresh agar form detail berfungsi
    window.one("refresh", function() {
        attachDetailEvents(); 
    });

    window.refresh({
        url: `/JurnalMemorial104/Add?kodeCabang=${dataItem.KodeCabang}&noVoucher=${dataItem.NoVoucher}`
    });
    window.title("Edit Jurnal Memorial 104");
    window.center().open();
}

function attachDetailEvents() {
    // Ambil komponen Kendo
    var mataUangCombo = $("#KodeMataUang").data("kendoComboBox");
    var debetInput = $("#NilaiDebet").data("kendoNumericTextBox");
    var kreditInput = $("#NilaiKredit").data("kendoNumericTextBox");
    var tanggalHeader = $("#JurnalHeader_Tanggal").data("kendoDatePicker");
    var cabangCombo = $("#JurnalHeader_KodeCabang").data("kendoComboBox");

    if (tanggalHeader) {
        tanggalHeader.bind("change", function() {
            hitungKursDetail(); // Existing
            generateNomorBukti(); // <-- TAMBAHAN BARU
        });
    }
    
    if (cabangCombo) {
        cabangCombo.bind("change", generateNomorBukti);
    }
    // Pasang Event Listener 'change'
    // Setiap kali user ubah Mata Uang, Angka, atau Tanggal -> Hitung ulang
    if (mataUangCombo) mataUangCombo.bind("change", hitungKursDetail);
    if (debetInput) debetInput.bind("change", hitungKursDetail);
    if (kreditInput) kreditInput.bind("change", hitungKursDetail);
    
    // Opsional: Jika tanggal header berubah, hitung ulang detail yang sedang diinput
    
}

function hitungKursDetail() {
    // 1. Ambil Nilai-nilai Form
    var kodeMtu = $("#KodeMataUang").data("kendoComboBox").value();
    var tanggal = $("#JurnalHeader_Tanggal").data("kendoDatePicker").value();
    var debetOrg = $("#NilaiDebet").data("kendoNumericTextBox").value() || 0;
    var kreditOrg = $("#NilaiKredit").data("kendoNumericTextBox").value() || 0;

    // Reset field Rp dulu biar bersih
    var debetRpInput = $("#NilaiDebetRp").data("kendoNumericTextBox");
    var kreditRpInput = $("#NilaiKreditRp").data("kendoNumericTextBox");

    // Validasi Dasar
    if (!kodeMtu || !tanggal) return;

    // 2. LOGIKA IDR (Rupiah)
    // Jika mata uang '001' (Rupiah), maka Nilai Rp = Nilai Original
    if (kodeMtu === '001') {
        debetRpInput.value(debetOrg);
        kreditRpInput.value(kreditOrg);
        return; 
    }

    // 3. LOGIKA ASING (USD, SGD, dll) -> Panggil AJAX ke Server
    var formattedDate = kendo.toString(tanggal, "yyyy-MM-dd");
    var url = `/JurnalMemorial104/GetKurs?kodeMataUang=${kodeMtu}&tanggalVoucher=${formattedDate}`;

    $.ajax({
        type: "GET",
        url: url,
        success: function (response) {
            // Asumsi response dari controller: { nilai_kurs: 15000 }
            if (response && response.nilai_kurs) {
                var kurs = response.nilai_kurs;
                
                // Hitung Kali Kurs
                var totalDebetRp = debetOrg * kurs;
                var totalKreditRp = kreditOrg * kurs;

                debetRpInput.value(totalDebetRp);
                kreditRpInput.value(totalKreditRp);
            } else {
                // Kurs tidak ketemu
                // Opsional: Bisa kasih alert atau biarkan 0
                console.warn("Kurs tidak ditemukan untuk mata uang ini pada tanggal tersebut.");
            }
        },
        error: function() {
            console.error("Gagal mengambil data kurs.");
        }
    });
}

function clearDetailForm() {
    $("#DetailNo").val(0);
    $("#KodeAkun").data("kendoComboBox").value("");
    $("#NoNota").val("");
    $("#KodeMataUang").data("kendoComboBox").value(""); // Atau set default IDR
    $("#NilaiDebet").data("kendoNumericTextBox").value(0);
    $("#NilaiKredit").data("kendoNumericTextBox").value(0);

    $("#NilaiDebetRp").data("kendoNumericTextBox").value(0);
    $("#NilaiKreditRp").data("kendoNumericTextBox").value(0);
    $("#KeteranganDetail").val("");
    
    $("#btn-save-detail").html('<i class="fa fa-plus"></i> Tambah Jurnal');
    $("#btn-cancel-detail").hide();
}

function onEditDetail(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    
    // Isi Form dengan Data Grid
    $("#DetailNo").val(dataItem.No);
    $("#KodeAkun").data("kendoComboBox").value(dataItem.KodeAkun);
    $("#NoNota").val(dataItem.NoNota);
    $("#KodeMataUang").data("kendoComboBox").value(dataItem.KodeMataUang);
    
    $("#NilaiDebet").data("kendoNumericTextBox").value(dataItem.NilaiDebet);
    $("#NilaiKredit").data("kendoNumericTextBox").value(dataItem.NilaiKredit);

    // --- TAMBAHAN: Isi field Rp juga ---
    $("#NilaiDebetRp").data("kendoNumericTextBox").value(dataItem.NilaiDebetRp);
    $("#NilaiKreditRp").data("kendoNumericTextBox").value(dataItem.NilaiKreditRp);
    $("#KeteranganDetail").val(dataItem.KeteranganDetail);
    
    // Pastikan event listener aktif (jaga-jaga)
    attachDetailEvents(); 

    // Ubah tombol jadi mode edit
    $("#btn-save-detail").html('<i class="fa fa-save"></i> Update Jurnal');
    $("#btn-cancel-detail").show();
}

function onDeleteDetail(e) {
    e.preventDefault();

    var grid = $("#DetailJurnalGrid").data("kendoGrid");
    var dataItem = grid.dataItem($(e.currentTarget).closest("tr"));

    showConfirmation(
        'Confirmation',
        'Are you sure you want to delete?',
        function () {

            var data = {
                NoVoucher: dataItem.NoVoucher,
                No: dataItem.No
            };

            ajaxPost(
                "/JurnalMemorial104/DeleteDetail",
                JSON.stringify(data),
                function (response) {

                    if (response.success === true || response.Result === "OK") {

                        // ✅ FIX undefined
                        showMessage('Success', 'Data berhasil dihapus');

                        // ✅ REFRESH GRID YANG BENAR
                        grid.dataSource.read();
                        grid.refresh();

                    } else {
                        showMessage(
                            'Error',
                            response.message || response.Message || 'Gagal menghapus data'
                        );
                    }
                }
            );
        }
    );
}

function onDeleteHeaderJurnalMemorial104_Click(e) {
    e.preventDefault();

    var grid = $("#JurnalMemorial104Grid").data("kendoGrid");
    var windowKendo = $("#JurnalMemorial104Window").data("kendoWindow");

    // var dataItem = this.dataItem($(e.currentTarget).closest("tr"));

    var row = $(e.currentTarget).closest("tr");
    var dataItem = grid.dataItem(row);

    showConfirmation(
        'Confirmation',
        'Are you sure you want to delete?',
        function () {

            var data = {
                KodeCabang: dataItem.KodeCabang,
                NoVoucher: dataItem.NoVoucher
            };

            ajaxPost(
                "/JurnalMemorial104/DeleteHeader",
                JSON.stringify(data),
                function (response) {

                    if (response.success === true || response.Result === "OK") {

                        showMessage('Success', 'Data berhasil dihapus');

                        // ✅ Refresh GRID utama
                        if (grid) {
                            grid.dataSource.read();
                            grid.refresh();
                        }

                        // ✅ Tutup window
                        if (windowKendo) {
                            windowKendo.close();
                        }

                    } else {
                        showMessage(
                            'Error',
                            response.message || response.Message || 'Gagal menghapus data'
                        );
                    }
                }
            );
        }
    );
}



// --- FIX UNTUK 104 (Ganti versi aggregates ke versi Looping) ---
function updateFooterTotalsLihat() {
    var grid = $("#DetailJurnalGrid_Lihat").data("kendoGrid");
    if (!grid) return;

    var data = grid.dataSource.data(); // Ambil semua data di memori
    var totalDebet = 0;
    var totalKredit = 0;

    for (var i = 0; i < data.length; i++) {
        totalDebet += data[i].NilaiDebetRp || 0;
        totalKredit += data[i].NilaiKreditRp || 0;
    }

    var balance = totalDebet - totalKredit;

    $("#lblTotalDebet_Lihat").text(kendo.toString(totalDebet, "n2"));
    $("#lblTotalKredit_Lihat").text(kendo.toString(totalKredit, "n2"));

    var lblBalance = $("#lblBalance_Lihat");
    lblBalance.text(kendo.toString(balance, "n2"));

    if (Math.abs(balance) < 0.01) {
        lblBalance.css("color", "green").text("0.00 (Balance)");
    } else {
        lblBalance.css("color", "red");
    }
}

function generateNomorBukti() {
    // 1. Ambil inputan user sekarang
    var kodeCabangInput = $("#JurnalHeader_KodeCabang").data("kendoComboBox")?.value();
    var tanggalPicker = $("#JurnalHeader_Tanggal").data("kendoDatePicker");
    var tanggalSelected = tanggalPicker ? tanggalPicker.value() : null;

    if (!tanggalSelected || !kodeCabangInput) return; 

    var bulanBaru = tanggalSelected.getMonth() + 1;
    var tahunBaru = tanggalSelected.getFullYear();

    // 2. LOGIKA SAKTI EDIT MODE
    if (originalVoucher.isEdit) {
        // Cek: Apakah Cabang, Bulan, dan Tahun masih sama dengan data awal?
        if (kodeCabangInput === originalVoucher.kodeCabang && 
            bulanBaru === originalVoucher.bulan && 
            tahunBaru === originalVoucher.tahun) {
            
            // Kalau semua sama, balikin ke No Voucher aslinya (...001)
            $("#JurnalHeader_NoVoucher").val(originalVoucher.noVoucher);
            console.log("Edit Mode: Data periode sama, No Voucher tetap: " + originalVoucher.noVoucher);
            return; // STOP! Jangan lanjut ke AJAX
        }
    }

    // 3. Kalau Add Mode ATAU user beneran pindah bulan/cabang, baru cari nomor baru
    $.ajax({
        type: "GET",
        url: `/JurnalMemorial104/GetNextNoVoucher?kodeCabang=${kodeCabangInput}&bulan=${bulanBaru}&tahun=${tahunBaru}`,
        success: function (response) {
            if (response && response.success) {
                $("#JurnalHeader_NoVoucher").val(response.noVoucher);
            }
        },
        error: function() {
            console.error("Gagal mendapatkan nomor urut baru.");
        }
    });
}



// Variabel sementara untuk menyimpan data baris yang mau di-copy
var tempCopyData = {};

function btnCopyJurnal_OnClick(e) {
    e.preventDefault();
    
    // Ambil data baris yang diklik
    var grid = $("#JurnalMemorial104Grid").data("kendoGrid");
    var row = $(e.target).closest("tr");
    var dataItem = grid.dataItem(row);

    // Simpan ke memori sementara
    tempCopyData = {
        KodeCabang: dataItem.KodeCabang,
        NoVoucherLama: dataItem.NoVoucher
    };

    // Set default DatePicker ke hari ini
    var datePicker = $("#CopyTanggalBaru").data("kendoDatePicker");
    if(datePicker) datePicker.value(new Date());

    // Buka Popup Kendo Window
    var win = $("#CopyJurnalWindow").data("kendoWindow");
    win.center().open();
}

function closeCopyWindow() {
    $("#CopyJurnalWindow").data("kendoWindow").close();
}

function prosesCopyJurnal() {
    var datePicker = $("#CopyTanggalBaru").data("kendoDatePicker");
    var newDate = datePicker.value();

    if (!newDate) {
        showMessage('Warning', 'Tanggal baru harus diisi.');
        return;
    }

    // Format tanggal untuk dikirim ke C#
    var tglBaru = kendo.toString(newDate, "yyyy-MM-dd");

    var payload = {
        KodeCabang: tempCopyData.KodeCabang,
        NoVoucherLama: tempCopyData.NoVoucherLama,
        TanggalBaru: tglBaru
    };

    // Tutup window sambil nunggu proses
    closeCopyWindow();

    // Tembak AJAX ke Controller
    $.ajax({
        type: "POST",
        url: "/JurnalMemorial104/CopyJurnal",
        contentType: "application/json",
        data: JSON.stringify(payload),
        success: function (response) {
            if (response.success) {
                // Pake showMessage buat sukses
                showMessage('Success', 'Jurnal berhasil di-copy! No Voucher Baru: ' + response.noVoucherBaru);
                
                // Refresh Grid
                $("#JurnalMemorial104Grid").data("kendoGrid").dataSource.read();
            } else {
                // Pake showMessage buat error dari controller
                showMessage('Error', response.message || 'Gagal melakukan copy jurnal.');
            }
        },
        error: function () {
            // Pake showMessage buat error koneksi/server
            showMessage('Error', 'Terjadi kesalahan pada server saat mengcopy jurnal.');
        }
    });
}