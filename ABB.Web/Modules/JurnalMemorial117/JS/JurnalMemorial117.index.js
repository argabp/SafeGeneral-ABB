function btnAddJurnal_OnClick() {
    var window = $("#JurnalMemorialWindow").data("kendoWindow");
    
    // --- TAMBAHKAN INI (Event Refresh) ---
    window.one("refresh", function() {
        attachDetailEvents(); // Pasang event listener
        clearDetailForm();
    });
    // -------------------------------------

    window.refresh({
        url: "/JurnalMemorial117/Add"
    });
     window.title("Add Jurnal Memorial 117");
    window.center().open();
}

function btnLihatJurnal_OnClick(e) {
    e.preventDefault();
    // Ambil data dari grid row
    var row = $(e.target).closest("tr");
    var grid = $("#JurnalMemorialGrid").data("kendoGrid");
    var dataItem = grid.dataItem(row);

    var window = $("#JurnalMemorialWindow").data("kendoWindow");

    // Refresh window dengan URL Action 'Lihat'
    window.refresh({
        url: `/JurnalMemorial117/Lihat?kodeCabang=${dataItem.KodeCabang}&noVoucher=${dataItem.NoVoucher}`
    });

    window.title("Lihat Jurnal Memorial 117");
    window.center().open();
}

// --- FUNGSI TOTAL FOOTER (KHUSUS LIHAT) ---
// Ditaruh disini agar global dan tidak error "undefined"
function updateFooterTotalsLihat() {
    var grid = $("#DetailJurnalGrid_Lihat").data("kendoGrid");
    if (!grid) return;

    var data = grid.dataSource.data();
    
    var totalDebet = 0;
    var totalKredit = 0;

    for(var i=0; i<data.length; i++) {
        totalDebet += (data[i].NilaiDebet || 0);
        totalKredit += (data[i].NilaiKredit || 0);
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

function btnEditJurnal_OnClick(e) {
    e.preventDefault();

    // ❌ SALAH (Kode Lama): 'this' bukan grid saat pakai onclick manual
    // var dataItem = this.dataItem($(e.currentTarget).closest("tr"));

    // ✅ BENAR (Perbaikan): Ambil grid secara manual
    var grid = $("#JurnalMemorialGrid").data("kendoGrid"); // Pastikan ID Grid benar
    var row = $(e.target).closest("tr");
    var dataItem = grid.dataItem(row);

    if (!dataItem) {
        console.error("Data item tidak ditemukan");
        return;
    }

    var window = $("#JurnalMemorialWindow").data("kendoWindow");
    
    window.one("refresh", function() {
        attachDetailEvents(); 
        // Mode edit: Jangan clear detail form sepenuhnya, tapi biarkan loading dari controller
    });

    window.refresh({
        url: `/JurnalMemorial117/Add?kodeCabang=${dataItem.KodeCabang}&noVoucher=${dataItem.NoVoucher}`
    });
    window.title("Edit Jurnal Memorial 117");
    window.center().open();
}

// --- HEADER LOGIC ---

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
        showMessage('Warning', 'Kode Cabang dan Tanggal harus diisi.');
        return;
    }

    // 3. AJAX Save
    $.ajax({
        type: "POST",
        url: "/JurnalMemorial117/SaveHeader",
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
                $("#JurnalMemorialGrid").data("kendoGrid").dataSource.read();
            } else {
                showMessage('Error', 'Gagal menyimpan header.');
            }
        },
        error: function(err) {
            showMessage('Error', 'Terjadi kesalahan server.');
        }
    });
}

function onDeleteHeader_Click(e) {
    e.preventDefault();

    var grid = $("#JurnalMemorialGrid").data("kendoGrid");
    var windowKendo = $("#JurnalMemorialWindow").data("kendoWindow");

    // ❌ SALAH
    // var dataItem = this.dataItem($(e.currentTarget).closest("tr"));

    // ✅ BENAR
    var row = $(e.target).closest("tr");
    var dataItem = grid.dataItem(row);

    if (!dataItem) {
        console.error("Data item tidak ditemukan");
        return;
    }

    showConfirmation(
        'Confirmation',
        `Apakah Anda yakin ingin menghapus Jurnal ${dataItem.NoVoucher}?`,
        function () {
            var data = {
                KodeCabang: dataItem.KodeCabang,
                NoVoucher: dataItem.NoVoucher
            };

            // Gunakan $.ajax atau ajaxPost helper kamu
            $.ajax({
                type: "POST",
                url: "/JurnalMemorial117/DeleteHeader",
                contentType: "application/json",
                data: JSON.stringify(data),
                success: function (response) {
                    if (response.success === true || response.Result === "OK") {
                        showMessage('Success', 'Data berhasil dihapus');
                        
                        if (grid) {
                            grid.dataSource.read(); // Refresh Grid
                        }
                    } else {
                        showMessage('Error', response.message || 'Gagal menghapus data');
                    }
                },
                error: function() {
                    showMessage('Error', 'Terjadi kesalahan server saat menghapus.');
                }
            });
        }
    );
}

// --- DETAIL LOGIC ---

