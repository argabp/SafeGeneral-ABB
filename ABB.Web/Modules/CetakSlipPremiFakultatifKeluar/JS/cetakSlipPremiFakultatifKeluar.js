$(document).ready(function () {
    searchKeyword_OnKeyUp();
});

function searchKeyword_OnKeyUp() {
    $('#SearchKeyword').keyup(function () {
        refreshGrid("#CetakSlipPremiFakultatifKeluarGrid");
    });
}

function viewReport(e){
    showProgressOnGrid('#CetakSlipPremiFakultatifKeluarGrid');
    
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    
    var form = {};
    form.kd_cb = dataItem.kd_cb_pol;
    form.kd_cob = dataItem.kd_cob;
    form.kd_scob = dataItem.kd_scob;
    form.kd_thn = dataItem.kd_thn;
    form.no_pol = dataItem.no_pol;
    form.no_updt = dataItem.no_updt;
    form.no_rsk = dataItem.no_rsk;
    form.kd_endt = dataItem.kd_endt;
    form.no_updt_reas = dataItem.no_updt_reas;
    form.kd_grp_sor = dataItem.kd_grp_pas;
    form.kd_rk_sor = dataItem.kd_rk_pas;
    
    ajaxPost("/CetakSlipPremiFakultatifKeluar/GenerateReport", JSON.stringify(form),
        function (response) {
            if(response.Status === "OK"){
                window.open("/Reports/" + response.Data + "/CetakSlipPremiFakultatifKeluar.pdf",  '_blank');
            } else {
                showMessage('Error', response.Message);
            }
            closeProgressOnGrid('#CetakSlipPremiFakultatifKeluarGrid');
        }, 
    );
}