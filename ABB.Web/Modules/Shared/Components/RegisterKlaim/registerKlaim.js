$(document).ready(function () {
    btnSaveRegisterKlaim_Click();
    btnNextRegisterKlaim();
    btnSelectNoPolis();
    showProgress('#RegisterKlaimWindow');
    setTimeout(setRegisterKlaimEditedValue, 3000);
});

function dataKodeSCOBDropDown(){
    return {
        kd_cob: $("#temp_kd_cob").val().trim()
    }
}

function setRegisterKlaimEditedValue(){
    var flag_konv = $("#tempFlag_konv").val();
    flag_konv == "Y" ? $("#flag_konv").prop("checked", true) : $("#flag_konv").prop("checked", false);
    
    $("#kd_scob").data("kendoDropDownList").value($("#temp_kd_scob").val().trim());
    
    if($("#IsEdit").val() === 'True')
    {
        $("#kd_cb").getKendoDropDownList().readonly(true);
        $("#kd_cob").getKendoDropDownList().readonly(true);
        $("#kd_scob").getKendoDropDownList().readonly(true);
        
        $("#btn-select-no-polis").hide();
    }

    closeProgress('#RegisterKlaimWindow');
}

function btnNextRegisterKlaim(){
    $('#btn-next-registerKlaim').click(function () {
        $("#RegisterKlaimTab").getKendoTabStrip().select(1);
    });
}

function btnSelectNoPolis(){
    $('#btn-select-no-polis').click(function () {
        openAkseptasiPolisWindow('/RegisterKlaim/AkseptasiPolis', 'Data Resiko Polis')
    });
}

function btnSaveRegisterKlaim_Click() {
    $('#btn-save-registerKlaim').click(function () {
        showProgress('#RegisterKlaimWindow');
        setTimeout(function () {
            saveRegisterKlaim('/RegisterKlaim/SaveRegisterKlaim')
        }, 500);
    });
}

function setRegisterKlaimModel(model){
    $("#no_kl").val(model.no_kl);
    $("#kd_cb").getKendoDropDownList().readonly(true);
    $("#kd_cob").getKendoDropDownList().readonly(true);
    $("#kd_scob").getKendoDropDownList().readonly(true);
    $("#kd_usr_updt").getKendoDropDownList().value(model.kd_usr_updt);
    $("#kd_usr_input").getKendoDropDownList().value(model.kd_usr_input);
    $("#kd_usr_input_field").getKendoDropDownList().value(model.kd_usr_input);
    $("#tgl_updt").getKendoDatePicker().value(model.tgl_updt);
    $("#tgl_input").getKendoDatePicker().value(model.tgl_input);
    var nomor_register = model.kd_cb.trim() + "." + model.kd_cob.trim() + model.kd_scob.trim() + "." + model.kd_thn + "." + model.no_kl.trim();
    $("#temp_nomor_register").val(nomor_register);
    $("#btn-select-no-polis").hide();
    $("#IsEdit").val("True");
}

function saveRegisterKlaim(url) {
    var form = getFormData($('#RegisterKlaimForm'));
    form.no_pol_lama = $("#no_pol_lama").getKendoMaskedTextBox().raw();
    form.flag_konv = $("#flag_konv")[0].checked ? "Y" : "N";
    var data = JSON.stringify(form);
    ajaxPost(url,  data,
        function (response) {
            refreshGrid("#RegisterKlaimGrid");
            if (response.Result == "OK") {
                showMessage('Success', response.Message);
                
                $("#btn-next-registerKlaim").prop("disabled", false);
                var tabstrip = $('#RegisterKlaimTab').data("kendoTabStrip");
                tabstrip.enable(tabstrip.items()[1]);
                tabstrip.enable(tabstrip.items()[2]);

                if (response.Model != undefined) {
                    setRegisterKlaimModel(response.Model);
                }
            }
            else if (response.Result == "ERROR")
                showMessage("Error", response.Message);
            else
                showMessage("Error", response);

            closeProgress('#RegisterKlaimWindow');
        }
    );
}


function OnTglRegistrasiChange(e){
    if($("#IsEdit").val() === 'True'){
        return;
    }
    
    var data = {
        tgl_reg: kendo.toString(e.sender._value, "MM/dd/yyyy")
    }
    var dataJson = JSON.stringify(data);
    ajaxPostSafely("/RegisterKlaim/GetKodeTahun", dataJson , (returnValue) => {
        $("#kd_thn").val(returnValue.split(",")[1]);
        var nomor_register = $("#kd_cb").val().trim() + "." + $("#kd_cob").val().trim() + $("#kd_scob").val().trim() + "." + returnValue.split(",")[1] + "." + $("#no_kl").val().trim();
        $("#temp_nomor_register").val(nomor_register);
    });
}

function OnJenisRegisterChange(e){
    ajaxGet(`/RegisterKlaim/GetJenisRegister?flag_tty_msk=${e.sender._cascadedValue}` , (returnValue) => {
        $("#flag_pol_lama").data("kendoDropDownList").value(returnValue.split(",")[1]);
    });
}

