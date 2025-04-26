$(document).ready(function () {
    searchKeyword_OnKeyUp();
});

function searchKeyword_OnKeyUp() {
    $('#SearchKeyword').keyup(function () {
        refreshGrid("#AkseptasiGrid");
    });
}

function searchFilter() {
    return {
        searchkeyword: $("#SearchKeyword").val()
    }
}

function viewReport(element){
    showProgressOnGrid('#AkseptasiGrid');
    
    var id = $(element).parent().parent().parent()[0].id;
    var dataItem = $("#AkseptasiGrid").getKendoGrid().dataItem($(element).parent().parent().parent().parent().parent()[0].previousElementSibling);
    var formElement = $('#' + id);
    
    var form = getFormData(formElement);
    form.kd_cb = dataItem.kd_cb;
    form.kd_cob = dataItem.kd_cob;
    form.kd_scob = dataItem.kd_scob;
    form.no_pol = dataItem.no_pol;
    form.no_updt = dataItem.no_updt;
    form.kd_thn = dataItem.kd_thn;
    form.nm_ttg = dataItem.nm_ttg;
    form.almt_ttg = dataItem.almt_ttg;
    form.no_surat = formElement[0].querySelector('input[name="no_surat"]').value
    
    ajaxPost("/RenewalNotice/GenerateReport", JSON.stringify(form),
        function (response) {
            if(response.Status === "OK"){
                window.open("/Reports/" + response.Data + "/RenewalNotice.pdf",  '_blank');
            } else {
                showMessage('Error', response.Message);
            }
            closeProgressOnGrid('#AkseptasiGrid');
        }, 
    );
}