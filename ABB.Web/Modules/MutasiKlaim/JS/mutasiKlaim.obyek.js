$(document).ready(function () {
    btnSaveMutasiKlaimObyek_Click();
});

function btnSaveMutasiKlaimObyek_Click() {
    $('#btn-save-mutasiKlaimObyek').click(function () {
        showProgress('#MutasiKlaimWindow');
        setTimeout(function () {
            saveMutasiKlaimObyek('/MutasiKlaim/SaveMutasiKlaimObyek')
        }, 500);
    });
}

function saveMutasiKlaimObyek(url) {
    var form = getFormData($('#MutasiKlaimObyekForm'));
    
    var parentId =
        form.kd_cb.trim() +
        form.kd_cob.trim() +
        form.kd_scob.trim() +
        form.kd_thn.trim() +
        form.no_kl.trim() 

    var mutasiGridName = "grid_obyek_" + parentId;
    var mutasiGridElement = $("#" + mutasiGridName);
    
    var data = JSON.stringify(form);
    ajaxPost(url,  data,
        function (response) {
            refreshGrid(mutasiGridElement);
            if (response.Result == "OK") {
                showMessage('Success', response.Message);
            }
            else if (response.Result == "ERROR")
                showMessage("Error", response.Message);
            else
                showMessage("Error", response);

            closeProgress('#MutasiKlaimWindow');
            closeWindow('#MutasiKlaimWindow')
        }
    );
}
