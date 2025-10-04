$(document).ready(function () {
    searchKeyword_OnKeyUp();
});

function searchKeyword_OnKeyUp() {
    $('#SearchKeyword').keyup(function () {
        refreshGrid("#CetakLampiranEndorsmentGrid");
    });
}

function searchFilter() {
    return {
        searchkeyword: $("#SearchKeyword").val()
    }
}

function viewReport(e){
    showProgressOnGrid('#CetakLampiranEndorsmentGrid');

    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));

    var form = {};
    form.kd_cb = dataItem.kd_cb;
    form.kd_cob = dataItem.kd_cob;
    form.kd_scob = dataItem.kd_scob;
    form.no_pol = dataItem.no_pol;
    form.no_updt = dataItem.no_updt;
    form.kd_thn = dataItem.kd_thn;

    ajaxPost("/CetakLampiranEndorsment/GenerateReport", JSON.stringify(form),
        function (response) {
            if(response.Status === "OK"){
                window.open("/Reports/" + response.Data + "/CetakLampiranEndorsment.pdf",  '_blank');
            } else {
                showMessage('Error', response.Message);
            }
            closeProgressOnGrid('#CetakLampiranEndorsmentGrid');
        },
    );
}