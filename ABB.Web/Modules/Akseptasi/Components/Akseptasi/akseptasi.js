$(document).ready(async function () {
    btnSaveAkseptasi_Click();
    btnNextAkseptasi();
    showProgress('#AkseptasiWindow');
    await setAkseptasiEditedValue();
});

function dataKodeTertanggungDropDown(){
    return {
        kd_grp_rk: $("#temp_kd_grp_ttg").val().trim(),
        kd_cb: $("#temp_kd_cb").val().trim()
    }
}

function dataKodeSumberBisnisDropDown(){
    return {
        kd_grp_rk: $("#temp_kd_grp_sb_bis").val().trim(),
        kd_cb: $("#temp_kd_cb").val().trim(),
        no_fax: "Y"
    }
}

function dataKodeBrokerDropDown(){
    return {
        kd_grp_rk: $("#temp_kd_grp_brk").val().trim(),
        kd_cb: $("#temp_kd_cb").val().trim()
    }
}

function dataKodePershAsuransiDropDown(){
    return {
        kd_grp_rk: $("#temp_kd_grp_pas").val().trim(),
        kd_cb: $("#temp_kd_cb").val().trim()
    }
}

function dataKodeBankDropDown(){
    return {
        kd_grp_rk: $("#temp_kd_grp_pas").val().trim(),
        kd_cb: $("#temp_kd_cb").val().trim()
    }
}

function dataKodeMarketingDropDown(){
    return {
        kd_grp_rk: $("#temp_kd_grp_mkt").val().trim(),
        kd_cb: $("#temp_kd_cb").val().trim()
    }
}

function dataKodeSCOBDropDown(){
    return {
        kd_cob: $("#temp_kd_cob").val().trim()
    }
}

async function setAkseptasiEditedValue(){
    showProgress('#AkseptasiWindow');
    var flag_konv = $("#tempFlag_konv").val();
    flag_konv == "Y" ? $("#flag_konv").prop("checked", true) : $("#flag_konv").prop("checked", false);
    
    await restoreDropdownValue("#kd_rk_ttg", "#temp_kd_rk_ttg");
    await restoreDropdownValue("#kd_rk_sb_bis", "#temp_kd_rk_sb_bis");
    await restoreDropdownValue("#kd_rk_brk", "#temp_kd_rk_brk");
    await restoreDropdownValue("#kd_rk_pas", "#kd_rk_pas");
    await restoreDropdownValue("#kd_rk_bank", "#kd_rk_bank");
    await restoreDropdownValue("#kd_rk_mkt", "#temp_kd_rk_mkt");
    await restoreDropdownValue("#kd_scob", "#temp_kd_scob");
    
    if($("#IsEdit").val() === 'True')
    {
        $("#kd_cb").getKendoDropDownList().readonly(true);
        $("#kd_cob").getKendoDropDownList().readonly(true);
        $("#kd_scob").getKendoDropDownList().readonly(true);
    }

    closeProgress('#AkseptasiWindow');
}

function btnNextAkseptasi(){
    $('#btn-next-akseptasi').click(function () {
        $("#akseptasiTab").getKendoTabStrip().select(1);
    });
}

function btnSaveAkseptasi_Click() {
    $('#btn-save-akseptasi').click(function () {
        showProgress('#AkseptasiWindow');
        setTimeout(function () {
            saveAkseptasi('/Akseptasi/SaveAkseptasi')
        }, 500);
    });
}

function setAkseptasiModel(model){
    $("#no_endt").val(model.no_endt);
    $("#link_file").val(model.link_file);
    $("#no_pol_ttg").val(model.no_pol_ttg);
    $("#no_aks").val(model.no_aks);
    $("#kd_cb").getKendoDropDownList().readonly(true);
    $("#kd_cob").getKendoDropDownList().readonly(true);
    $("#kd_scob").getKendoDropDownList().readonly(true);
    $("#kd_usr_updt").getKendoTextBox().value(model.kd_usr_updt);
    $("#kd_usr_input").getKendoTextBox().value(model.kd_usr_input);
    $("#tgl_updt").getKendoDatePicker().value(model.tgl_updt);
    $("#tgl_input").getKendoDatePicker().value(model.tgl_input);
    var no_aks = model.kd_cb.trim() + "." + model.kd_cob.trim() + model.kd_scob.trim() + "." + model.kd_thn + "." + model.no_aks.trim();
    $("#temp_nomor_akseptasi").val(no_aks);
}

