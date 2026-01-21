// --- Event Handler Dropdown ---
var levelBox = $("#Level").data("kendoNumericTextBox");
     
if (levelBox) {
            // Ambil nilai saat ini
            var currentVal = levelBox.value();
            
            // Jika kosong atau 0 (artinya mode Add baru), kita set default
            if (currentVal === null || currentVal === 0) {
                // Panggil fungsi change manual untuk set default
                onTipeBarisChange();
            }
        }
function onTipeBarisChange(e) {
    // Ambil value. Jika e ada (dipanggil dari event), ambil value dari sender.
    // Jika dipanggil manual, ambil langsung dari element
    var value;
    var dropdown = $("#TipeBaris").data("kendoDropDownList");
      
    var numLevel = $("#Level").data("kendoNumericTextBox");
   
    if (dropdown) {
        value = dropdown.value();
    } else {
        return;
    }

    var hint = $("#rumus-hint");
    var input = $("#Rumus").data("kendoTextArea");

    if (!input) return; // Jaga-jaga jika input belum siap

    if (value === "HEADING" || value === "BLANK") {
        input.value("");
        input.enable(false);
         numLevel.value(1); 
        hint.text("* Tipe ini tidak memerlukan rumus.");
    } else if (value === "DETAIL") {
         numLevel.value(3); 
        input.enable(true);
        hint.html("* Masukkan <b>Kode Akun</b> dipisah koma (Contoh: 1101,1102).");
    } else if (value === "TOTAL") {
        input.enable(true); 
         numLevel.value(2); 
        hint.html("* Masukkan logika penjumlahan (Contoh: SUM(HEADER_ASET)).");
    }
}

// --- Fungsi Membuka Window ---

function onAddClick() {
    var win = $("#TemplateWindow").data("kendoWindow");
    
    // Gunakan event 'refresh' agar script jalan SETELAH konten Partial View dimuat
    win.one("refresh", function() {
        // Panggil fungsi change sekali untuk set initial state (disable/enable rumus)
        setTimeout(function(){ onTipeBarisChange(); }, 100);
    });

    win.refresh({ url: "/TemplateLapKeu/Add" });
    win.center().open();
}

function onEditClick(e) {
    e.preventDefault();
    var tr = $(e.target).closest("tr");
    var data = this.dataItem(tr);
    
    var win = $("#TemplateWindow").data("kendoWindow");

    // Gunakan event 'refresh' agar script jalan SETELAH konten Partial View dimuat
    win.one("refresh", function() {
        // Panggil fungsi change sekali agar field Rumus ter-disable jika datanya HEADING
        setTimeout(function(){ onTipeBarisChange(); }, 100);
    });

    win.refresh({ url: "/TemplateLapKeu/Add?id=" + data.Id });
    win.center().open();
}

function closeWindow() {
    var win = $("#TemplateWindow").data("kendoWindow");
    if(win) win.close();
}

// --- CRUD Operations ---

function saveTemplate() {
    var form = $("#TemplateForm");
    
    // Cek apakah form ada?
    if (form.length === 0) {
        console.error("Form TemplateForm tidak ditemukan!");
        return;
    }

    if (!form[0].checkValidity()) {
        form[0].reportValidity();
        return;
    }

    var data = {
        Id: $("#Id").val(),
        TipeLaporan: $("#TipeLaporan").data("kendoDropDownList").value(),
        TipeBaris: $("#TipeBaris").data("kendoDropDownList").value(),
        Deskripsi: $("#Deskripsi").val(),
        Rumus: $("#Rumus").val(),
        Level: $("#Level").val(),
        Urutan: $("#Urutan").val(),
    };

    $.ajax({
        url: "/TemplateLapKeu/Save",
        type: "POST",
        contentType: "application/json",
        data: JSON.stringify(data),
        success: function(res) {
            // Menggunakan format success: true sesuai controller C# sebelumnya
            if (res.success) {
                // Panggil showMessage bawaan
                showMessage('Success', 'Data berhasil disimpan');
                closeWindow();
                $("#TemplateGrid").data("kendoGrid").dataSource.read();
            } else {
                showMessage('Error', res.message || "Gagal menyimpan data");
            }
        },
        error: function(err) {
            console.error(err);
            showMessage("Error", "Terjadi kesalahan server");
        }
    });
}

function onDeleteClick(e) {
    e.preventDefault();
    var tr = $(e.target).closest("tr");
    var data = this.dataItem(tr);

    // Menggunakan Native Confirm Browser (Bawaan) pengganti Swal
    if (confirm("Yakin ingin menghapus baris ini?")) {
        $.ajax({
            url: "/TemplateLapKeu/Delete",
            type: "POST",
            contentType: "application/json",
            data: JSON.stringify({ Id: data.Id }),
            success: function(res) {
                if (res.success) {
                    showMessage("Success", "Data dihapus");
                    $("#TemplateGrid").data("kendoGrid").dataSource.read();
                } else {
                    showMessage("Error", res.message || "Gagal menghapus data");
                }
            },
            error: function() {
                showMessage("Error", "Terjadi kesalahan server saat menghapus");
            }
        });
    }
}