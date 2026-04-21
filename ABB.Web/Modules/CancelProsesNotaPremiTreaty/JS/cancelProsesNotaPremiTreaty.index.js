$(document).ready(function () {
    cancelProsesNotaPremiTreaty();
});

function cancelProsesNotaPremiTreaty(){
    $('#btn-cancel').click(function () {
        showConfirmation('Confirmation', `Are you sure you want to cancel proses?`,
            function () {
                showProgressByElement($('#CancelProsesNotaPremiTreatyForm'));
                var data = {
                    tgl_proses: $("#tgl_proses").val(),
                    kd_cob: $("#kd_cob").val()
                }

                ajaxPost("/CancelProsesNotaPremiTreaty/Cancel", JSON.stringify(data), (response) => {
                    if (response.Result == "OK") {
                        showMessage('Success', response.Message);
                    } else
                        showMessage('Error', response.Message);
                    closeProgressByElement($('#CancelProsesNotaPremiTreatyForm'));
                });
            });
    });
}