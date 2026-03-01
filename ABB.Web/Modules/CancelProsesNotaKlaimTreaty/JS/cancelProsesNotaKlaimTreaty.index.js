$(document).ready(function () {
    cancelProsesNotaKlaimTreaty();
});

function cancelProsesNotaKlaimTreaty(){
    $('#btn-cancel').click(function () {
        showConfirmation('Confirmation', `Are you sure you want to cancel proses?`,
            function () {
                var data = {
                    tgl_proses: $("#tgl_proses").val()
                }

                ajaxPost("/CancelProsesNotaKlaimTreaty/Cancel", JSON.stringify(data), (response) => {
                    if (response.Result == "OK") {
                        showMessage('Success', response.Message);
                    } else
                        showMessage('Error', response.Message);
                });
            });
    });
}