function saveAkseptasi(url) {
    var form = new FormData($('#AkseptasiForm')[0]);
    form.append("flag_konv", $("#flag_konv")[0].checked ? "Y" : "N");

    $("#linkFile").getKendoUpload().getFiles().forEach((data, index) => {
        form.append("file", data.rawFile);
    });

    ajaxUpload(url, form,
        function (response) {
            refreshGrid("#AkseptasiGrid");
            if (response.Result == "OK") {
                showMessage('Success', response.Message);
                
                $("#btn-next-akseptasi").prop("disabled", false);
                var tabstrip = $('#akseptasiTab').data("kendoTabStrip");
                tabstrip.enable(tabstrip.items()[1]);
                tabstrip.enable(tabstrip.items()[2]);

                if (response.Model != undefined) {
                    setAkseptasiModel(response.Model);
                }
            }
            else if (response.Result == "ERROR")
                showMessage("Error", response.Message);
            else
                showMessage("Error", response);

            closeProgress('#AkseptasiWindow');
        }
    );
}


function OnTglMulChange(e){
    var data = {
        tgl_mul_ptg: kendo.toString(e.sender._value, "MM/dd/yyyy")
    }
    var dataJson = JSON.stringify(data);
    ajaxPostSafely("/Akseptasi/GetTahunUnderwriting", dataJson , (returnValue) => {
        $("#thn_uw").getKendoNumericTextBox().value(returnValue);
    });
}

function OnTglAkhChange(e){
    var data = {
        tgl_mul_ptg: $("#tgl_mul_ptg").val(),
        tgl_akh_ptg: kendo.toString(e.sender._value, "MM/dd/yyyy"),
        kd_cob: $("#kd_cob").val()
    }
    var dataJson = JSON.stringify(data);
    ajaxPostSafely("/Akseptasi/GetJangkaWaktuPertanggungan", dataJson , (returnValue) => {
        $("#jk_wkt_ptg").getKendoNumericTextBox().value(returnValue);
    });
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
    var value = e.sender._cascadedValue;
    $("#temp_kd_cb").val(value);
    var no_aks = value.trim() + "." + $("#kd_cob").val().trim() + $("#kd_scob").val().trim() + "." + $("#kd_thn").val().trim() + "." + $("#no_aks").val().trim();
    $("#temp_nomor_akseptasi").val(no_aks);
}

function OnKodeCOBChange(e){
    var value = e.sender._cascadedValue;
    $("#temp_kd_cob").val(value);
    var kd_scob = $("#kd_scob").data("kendoDropDownList");
    kd_scob.dataSource.read({kd_cob : e.sender._cascadedValue});
    var no_aks = $("#kd_cb").val().trim() + "." + value.trim() + $("#kd_scob").val().trim() + "." + $("#kd_thn").val().trim() + "." + $("#no_aks").val().trim();
    $("#temp_nomor_akseptasi").val(no_aks);
    
    if(value == "E"){
        $("#RekayasaDiv").show();
    } else {
        $("#RekayasaDiv").hide();
    }
}

function OnKodeSCOBChange(e){
    var value = e.sender._cascadedValue;
    $("#temp_kd_scob").val(value);
    var no_aks = $("#kd_cb").val().trim() + "." + $("#kd_cob").val().trim() + value.trim() + "." + $("#kd_thn").val().trim() + "." + $("#no_aks").val().trim();
    $("#temp_nomor_akseptasi").val(no_aks);
}

