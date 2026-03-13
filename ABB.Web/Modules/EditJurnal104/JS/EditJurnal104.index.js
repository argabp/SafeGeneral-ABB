// --- BAGIAN INQUIRY (HALAMAN AWAL) ---
function getSearchFilter() {
    var tglAwal = $("#FilterTglAwal").data("kendoDatePicker").value();
    var tglAkhir = $("#FilterTglAkhir").data("kendoDatePicker").value();

    return {
        // kendo.toString akan memaksa formatnya menjadi Tahun-Bulan-Tanggal
        TglAwal: tglAwal ? kendo.toString(tglAwal, "yyyy-MM-dd") : null,
        TglAkhir: tglAkhir ? kendo.toString(tglAkhir, "yyyy-MM-dd") : null,
        NoBukti: $("#SearchKeyword").val()
    };
}

function btnCari_OnClick() {
    $("#EditJurnal104Grid").data("kendoGrid").dataSource.read();
}

function onGridDataBound(e) {
    e.sender.resize();
}
function btnEditJurnal_OnClick(e) {
    e.preventDefault();
    var grid = $("#EditJurnal104Grid").data("kendoGrid");
    var row = $(e.target).closest("tr");
    var dataItem = grid.dataItem(row);

    var window = $("#EditJurnal104Window").data("kendoWindow");

    // Bersihkan spasi dari NoBukti dan encode URL agar aman
    var noBuktiAsli = dataItem.NoBukti ? dataItem.NoBukti.trim() : "";
    var cleanNoBukti = encodeURIComponent(noBuktiAsli);

    // 1. PASANG TELINGA DULU (Event Listener)
    window.one("refresh", function () {
        $("#Edit_GlTran").val(dataItem.GlTran ? dataItem.GlTran.trim() : "");
        // Setelah form Edit berhasil diload dari server, baru jalankan ini:
        $("#Edit_KodeLokasi").val(dataItem.NamaCabang ? dataItem.NamaCabang.trim() : "");
        $("#Edit_Tanggal").val(kendo.toString(dataItem.Tanggal, "dd-MM-yyyy"));
        $("#Edit_KeteranganUtama").val(dataItem.Keterangan ? dataItem.Keterangan.trim() : "");

        // Panggil grid detailnya!
        initDetailGridEdit(noBuktiAsli);
    });

    // 2. BARU SURUH LOAD URL
    window.refresh({
        url: `/EditJurnal104/Edit?noBukti=${cleanNoBukti}`
    });

    // 3. TAMPILKAN WINDOW
    window.center().open();
}

