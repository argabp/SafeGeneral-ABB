
function cancelPolis(){
    showConfirmation('Confirmation', `Apakah anda akan melanjutkan Cancel Posting Nota Komisi Tambahan?`,
        function () {
            showProgressOnGrid('#CancelPostingNotaKomisiTambahanGrid');
            // Get the Kendo UI Grid instance
            var grid = $("#CancelPostingNotaKomisiTambahanGrid").data("kendoGrid");

            // Get all selected rows (checked rows)
            var selectedRows = grid.select();

            // Create an array to hold the selected objects
            var selectedData = [];

            // Loop through selected rows and extract the data
            selectedRows.each(function() {
                // Get the data item for the selected row
                var dataItem = grid.dataItem(this);

                // Push the dataItem to the selectedData array (you can modify this to fit your needs)
                selectedData.push({
                    jns_sb_nt: dataItem.jns_sb_nt,
                    kd_cb: dataItem.kd_cb,
                    jns_tr: dataItem.jns_tr,
                    jns_nt_msk: dataItem.jns_nt_msk,
                    kd_thn: dataItem.kd_thn,
                    kd_bln: dataItem.kd_bln,
                    no_nt_msk: dataItem.no_nt_msk,
                    jns_nt_kel: dataItem.jns_nt_kel,
                    no_nt_kel: dataItem.no_nt_kel
                });
            });

            ajaxPost("/CancelPostingNotaKomisiTambahan/Cancel", JSON.stringify(selectedData),
                function (response) {
                    if(response.Status === "OK"){
                        showMessage("Success", "Cancel berhasil")
                    } else {
                        showMessage('Error', response.Message);
                    }
                    closeProgressOnGrid('#CancelPostingNotaKomisiTambahanGrid');
                },
            );
        }
    );
}