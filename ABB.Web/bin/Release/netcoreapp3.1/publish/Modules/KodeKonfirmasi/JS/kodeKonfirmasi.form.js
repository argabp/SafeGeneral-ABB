$(document).ready(function () {
    searchKeyword_OnKeyUp();
});

function saveKodeKonfirmasi(url) {
    var form = getFormData($('#KodeKonfirmasiForm'));
    form.flag_polis = $("#flag_polis_induk")[0].checked ? "Y" : "N";
    var data = JSON.stringify(form);
    ajaxPostSafely(url, data,
        function (response) {
            refreshGrid("#KodeKonfirmasiGrid");
            if (response.Result == "OK") {
                showMessage('Success', response.Message);
                // closeWindow('#PolisIndukWindow');
            }
            else if (response.Result == "ERROR")
                $("#KodeKonfirmasiWindow").html(response.Message);
            else
                $("#KodeKonfirmasiWindow").html(response);

            closeProgress('#KodeKonfirmasiWindow');
        }
    );
}

function OnKodeCabangChange(e){
    var value = e.sender._cascadedValue;
    $("#temp_kd_cb").val(value);
}

function OnKodeCOBChange(e){
    var value = e.sender._cascadedValue;
    $("#temp_kd_cob").val(value);
}

function OnKodeSCOBChange(e){
    var value = e.sender._cascadedValue;
    $("#temp_kd_scob").val(value);
}