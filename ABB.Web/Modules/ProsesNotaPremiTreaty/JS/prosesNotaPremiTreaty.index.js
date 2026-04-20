$(document).ready(function () {
    prosesNotaPremiTreaty();
});

function prosesNotaPremiTreaty(){
    $('#btn-proses').click(function () {
        showConfirmation('Confirmation', `Are you sure you want to proses?`,
            function () {
                showProgressByElement($('#ProsesNotaPremiTreatyForm'));
                var data = {
                    tgl_proses: $("#tgl_proses").val(),
                    kd_cob: $("#kd_cob").val(),
                }

                ajaxPost("/ProsesNotaPremiTreaty/Proses", JSON.stringify(data), (response) => {
                    if (response.Result == "OK") {
                        showMessage('Success', response.Message);
                    } else
                        showMessage('Error', response.Message);
                    closeProgressByElement($('#ProsesNotaPremiTreatyForm'));
                });
            });
    });
}