function OnKodePenyebabChange(e){
    ajaxGet(`/RegisterKlaim/GetSebabKerugian?kd_cob=${$("#kd_cob").val()}&kd_sebab=${e.sender._cascadedValue}` , (returnValue) => {
        if(returnValue[0] != null) {
            $("#sebab_kerugian").data("kendoTextBox").value(returnValue.split(",")[1]);
        }
    });
}

function OnKodeCabangChange(e){
    var value = e.sender._cascadedValue;
    $("#temp_kd_cb").val(value);
    var nomor_register = value.trim() + "." + $("#kd_cob").val().trim() + $("#kd_scob").val().trim() + "." + $("#kd_thn").val().trim() + "." + $("#no_kl").val().trim();
    $("#temp_nomor_register").val(nomor_register);
}

function OnKodeCOBChange(e){
    var value = e.sender._cascadedValue;
    $("#temp_kd_cob").val(value);
    var kd_scob = $("#kd_scob").data("kendoDropDownList");
    kd_scob.dataSource.read({kd_cob : e.sender._cascadedValue});
    var kd_sebab = $("#kd_sebab").data("kendoDropDownList");
    kd_sebab.dataSource.read({kd_cob : e.sender._cascadedValue});
    var nomor_register = $("#kd_cb").val().trim() + "." + value.trim() + $("#kd_scob").val().trim() + "." + $("#kd_thn").val().trim() + "." + $("#no_kl").val().trim();
    $("#temp_nomor_register").val(nomor_register);
    
    getDocumentNames();
    initDokumenRegisterKlaimGrid();
    loadDokumenRegisterKlaimDS();
}

function OnKodeSCOBChange(e){
    var value = e.sender._cascadedValue;
    $("#temp_kd_scob").val(value);
    var nomor_register = $("#kd_cb").val().trim() + "." + $("#kd_cob").val().trim() + value.trim() + "." + $("#kd_thn").val().trim() + "." + $("#no_kl").val().trim();
    $("#temp_nomor_register").val(nomor_register);
}

function OnJenisPenyelesaianChange(e){
    var value = e.sender._cascadedValue;
    
    if(value.trim() == "E"){
        $("#div_kd_pas").show();
    } else {
        $("#kd_pas").getKendoTextBox().value("");
        $("#div_kd_pas").hide();
    }

    var data = {
        kd_cb: $("#kd_cb").val(),
        kd_cob: $("#kd_cob").val(),
        kd_scob: $("#kd_scob").val(),
        kd_thn: $("#kd_thn").val(),
        no_pol: $("#no_pol").val(),
        no_updt: $("#no_updt").val(),
        no_rsk: $("#no_rsk").val()
    }

    var dataJson = JSON.stringify(data);
    ajaxPostSafely("/RegisterKlaim/GetKeterangan", dataJson , (returnValue) => {
        if(returnValue != null) {
            $("#kond_ptg").getKendoTextBox().value(returnValue.split(",")[1]);
            $("#ket_oby").getKendoTextArea().value(returnValue.split(",")[4]);
        }
    });
}

function OnNoPolisChange(e){
    var data = {
        kd_cb: $("#kd_cb").val(),
        kd_cob: $("#kd_cob").val(),
        kd_scob: $("#kd_scob").val(),
        kd_thn: $("#kd_thn").val(),
        no_kl: $("#no_kl").val(),
        no_pol: $("#no_pol").val(),
        no_updt: $("#no_updt").val(),
        tgl_kej: kendo.toString($("#tgl_kej").val(), "MM/dd/yyyy")
    }
    
    GetTanggalDanBuktiLunas(data);
}

function openAkseptasiPolisWindow(url, title) {
    openWindow('#ApprovalWindow', url, title);
}

function OnNoUpdtChange(e){
    var data = {
        kd_cb: $("#kd_cb").val(),
        kd_cob: $("#kd_cob").val(),
        kd_scob: $("#kd_scob").val(),
        kd_thn: $("#kd_thn").val(),
        no_kl: $("#no_kl").val(),
        no_updt: e.sender.value(),
        no_pol: $("#no_pol").val(),
        tgl_kej: kendo.toString($("#tgl_kej").val(), "MM/dd/yyyy")
    }
    
    GetTanggalDanBuktiLunas(data);
}

function GetTanggalDanBuktiLunas(data){
    var dataJson = JSON.stringify(data);
    showProgress('#RegisterKlaimWindow');
    ajaxPostSafely("/RegisterKlaim/GetTanggalDanBuktiLunas", dataJson , (returnValue) => {
        if(returnValue != null) {
            if(returnValue.includes(",")){
                var tgl_lns_prm = returnValue.split(",")[1];
                
                if(tgl_lns_prm.includes("/")){
                    tgl_lns_prm = tgl_lns_prm.replaceAll("/", "-");
                }
                
                $("#tgl_lns_prm").getKendoDatePicker().value(tgl_lns_prm);
                $("#no_bukti_lns").getKendoTextBox().value(returnValue.split(",")[4]);
            } else {
                showMessage("Error", returnValue);

                $("#tgl_lns_prm").getKendoDatePicker().value("0-0-0");
                $("#no_bukti_lns").getKendoTextBox().value("");
            }
        } else{
            $("#tgl_lns_prm").getKendoDatePicker().value("0-0-0");
            $("#no_bukti_lns").getKendoTextBox().value("");
        }
        closeProgress('#RegisterKlaimWindow');
    });
}