$(document).ready(function () {
    searchKeyword_OnKeyUp();
});

function searchKeyword_OnKeyUp() {
    $('#SearchKeyword').keyup(function () {
        refreshGrid("#AkseptasiGrid");
    });
}

function postingPolis(){
    showConfirmation('Confirmation', `Are you sure you want to posting?`,
        function () {
            showProgressOnGrid('#AkseptasiGrid');
            // Get the Kendo UI Grid instance
            var grid = $("#AkseptasiGrid").data("kendoGrid");

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
                    no_pol: dataItem.no_pol,
                    no_updt: dataItem.no_updt,
                    no_pol_ttg: dataItem.no_pol_ttg,
                    tgl_closing: dataItem.tgl_closing,
                    nm_ttg: dataItem.nm_ttg,
                    kd_usr_posting: dataItem.kd_usr_posting
                });
            });

            ajaxPost("/PostingPolis/Posting", JSON.stringify(selectedData),
                function (response) {
                    if(response.Status === "OK"){
                        showMessage("Success", "Posting Sukses")
                    } else {
                        showMessage('Error', response.Message);
                    }
                    closeProgressOnGrid('#AkseptasiGrid');
                },
            );
        }
    );
}