function OnPolisIndukChange(e){
    ajaxGet("/Akseptasi/GetPolisInduk?no_pol_induk=" + e.sender._cascadedValue, (returnValue) => {
        $("#almt_ttg").getKendoTextArea().value(returnValue[0].split(",")[1]);
        $("#ctt_pol").getKendoTextArea().value(returnValue[1].split(",")[1]);
        $("#desk_deduct").getKendoTextArea().value(returnValue[2].split(",")[1]);
        $("#faktor_prd").getKendoNumericTextBox().value(returnValue[3].split(",")[1]);
        returnValue[4].split(",")[1] == "Y" ? $("#flag_konv").prop("checked", true) : $("#flag_konv").prop("checked", false);
        $("#kd_cb").getKendoDropDownList().value(returnValue[5].split(",")[1]);
        $("#kd_cb").getKendoDropDownList().trigger("change");
        $("#kd_cob").getKendoDropDownList().value(returnValue[6].split(",")[1]);
        $("#kd_cob").getKendoDropDownList().trigger("change");
        $("#kd_grp_bank").getKendoDropDownList().value(returnValue[7].split(",")[1]);
        $("#kd_grp_bank").getKendoDropDownList().trigger("change");
        $("#kd_grp_brk").getKendoDropDownList().value(returnValue[8].split(",")[1]);
        $("#kd_grp_brk").getKendoDropDownList().trigger("change");
        $("#kd_grp_mkt").getKendoDropDownList().value(returnValue[9].split(",")[1]);
        $("#kd_grp_mkt").getKendoDropDownList().trigger("change");
        $("#kd_grp_pas").getKendoDropDownList().value(returnValue[10].split(",")[1]);
        $("#kd_grp_pas").getKendoDropDownList().trigger("change");
        $("#kd_grp_sb_bis").getKendoDropDownList().value(returnValue[11].split(",")[1]);
        $("#kd_grp_sb_bis").getKendoDropDownList().trigger("change");
        $("#kd_grp_ttg").getKendoDropDownList().value(returnValue[12].split(",")[1]);
        $("#kd_grp_ttg").getKendoDropDownList().trigger("change");
        $("#kd_thn").val(returnValue[20].split(",")[1]);
        $("#ket_klausula").getKendoTextArea().value(returnValue[21].split(",")[1]);
        $("#kt_ttg").getKendoTextBox().value(returnValue[22].split(",")[1]);
        $("#lamp_pol").getKendoTextArea().value(returnValue[23].split(",")[1]);
        $("#nm_qq").getKendoTextBox().value(returnValue[24].split(",")[1]);
        $("#nm_ttg").getKendoTextBox().value(returnValue[25].split(",")[1]);
        $("#no_pol_pas").getKendoTextBox().value(returnValue[26].split(",")[1]);
        $("#pst_share_bgu").getKendoNumericTextBox().value(returnValue[27].split(",")[1]);
        $("#st_pas").getKendoDropDownList().value(returnValue[28].split(",")[1]);
        $("#thn_uw").getKendoNumericTextBox().value(returnValue[29].split(",")[1]);
        $("#wpc").getKendoNumericTextBox().value(returnValue[30].split(",")[1]);
        
        setTimeout(() => {
            $("#kd_rk_bank").getKendoDropDownList().value(returnValue[13].split(",")[1]);
            $("#kd_rk_brk").getKendoDropDownList().value(returnValue[14].split(",")[1]);
            $("#kd_rk_mkt").getKendoDropDownList().value(returnValue[15].split(",")[1]);
            $("#kd_rk_pas").getKendoDropDownList().value(returnValue[16].split(",")[1]);
            $("#kd_rk_sb_bis").getKendoDropDownList().value(returnValue[17].split(",")[1]);
            $("#kd_rk_ttg").getKendoDropDownList().value(returnValue[18].split(",")[1]);
            $("#kd_scob").getKendoDropDownList().value(returnValue[19].split(",")[1]);
            $("#kd_scob").getKendoDropDownList().trigger("change");
        }, 500);
    });
}

function OnKodeRekananTertanggungChange(e){
    ajaxGet(`/Akseptasi/GetKodeRekananTertanggung?kd_cb=${$("#kd_cb").val()}&kd_grp_rk=${$("#kd_grp_ttg").val()}&kd_rk=${e.sender._cascadedValue}`, (returnValue) => {
        var strings = returnValue.split(",");
        $("#nm_ttg").getKendoTextBox().value(strings[1]);
        $("#almt_ttg").getKendoTextArea().value(strings[4]);
        $("#kt_ttg").getKendoTextBox().value(strings[7]);
    });
}

function OnKodeRekananSumberBisnisChange(e){
    ajaxGet(`/Akseptasi/GetKodeAkseptasi?st_pas=${$("#st_pas").val()}&kd_grp_sb_bis=${$("#kd_grp_sb_bis").val()}&kd_rk_sb_bis=${e.sender._cascadedValue}&no_fax="Y"`, (returnValue) => {        
        $("#kd_grp_pas").getKendoDropDownList().value(returnValue[2].split(",")[1]);
        $("#kd_grp_pas").getKendoDropDownList().trigger("change");
        $("#kd_grp_brk").getKendoDropDownList().value(returnValue[1].split(",")[1]);
        $("#kd_grp_brk").getKendoDropDownList().trigger("change");
        $("#kd_grp_bank").getKendoDropDownList().value(returnValue[0].split(",")[1]);
        $("#kd_grp_bank").getKendoDropDownList().trigger("change");
        
        setTimeout(() => {
            $("#kd_rk_pas").getKendoDropDownList().value(returnValue[6].split(",")[1]);
            $("#kd_rk_brk").getKendoDropDownList().value(returnValue[4].split(",")[1]);
            $("#kd_rk_bank").getKendoDropDownList().value(returnValue[3].split(",")[1]);
        }, 500);
        
    });
}

function OnJkWktMainChange(e){
    ajaxGet(`/Akseptasi/GetTglAkhPtg?jk_wkt_main=${e.sender.value()}&tgl_akh_ptg=${$("#tgl_akh_ptg").val()}`, (returnValue) => {        
        $("#tgl_akh_ptg").getKendoDatePicker().value(new Date(returnValue.split(",")[1]));
        $("#tgl_akh_ptg").getKendoDatePicker().trigger("change");
    });
}