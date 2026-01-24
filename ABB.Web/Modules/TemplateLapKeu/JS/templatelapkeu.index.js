// ==========================================================================
// FUNGSI HELPER (FILTER DATASOURCE)
// ==========================================================================

// 1. Mengirim parameter 'TipeLaporan' saat Dropdown Start/End Row dibuka
function getTemplateParam() {
        return {
            tipeLaporan: $("#TipeLaporan").val() 
        };
    }

// ==========================================================================
// LOGIKA UI FORM (VISIBILITY & INTERACTION)
// ==========================================================================

// 2. Dipanggil saat Tipe Baris berubah (Heading/Detail/Total)
function onTipeBarisChange() {
    var dropdown = $("#TipeBaris").data("kendoDropDownList");
    var value = dropdown.value();
    var levelBox = $("#Level").data("kendoNumericTextBox");
    
    // Setup Tampilan (Show/Hide Div)
    setupUI(value);

    // Auto Level Logic (Agar user gak cape input manual)
    if (levelBox) {
        if (value === "HEADING") levelBox.value(1);
        else if (value === "TOTAL") levelBox.value(2);
        else if (value === "DETAIL") levelBox.value(3);
    }
}

// 3. Fungsi inti mengatur Show/Hide elemen
function setupUI(tipe) {
    var hint = $("#rumus-hint");
    var manualInput = $("#RumusManual").data("kendoTextArea");

    if (tipe === "TOTAL") {
        $("#div-manual").hide();
        $("#div-range").show();
        // Set hint untuk manual input jaga-jaga kalau checkbox dicentang
        if (hint) hint.text("* Masukkan No Urut dipisah koma (Contoh: 10, 20).");
    } 
    else if (tipe === "DETAIL") {
        $("#div-manual").show();
        $("#div-range").hide();
        if (manualInput) manualInput.enable(true);
        if (hint) hint.html("* Masukkan <b>Kode Akun</b> dipisah koma (Contoh: 1101,1102).");
    } 
    else {
        // Heading / Blank
        $("#div-manual").show();
        $("#div-range").hide();
        if (manualInput) {
            manualInput.value("");
            manualInput.enable(false); // Disable input
        }
        if (hint) hint.text("* Tipe ini tidak memerlukan rumus.");
    }
}

// 4. Dipanggil saat Checkbox "Input Manual" di centang/uncentang
function toggleManualTotal() {
    var manualInput = $("#RumusManual").data("kendoTextArea");
    
    if ($("#chkManualTotal").is(":checked")) {
        // Mode Manual Total (Grand Total)
        $("#div-manual").show();
        $("#div-range").hide();
        if (manualInput) {
            manualInput.enable(true);
            manualInput.value("");
            // Ganti placeholder biar user ngeh
            $(manualInput.element).attr("placeholder", "Contoh: 10, 20");
        }
    } else {
        // Balik ke Mode Range
        $("#div-manual").hide();
        $("#div-range").show();
    }
}

// 5. Fungsi Inisialisasi Data saat Edit Mode (Parsing "5-8")
function initEditForm() {
    var currentTipe = $("#TipeBaris").val();
    var currentRumus = $("#RumusFinal").val(); // Ambil dari Hidden Field

    // Atur tampilan awal
    setupUI(currentTipe);

    // Jika ada rumus dan Tipe TOTAL
    if (currentRumus && currentTipe === "TOTAL") {
        // Cek apakah format range "5-8"
        if (currentRumus.includes("-")) {
            var parts = currentRumus.split("-");
            // Delay 500ms agar DataSource Dropdown sempat loading
            setTimeout(function() {
                var startDrop = $("#StartRow").data("kendoDropDownList");
                var endDrop = $("#EndRow").data("kendoDropDownList");
                
                if (startDrop) startDrop.value(parts[0]);
                if (endDrop) endDrop.value(parts[1]);
            }, 500);
        } else {
            // Kalau isinya koma (10,20), berarti itu Manual Total
            // Centang checkbox manual
            $("#chkManualTotal").prop("checked", true);
            toggleManualTotal();
            $("#RumusManual").data("kendoTextArea").value(currentRumus);
        }
    } else if (currentRumus) {
        // Detail (Kode Akun)
        $("#RumusManual").data("kendoTextArea").value(currentRumus);
    }
}


// ==========================================================================
// WINDOW & GRID EVENTS
// ==========================================================================

function onAddClick() {
    var win = $("#TemplateWindow").data("kendoWindow");
    
    // Event Refresh: Jalan setelah konten HTML termuat
    win.one("refresh", function() {
        // Panggil init
        setTimeout(function(){ 
            onTipeBarisChange(); // Set default state
        }, 100);
    });

    win.refresh({ url: "/TemplateLapKeu/Add" });
    win.center().open();
}
function onTipeLaporanChange() {
        var startDrop = $("#StartRow").data("kendoDropDownList");
        var endDrop = $("#EndRow").data("kendoDropDownList");

        if (startDrop) startDrop.dataSource.read();
        if (endDrop) endDrop.dataSource.read();
    }

