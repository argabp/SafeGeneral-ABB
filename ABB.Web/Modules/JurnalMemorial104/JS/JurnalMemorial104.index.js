function btnAddJurnalMamorial104_OnClick() {
    var window = $("#JurnalMemorial104Window").data("kendoWindow");
    window.refresh({
        url: "/JurnalMemorial104/Add"
    });
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

function btnEditJurnalMemorial104_OnClick(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    var window = $("#JurnalMemorial104Window").data("kendoWindow");
    
    // --- TAMBAHKAN INI (Event Refresh) ---
    window.one("refresh", function() {
        attachDetailEvents(); // Pasang event listener
        // Jangan clear form disini karena ini mode edit header
    });
    // -------------------------------------

    window.refresh({
        url: `/JurnalMemorial104/Add?kodeCabang=${dataItem.KodeCabang}&noVoucher=${dataItem.NoVoucher}`
    });
    window.center().open();
}

function attachDetailEvents() {
    // Ambil komponen Kendo
    var mataUangCombo = $("#KodeMataUang").data("kendoComboBox");
    var debetInput = $("#NilaiDebet").data("kendoNumericTextBox");
    var kreditInput = $("#NilaiKredit").data("kendoNumericTextBox");
    var tanggalHeader = $("#JurnalHeader_Tanggal").data("kendoDatePicker");

    // Pasang Event Listener 'change'
    // Setiap kali user ubah Mata Uang, Angka, atau Tanggal -> Hitung ulang
    if (mataUangCombo) mataUangCombo.bind("change", hitungKursDetail);
    if (debetInput) debetInput.bind("change", hitungKursDetail);
    if (kreditInput) kreditInput.bind("change", hitungKursDetail);
    
    // Opsional: Jika tanggal header berubah, hitung ulang detail yang sedang diinput
    if (tanggalHeader) tanggalHeader.bind("change", hitungKursDetail);
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

    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));

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


