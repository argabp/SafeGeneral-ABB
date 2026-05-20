function postingPenyelesaianPiutang() {
    showConfirmation('Confirmation', `Are you sure you want to posting?`,
        function () {
            showProgressOnGrid('#AkseptasiGrid');

            var grid = $("#AkseptasiGrid").data("kendoGrid");
            var selectedRows = grid.select();
            var selectedData = [];

            selectedRows.each(function () {
                var dataItem = grid.dataItem(this);

                var rawDate = dataItem.Tanggal;
                var formattedDate = kendo.toString(kendo.parseDate(rawDate), "yyyy-MM-dd");

                // hanya kirim NoVoucher
                selectedData.push({
                    NomorBukti: dataItem.NomorBukti,
                    Tanggal: formattedDate
                });
            });
            console.log(selectedData)
            ajaxPost("/PostingPenyelesaianPiutang/Posting", JSON.stringify(selectedData),
                function (response) {
                    if (response.Status === "OK") {
                        showMessage("Success", "Posting succeed");
                        grid.dataSource.read(); // refresh grid setelah sukses
                         grid.clearSelection();
                    } else {
                        showMessage("Error", response.Message);
                    }
                    closeProgressOnGrid('#AkseptasiGrid');
                }
            );
        }
    );
}
