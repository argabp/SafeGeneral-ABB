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
    form.no_kl = dataItem.no_kl;
    form.no_mts = dataItem.no_mts;
    form.kd_thn = dataItem.kd_thn;
    form.laporan = formElement[0].querySelector('select[name="laporan"]').value
    form.tanda_tangan = formElement[0].querySelector('input[name="tanda_tangan"]').value
    form.jabatan = formElement[0].querySelector('input[name="jabatan"]').value
    form.tipe_mts = dataItem.tipe_mts;
    
    ajaxPost("/LaporanKerugianPasti/GenerateReport", JSON.stringify(form),
        function (response) {
            if(response.Status === "OK"){
                window.open("/Reports/" + response.Data + "/LaporanKerugianPasti.pdf",  '_blank');
            } else {
                showMessage('Error', response.Message);
            }
            closeProgressOnGrid('#AkseptasiGrid');
        }, 
    );
}