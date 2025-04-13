$(document).ready(function () {
    btnPreview_Click();
});

function dataKodeTertanggungDropDown(){
    return {
        kd_grp_rk: $("#kd_grp_ttg").val().trim(),
        kd_cb: $("#kd_cb").val().trim()
    }
}

function dataKodeSumberBisnisDropDown(){
    return {
        kd_grp_rk: $("#kd_grp_sb_bis").val().trim(),
        kd_cb: $("#kd_cb").val().trim()
    }
}

function dataKodeMarketingDropDown(){
    return {
        kd_grp_rk: $("#kd_grp_mkt").val().trim(),
        kd_cb: $("#kd_cb").val().trim()
    }
}

function dataKodeSCOBDropDown(){
    return {
        kd_cob: $("#kd_cob").val().trim()
    }
}



function btnPreview_Click() {
    $('#btn-preview').click(function () {
        showProgressByElement($('#LaporanProduksiAsuransiForm'));
        setTimeout(function () {
            previewReport('/LaporanProduksiAsuransi/GenerateReport')
        }, 500);
    });
}

function previewReport(url) {
    var form = getFormData($('#LaporanProduksiAsuransiForm'));

    var data = JSON.stringify(form);

    ajaxPost(url, data,
        function (response) {
            if(response.Status === "OK"){
                window.open("/Reports/" + response.Data + "/LaporanProduksiAsuransi.pdf",  '_blank');
            } else {
                showMessage('Error', response.Message);
            }

            closeProgressByElement($('#LaporanProduksiAsuransiForm'));
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
function OnKodeMarketingChange(e){
    var kd_rk_mkt = $("#kd_rk_mkt").data("kendoDropDownList");
    kd_rk_mkt.dataSource.read({kd_grp_rk : e.sender._cascadedValue, kd_cb: $("#kd_cb").val()});
}

function OnKodeCabangChange(e){
    var kd_rk_ttg = $("#kd_rk_ttg").data("kendoDropDownList");
    kd_rk_ttg.dataSource.read({kd_grp_rk : $("#kd_grp_ttg").val(), kd_cb: e.sender._cascadedValue});
    var kd_rk_sb_bis = $("#kd_rk_sb_bis").data("kendoDropDownList");
    kd_rk_sb_bis.dataSource.read({kd_grp_rk : $("#kd_grp_sb_bis").val(), kd_cb: e.sender._cascadedValue});
    var kd_rk_mkt = $("#kd_rk_mkt").data("kendoDropDownList");
    kd_rk_mkt.dataSource.read({kd_grp_rk : $("#kd_grp_mkt").val(), kd_cb: e.sender._cascadedValue});
}

function OnKodeCOBChange(e){
    var kd_scob = $("#kd_scob").data("kendoDropDownList");
    kd_scob.dataSource.read({kd_cob : e.sender._cascadedValue});
}

function OnKodeRekananTertanggungChange(e){
    ajaxGet(`/LaporanProduksiAsuransi/GetKodeRekananTertanggung?kd_cb=${$("#kd_cb").val()}&kd_grp_rk=${$("#kd_grp_ttg").val()}&kd_rk=${e.sender._cascadedValue}`, (returnValue) => {
        var strings = returnValue.split(",");
        $("#nm_ttg").getKendoTextBox().value(strings[1]);
        $("#almt_ttg").getKendoTextArea().value(strings[4]);
        $("#kt_ttg").getKendoTextBox().value(strings[7]);
    });
}