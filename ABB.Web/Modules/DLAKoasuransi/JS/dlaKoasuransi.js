$(document).ready(function () {
    searchKeyword_OnKeyUp();
});

function searchKeyword_OnKeyUp() {
    $('#SearchKeyword').keyup(function () {
        refreshGrid("#DLAKoasuransiGrid");
    });
}

function searchFilter() {
    return {
        searchkeyword: $("#SearchKeyword").val()
    }
}

function viewReport(element){
    showProgressOnGrid('#DLAKoasuransiGrid');
    
    var id = $(element).parent().parent().parent()[0].id;
    var dataItem = $("#DLAKoasuransiGrid").getKendoGrid().dataItem($(element).parent().parent().parent().parent().parent()[0].previousElementSibling);
    var formElement = $('#' + id);
    
    var form = getFormData(formElement);
    form.kd_cb = dataItem.kd_cb;
    form.kd_cob = dataItem.kd_cob;
    form.kd_scob = dataItem.kd_scob;
    form.no_kl = dataItem.no_kl;
    form.no_mts = dataItem.no_mts;
    form.kd_thn = dataItem.kd_thn;
    form.bahasa = formElement[0].querySelector('select[name="bahasa"]').value
    
    ajaxPost("/DLAKoasuransi/GenerateReport", JSON.stringify(form),
        function (response) {
            if(response.Status === "OK"){
                window.open("/Reports/" + response.Data + "/DLAKoasuransi.pdf",  '_blank');
            } else {
                showMessage('Error', response.Message);
            }
            closeProgressOnGrid('#DLAKoasuransiGrid');
        }, 
    );
}