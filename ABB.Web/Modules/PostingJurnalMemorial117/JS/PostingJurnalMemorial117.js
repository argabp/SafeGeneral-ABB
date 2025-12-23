function postingJurnalMemorial117() {
    // 1. Konfirmasi
    showConfirmation('Konfirmasi Posting', `Apakah Anda yakin ingin memposting data Jurnal Memorial yang dipilih?`,
        function () {
            // Tampilkan loading di Grid
            showProgressOnGrid('#PostingJurnalGrid');

            var grid = $("#PostingJurnalGrid").data("kendoGrid");
            
            // 2. Ambil baris yang dicentang
            var selectedRows = grid.select();
            var selectedData = [];

            selectedRows.each(function () {
                var dataItem = grid.dataItem(this);

                // 3. Susun data sesuai DTO (JurnalMemorial117Dto)
                // Kita butuh NoVoucher dan KodeCabang untuk Command
                selectedData.push({
                    NoVoucher: dataItem.NoVoucher,
                    KodeCabang: dataItem.KodeCabang 
                });
            });

            console.log("Data to Post:", selectedData);

            // Validasi jika tidak ada data dipilih
            if (selectedData.length === 0) {
                showMessage("Info", "Pilih data terlebih dahulu.");
                closeProgressOnGrid('#PostingJurnalGrid');
                return;
            }

            // 4. Kirim ke Controller
            ajaxPost("/PostingJurnalMemorial117/Posting", JSON.stringify(selectedData),
                function (response) {
                    if (response.Status === "OK") {
                        showMessage("Success", "Data berhasil diposting.");
                        
                        grid.dataSource.read(); // Refresh grid agar data yang sudah posting hilang
                        grid.clearSelection();  // Bersihkan centangan
                    } else {
                        showMessage("Error", response.Message);
                    }
                    // Matikan loading
                    closeProgressOnGrid('#PostingJurnalGrid');
                }
            );
        }
    );
}

// Pastikan fungsi helper (ajaxPost, showMessage, showConfirmation) sudah tersedia di project Anda (biasanya di site.js / global.js).