function onEditClick(e) {
    e.preventDefault();
    var tr = $(e.target).closest("tr");
    // Deteksi Grid mana yang diklik (karena ada 3 Tab)
    var grid = $(e.target).closest(".k-grid").data("kendoGrid");
    var data = grid.dataItem(tr);
    
    var win = $("#TemplateWindow").data("kendoWindow");

    win.one("refresh", function() {
        // Panggil initEditForm untuk parsing data lama
        initEditForm();
    });

    win.refresh({ url: "/TemplateLapKeu/Add?id=" + data.Id });
    win.center().open();
}

function closeWindow() {
    var win = $("#TemplateWindow").data("kendoWindow");
    if(win) win.close();
}

// ==========================================================================
// CRUD OPERATIONS (SAVE & DELETE)
// ==========================================================================

function saveTemplate() {
    // 1. Validasi Form Standard HTML5
    var form = $("#TemplateForm");
    if (form.length === 0) { console.error("Form tidak ditemukan"); return; }
    
    // 2. Logic Konstruksi Rumus
    var tipe = $("#TipeBaris").val();
    var finalRumus = "";

    // Jika TOTAL dan Checkbox Manual MATI -> Pakai Range
    if (tipe === "TOTAL" && !$("#chkManualTotal").is(":checked")) {
        var start = $("#StartRow").val();
        var end = $("#EndRow").val();
        
        if(!start || !end) {
            alert("Harap pilih Range Baris (Awal & Akhir)");
            return;
        }
        if(parseInt(start) > parseInt(end)){
            alert("Baris Awal tidak boleh lebih besar dari baris Akhir");
            return;
        }
        finalRumus = start + "-" + end; // Gabungkan jadi string "5-8"
    } 
    else {
        // Ambil dari Text Area (Detail / Manual Total)
        finalRumus = $("#RumusManual").val();
    }

    // 3. Masukkan hasil ke Object Data
    var dataToSend = {
        Id: $("#Id").val(),
        Urutan: $("#Urutan").val(),
        TipeLaporan: $("#TipeLaporan").val(),
        TipeBaris: $("#TipeBaris").val(),
        Deskripsi: $("#Deskripsi").val(),
        Level: $("#Level").val(),
        Rumus: finalRumus // <--- PENTING
    };

    // 4. AJAX Submit
    $.ajax({
        url: "/TemplateLapKeu/Save",
        type: "POST",
        contentType: "application/json",
        data: JSON.stringify(dataToSend),
        success: function(res) {
            if (res.success) {
                // if (typeof showMessage === 'function') showMessage('Success', 'Data berhasil disimpan');
                alert("Data Berhasil Disimpan");
                closeWindow();
                
                // Refresh Grid yang aktif
                refreshActiveGrid();
            } else {
                alert(res.message || "Gagal menyimpan data");
            }
        },
        error: function(err) {
            console.error(err);
            alert("Terjadi kesalahan server");
        }
    });
}

function onDeleteClick(e) {
    e.preventDefault();
    var tr = $(e.target).closest("tr");
    var grid = $(e.target).closest(".k-grid").data("kendoGrid");
    var data = grid.dataItem(tr);

    if (confirm("Yakin ingin menghapus baris ini?")) {
        $.ajax({
            url: "/TemplateLapKeu/Delete",
            type: "POST",
            contentType: "application/json",
            data: JSON.stringify({ Id: data.Id }),
            success: function(res) {
                if (res.success) {
                    alert("Data dihapus");
                    grid.dataSource.read(); // Refresh grid spesifik yang diklik
                } else {
                    alert(res.message || "Gagal menghapus data");
                }
            },
            error: function() {
                alert("Terjadi kesalahan server saat menghapus");
            }
        });
    }
}

// Helper untuk refresh grid sesuai Tab yang aktif
function refreshActiveGrid() {
    var tabStrip = $("#tabStripTemplate").data("kendoTabStrip");
    if (!tabStrip) {
        // Fallback kalau gak pake tab (kode lama)
        $("#TemplateGrid").data("kendoGrid").dataSource.read();
        return;
    }
    
    var activeIndex = tabStrip.select().index();
    if(activeIndex === 0) $("#GridNeraca").data("kendoGrid").dataSource.read();
    if(activeIndex === 1) $("#GridLabaRugi").data("kendoGrid").dataSource.read();
    if(activeIndex === 2) $("#GridArusKas").data("kendoGrid").dataSource.read();
}