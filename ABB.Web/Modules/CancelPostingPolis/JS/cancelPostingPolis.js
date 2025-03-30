
function cancelPolis(){
    showConfirmation('Confirmation', `Apakah anda akan melanjutkan Batal Posting Polis?`,
        function () {
            showProgressOnGrid('#CancelPostingPolisGrid');
            // Get the Kendo UI Grid instance
            var grid = $("#CancelPostingPolisGrid").data("kendoGrid");

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
                    kd_cb: dataItem.kd_cb,
                    kd_cob: dataItem.kd_cob,
                    kd_scob: dataItem.kd_scob,
                    kd_thn: dataItem.kd_thn,
                    no_pol: dataItem.no_pol,
                    no_updt: dataItem.no_updt,
                });
            });

            ajaxPost("/CancelPostingPolis/Cancel", JSON.stringify(selectedData),
                function (response) {
                    if(response.Status === "OK"){
                        showMessage("Success", "Cancel berhasil")
                    } else {
                        showMessage('Error', response.Message);
                    }
                    closeProgressOnGrid('#CancelPostingPolisGrid');
                },
            );
        }
    );
}