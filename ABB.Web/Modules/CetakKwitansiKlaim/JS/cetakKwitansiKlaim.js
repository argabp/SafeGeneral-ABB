$(document).ready(function () {
    searchKeyword_OnKeyUp();
});

function searchKeyword_OnKeyUp() {
    $('#SearchKeyword').keyup(function () {
        refreshGrid("#CetakKwitansiKlaimGrid");
    });
}

function searchFilter() {
    return {
        searchkeyword: $("#SearchKeyword").val()
    }
}

function viewReport(e){
    showProgressOnGrid('#CetakKwitansiKlaimGrid');
    
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    
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
    form.kd_bln = dataItem.kd_bln;
    
    ajaxPost("/CetakKwitansiKlaim/GenerateReport", JSON.stringify(form),
        function (response) {
            if(response.Status === "OK"){
                window.open("/Reports/" + response.Data + "/CetakKwitansiKlaim.pdf",  '_blank');
            } else {
                showMessage('Error', response.Message);
            }
            closeProgressOnGrid('#CetakKwitansiKlaimGrid');
        }, 
    );
}