function onSaveDetail() {
    var noVoucher = $("#JurnalHeader_NoVoucher").val();
    if(!noVoucher) {
        showMessage('Error', 'Simpan Header terlebih dahulu.');
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
           Keterangan: $("#KeteranganDetail").data("kendoTextArea").value() || ""
            // ---------------------
        };

    // Validasi
    if(!detailData.KodeAkun || !detailData.KodeMataUang) {
        showMessage('Warning', 'Kode Akun dan Mata Uang harus diisi.');
        return;
    }
    
    if(detailData.NilaiDebet === 0 && detailData.NilaiKredit === 0) {
        showMessage('Warning', 'Isi Nilai Debet atau Kredit.');
        return;
    }

    $.ajax({
        type: "POST",
        url: "/JurnalMemorial117/SaveDetail",
        contentType: "application/json",
        data: JSON.stringify(detailData),
        success: function (response) {
            if (response.success) {
                // showMessage('Success', 'Detail berhasil disimpan.', 'success'); // Optional, biar cepat inputnya
                
                // Refresh Grid Detail
                $("#DetailJurnalGrid").data("kendoGrid").dataSource.read();
                
                // Reset Form Detail
                clearDetailForm();
                
            } else {
                showMessage('Error', 'Gagal menyimpan detail.');
            }
        }
    });
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
    $("#KeteranganDetail").data("kendoTextArea").value(dataItem.Keterangan);
    // --- TAMBAHAN: Isi field Rp juga ---
    $("#NilaiDebetRp").data("kendoNumericTextBox").value(dataItem.NilaiDebetRp);
    $("#NilaiKreditRp").data("kendoNumericTextBox").value(dataItem.NilaiKreditRp);
    
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
                "/JurnalMemorial117/DeleteDetail",
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

function clearDetailForm() {
    $("#DetailNo").val(0);
    $("#KodeAkun").data("kendoComboBox").value("");
    $("#NoNota").val("");
    $("#KodeMataUang").data("kendoComboBox").value(""); // Atau set default IDR
    $("#NilaiDebet").data("kendoNumericTextBox").value(0);
    $("#NilaiKredit").data("kendoNumericTextBox").value(0);
    $("#NilaiDebetRp").data("kendoNumericTextBox").value(0);
    $("#NilaiKreditRp").data("kendoNumericTextBox").value(0);
    $("#KeteranganDetail").data("kendoTextArea").value("");
    $("#btn-save-detail").html('<i class="fa fa-plus"></i> Tambah Jurnal');
    $("#btn-cancel-detail").hide();
    
}

// --- UTILITIES ---

function updateFooterTotals() {
    var grid = $("#DetailJurnalGrid").data("kendoGrid");
    var data = grid.dataSource.data();
    
    var totalDebet = 0;
    var totalKredit = 0;

    for(var i=0; i<data.length; i++) {
        totalDebet += (data[i].NilaiDebet || 0);
        totalKredit += (data[i].NilaiKredit || 0);
    }

    var balance = totalDebet - totalKredit;
    
    $("#lblTotalDebet").text(kendo.toString(totalDebet, "n2"));
    $("#lblTotalKredit").text(kendo.toString(totalKredit, "n2"));
    
    var lblBalance = $("#lblBalance");
    lblBalance.text(kendo.toString(balance, "n2"));

    if (Math.abs(balance) < 0.01) {
        lblBalance.css("color", "green").text("0.00 (Balance)");
    } else {
        lblBalance.css("color", "red");
    }
}


function attachDetailEvents() {
    // Ambil komponen Kendo
    var mataUangCombo = $("#KodeMataUang").data("kendoComboBox");
    var debetInput = $("#NilaiDebet").data("kendoNumericTextBox");
    var kreditInput = $("#NilaiKredit").data("kendoNumericTextBox");
    var tanggalHeader = $("#JurnalHeader_Tanggal").data("kendoDatePicker");
    var cabangCombo = $("#JurnalHeader_KodeCabang").data("kendoComboBox"); // Tambahkan ini
    if (tanggalHeader) {
        tanggalHeader.bind("change", function() {
            hitungKursDetail();   // Hitung kurs (logika lama)
            generateNomorBukti(); // Generate No Voucher (logika baru)
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
    var url = `/JurnalMemorial117/GetKurs?kodeMataUang=${kodeMtu}&tanggalVoucher=${formattedDate}`;

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

function generateNomorBukti() {
    var kodeCabangText = $("#JurnalHeader_KodeCabang").data("kendoComboBox")?.input.val() || "";
    var kodeCabang = kodeCabangText.split(" - ")[0].trim();
    
    // Ambil tanggal dari Kendo DatePicker
    var kendoDatePicker = $("#JurnalHeader_Tanggal").data("kendoDatePicker");
    var tanggalSelected = kendoDatePicker ? kendoDatePicker.value() : null;

    if (!tanggalSelected) {
        $("#JurnalHeader_NoVoucher").val(""); 
        return;
    }

    // Format tanggal (yyyy-MM-dd)
    var formattedDate = kendo.toString(tanggalSelected, "yyyy-MM-dd");

    // Pastikan Controller punya Action GetNextNoVoucherJurnal yang menerima parameter tanggal
    // Anda mungkin perlu menyesuaikan nama parameter di Controller (misal: bulan, tahun atau tanggalVoucher)
    // Asumsi Controller menerima: kodeCabang, bulan, tahun
    
    var bulan = tanggalSelected.getMonth() + 1;
    var tahun = tanggalSelected.getFullYear();

    if (kodeCabang) {
        $.ajax({
            type: "GET",
            // Sesuaikan URL ini dengan Controller Anda
            // Contoh jika controller menerima Bulan & Tahun terpisah:
            url: `/JurnalMemorial117/GetNextNoVoucher?kodeCabang=${kodeCabang}&bulan=${bulan}&tahun=${tahun}`,
            
            // ATAU jika controller menerima TanggalVoucher:
            // url: `/JurnalMemorial117/GetNextNoVoucher?kodeCabang=${kodeCabang}&tanggalVoucher=${formattedDate}`,
            
            success: function (response) {
                if (response && response.noVoucher) {
                    $("#JurnalHeader_NoVoucher").val(response.noVoucher);
                }
            }
        });
    }
}