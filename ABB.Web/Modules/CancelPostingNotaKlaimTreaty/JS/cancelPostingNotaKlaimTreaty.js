$(document).ready(function () {
    searchKeyword_OnKeyUp();
});

var selectedRowsData = [];

function searchKeyword_OnKeyUp() {
    $('#SearchKeyword').keyup(function () {
        refreshGrid("#CancelPostingNotaKlaimTreatyGrid");
    });
}

function searchFilterCancelPostingNotaKlaimTreaty(e) {
    const gridReq = buildGridRequest(e, "SearchKeyword");

    return {
        grid: gridReq
    };
}

function onChangeGridCancelPostingNotaKlaimTreaty(e) {
    var grid = e.sender;
    var selectedIds = grid.selectedKeyNames();

    // Clear our tracker and rebuild based on what's currently in the visible view
    // and what was already selected.
    var currentData = grid.dataSource.view();

    currentData.forEach(item => {
        var index = selectedRowsData.findIndex(x => x.nomor_nota == item.nomor_nota);
        var isSelected = selectedIds.includes(item.nomor_nota.toString());

        if (isSelected && index === -1) {
            // If selected and not in our list, add it
            selectedRowsData.push({
                nomor_nota: item.nomor_nota,
                kd_cb: item.kd_cb,
                jns_tr: item.jns_tr,
                jns_nt_msk: item.jns_nt_msk,
                kd_thn: item.kd_thn,
                kd_bln: item.kd_bln,
                no_nt_msk: item.no_nt_msk,
                jns_nt_kel: item.jns_nt_kel,
                no_nt_kel: item.no_nt_kel
            });
        } else if (!isSelected && index !== -1) {
            // If deselected and in our list, remove it
            selectedRowsData.splice(index, 1);
        }
    });
}

function cancelPostingNotaKlaimTreaty(){
    showConfirmation('Confirmation', `Are you sure you want to cancel?`,
        function () {
            showProgressOnGrid('#CancelPostingNotaKlaimTreatyGrid');

            ajaxPost("/CancelPostingNotaKlaimTreaty/Cancel", JSON.stringify(selectedRowsData),
                function (response) {
                    if(response.Status === "OK"){
                        showMessage("Success", "Cancel Sukses")
                    } else {
                        showMessage('Error', response.Message);
                    }
                    var grid = $("#CancelPostingNotaKlaimTreatyGrid").data("kendoGrid");
                    if (grid) {
                        grid.clearSelection();
                    }
                    selectedRowsData = [];
                    refreshGrid('#CancelPostingNotaKlaimTreatyGrid');
                    closeProgressOnGrid('#CancelPostingNotaKlaimTreatyGrid');
                },
            );
        }
    );
}

function postingAccounting(){
    showConfirmation('Confirmation', `Are you sure you want to posting?`,
        function () {
            showProgressOnGrid('#CancelPostingNotaKlaimTreatyGrid');

            ajaxPost("/CancelPostingNotaKlaimTreaty/Posting", JSON.stringify(selectedRowsData),
                function (response) {
                    if(response.Status === "OK"){
                        showMessage("Success", "Posting Accounting Sukses")
                    } else {
                        showMessage('Error', response.Message);
                    }
                    var grid = $("#CancelPostingNotaKlaimTreatyGrid").data("kendoGrid");
                    if (grid) {
                        grid.clearSelection();
                    }
                    selectedRowsData = [];
                    refreshGrid('#CancelPostingNotaKlaimTreatyGrid');
                    closeProgressOnGrid('#CancelPostingNotaKlaimTreatyGrid');
                },
            );
        }
    );
}