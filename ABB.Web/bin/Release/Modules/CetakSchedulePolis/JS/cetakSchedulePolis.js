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
    
    var id = $(element).parent()[0].id;
    var dataItem = $("#AkseptasiGrid").getKendoGrid().dataItem($(element).parent().parent().parent()[0].previousElementSibling);
    var formElement = $('#' + id);
    
    var form = getFormData(formElement);
    form.kd_cb = dataItem.kd_cb;
    form.kd_cob = dataItem.kd_cob;
    form.kd_scob = dataItem.kd_scob;
    form.no_aks = dataItem.no_aks;
    form.no_updt = dataItem.no_updt;
    form.kd_thn = dataItem.kd_thn;
    form.nm_ttg = dataItem.nm_ttg;
    form.jenisLaporan = formElement[0].querySelector('select[name="jenisLaporan"]').value
    form.bahasa = formElement[0].querySelector('select[name="bahasa"]').value
    form.jenisLampiran = formElement[0].querySelector('select[name="jenisLampiran"]').value
    
    ajaxPost("/CetakSchedulePolis/GenerateReport", JSON.stringify(form),
        function (response) {
            if(response.Status === "OK"){
                window.open("/Reports/" + response.Data + "/CetakSchedulePolis.pdf",  '_blank');
            } else {
                showMessage('Error', response.Message);
            }
            closeProgressOnGrid('#AkseptasiGrid');
        }, 
    );
}