$(document).ready(function () {
    searchKeyword_OnKeyUp();
});

function searchKeyword_OnKeyUp() {
    $('#SearchKeyword').keyup(function () {
        refreshGrid("#CancelPostingMutasiKlaimGrid");
    });
}

function cancelPostingMutasiKlaim(){
    showConfirmation('Confirmation', `Are you sure you want to cancel?`,
        function () {
            showProgressOnGrid('#CancelPostingMutasiKlaimGrid');
            // Get the Kendo UI Grid instance
            var grid = $("#CancelPostingMutasiKlaimGrid").data("kendoGrid");

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
                    id: dataItem.Id,
                    kd_cb: dataItem.kd_cb,
                    kd_cob: dataItem.kd_cob,
                    kd_scob: dataItem.kd_scob,
                    kd_thn: dataItem.kd_thn,
                    no_kl: dataItem.no_kl,
                    no_mts: dataItem.no_mts,
                });
            });

            ajaxPost("/CancelPostingMutasiKlaim/Cancel", JSON.stringify(selectedData),
                function (response) {
                    if(response.Status === "OK"){
                        showMessage("Success", "Cancel Sukses")
                    } else {
                        showMessage('Error', response.Message);
                    }
                    refreshGrid('#CancelPostingMutasiKlaimGrid');
                    closeProgressOnGrid('#CancelPostingMutasiKlaimGrid');
                },
            );
        }
    );
}

function postingAccounting(){
    showConfirmation('Confirmation', `Are you sure you want to posting?`,
        function () {
            showProgressOnGrid('#CancelPostingMutasiKlaimGrid');
            // Get the Kendo UI Grid instance
            var grid = $("#CancelPostingMutasiKlaimGrid").data("kendoGrid");

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
                    id: dataItem.Id,
                    kd_cb: dataItem.kd_cb,
                    kd_cob: dataItem.kd_cob,
                    kd_scob: dataItem.kd_scob,
                    kd_thn: dataItem.kd_thn,
                    no_kl: dataItem.no_kl,
                    no_mts: dataItem.no_mts,
                });
            });

            ajaxPost("/CancelPostingMutasiKlaim/Posting", JSON.stringify(selectedData),
                function (response) {
                    if(response.Status === "OK"){
                        showMessage("Success", "Posting Accounting Sukses")
                    } else {
                        showMessage('Error', response.Message);
                    }
                    refreshGrid('#CancelPostingMutasiKlaimGrid');
                    closeProgressOnGrid('#CancelPostingMutasiKlaimGrid');
                },
            );
        }
    );
}