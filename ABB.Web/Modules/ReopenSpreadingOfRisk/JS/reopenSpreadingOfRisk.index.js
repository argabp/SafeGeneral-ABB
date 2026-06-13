$(document).ready(function () {
    searchKeyword_OnKeyUp();
});

var selectedRowsData = [];

function searchKeyword_OnKeyUp() {
    $('#SearchKeyword').keyup(function () {
        refreshGrid("#ReopenSpreadingOfRiskGrid");
    });
}

function onSearchClick() {
    // Cukup perintahkan grid untuk membaca ulang datanya
    $("#ReopenSpreadingOfRiskGrid").data("kendoGrid").dataSource.read();
}

function searchFilterReopenSpreadingOfRisk(e) {
    var startDatePicker = $("#StartDate").data("kendoDatePicker");
    var endDatePicker = $("#EndDate").data("kendoDatePicker");
    var KodeCabang = $("#kd_cb").data("kendoDropDownList");

    return {
        startDate: startDatePicker && startDatePicker.value() ? kendo.toString(startDatePicker.value(), "yyyy-MM-dd") : null,
        endDate: endDatePicker && endDatePicker.value() ? kendo.toString(endDatePicker.value(), "yyyy-MM-dd") : null,
        kodeCabang: KodeCabang && KodeCabang.value() ? KodeCabang.value() : null,
    };
}

function onChangeGridReopenSpreadingOfRisk(e) {
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

function reopenSpreadingOfRisk(){
    showConfirmation('Confirmation', `Are you sure you want to Reopen?`,
        function () {
            showProgressOnGrid('#ReopenSpreadingOfRiskGrid');

            ajaxPost("/ReopenSpreadingOfRisk/Reopen", JSON.stringify(selectedRowsData),
                function (response) {
                    if(response.Result === "OK"){
                        showMessage("Success", "Reopen SOR Reasuransi Selesai!")
                    } else {
                        showMessage('Error', response.Message);
                    }
                    var grid = $("#ReopenSpreadingOfRiskGrid").data("kendoGrid");
                    if (grid) {
                        grid.clearSelection();
                    }
                    selectedRowsData = [];
                    closeProgressOnGrid('#ReopenSpreadingOfRiskGrid');
                    refreshGrid('#ReopenSpreadingOfRiskGrid');
                },
            );
        }
    );
}