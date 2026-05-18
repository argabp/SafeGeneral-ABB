$(document).ready(function () {
    searchKeyword_OnKeyUp();
});

function searchKeyword_OnKeyUp() {
    $('#SearchKeyword').keyup(function () {
        refreshGrid("#CetakNotaPremiFakultatifMasukGrid");
    });
}

function viewReport(element){
    showProgressOnGrid('#CetakNotaPremiFakultatifMasukGrid');
    var id = $(element).parent()[0].id;
    var dataItem = $("#CetakNotaPremiFakultatifMasukGrid").getKendoGrid().dataItem($(element).parent().parent().parent()[0].previousElementSibling);
    var formElement = $('#' + id);
    
    var form = {};
    form.kd_cb = dataItem.kd_cb;
    form.kd_cob = dataItem.kd_cob;
    form.kd_scob = dataItem.kd_scob;
    form.kd_thn = dataItem.kd_thn;
    form.no_pol = dataItem.no_pol;
    form.no_updt = dataItem.no_updt;
    form.jns_lap = formElement[0].querySelector('select[name="jenisLaporan"]').value
    form.kd_mtu = formElement[0].querySelector('select[name="mataUang"]').value
    
    ajaxPost("/CetakNotaPremiFakultatifMasuk/GenerateReport", JSON.stringify(form),
        function (response) {
            if(response.Status === "OK"){
                window.open("/Reports/" + response.Data + "/CetakNotaPremiFakultatifMasuk.pdf",  '_blank');
            } else {
                showMessage('Error', response.Message);
            }
            closeProgressOnGrid('#CetakNotaPremiFakultatifMasukGrid');
        }, 
    );
}