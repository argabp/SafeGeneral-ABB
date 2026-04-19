$(document).ready(function () {
    searchKeyword_OnKeyUp();
});

var selectedRowsData = [];

function searchKeyword_OnKeyUp() {
    $('#SearchKeyword').keyup(function () {
        refreshGrid("#ProsesSpreadingOfRiskGrid");
    });
}

function searchFilterProsesSpreadingOfRisk(e) {
    const gridReq = buildGridRequest(e, "SearchKeyword");

    return {
        grid: gridReq
    };
}

function onChangeGridProsesSpreadingOfRisk(e) {
    var grid = e.sender;
    var selectedIds = grid.selectedKeyNames();

    // Clear our tracker and rebuild based on what's currently in the visible view
    // and what was already selected.
    var currentData = grid.dataSource.view();

    currentData.forEach(item => {
        // // Check if this item's ID is in our global tracker
        // var isFound = selectedRowsData.some(x => x.Id == item.Id);
        //
        // if (isFound) {
        //     // Tell Kendo to select the row visually
        //     grid.select("tr[data-uid='" + item.uid + "']");
        // }
        
        var index = selectedRowsData.findIndex(x => x.Id == item.Id);
        var isSelected = selectedIds.includes(item.Id.toString());

        if (isSelected && index === -1) {
            // If selected and not in our list, add it
            selectedRowsData.push({
                Id: item.Id,
                kd_cb: item.kd_cb,
                kd_cob: item.kd_cob,
                kd_scob: item.kd_scob,
                kd_thn: item.kd_thn,
                no_pol: item.no_pol,
                no_updt: item.no_updt,
                tgl_closing: item.tgl_closing
            });
        } else if (!isSelected && index !== -1) {
            // If deselected and in our list, remove it
            selectedRowsData.splice(index, 1);
        }
    });
}

function prosesSpreadingOfRisk(){
    showConfirmation('Confirmation', `Are you sure you want to Auto SOL?`,
        function () {
            showProgressOnGrid('#ProsesSpreadingOfRiskGrid');

            ajaxPost("/ProsesSpreadingOfRisk/AlokasiReasuransi", JSON.stringify(selectedRowsData),
                function (response) {
                    if(response.Result === "OK"){
                        showMessage("Success", "Proses SOR Reasuransi Selesai!")
                    } else {
                        showMessage('Error', response.Message);
                    }
                    var grid = $("#ProsesSpreadingOfRiskGrid").data("kendoGrid");
                    if (grid) {
                        grid.clearSelection();
                    }
                    selectedRowsData = [];
                    refreshGrid('#ProsesSpreadingOfRiskGrid');
                    closeProgressOnGrid('#ProsesSpreadingOfRiskGrid');
                },
            );
        }
    );
}