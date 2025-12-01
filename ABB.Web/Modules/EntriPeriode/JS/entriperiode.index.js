function btnAddPeriode_OnClick() {
    var window = $("#EntriPeriodeWindow").data("kendoWindow");
    
    // Reset form saat dibuka
    window.refresh({
        url: "/EntriPeriode/Add" // Pastikan Action Add di Controller ada
    });
    
    window.center().open();
}
function btnEditPeriode_OnClick(e) {
    e.preventDefault(); // Mencegah link default
    
    // Ambil data baris yang diklik
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    
    var window = $("#EntriPeriodeWindow").data("kendoWindow");
    
    // Panggil Action Edit di Controller dengan parameter PK (Thn & Bln)
    // Pastikan nama properti (ThnPrd, BlnPrd) sesuai dengan DTO Anda
    window.refresh({
        url: `/EntriPeriode/Edit?thn=${dataItem.ThnPrd}&bln=${dataItem.BlnPrd}`
    });
    
    window.center().open();
}

// 3. Fungsi untuk Tombol "Hapus" (di dalam Grid)
function btnDeletePeriode_OnClick(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));

    showConfirmation("Konfirmasi Hapus", "Apakah Anda yakin ingin menghapus periode ini?", function() {
        $.ajax({
            type: "POST",
            url: `/EntriPeriode/Delete?thn=${dataItem.ThnPrd}&bln=${dataItem.BlnPrd}`,
            success: function(response) {
                if (response.success) {
                    showMessage("Sukses", "Data berhasil dihapus.");
                    // Refresh kedua grid
                   $("#PeriodeGrid").data("kendoGrid").dataSource.read();
                } else {
                    showMessage("Error", response.message);
                }
            }
        });
    });
}

function onSavePeriode() {
    // 1. Ambil data dari form
    var thnPrd = $("#ThnPrd").data("kendoNumericTextBox").value();
    
    // --- PERBAIKAN DI SINI ---
    // Cek apakah BlnPrd adalah DropDownList (saat Add) atau Input Hidden (saat Edit)
    var blnPrdWidget = $("#BlnPrd").data("kendoDropDownList");
    var blnPrd;

    if (blnPrdWidget) {
        // Jika dropdown (Mode Add), ambil value dari widget
        blnPrd = blnPrdWidget.value();
    } else {
        // Jika hidden/readonly (Mode Edit), ambil value dari element langsung
        blnPrd = $("#BlnPrd").val();
    }
    // -------------------------

    var tglMul = $("#TglMul").data("kendoDatePicker").value();
    var tglAkh = $("#TglAkh").data("kendoDatePicker").value();
    var flagClosing = $("#FlagClosing").data("kendoDropDownList").value();

    // 2. Validasi sederhana
    if (!thnPrd || !blnPrd || !tglMul || !tglAkh) {
        showMessage("Warning", "Mohon lengkapi semua data (Tahun, Bulan, Tanggal).");
        return;
    }

    var data = {
        ThnPrd: thnPrd,
        BlnPrd: blnPrd,
        TglMul: kendo.toString(tglMul, "yyyy-MM-dd"), // Format tanggal agar aman
        TglAkh: kendo.toString(tglAkh, "yyyy-MM-dd"),
        FlagClosing: flagClosing
    };

    // 3. Kirim ke Controller
    $.ajax({
        type: "POST",
        url: "/EntriPeriode/Save",
        contentType: "application/json",
        data: JSON.stringify(data),
        success: function (response) {
            if (response.success) {
                showMessage("Success", "Data berhasil disimpan.");
                
                // Tutup window
                $("#EntriPeriodeWindow").data("kendoWindow").close();

                // Refresh kedua grid agar data terbaru muncul
                $("#PeriodeGrid").data("kendoGrid").dataSource.read();

            } else {
                showMessage("Error", response.message);
            }
        },
        error: function () {
            showMessage("Error", "Terjadi kesalahan pada server.");
        }
    });
}

// Helper untuk menutup window
function closeWindow(selector) {
    $(selector).data("kendoWindow").close();
}