$(document).ready(function () {
    btnPreviousCoverage();
    btnNextCoverage();
    searchKeywordCoverage_OnKeyUp();
});


function btnPreviousCoverage(){
    $('#btn-previous-inquiryResikoCoverage').click(function () {
        $("#resikoTab").getKendoTabStrip().select(0);
    });
}
function btnNextCoverage(){
    $('#btn-next-inquiryResikoCoverage').click(function () {
        $("#resikoTab").getKendoTabStrip().select(2);
    });
}

function searchKeywordCoverage_OnKeyUp() {
    $('#SearchKeywordCoverage').keyup(function () {
        refreshGrid("#InquiryCoverageGrid");
    });
}

function openInquiryCoverageWindow(url, title) {
    openWindow('#InquiryCoverageWindow', url, title);
}

function btnViewInquiryCoverage_OnClick(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    console.log('dataItem', dataItem);
    openInquiryCoverageWindow(`/Inquiry/ViewCoverage?kd_cb=${dataItem.kd_cb}&kd_cob=${dataItem.kd_cob}&kd_scob=${dataItem.kd_scob}&kd_thn=${dataItem.kd_thn}&no_pol=${dataItem.no_pol}&no_updt=${dataItem.no_updt}&no_rsk=${dataItem.no_rsk}&kd_endt=${dataItem.kd_endt}&kd_cvrg=${dataItem.kd_cvrg}`, 'View Coverage');
}

function searchFilterCoverage() {
    return {
        searchkeyword: $("#SearchKeywordCoverage").val(),
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
