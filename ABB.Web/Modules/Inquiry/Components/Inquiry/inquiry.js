$(document).ready(function () {
    btnNextInquiry();
    showProgress('#InquiryWindow');
    setTimeout(setInquiryEditedValue, 3000);
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

function setInquiryEditedValue(){
    var flag_konv = $("#tempFlag_konv").val();
    flag_konv == "Y" ? $("#flag_konv").prop("checked", true) : $("#flag_konv").prop("checked", false);
    
    $("#kd_rk_ttg").data("kendoDropDownList").value($("#temp_kd_rk_ttg").val().trim());
    $("#kd_rk_sb_bis").data("kendoDropDownList").value($("#temp_kd_rk_sb_bis").val().trim());
    $("#kd_rk_brk").data("kendoDropDownList").value($("#temp_kd_rk_brk").val().trim());
    $("#kd_rk_pas").data("kendoDropDownList").value($("#temp_kd_rk_pas").val().trim());
    $("#kd_rk_bank").data("kendoDropDownList").value($("#temp_kd_rk_bank").val().trim());
    $("#kd_rk_mkt").data("kendoDropDownList").value($("#temp_kd_rk_mkt").val().trim());
    $("#kd_scob").data("kendoDropDownList").value($("#temp_kd_scob").val().trim());
    
    if($("#IsEdit").val() === 'True')
    {
        $("#kd_cb").getKendoDropDownList().readonly(true);
        $("#kd_cob").getKendoDropDownList().readonly(true);
        $("#kd_scob").getKendoDropDownList().readonly(true);
    }

    closeProgress('#InquiryWindow');
}

function btnNextInquiry(){
    $('#btn-next-inquiry').click(function () {
        $("#inquiryTab").getKendoTabStrip().select(1);
    });
}

function setInquiryModel(model){
    $("#no_endt").val(model.no_endt);
    $("#link_file").val(model.link_file);
    $("#no_pol_ttg").val(model.no_pol_ttg);
    $("#no_pol").val(model.no_pol);
    $("#kd_cb").getKendoDropDownList().readonly(true);
    $("#kd_cob").getKendoDropDownList().readonly(true);
    $("#kd_scob").getKendoDropDownList().readonly(true);
    var nomor_akseptasi = model.kd_cb.trim() + "." + model.kd_cob.trim() + model.kd_scob.trim() + "." + model.kd_thn + "." + model.no_pol.trim();
    $("#temp_nomor_akseptasi").val(nomor_akseptasi);
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
    var nomor_akseptasi = value.trim() + "." + $("#kd_cob").val().trim() + $("#kd_scob").val().trim() + "." + $("#kd_thn").val().trim() + "." + $("#no_pol").val().trim();
    $("#temp_nomor_akseptasi").val(nomor_akseptasi);
}

function OnKodeCOBChange(e){
    var value = e.sender._cascadedValue;
    $("#temp_kd_cob").val(value);
    var kd_scob = $("#kd_scob").data("kendoDropDownList");
    kd_scob.dataSource.read({kd_cob : e.sender._cascadedValue});
    var nomor_akseptasi = $("#kd_cb").val().trim() + "." + value.trim() + $("#kd_scob").val().trim() + "." + $("#kd_thn").val().trim() + "." + $("#no_pol").val().trim();
    $("#temp_nomor_akseptasi").val(nomor_akseptasi);
    
    if(value == "E"){
        $("#RekayasaDiv").show();
    } else {
        $("#RekayasaDiv").hide();
    }
}

function OnKodeSCOBChange(e){
    var value = e.sender._cascadedValue;
    $("#temp_kd_scob").val(value);
    var nomor_akseptasi = $("#kd_cb").val().trim() + "." + $("#kd_cob").val().trim() + value.trim() + "." + $("#kd_thn").val().trim() + "." + $("#no_pol").val().trim();
    $("#temp_nomor_akseptasi").val(nomor_akseptasi);
}