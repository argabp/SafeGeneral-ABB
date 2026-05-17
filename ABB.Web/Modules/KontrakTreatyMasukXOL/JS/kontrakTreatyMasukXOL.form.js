$(document).ready(async function () {
    btnSaveKontrakTreatyMasukXOL_Click();
});

function btnSaveKontrakTreatyMasukXOL_Click() {
    $('#btn-save-kontrakTreatyMasukXOL').click(function () {
        showProgress('#KontrakTreatyMasukXOLWindow');
        setTimeout(function () {
            saveKontrakTreatyMasukXOL('/KontrakTreatyMasukXOL/SaveKontrakTreatyMasukXOL')
        }, 500);
    });
}

function saveKontrakTreatyMasukXOL(url){
    var form = getFormData($('#KontrakTreatyMasukXOLForm'));
    
    ajaxPost(url, JSON.stringify(form),
        function (response) {
            refreshGrid("#KontrakTreatyMasukXOLGrid");
            if (response.Result == "OK") {
                showMessage('Success', response.Message);
            } else
                showMessage('Error', response.Message);

            closeProgress("#KontrakTreatyMasukXOLWindow");
            closeWindow("#KontrakTreatyMasukXOLWindow")
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

function OnTahunUnderwritingChange(e){
    var kd_cob = $("#kd_cob").val();
    var kd_jns_sor = $("#kd_jns_sor").data("kendoDropDownList");
    var thn_uw = e.sender.value();

    ajaxGet(`/KontrakTreatyMasukXOL/GetKeteranganTreaty?kd_cob=${kd_cob}&nm_jns_sor=${kd_jns_sor.text()}&thn_uw=${thn_uw}`,
        function (response) {
            if (response.Result == "OK") {
                var desk_tty = response.Data.split(",")[1];
                $("#desk_tty").getKendoTextArea().value(desk_tty);
            }
            else if (response.Result == "ERROR")
                showMessage("Error", response.Message);
            else
                showMessage("Error", response);
        }
    );
}