$(document).ready(function () {
    refreshViewSourceDataGrid();
    proses();
    prosesAll();
});

var selectedRowsData = [];

function dataViewSourceData(){
    return {
        // TipeTransaksi: $("#TipeTransaksi").val(),
        KodeMetode: $("#KodeMetode").val(),
    }
}

function onChangeGridViewSourceData(e) {
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
                PeriodeProses: item.PeriodeProses
            });
        } else if (!isSelected && index !== -1) {
            // If deselected and in our list, remove it
            selectedRowsData.splice(index, 1);
        }
    });
}

function refreshViewSourceDataGrid(){
    $('#btn-refresh-grid').click(function () {
        $("#custom-loader").show();
        $("#ViewSourceDataGrid").getKendoGrid().dataSource.read();
        $("#custom-loader").hide();
    });
}

function proses(){
    $('#btn-proses').click(function () {
        showConfirmation('Confirmation', `Are you sure you want to Process?`,
            function () {
                showProgressOnGrid('#ViewSourceDataGrid');
                if (selectedRowsData.length === 0) {
                    showMessage('Warning', 'Please select at least one row.');
                    return;
                }

                var data = {
                    Datas: selectedRowsData,
                    KodeMetode: $("#KodeMetode").val(),
                };

                ajaxPost("/ProsesCSM/Proses", JSON.stringify(data), (response) => {
                    if (response.Result == "OK") {
                        showMessage('Success', response.Message);
                        refreshGrid("#ViewSourceDataGrid");
                    }
                    else
                        showMessage('Error', response.Message);
                    
                    $("#ViewSourceDataGrid").getKendoGrid().dataSource.read();
                    ajaxGet("/ProsesCSM/GetProgressDone");
                    closeProgressOnGrid('#ViewSourceDataGrid');
                });
            }
        );
    });
}

function prosesAll(){
    $('#btn-proses-all').click(function () {
        showConfirmation('Confirmation', `Are you sure you want to Process All?`,
            function () {
                showProgressOnGrid('#ViewSourceDataGrid');
                var data = {
                    Datas: [],
                    KodeMetode: $("#KodeMetode").val(),
                }
                
                ajaxPost("/ProsesCSM/ProsesAll", JSON.stringify(data), (response) => {
                    if (response.Result == "OK") {
                        showMessage('Success', response.Message);
                    }
                    else
                        showMessage('Error', response.Message);
                    
                    $("#ViewSourceDataGrid").getKendoGrid().dataSource.read();
                    ajaxGet("/ProsesCSM/GetProgressDone");
                    closeProgressOnGrid('#ViewSourceDataGrid');
                });
            }
        );
    });
}

setInterval(() => updateProgressBar(), 1000);

function updateProgressBar() {
    ajaxGet("/ProsesCSM/GetProgress", (result) => {
        let percentage = 0;
        if(result.Remaining > 0 && result.Total > 0)
            percentage = result.Remaining / result.Total * 100;
        let elementProgressBar = document.getElementById("progressBar");
        elementProgressBar.style.width = percentage.toFixed(0) + '%';
        elementProgressBar.innerHTML = percentage.toFixed(0) + '%';

        let elementProgressMessage = document.getElementById("progressBarMessage");
        elementProgressMessage.innerHTML = "Steps " + result.Remaining + " of " + result.Total ;
    })
}