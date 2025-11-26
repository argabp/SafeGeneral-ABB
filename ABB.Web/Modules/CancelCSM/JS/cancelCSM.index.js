$(document).ready(function () {
    refreshViewSourceDataCancelGrid();
    proses();
    prosesAll();
});

function dataViewSourceDataCancel(){
    return {
        // TipeTransaksi: $("#TipeTransaksi").val(),
        KodeMetode: $("#KodeMetode").val(),
    }
}

function refreshViewSourceDataCancelGrid(){
    $('#btn-refresh-grid').click(function () {
        $("#custom-loader").show();
        $("#ViewSourceDataCancelGrid").getKendoGrid().dataSource.read();
        $("#custom-loader").hide();
    });
}

function proses(){
    $('#btn-proses').click(function () {
        let selectedKeys = $("#ViewSourceDataCancelGrid").getKendoGrid().selectedKeyNames();
        var data = {
            Id: selectedKeys,
            // TipeTransaksi: $("#TipeTransaksi").val(),
            KodeMetode: $("#KodeMetode").val(),            
        }
        ajaxPost("/CancelCSM/Proses", JSON.stringify(data), (response) => {
            if (response.Result == "OK") {
                showMessage('Success', response.Message);
                refreshGrid("#ViewSourceDataCancelGrid");
            }
            else
                showMessage('Error', response.Message);
            
            $("#ViewSourceDataCancelGrid").getKendoGrid().dataSource.read();
            ajaxGet("/CancelCSM/GetProgressDone");
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
        ajaxPost("/CancelCSM/ProsesAll", JSON.stringify(data), (response) => {
            if (response.Result == "OK") {
                showMessage('Success', response.Message);
            }
            else
                showMessage('Error', response.Message);
            
            $("#ViewSourceDataCancelGrid").getKendoGrid().dataSource.read();
            ajaxGet("/CancelCSM/GetProgressDone");
        });
    });
}

setInterval(() => updateProgressBar(), 1000);

function updateProgressBar() {
    ajaxGet("/CancelCSM/GetProgress", (result) => {
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