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
    
    var form = getFormData($('#' + id));
    form.kd_cb = dataItem.kd_cb;
    form.kd_cob = dataItem.kd_cob;
    form.kd_scob = dataItem.kd_scob;
    form.no_pol = dataItem.no_pol;
    form.no_updt = dataItem.no_updt;
    form.kd_thn = dataItem.kd_thn;
    form.jenisLaporan = formElement[0].querySelector('select[name="jenisLaporan"]').value
    form.mataUang = formElement[0].querySelector('select[name="mataUang"]').value
    
    ajaxPost("/CetakNotaDanKwitansiPolis/GenerateReport", JSON.stringify(form),
        function (response) {
            if(response.Status === "OK"){
                window.open("/Reports/" + response.Data + "/CetakNotaDanKwitansiPolis.pdf",  '_blank');
            } else {
                showMessage('Error', response.Message);
            }
            closeProgressOnGrid('#AkseptasiGrid');
        },
    );
}