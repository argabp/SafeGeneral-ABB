$(document).ready(function () {
    refreshViewSourceDataGrid();
    proses();
    prosesAll();
});

function dataViewSourceData(){
    return {
        // TipeTransaksi: $("#TipeTransaksi").val(),
        KodeMetode: $("#KodeMetode").val(),
    }
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
        let selectedKeys = $("#ViewSourceDataGrid").getKendoGrid().selectedKeyNames();
        var data = {
            Id: selectedKeys,
            // TipeTransaksi: $("#TipeTransaksi").val(),
            KodeMetode: $("#KodeMetode").val(),            
        }
        ajaxPost("/ProsesCSM/Proses", JSON.stringify(data), (response) => {
            if (response.Result == "OK") {
                showMessage('Success', response.Message);
                refreshGrid("#ViewSourceDataGrid");
            }
            else
                showMessage('Error', response.Message);
            
            $("#ViewSourceDataGrid").getKendoGrid().dataSource.read();
            ajaxGet("/ProsesCSM/GetProgressDone");
        });
    });
}

function prosesAll(){
    $('#btn-proses-all').click(function () {
        var data = {
            Id: [],
            // TipeTransaksi: $("#TipeTransaksi").val(),
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
        });
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