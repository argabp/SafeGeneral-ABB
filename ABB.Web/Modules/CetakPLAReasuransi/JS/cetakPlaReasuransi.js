$(document).ready(function () {
    searchKeyword_OnKeyUp();
});

function searchKeyword_OnKeyUp() {
    $('#SearchKeyword').keyup(function () {
        refreshGrid("#CetakPLAReasuransiGrid");
    });
}

function searchFilter() {
    return {
        searchkeyword: $("#SearchKeyword").val()
    }
}

function viewReport(element){
    showProgressOnGrid('#CetakPLAReasuransiGrid');
    
    var id = $(element).parent().parent().parent()[0].id;
    var dataItem = $("#CetakPLAReasuransiGrid").getKendoGrid().dataItem($(element).parent().parent().parent().parent().parent()[0].previousElementSibling);
    var formElement = $('#' + id);
    
    var form = {};
    form.kd_cb = dataItem.kd_cb;
    form.kd_cob = dataItem.kd_cob;
    form.kd_scob = dataItem.kd_scob;
    form.no_kl = dataItem.no_kl;
    form.no_mts = dataItem.no_mts;
    form.kd_thn = dataItem.kd_thn;
    // form.bahasa = formElement[0].querySelector('select[name="bahasa"]').value
    
    ajaxPost("/CetakPLAReasuransi/GenerateReport", JSON.stringify(form),
        function (response) {
            if(response.Status === "OK"){
                window.open("/Reports/" + response.Data + "/CetakPLAReasuransi.pdf",  '_blank');
            } else {
                showMessage('Error', response.Message);
            }
            closeProgressOnGrid('#CetakPLAReasuransiGrid');
        }, 
    );
}