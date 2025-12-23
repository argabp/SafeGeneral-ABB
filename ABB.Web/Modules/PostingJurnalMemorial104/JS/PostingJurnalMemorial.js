function postingJurnalMemorial104() {
    showConfirmation('Confirmation', `Are you sure you want to posting?`,
        function () {
            showProgressOnGrid('#AkseptasiGrid');

            var grid = $("#AkseptasiGrid").data("kendoGrid");
            var selectedRows = grid.select();
            var selectedData = [];

            selectedRows.each(function () {
                var dataItem = grid.dataItem(this);

                // hanya kirim NoVoucher
                selectedData.push({
                    NoVoucher: dataItem.NoVoucher
                });
            });

            ajaxPost("/PostingJurnalMemorial104/Posting", JSON.stringify(selectedData),
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
