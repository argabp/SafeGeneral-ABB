$(document).ready(function () {
    btnSavePengajuanAkseptasi_Click();
    btnNextPengajuanAkseptasi();
    showProgress('#PengajuanAkseptasiWindow');
    setTimeout(setPengajuanAkseptasiEditedValue, 3000);
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
        kd_cb: $("#temp_kd_cb").val().trim()
    }
}

function dataKodeGrpPas1(){
    return {
        kd_grp_rk: $("#temp_kd_grp_pas1").val().trim(),
        kd_cb: $("#temp_kd_cb").val().trim()
    }
}

function dataKodeGrpPas2(){
    return {
        kd_grp_rk: $("#temp_kd_grp_pas2").val().trim(),
        kd_cb: $("#temp_kd_cb").val().trim()
    }
}

function dataKodeGrpPas3(){
    return {
        kd_grp_rk: $("#temp_kd_grp_pas3").val().trim(),
        kd_cb: $("#temp_kd_cb").val().trim()
    }
}

function dataKodeGrpPas4(){
    return {
        kd_grp_rk: $("#temp_kd_grp_pas4").val().trim(),
        kd_cb: $("#temp_kd_cb").val().trim()
    }
}

function dataKodeGrpPas5(){
    return {
        kd_grp_rk: $("#temp_kd_grp_pas5").val().trim(),
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

function setPengajuanAkseptasiEditedValue(){    
    $("#kd_rk_ttg").data("kendoDropDownList").value($("#temp_kd_rk_ttg").val().trim());
    $("#kd_rk_sb_bis").data("kendoDropDownList").value($("#temp_kd_rk_sb_bis").val().trim());
    $("#kd_rk_pas1").data("kendoDropDownList").value($("#temp_kd_rk_pas1").val().trim());
    $("#kd_rk_pas2").data("kendoDropDownList").value($("#temp_kd_rk_pas2").val().trim());
    $("#kd_rk_pas3").data("kendoDropDownList").value($("#temp_kd_rk_pas3").val().trim());
    $("#kd_rk_pas4").data("kendoDropDownList").value($("#temp_kd_rk_pas4").val().trim());
    $("#kd_rk_pas5").data("kendoDropDownList").value($("#temp_kd_rk_pas5").val().trim());
    $("#kd_rk_mkt").data("kendoDropDownList").value($("#temp_kd_rk_mkt").val().trim());
    $("#kd_scob").data("kendoDropDownList").value($("#temp_kd_scob").val().trim());
    
    if($("#IsEdit").val() === 'True')
    {
        $("#kd_cb").getKendoDropDownList().readonly(true);
        $("#kd_cob").getKendoDropDownList().readonly(true);
        $("#kd_scob").getKendoDropDownList().readonly(true);
    }

    closeProgress('#PengajuanAkseptasiWindow');
}

function btnNextPengajuanAkseptasi(){
    $('#btn-next-pengajuanAkseptasi').click(function () {
        $("#PengajuanAkseptasiTab").getKendoTabStrip().select(1);
    });
}

function btnSavePengajuanAkseptasi_Click() {
    $('#btn-save-pengajuanAkseptasi').click(function () {
        showProgress('#PengajuanAkseptasiWindow');
        setTimeout(function () {
            savePengajuanAkseptasi('/PengajuanAkseptasi/SavePengajuanAkseptasi')
        }, 500);
    });
}

function setPengajuanAkseptasiModel(model){
    $("#kd_cb").getKendoDropDownList().readonly(true);
    $("#kd_cob").getKendoDropDownList().readonly(true);
    $("#kd_scob").getKendoDropDownList().readonly(true);
    $("#kd_thn").val(model.kd_thn);
    $("#no_aks").val(model.no_aks);
    $("#nomor_pengajuan").getKendoTextBox().value(model.nomor_pengajuan);
}

function savePengajuanAkseptasi(url) {
    var form = getFormData($('#PengajuanAkseptasiForm'));
    form.ket_rsk = $("#ket_rsk").getKendoEditor().value();
    var data = JSON.stringify(form);
    ajaxPost(url,  data,
        function (response) {
            refreshGrid("#PengajuanAkseptasiGrid");
            if (response.Result == "OK") {
                showMessage('Success', response.Message);
                
                $("#btn-next-pengajuanAkseptasi").prop("disabled", false);
                var tabstrip = $('#PengajuanAkseptasiTab').data("kendoTabStrip");
                tabstrip.enable(tabstrip.items()[1]);

                if (response.Model != undefined) {
                    setPengajuanAkseptasiModel(response.Model);
                }
                refreshGridLampiranPengajuanAkseptasi();
            }
            else if (response.Result == "ERROR")
                showMessage("Error", response.Message);
            else
                showMessage("Error", response);

            closeProgress('#PengajuanAkseptasiWindow');
        }
    );
}

function OnKodeTertanggungChange(e){
    var kd_rk_ttg = $("#kd_rk_ttg").data("kendoDropDownList");
    kd_rk_ttg.dataSource.read({kd_grp_rk : e.sender._cascadedValue, kd_cb: $("#kd_cb").val()});
}

function OnKodeSumberBisnisChange(e){
    var kd_rk_sb_bis = $("#kd_rk_sb_bis").data("kendoDropDownList");
    kd_rk_sb_bis.dataSource.read({kd_grp_rk : e.sender._cascadedValue, kd_cb: $("#kd_cb").val()});
}

function On_kd_grp_pas1_change(e){
    var kd_rk_pas = $("#kd_rk_pas1").data("kendoDropDownList");
    kd_rk_pas.dataSource.read({kd_grp_rk : e.sender._cascadedValue, kd_cb: $("#kd_cb").val()});
}

function On_kd_grp_pas2_change(e){
    var kd_rk_pas = $("#kd_rk_pas2").data("kendoDropDownList");
    kd_rk_pas.dataSource.read({kd_grp_rk : e.sender._cascadedValue, kd_cb: $("#kd_cb").val()});
}

function On_kd_grp_pas3_change(e){
    var kd_rk_pas = $("#kd_rk_pas3").data("kendoDropDownList");
    kd_rk_pas.dataSource.read({kd_grp_rk : e.sender._cascadedValue, kd_cb: $("#kd_cb").val()});
}

function On_kd_grp_pas4_change(e){
    var kd_rk_pas = $("#kd_rk_pas4").data("kendoDropDownList");
    kd_rk_pas.dataSource.read({kd_grp_rk : e.sender._cascadedValue, kd_cb: $("#kd_cb").val()});
}

function On_kd_grp_pas5_change(e){
    var kd_rk_pas = $("#kd_rk_pas5").data("kendoDropDownList");
    kd_rk_pas.dataSource.read({kd_grp_rk : e.sender._cascadedValue, kd_cb: $("#kd_cb").val()});
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
    var kd_rk_pas1 = $("#kd_rk_pas1").data("kendoDropDownList");
    kd_rk_pas1.dataSource.read({kd_grp_rk : $("#kd_grp_pas1").val(), kd_cb: e.sender._cascadedValue});
    var kd_rk_pas2 = $("#kd_rk_pas2").data("kendoDropDownList");
    kd_rk_pas2.dataSource.read({kd_grp_rk : $("#kd_grp_pas2").val(), kd_cb: e.sender._cascadedValue});
    var kd_rk_pas3 = $("#kd_rk_pas3").data("kendoDropDownList");
    kd_rk_pas3.dataSource.read({kd_grp_rk : $("#kd_grp_pas3").val(), kd_cb: e.sender._cascadedValue});
    var kd_rk_pas4 = $("#kd_rk_pas4").data("kendoDropDownList");
    kd_rk_pas4.dataSource.read({kd_grp_rk : $("#kd_grp_pas4").val(), kd_cb: e.sender._cascadedValue});
    var kd_rk_pas5 = $("#kd_rk_pas5").data("kendoDropDownList");
    kd_rk_pas5.dataSource.read({kd_grp_rk : $("#kd_grp_pas5").val(), kd_cb: e.sender._cascadedValue});
    var kd_rk_mkt = $("#kd_rk_mkt").data("kendoDropDownList");
    kd_rk_mkt.dataSource.read({kd_grp_rk : $("#kd_grp_mkt").val(), kd_cb: e.sender._cascadedValue});
    var value = e.sender._cascadedValue;
    $("#temp_kd_cb").val(value);
}

function OnKodeCOBChange(e){
    var value = e.sender._cascadedValue;
    $("#temp_kd_cob").val(value);
    var kd_scob = $("#kd_scob").data("kendoDropDownList");
    kd_scob.dataSource.read({kd_cob : e.sender._cascadedValue});
}

function OnKodeSCOBChange(e){
    var value = e.sender._cascadedValue;
    $("#temp_kd_scob").val(value);
    ajaxGet(`/PengajuanAkseptasi/GenerateKeteranganResiko?kd_cob=${$("#temp_kd_cob").val()}&kd_scob=${value}`,
        function (response) {
            $("#ket_rsk").getKendoEditor().value(response);
        }
    );
}