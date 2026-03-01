$(document).ready(function () {
    searchKeyword_OnKeyUp();
});

var selectedRowsData = [];

function searchKeyword_OnKeyUp() {
    $('#SearchKeyword').keyup(function () {
        refreshGrid("#CancelPostingNotaKlaimReasuransiGrid");
    });
}

function searchFilterCancelPostingNotaKlaimReasuransi(e) {
    const gridReq = buildGridRequest(e, "SearchKeyword");

    return {
        grid: gridReq
    };
}

function onChangeGridCancelPostingNotaKlaimReasuransi(e) {
    var grid = e.sender;
    var selectedIds = grid.selectedKeyNames();

    // Clear our tracker and rebuild based on what's currently in the visible view
    // and what was already selected.
    var currentData = grid.dataSource.view();

    currentData.forEach(item => {
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
                no_kl: item.no_kl,
                no_mts: item.no_mts,
            });
        } else if (!isSelected && index !== -1) {
            // If deselected and in our list, remove it
            selectedRowsData.splice(index, 1);
        }
    });
}

function cancelPostingNotaKlaimReasuransi(){
    showConfirmation('Confirmation', `Are you sure you want to cancel?`,
        function () {
            showProgressOnGrid('#CancelPostingNotaKlaimReasuransiGrid');

            ajaxPost("/CancelPostingNotaKlaimReasuransi/Cancel", JSON.stringify(selectedRowsData),
                function (response) {
                    if(response.Status === "OK"){
                        showMessage("Success", "Cancel Sukses")
                    } else {
                        showMessage('Error', response.Message);
                    }
                    var grid = $("#CancelPostingNotaKlaimReasuransiGrid").data("kendoGrid");
                    if (grid) {
                        grid.clearSelection();
                    }
                    selectedRowsData = [];
                    refreshGrid('#CancelPostingNotaKlaimReasuransiGrid');
                    closeProgressOnGrid('#CancelPostingNotaKlaimReasuransiGrid');
                },
            );
        }
    );
}

function postingAccounting(){
    showConfirmation('Confirmation', `Are you sure you want to posting?`,
        function () {
            showProgressOnGrid('#CancelPostingNotaKlaimReasuransiGrid');

            ajaxPost("/CancelPostingNotaKlaimReasuransi/Posting", JSON.stringify(selectedRowsData),
                function (response) {
                    if(response.Status === "OK"){
                        showMessage("Success", "Posting Accounting Sukses")
                    } else {
                        showMessage('Error', response.Message);
                    }
                    var grid = $("#CancelPostingNotaKlaimReasuransiGrid").data("kendoGrid");
                    if (grid) {
                        grid.clearSelection();
                    }
                    selectedRowsData = [];
                    refreshGrid('#CancelPostingNotaKlaimReasuransiGrid');
                    closeProgressOnGrid('#CancelPostingNotaKlaimReasuransiGrid');
                },
            );
        }
    );
}