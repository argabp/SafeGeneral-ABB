$(document).ready(function () {
    searchKeyword_OnKeyUp();
    var kd_rk_ttg = $("#kd_rk_ttg").data("kendoDropDownList");
    kd_rk_ttg.dataSource.read({kd_grp_rk : $("#kd_grp_ttg").val(), kd_cb: $("#kd_cb").val()});
    var kd_rk_sb_bis = $("#kd_rk_sb_bis").data("kendoDropDownList");
    kd_rk_sb_bis.dataSource.read({kd_grp_rk : $("#kd_grp_sb_bis").val(), kd_cb: $("#kd_cb").val()});
    var kd_rk_brk = $("#kd_rk_brk").data("kendoDropDownList");
    kd_rk_brk.dataSource.read({kd_grp_rk : $("#kd_grp_brk").val(), kd_cb: $("#kd_cb").val()});
    var kd_rk_pas = $("#kd_rk_pas").data("kendoDropDownList");
    kd_rk_pas.dataSource.read({kd_grp_rk : $("#kd_grp_pas").val(), kd_cb: $("#kd_cb").val()});
    var kd_rk_bank = $("#kd_rk_bank").data("kendoDropDownList");
    kd_rk_bank.dataSource.read({kd_grp_rk : $("#kd_grp_bank").val(), kd_cb: $("#kd_cb").val()});
    var kd_rk_mkt = $("#kd_rk_mkt").data("kendoDropDownList");
    kd_rk_mkt.dataSource.read({kd_grp_rk : $("#kd_grp_mkt").val(), kd_cb: $("#kd_cb").val()});
    var kd_scob = $("#kd_scob").data("kendoDropDownList");
    kd_scob.dataSource.read({kd_cob : $("#kd_cob").val()});
});

function OnTglMulChange(e){
    var data = {
        tgl_mul_ptg: kendo.toString(e.sender._value, "MM/dd/yyyy")
    }
    var dataJson = JSON.stringify(data);
    ajaxPostSafely("/PolisInduk/GetTahunUnderwriting", dataJson , (returnValue) => {
        $("#thn_uw").val(returnValue);
    });
}

function OnTglAkhChange(e){
    var data = {
        tgl_mul_ptg: $("#tgl_mul_ptg").val(),
        tgl_akh_ptg: kendo.toString(e.sender._value, "MM/dd/yyyy"),
        kd_cob: $("#kd_cob").val()
    }
    var dataJson = JSON.stringify(data);
    ajaxPostSafely("/PolisInduk/GetJangkaWaktuPertanggungan", dataJson , (returnValue) => {
        $("#jk_wkt_ptg").val(returnValue);
    });
}

function savePolisInduk(url) {
    var form = getFormData($('#PolisIndukForm'));
    form.flag_konv = $("#flag_konv")[0].checked ? "Y" : "N";
    var data = JSON.stringify(form);
    ajaxPostSafely(url, data,
        function (response) {
            refreshGrid("#PolisIndukGrid");
            if (response.Result == "OK") {
                showMessage('Success', response.Message);
                closeWindow('#PolisIndukWindow');
            }
            else if (response.Result == "ERROR")
                $("#PolisIndukWindow").html(response.Message);
            else
                $("#PolisIndukWindow").html(response);

            closeProgress('#PolisIndukWindow');
        }
    );
}

function OnKodeCabangChange(e){
    var kd_rk_ttg = $("#kd_rk_ttg").data("kendoDropDownList");
    kd_rk_ttg.dataSource.read({kd_grp_rk : $("#kd_grp_ttg").val(), kd_cb: e.sender._cascadedValue});
    var kd_rk_sb_bis = $("#kd_rk_sb_bis").data("kendoDropDownList");
    kd_rk_sb_bis.dataSource.read({kd_grp_rk : $("#kd_grp_sb_bis").val(), kd_cb: e.sender._cascadedValue});
    var kd_rk_brk = $("#kd_rk_brk").data("kendoDropDownList");
    kd_rk_brk.dataSource.read({kd_grp_rk : $("#kd_grp_brk").val(), kd_cb: e.sender._cascadedValue});
    var kd_rk_pas = $("#kd_rk_pas").data("kendoDropDownList");
    kd_rk_pas.dataSource.read({kd_grp_rk : $("#kd_grp_pas").val(), kd_cb: e.sender._cascadedValue});
    var kd_rk_bank = $("#kd_rk_bank").data("kendoDropDownList");
    kd_rk_bank.dataSource.read({kd_grp_rk : $("#kd_grp_bank").val(), kd_cb: e.sender._cascadedValue});
    var kd_rk_mkt = $("#kd_rk_mkt").data("kendoDropDownList");
    kd_rk_mkt.dataSource.read({kd_grp_rk : $("#kd_grp_mkt").val(), kd_cb: e.sender._cascadedValue});
}

function OnKodeCOBChange(e){
    var kd_scob = $("#kd_scob").data("kendoDropDownList");
    kd_scob.dataSource.read({kd_cob : e.sender._cascadedValue});
}

function OnKodeTertanggungChange(e){
    var kd_rk_ttg = $("#kd_rk_ttg").data("kendoDropDownList");
    kd_rk_ttg.dataSource.read({kd_grp_rk : e.sender._cascadedValue, kd_cb: $("#kd_cb").val()});
}

function OnKodeSumberBisnisChange(e){
    var kd_rk_sb_bis = $("#kd_rk_sb_bis").data("kendoDropDownList");
    kd_rk_sb_bis.dataSource.read({kd_grp_rk : e.sender._cascadedValue, kd_cb: $("#kd_cb").val()});
}

function OnKodeBrokerChange(e){
    var kd_rk_brk = $("#kd_rk_brk").data("kendoDropDownList");
    kd_rk_brk.dataSource.read({kd_grp_rk : e.sender._cascadedValue, kd_cb: $("#kd_cb").val()});
}

function OnKodePershAsuransiChange(e){
    var kd_rk_pas = $("#kd_rk_pas").data("kendoDropDownList");
    kd_rk_pas.dataSource.read({kd_grp_rk : e.sender._cascadedValue, kd_cb: $("#kd_cb").val()});
}

function OnKodeBankChange(e){
    var kd_rk_bank = $("#kd_rk_bank").data("kendoDropDownList");
    kd_rk_bank.dataSource.read({kd_grp_rk : e.sender._cascadedValue, kd_cb: $("#kd_cb").val()});
}

function OnKodeMarketingChange(e){
    var kd_rk_mkt = $("#kd_rk_mkt").data("kendoDropDownList");
    kd_rk_mkt.dataSource.read({kd_grp_rk : e.sender._cascadedValue, kd_cb: $("#kd_cb").val()});
}

function OnKodeRekananTertanggungChange(e){
    ajaxGet(`/PolisInduk/GetKodeRekananTertanggung?kd_cb=${$("#kd_cb").val()}&kd_grp_rk=${$("#kd_grp_ttg").val()}&kd_rk=${e.sender._cascadedValue}`, (returnValue) => {
        var strings = returnValue.split(",");
        $("#nm_ttg").getKendoTextBox().value(strings[1]);
        $("#almt_ttg").getKendoTextArea().value(strings[4]);
        $("#kt_ttg").getKendoTextBox().value(strings[7]);
    });
}