function btnCloseWindow() {
    $("#EditJurnal104Window").data("kendoWindow").close();
}
function initDetailGridEdit(noBukti) {
    var cleanNoBukti = encodeURIComponent(noBukti.trim());

    if ($("#EditDetailGrid").data("kendoGrid")) {
        $("#EditDetailGrid").data("kendoGrid").destroy();
        $("#EditDetailGrid").empty();
    }

    $("#EditDetailGrid").kendoGrid({
        dataSource: {
            transport: {
                read: {
                    url: `/EditJurnal104/GetDetailJurnal?noBukti=${cleanNoBukti}`,
                    type: "GET",
                    dataType: "json"
                }
            },
            schema: {
                data: "Data", 
                model: {
                    id: "Id",
                    fields: {
                        NoUrut: { type: "number", editable: false },
                        NoNota: { type: "string", editable: false }, 
                        MataUang: { type: "string", editable: false }, 
                        DK: { type: "string", editable: true },
                        KodeAkun: { type: "string", editable: true }, // Akan diisi dari ComboBox
                        NilaiOrg: { type: "number", editable: true },
                        NilaiIdr: { type: "number", editable: true }
                    }
                }
            },
            change: updateFooterTotalsEdit, 
            requestEnd: function(e) {
                if (e.response && e.response.Status === "ERROR") {
                    showMessage('Error', e.response.Message, 'error');
                }
            }
        },
        editable: true, 
        navigatable: true,
        scrollable: true,
        height: 300,
        
        // --- FITUR AUTO-REPLACE NILAI IDR (Bisa diedit manual juga) ---
        // --- FITUR AUTO-CALCULATE NILAI IDR + KURS ---
        save: function(e) {
            // Jika yang diketik adalah NilaiOrg
            if (e.values.hasOwnProperty("NilaiOrg")) {
                var nilaiOrgBaru = e.values.NilaiOrg;
                var mataUang = e.model.MataUang;
                
                // Ambil Tanggal Jurnal (Header) untuk cek Kurs
                var tglString = $("#Edit_Tanggal").val(); 
                var tglBukti = kendo.toString(kendo.parseDate(tglString, "dd-MM-yyyy"), "yyyy-MM-dd");

                // Jika Rupiah (001), langsung set angkanya sama (tanpa perlu ke server)
                if (mataUang === "001" || !mataUang) {
                    e.model.set("NilaiIdr", nilaiOrgBaru);
                } 
                // Jika Valas, tembak API Kurs
                else {
                    $.ajax({
                        url: '/EditJurnal104/GetKurs',
                        type: 'GET',
                        data: {
                            kodeMataUang: mataUang,
                            tanggalVoucher: tglBukti
                        },
                        success: function(res) {
                            var kurs = res.nilai_kurs || 1;
                            var nilaiIdrBaru = nilaiOrgBaru * kurs; // Rumus: ORG * KURS
                            e.model.set("NilaiIdr", nilaiIdrBaru);
                        }
                    });
                }
            }
        },

        columns: [
            { field: "NoUrut", title: "Urut", width: 60 },
            { field: "NoNota", title: "No. Nota", width: 150 },
            { field: "MataUang", title: "MTU", width: 50 },
            { 
                field: "DK", title: "D/K", width: 50,
                editor: function(container, options) {
                    $('<input required name="' + options.field + '"/>')
                        .appendTo(container)
                        .kendoDropDownList({ dataSource: ["D", "K"] });
                }
            },
            
            // --- FITUR COMBOBOX COA SUPER PINTAR ---
            { 
                field: "KodeAkun", title: "Kode Akun", width: 120,
                editor: function(container, options) {
                    $('<input required name="' + options.field + '"/>')
                        .appendTo(container)
                        .kendoComboBox({
                            dataTextField: "NamaTampil", 
                            dataValueField: "Kode",      
                            filter: "contains",
                            minLength: 3, 
                            autoBind: false,
                            placeholder: "Ketik kode/nama akun...",
                            dataSource: {
                                serverFiltering: true, 
                                serverPaging: true,
                                pageSize: 25,
                                transport: {
                                    read: {
                                        // PASTIKAN URL-NYA SUDAH MENGARAH KE EDIT JURNAL 104
                                        url: "/EditJurnal104/GetCoa", 
                                        type: "GET",
                                        dataType: "json",
                                        data: function(options) {
                                            var keyword = "";
                                            if (options.filter && options.filter.filters.length > 0) {
                                                keyword = options.filter.filters[0].value;
                                            }
                                            return {
                                                page: 1,
                                                pageSize: 25,
                                                searchKeyword: keyword 
                                            };
                                        }
                                    }
                                },
                                schema: {
                                    data: function(response) {
                                        // Karena kita pakai ToDataSourceResult di C#, datanya ada di response.Data
                                        var items = response.Data || response; 
                                        
                                        if(items.length > 0) {
                                            for(var i=0; i < items.length; i++){
                                                var kodeBersih = items[i].Kode ? items[i].Kode.trim() : "";
                                                var namaBersih = items[i].Nama ? items[i].Nama.trim() : "";
                                                
                                                items[i].Kode = kodeBersih;
                                                items[i].NamaTampil = kodeBersih + " - " + namaBersih; 
                                            }
                                        }
                                        return items;
                                    },
                                    total: "Total" // Ambil total data dari ToDataSourceResult
                                }
                            }
                        });
                }
            },
            // ---------------------------------------

            { field: "NilaiOrg", title: "Nilai (ORG)", width: 120, format: "{0:n2}", attributes: { style: "text-align:right;" } },
            { field: "NilaiIdr", title: "Nilai (IDR)", width: 120, format: "{0:n2}", attributes: { style: "text-align:right;" } },
            { command: "destroy", title: "Aksi", width: 90 }
        ],
        dataBound: function() {
            updateFooterTotalsEdit();
        }
    });
}

