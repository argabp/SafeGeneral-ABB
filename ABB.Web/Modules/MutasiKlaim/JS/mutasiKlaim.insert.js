$(document).ready(function () {
    btnInsertMutasiKlaim_Click();
});

function btnInsertMutasiKlaim_Click() {
    $('#btn-insert-mutasiKlaim').click(function () {
        showProgress('#MutasiKlaimWindow');
        setTimeout(function () {
            insertMutasiKlaim('/MutasiKlaim/Insert')
        }, 500);
    });
}

function insertMutasiKlaim(url) {
    var form = getFormData($('#InsertMutasiKlaimForm'));
    form.flag_konv = $("#flag_konv")[0].checked ? "Y" : "N";
    
    var parentId =
        form.kd_cb.trim() +
        form.kd_cob.trim() +
        form.kd_scob.trim() +
        form.kd_thn.trim() +
        form.no_kl.trim()

    var mutasiGridName = "grid_mutasi_" + parentId;
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