function CancelpostingPenyelesaianPiutang() {
    showConfirmation('Confirmation', `Are you sure you want to cancel posting?`,
        function () {
            showProgressOnGrid('#AkseptasiGrid');

            var grid = $("#AkseptasiGrid").data("kendoGrid");
            var selectedRows = grid.select();
            var selectedData = [];

            selectedRows.each(function () {
                var dataItem = grid.dataItem(this);

                // hanya kirim NoVoucher
                selectedData.push({
                    NomorBukti: dataItem.NomorBukti
                });
            });

            ajaxPost("/CancelPostingPenyelesaianPiutang/CancelPosting", JSON.stringify(selectedData),
                function (response) {
                    if (response.Status === "OK") {
                        showMessage("Success", "Cancel Posting succeed");
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
