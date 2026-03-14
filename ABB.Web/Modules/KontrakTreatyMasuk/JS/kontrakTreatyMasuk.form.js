$(document).ready(async function () {
    btnSaveKontrakTreatyMasuk_Click();
});

function btnSaveKontrakTreatyMasuk_Click() {
    $('#btn-save-kontrakTreatyMasuk').click(function () {
        showProgress('#KontrakTreatyMasukWindow');
        setTimeout(function () {
            saveKontrakTreatyMasuk('/KontrakTreatyMasuk/SaveKontrakTreatyMasuk')
        }, 500);
    });
}

function saveKontrakTreatyMasuk(url){
    var form = getFormData($('#KontrakTreatyMasukForm'));
    
    ajaxPost(url, JSON.stringify(form),
        function (response) {
            refreshGrid("#KontrakTreatyMasukGrid");
            if (response.Result == "OK") {
                showMessage('Success', response.Message);
            } else
                showMessage('Error', response.Message);

            closeProgress("#KontrakTreatyMasukWindow");
            closeWindow("#KontrakTreatyMasukWindow")
        }
    );
}

function OnKodePasChange(e){
    var kd_rk_pas = $("#kd_rk_pas").data("kendoDropDownList");
    kd_rk_pas.dataSource.read({
        kd_grp : e.sender._cascadedValue,
        kd_cb : $("#kd_cb").val().trim(),
    });
}

function OnKodeSbBisChange(e){
    var kd_rk_sb_bis = $("#kd_rk_sb_bis").data("kendoDropDownList");
    kd_rk_sb_bis.dataSource.read({
        kd_grp : e.sender._cascadedValue,
        kd_cb : $("#kd_cb").val().trim(),
    });
}

function OnKodeCabangChange(e){
    var value = e.sender._cascadedValue;
    $("#temp_kd_cb").val(value);
    var kd_rk_pas = $("#kd_rk_pas").data("kendoDropDownList");
    kd_rk_pas.dataSource.read({
        kd_cb : value,
        kd_grp : $("#kd_grp_pas").val().trim(),
    });
    var kd_rk_sb_bis = $("#kd_rk_sb_bis").data("kendoDropDownList");
    kd_rk_sb_bis.dataSource.read({
        kd_cb : value,
        kd_grp : $("#kd_grp_sb_bis").val().trim(),
    });
}

function dataKodePasDropDown(){
    return {
        kd_grp: $("#kd_grp_pas").val().trim(),
        kd_cb: $("#kd_cb").val().trim(),
    }
}

function dataKodeSbBisDropDown(){
    return {
        kd_grp: $("#kd_grp_sb_bis").val().trim(),
        kd_cb: $("#kd_cb").val().trim(),
    }
}