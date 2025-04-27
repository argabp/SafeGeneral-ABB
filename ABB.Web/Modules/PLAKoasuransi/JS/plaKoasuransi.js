$(document).ready(function () {
    searchKeyword_OnKeyUp();
});

function searchKeyword_OnKeyUp() {
    $('#SearchKeyword').keyup(function () {
        refreshGrid("#PLAKoasuransiGrid");
    });
}

function searchFilter() {
    return {
        searchkeyword: $("#SearchKeyword").val()
    }
}

function viewReport(element){
    showProgressOnGrid('#PLAKoasuransiGrid');
    
    var id = $(element).parent().parent().parent()[0].id;
    var dataItem = $("#PLAKoasuransiGrid").getKendoGrid().dataItem($(element).parent().parent().parent().parent().parent()[0].previousElementSibling);
    var formElement = $('#' + id);
    
    var form = getFormData(formElement);
    form.kd_cb = dataItem.kd_cb;
    form.kd_cob = dataItem.kd_cob;
    form.kd_scob = dataItem.kd_scob;
    form.no_kl = dataItem.no_kl;
    form.no_mts = dataItem.no_mts;
    form.kd_thn = dataItem.kd_thn;
    form.bahasa = formElement[0].querySelector('select[name="bahasa"]').value
    
    ajaxPost("/PLAKoasuransi/GenerateReport", JSON.stringify(form),
        function (response) {
            if(response.Status === "OK"){
                window.open("/Reports/" + response.Data + "/PLAKoasuransi.pdf",  '_blank');
            } else {
                showMessage('Error', response.Message);
            }
            closeProgressOnGrid('#PLAKoasuransiGrid');
        }, 
    );
}