$(document).ready(function () {
    prosesNotaKlaimTreaty();
});

function prosesNotaKlaimTreaty(){
    $('#btn-proses').click(function () {
        showConfirmation('Confirmation', `Are you sure you want to proses?`,
            function () {
                var data = {
                    tgl_proses: $("#tgl_proses").val()
                }

                ajaxPost("/ProsesNotaKlaimTreaty/Proses", JSON.stringify(data), (response) => {
                    if (response.Result == "OK") {
                        showMessage('Success', response.Message);
                    } else
                        showMessage('Error', response.Message);
                });
            });
    });
}