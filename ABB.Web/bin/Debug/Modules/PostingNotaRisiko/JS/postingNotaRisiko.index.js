$(document).ready(function () {
    postingNotaRisiko();
});

function postingNotaRisiko(){
    $('#btn-posting').click(function () {
        var data = {
            TipeTransaksi: $("#TipeTransaksi").val(),
            PeriodeProses: $("#PeriodeProses").val(),
        }
        
        ajaxPost("/PostingNotaRisiko/Posting", JSON.stringify(data), (response) => {
            if (response.Result == "OK") {
                showMessage('Success', response.Message);
            }
            else
                showMessage('Error', response.Message);
        });
    });
}