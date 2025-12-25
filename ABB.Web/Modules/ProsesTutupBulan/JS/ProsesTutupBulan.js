function executeClosing() {
    // 1. Ambil instance Grid
    var grid = $("#GridTutupBulan").data("kendoGrid");
    
    // 2. Ambil baris yang dicentang (Selected Rows)
    var selectedRows = grid.select();
    var selectedData = [];

    // 3. Loop baris terpilih dan ambil data ThnPrd & BlnPrd
    selectedRows.each(function () {
        var dataItem = grid.dataItem(this);
        
        // Kita masukkan ke array object sesuai struktur Command di C#
        selectedData.push({
            ThnPrd: dataItem.ThnPrd, // String (ex: "2023")
            BlnPrd: dataItem.BlnPrd  // String (ex: "01")
        });
    });

    // 4. Validasi: User harus pilih minimal satu
    if (selectedData.length === 0) {
        showMessage("Info", "Silakan centang periode yang akan ditutup terlebih dahulu.");
        return;
    }

    // 5. Konfirmasi sebelum eksekusi (PENTING karena Tutup Bulan biasanya fatal kalau salah)
    showConfirmation('Konfirmasi Tutup Bulan', 
        `Apakah Anda yakin ingin melakukan Closing untuk <b>${selectedData.length}</b> periode terpilih? <br/>Proses ini tidak dapat dibatalkan.`,
        function () {
            // Tampilkan Loading di atas Grid biar user tau proses berjalan
            showProgressOnGrid('#GridTutupBulan');

            // 6. Kirim ke Controller via AJAX
            // URL sesuaikan dengan Controller: /ProsesTutupBulan/ProcessClosing
            ajaxPost("/ProsesTutupBulan/ProcessClosing", JSON.stringify(selectedData),
                function (response) {
                    
                    // Matikan Loading
                    closeProgressOnGrid('#GridTutupBulan');

                    if (response.success) { // Sesuaikan dengan return Json di Controller (success/Status)
                        showMessage("Sukses", response.message);
                        
                        // 7. Refresh Grid supaya periode yang sudah ditutup hilang dari list
                        grid.dataSource.read(); 
                        grid.clearSelection();
                    } else {
                        showMessage("Error", response.message);
                    }
                }
            );
        }
    );
}

// --- Helper Functions (Standar Kendo) ---

// Dipakai di .Data("searchFilter") pada DataSource Grid
function searchFilter() {
    return {
        // Jika nanti ada filter pencarian, masukkan disini.
        // Saat ini kosong dulu tidak apa-apa.
    };
}

// Dipakai di .Events(ev => ev.DataBound("gridAutoFit"))
function gridAutoFit(e) {
    // Opsional: Otomatis sesuaikan lebar kolom
    // e.sender.resize(); 
}