function btnTambahBaris_OnClick() {
    var grid = $("#EditDetailGrid").data("kendoGrid");
    if (!grid) return;

    // Ambil semua data yang ada di grid saat ini
    var data = grid.dataSource.data();
    var maxUrut = 0;

    // Looping untuk mencari NoUrut paling besar
    for (var i = 0; i < data.length; i++) {
        // Pastikan kita mengecek field NoUrut (bisa jadi ada baris yg baru ditambah)
        var urutSaatIni = data[i].NoUrut || 0; 
        if (urutSaatIni > maxUrut) {
            maxUrut = urutSaatIni;
        }
    }

    // Nomor urut baru adalah nilai maksimum + 1
    var newUrut = maxUrut + 1;

    // Tambah baris kosong dengan nomor urut yang sudah presisi
    grid.dataSource.add({
        NoUrut: newUrut,
        NoNota: "-", // Default strip
        MataUang: "001", // Defaultkan ke Rupiah
        DK: "D",
        KodeAkun: "",
        NilaiOrg: 0,
        NilaiIdr: 0
    });
}

function updateFooterTotalsEdit() {
    var grid = $("#EditDetailGrid").data("kendoGrid");
    if (!grid) return;

    var data = grid.dataSource.data();
    var totalDebet = 0;
    var totalKredit = 0;

    for (var i = 0; i < data.length; i++) {
        // Sesuaikan nama field dengan data JSON
        if (data[i].DK === "D") totalDebet += data[i].NilaiIdr || 0;
        if (data[i].DK === "K") totalKredit += data[i].NilaiIdr || 0;
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

// EKSEKUSI SIMPAN SEMUA
function onSaveAllEditJurnal() {
    var grid = $("#EditDetailGrid").data("kendoGrid");
    var detailData = grid.dataSource.data();

    if (detailData.length === 0) {
        showMessage('Warning', 'Jurnal tidak boleh kosong!', 'warning');
        return;
    }

    // Validasi Balance (Sesuaikan huruf besar)
    var debet = 0; var kredit = 0;
    $.each(detailData, function(i, val) {
        if (val.DK === "D") debet += val.NilaiIdr;
        if (val.DK === "K") kredit += val.NilaiIdr;
    });

    if (Math.abs(debet - kredit) > 0.01) {
        showMessage('Error', 'Total Debet dan Kredit tidak seimbang!', 'error');
        return;
    }

    // Susun Payload C#
    var payload = {
        GlTran: $("#Edit_GlTran").val(),
        NoBukti: $("#Edit_NoBukti").val(),
        KodeLokasi: $("#Edit_KodeLokasi").val(),
        TglBukti: kendo.parseDate($("#Edit_Tanggal").val(), "dd-MM-yyyy"), 
        KeteranganUtama: $("#Edit_KeteranganUtama").val(),
        Details: []
    };

    // Ambil data Grid + PENYELAMATAN STRING (TRIM)
    $.each(detailData, function(i, val) {
        payload.Details.push({
            NoUrut: val.NoUrut,
            // ToString() dan trim() untuk membersihkan semua spasi gaib dari DB
            NoNota: val.NoNota ? val.NoNota.toString().trim() : "",
            MataUang: val.MataUang ? val.MataUang.toString().trim() : "",
            DK: val.DK ? val.DK.toString().trim() : "",
            KodeAkun: val.KodeAkun ? val.KodeAkun.toString().trim() : "", 
            NilaiOrg: val.NilaiOrg,
            NilaiIdr: val.NilaiIdr
        });
    });

    // Konfirmasi
    if (confirm("Pastikan nominal debet & kredit sudah sesuai. Lanjutkan simpan perubahan?")) {
        $.ajax({
            type: "POST",
            url: "/EditJurnal104/SaveEditJurnal",
            contentType: "application/json",
            data: JSON.stringify(payload),
            success: function (res) {
                // Jangan lupa ini juga harus sesuai JSON dari fungsi Save Anda
                // Kalau Save mengembalikan huruf besar, pakai huruf besar.
                if (res.Status === "OK" || res.status === "OK") { 
                    showMessage('Success', res.Message || res.message, 'success');
                    btnCloseWindow();
                    btnCari_OnClick(); // Refresh tabel utama
                } else {
                    showMessage('Error', res.Message || res.message, 'error');
                }
            },
            error: function () {
                showMessage('Error', 'Terjadi kesalahan pada server saat menyimpan.', 'error');
            }
        });
    }
}