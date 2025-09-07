$(document).ready(function () {
    btnSaveEntriNota_Click();
    btnCancelEntriNota_Click();
    setTimeout(setEntriNotaEditedValue, 1000);
});

function setEntriNotaEditedValue(){
    var flag_posting = $("#tempFlag_posting").val();
    flag_posting == "Y" ? $("#flag_posting").prop("checked", true) : $("#flag_posting").prop("checked", false);
    var flag_cancel = $("#tempFlag_cancel").val();
    flag_cancel == "Y" ? $("#flag_cancel").prop("checked", true) : $("#flag_cancel").prop("checked", false);

    $("#kd_rk_ttj").data("kendoDropDownList").value($("#temp_kd_rk_ttj").val().trim());

    closeProgress('#EntriNotaWindow');
}

function btnSaveEntriNota_Click() {
    $('#btn-save-entriNota').click(function () {
        showProgress('#EntriNotaWindow');
        setTimeout(function () {
            saveEntriNota('/EntriNota/SaveEntriNota')
        }, 500);
    });
}

function btnCancelEntriNota_Click() {
    $('#btn-cancel-entriNota').click(function () {
        openWindow("#EntriNotaCancelWindow", "/EntriNota/NotaCancel", "Nota Cancel")
    });
}

function saveEntriNota(url){
    var form = getFormData($('#EntriNotaForm'));
    var grid = $("#DetailEntriNotaGrid").data("kendoGrid");
    var details = grid.dataSource.data();
    form.Details = details;
    form.flag_posting = $("#flag_posting")[0].checked ? "Y" : "N";
    form.flag_cancel = $("#flag_cancel")[0].checked ? "Y" : "N";

    var totalAng = 0;

    grid.dataSource.view().forEach(function(dataItem) {
        totalAng += dataItem.nilai_ang || 0;
    });
    
    ajaxGet(`/EntriNota/ValidateSaveDetailNota?no_pol=${$("#no_pol").val()}&nilai_nt=${$("#nilai_nt").val()}&nilai_ang=${totalAng}`, 
        function (returnValue){

            if(returnValue == ""){
                ajaxPost(url, JSON.stringify(form),
                    function (response) {
                        refreshGrid("#EntriNotaGrid");
                        if (response.Result == "OK") {
                            showMessage('Success', response.Message);
                        } else
                            showMessage('Error', response.Message);

                        closeProgress("#EntriNotaWindow");
                        closeWindow("#EntriNotaWindow")
                    }
                );
            } else {
                showMessage("Error", returnValue);
                closeProgress("#EntriNotaWindow");
            }
        })
    
}

function OnKodeTertujuChange(e){
    var kd_rk_ttj = $("#kd_rk_ttj").data("kendoDropDownList");
    kd_rk_ttj.dataSource.read({
        kd_grp_ttj : e.sender._cascadedValue,
        kd_cb: $("#kd_cb").val().trim(),
        kd_cob: $("#kd_cob").val().trim(),
        kd_scob: $("#kd_scob").val().trim(),
        kd_thn: $("#kd_thn").val().trim(),
        no_pol: $("#no_pol").val().trim(),
        no_updt: $("#no_updt").val()
    });
}

// Format totalNilaiAng as currency (money format)
var currencyFormatter = new Intl.NumberFormat('en-US', {
    style: 'decimal',
    minimumFractionDigits: 3,
    maximumFractionDigits: 3
});

function OnDetailNotaDataBound(e){
    // Get the grid instance
    var grid = e.sender;
    var totalPstAng = 0;
    var totalAng = 0;

    grid.dataSource.view().forEach(function(dataItem) {
        totalPstAng += dataItem.pst_ang || 0;  // Ensure we sum the value or add 0 if undefined
        totalAng += dataItem.nilai_ang || 0;
    });

    $("#totalPersentaseAngsuran").text(totalPstAng.toFixed(2));
    $("#totalAngsuran").text(currencyFormatter.format(totalAng));
}

function dataNotaCancel(){
    return {
        kd_cb: $("#kd_cb").val(),
        kd_cob: $("#kd_cob").val(),
        kd_scob: $("#kd_scob").val(),
        kd_thn: $("#kd_thn").val(),
        no_pol: $("#no_pol").val(),
        no_updt: $("#no_updt").val(),
    }
}