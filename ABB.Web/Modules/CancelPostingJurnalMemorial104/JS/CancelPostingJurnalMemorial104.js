function CancelPosting() {
    showConfirmation('Konfirmasi Cancel', `Yakin ingin membatalkan posting data yang dipilih?`,
        function () {
            showProgressOnGrid('#AkseptasiGrid');

            var grid = $("#AkseptasiGrid").data("kendoGrid");
            var selectedRows = grid.select();
            var selectedData = [];

            selectedRows.each(function () {
                var dataItem = grid.dataItem(this);
                selectedData.push({
                    NoVoucher: dataItem.NoVoucher,
                    KodeCabang: dataItem.KodeCabang
                });
            });

            if (selectedData.length === 0) {
                showMessage("Info", "Pilih data terlebih dahulu.");
                closeProgressOnGrid('#AkseptasiGrid');
                return;
            }

            ajaxPost("/CancelPostingJurnalMemorial104/CancelPosting", JSON.stringify(selectedData),
                function (response) {
                    if (response.Status === "OK") {
                        showMessage("Success", "Cancel Posting berhasil.");
                        grid.dataSource.read();
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

function searchFilter() { return {}; }
function gridAutoFit(e) { }