$(document).ready(function () {
    btnSaveKeteranganEndorsment_Click();
});

function btnSaveKeteranganEndorsment_Click() {
    $('#btn-save-keteranganEndorsment').click(function () {
        showProgress('#AkseptasiWindow');
        setTimeout(function () {
            saveKeteranganEndorsment('/Akseptasi/SaveKeteranganEndorsment')
        }, 500);
    });
}

function saveKeteranganEndorsment(url){
    var form = getFormData($('#KeteranganEndorsmentForm'));

    ajaxPost(url, JSON.stringify(form),
        function (response) {
            refreshGrid("#AksepatasiGrid");
            if (response.Result == "OK") {
                showMessage('Success', response.Message);
                closeProgress("#AkseptasiWindow");
                closeWindow("#AkseptasiWindow")
            } else {
                showMessage('Error', response.Message);

                closeProgress("#AkseptasiWindow");
            }
        }
    );
}