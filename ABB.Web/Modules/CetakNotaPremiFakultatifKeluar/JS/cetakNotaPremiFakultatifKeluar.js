$(document).ready(function () {
    searchKeyword_OnKeyUp();
});

function searchKeyword_OnKeyUp() {
    $('#SearchKeyword').keyup(function () {
        refreshGrid("#CetakNotaPremiFakultatifKeluarGrid");
    });
}

function viewReport(element){
    showProgressOnGrid('#CetakNotaPremiFakultatifKeluarGrid');
    var dataItem = $("#CetakNotaPremiFakultatifKeluarGrid").getKendoGrid().dataItem($(element).parent().parent().parent()[0].previousElementSibling);
    
    var form = {};
    form.kd_cb = dataItem.kd_cb;
    form.jns_tr = dataItem.jns_tr;
    form.jns_nt_msk = dataItem.jns_nt_msk;
    form.kd_thn = dataItem.kd_thn;
    form.kd_bln = dataItem.kd_bln;
    form.no_nt_msk = dataItem.no_nt_msk;
    form.jns_nt_kel = dataItem.jns_nt_kel;
    form.no_nt_kel = dataItem.no_nt_kel;
    form.flag_posting = dataItem.flag_posting;
    form.jns_lap = $(element).parent()[0].querySelector('select[name="jenisLaporan"]').value
    
    ajaxPost("/CetakNotaPremiFakultatifKeluar/GenerateReport", JSON.stringify(form),
        function (response) {
            if(response.Status === "OK"){
                window.open("/Reports/" + response.Data + "/CetakNotaPremiFakultatifKeluar.pdf",  '_blank');
            } else {
                showMessage('Error', response.Message);
            }
            closeProgressOnGrid('#CetakNotaPremiFakultatifKeluarGrid');
        }, 
    );
}