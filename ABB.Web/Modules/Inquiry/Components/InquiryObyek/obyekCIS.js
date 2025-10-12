$(document).ready(function () {
    btnPreviousObyekCIS();
    btnNextObyekCIS();
    searchKeywordObyekCIS_OnKeyUp();
});


function btnPreviousObyekCIS(){
    $('#btn-previous-inquiryResikoObyekCIS').click(function () {
        $("#resikoTab").getKendoTabStrip().select(1);
    });
}
function btnNextObyekCIS(){
    $('#btn-next-inquiryResikoObyekCIS').click(function () {
        $("#resikoTab").getKendoTabStrip().select(3);
    });
}

function searchKeywordObyekCIS_OnKeyUp() {
    $('#SearchKeywordObyekCIS').keyup(function () {
        refreshGrid("#InquiryObyekGrid");
    });
}

function openInquiryObyekCISWindow(url, title) {
    openWindow('#InquiryObyekWindow', url, title);
}

function searchFilterObyekCIS() {
    return {
        searchkeyword: $("#SearchKeywordObyekCIS").val(),
        kd_cb: $("#kd_cb").val(),
        kd_cob: $("#kd_cob").val(),
        kd_scob: $("#kd_scob").val(),
        kd_thn: $("#kd_thn").val(),
        no_pol: $("#no_pol").val(),
        no_updt: $("#no_updt").val(),
        no_rsk: resiko?.no_rsk,
        kd_endt: resiko?.kd_endt
    }
}

// Format totalNilaiAng as currency (money format)
var currencyFormatter = new Intl.NumberFormat('en-US', {
    style: 'decimal',
    minimumFractionDigits: 3,
    maximumFractionDigits: 3
});

function OnInquiryObyekDataBound(e){
    // Get the grid instance
    var grid = e.sender;
    var totalSaldo = 0;

    grid.dataSource.view().forEach(function(dataItem) {
        totalSaldo += dataItem.nilai_saldo || 0;
    });

    $("#totalSaldo").text(currencyFormatter.format(totalSaldo));

    var grid = this;
    gridAutoFit(grid);
}
